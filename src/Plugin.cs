using System;
using System.IO;
using System.Reflection;
using BepInEx;
using ObjectVelocityTracker;
using UnityEngine;
using UnityEngine.XR;
using Utilla;

namespace BeachBall
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class BeachBallMain : BaseUnityPlugin
    {
        public static BeachBallMain instance;
        public bool Can = false;
        public GameObject RightHand;
        public GameObject LeftHand;
        public GameObject Cabinetobj;
        public GameObject BB;

        public Vector3 originalPosition;
        public Vector3 originalEulerAngles;

        public bool GameInitialized;

        public InputFeatureUsage<bool> leftHandUsage = CommonUsages.triggerButton;
        public InputFeatureUsage<bool> rightHandUsage = CommonUsages.triggerButton;

        void Start()
        {
            instance = this;

            if (!GameInitialized)
                Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            if (!GameInitialized)
                return;

            if (BB != null)
            {
                BB.SetActive(true);
            }

            if (Cabinetobj != null)
            {
                Cabinetobj.SetActive(true);
            }
        }

        void OnDisable()
        {
            if (!GameInitialized)
                return;

            if (BB != null)
            {
                BB.SetActive(false);
            }

            if (Cabinetobj != null)
            {
                Cabinetobj.SetActive(false);
            }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            GameInitialized = true;

            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Jam_Submission.Assets.beachballers");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject beachballcabinet = bundle.LoadAsset<GameObject>("Ball Cabinet");

            Cabinetobj = Instantiate(beachballcabinet);
            Cabinetobj.transform.Find("Cabinet/LeftDoorOpen/t").gameObject.AddComponent<DoorTriggers>();
            Cabinetobj.transform.Find("Cabinet/RightDoorOpen/t").gameObject.AddComponent<DoorTriggers>();
            Cabinetobj.transform.Find("Cabinet/LeftDoorClose/t").gameObject.AddComponent<DoorTriggers>();
            Cabinetobj.transform.Find("Cabinet/RightDoorClose/t").gameObject.AddComponent<DoorTriggers>();
            Cabinetobj.transform.Find("Cabinet/Reset Button").gameObject.AddComponent<ResetTrigger>();

            BB = Cabinetobj.transform.Find("Cabinet/Beach BallMonoObject").gameObject;
            BB.AddComponent<BeachBallStuff>();

            LeftHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L");
            RightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");

            ModdedRoomActions(false);

            Transform cabTemp = Cabinetobj.transform.Find("Cabinet");
            Destroy(cabTemp.GetComponent<MeshCollider>());

            cabTemp.gameObject.AddComponent<MeshCollider>();
            cabTemp.gameObject.AddComponent<GorillaSurfaceOverride>().overrideIndex = 9;

            GameObject.Find("Player/GorillaPlayer/TurnParent/LeftHandTriggerCollider").AddComponent<VelocityTracker>();
            GameObject.Find("Player/GorillaPlayer/TurnParent/RightHandTriggerCollider").AddComponent<VelocityTracker>();

            originalPosition = BB.transform.position;
            originalEulerAngles = BB.transform.eulerAngles;
        }

        public void Update()
        {
            if (BB == null)
                return;

            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(leftHandUsage, out bool Lefttrigger);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(rightHandUsage, out bool Righttrigger);

            if (Righttrigger && !Lefttrigger)
            {
                if (Vector3.Distance(RightHand.transform.position, BB.transform.position) < 1)
                {
                    if (!Can)
                    {
                        BB.GetComponent<AudioSource>().enabled = false;
                    }
                    Can = true;
                    BB.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    BB.transform.SetParent(RightHand.transform, true);
                    BB.transform.localPosition = new Vector3(-0.17f, 0f, 0f);
                }
            }
            else if (Can)
            {
                Can = false;
                BB.GetComponent<AudioSource>().enabled = true;
                BB.transform.SetParent(null, true);
            }

            if (Lefttrigger)
            {
                if (Vector3.Distance(LeftHand.transform.position, BB.transform.position) < 1)
                {
                    if (!Can)
                    {
                        BB.GetComponent<AudioSource>().enabled = false;
                    }
                    Can = true;
                    BB.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    BB.transform.SetParent(LeftHand.transform, true);
                    BB.transform.localPosition = new Vector3(0f, 0f, 0.17f);
                }
            }
            else if (Can)
            {
                Can = false;
                BB.GetComponent<AudioSource>().enabled = true;
                BB.transform.SetParent(null, true);
            }
        }

        void ModdedRoomActions(bool inModdedRoom)
        {
            if (inModdedRoom)
            {
                Cabinetobj.transform.Find("Cabinet").gameObject.layer = 9;
                Cabinetobj.transform.Find("Cabinet/LeftDoorOpen").gameObject.layer = 9;
                Cabinetobj.transform.Find("Cabinet/RightDoorOpen").gameObject.layer = 9;
                Cabinetobj.transform.Find("Cabinet/LeftDoorClose").gameObject.layer = 9;
                Cabinetobj.transform.Find("Cabinet/RightDoorClose").gameObject.layer = 9;
                Cabinetobj.transform.Find("Cabinet/Canvas/Modded Lobby").gameObject.SetActive(false);
                return;
            }

            Cabinetobj.transform.Find("Cabinet").gameObject.layer = 8;
            Cabinetobj.transform.Find("Cabinet/LeftDoorOpen").gameObject.layer = 8;
            Cabinetobj.transform.Find("Cabinet/RightDoorOpen").gameObject.layer = 8;
            Cabinetobj.transform.Find("Cabinet/LeftDoorClose").gameObject.layer = 8;
            Cabinetobj.transform.Find("Cabinet/RightDoorClose").gameObject.layer = 8;
            Cabinetobj.transform.Find("Cabinet/Canvas/Modded Lobby").gameObject.SetActive(true);
            return;
        }

        [ModdedGamemodeJoin] public void OnJoin() => ModdedRoomActions(true);
        [ModdedGamemodeLeave] public void OnLeave() => ModdedRoomActions(false);

    }
}

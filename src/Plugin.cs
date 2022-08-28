using System;
using System.IO;
using System.Reflection;
using BepInEx;
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
		GameObject RightHand;
		GameObject LeftHand;
		GameObject Cabinetobj;
		public static GameObject BB;
		void OnEnable()
		{

			HarmonyPatches.ApplyHarmonyPatches();
			Utilla.Events.GameInitialized += OnGameInitialized;
		}
		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
			Utilla.Events.GameInitialized -= OnGameInitialized;
		}
		void OnGameInitialized(object sender, EventArgs e)
		{
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
		}
       public void Update()
        {
			if (BB != null)
            {
				bool Lefttrigger;
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out Lefttrigger);
				bool Righttrigger;
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out Righttrigger);
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
		}
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			Cabinetobj.transform.Find("Cabinet").gameObject.layer = 0;
			Cabinetobj.transform.Find("Cabinet/LeftDoorOpen").gameObject.layer = 0;
			Cabinetobj.transform.Find("Cabinet/RightDoorOpen").gameObject.layer = 0;
			Cabinetobj.transform.Find("Cabinet/LeftDoorClose").gameObject.layer = 0;
			Cabinetobj.transform.Find("Cabinet/RightDoorClose").gameObject.layer = 0;
			Cabinetobj.transform.Find("Cabinet/Canvas/Modded Lobby").gameObject.SetActive(false);
		}
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			Cabinetobj.transform.Find("Cabinet").gameObject.layer = 2;
			Cabinetobj.transform.Find("Cabinet/LeftDoorOpen").gameObject.layer = 2;
			Cabinetobj.transform.Find("Cabinet/RightDoorOpen").gameObject.layer = 2;
			Cabinetobj.transform.Find("Cabinet/LeftDoorClose").gameObject.layer = 2;
			Cabinetobj.transform.Find("Cabinet/RightDoorClose").gameObject.layer = 2;
			Cabinetobj.transform.Find("Cabinet/Canvas/Modded Lobby").gameObject.SetActive(true);
		}
	}

}

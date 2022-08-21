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
		bool Can = false;
		public static BeachBallMain instance;
		GameObject RightHand;
		GameObject LeftHand;
		public GameObject Cabinetobj;
		public GameObject BB;
		public GameObject HandTrigger;
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
		// unneeded maybe Cabinet.transform.rotation = new Quaternion(0f, 161f, 0f, 0f);
		void OnGameInitialized(object sender, EventArgs e)
		{
			Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Jam_Submission.Assets.beachballers");
			AssetBundle bundle = AssetBundle.LoadFromStream(str);
			GameObject beachballcabinet = bundle.LoadAsset<GameObject>("Ball Cabinet");
			Cabinetobj = Instantiate(beachballcabinet);
			//Cabinetobj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			//Cabinetobj.transform.position = new Vector3(-68.5422f, 11.9302f, -84.247f);
			Cabinetobj.transform.Find("Cabinet/LeftDoorOpen/t").gameObject.AddComponent<DoorTriggers>();
			Cabinetobj.transform.Find("Cabinet/RightDoorOpen/t").gameObject.AddComponent<DoorTriggers>();
			Cabinetobj.transform.Find("Cabinet/LeftDoorClose/t").gameObject.AddComponent<DoorTriggers>();
			Cabinetobj.transform.Find("Cabinet/RightDoorClose/t").gameObject.AddComponent<DoorTriggers>();
			Cabinetobj.transform.Find("Cabinet/Reset Button").gameObject.AddComponent<ResetTrigger>();
			BB = Cabinetobj.transform.Find("Cabinet/Beach Ball").gameObject;
			BB.AddComponent<BeachBallStuff>();
			LeftHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L");
			RightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");
		}
       public void Update()
        {
			if (BB != null)
            {
				bool Leftgrip;
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out Leftgrip);
				bool Rightgrip;
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out Rightgrip);
				if (Rightgrip && !Leftgrip)
				{
					if (Vector3.Distance(RightHand.transform.position, BB.transform.position) < 3)
					{
						Can = true;
						BB.GetComponent<Rigidbody>().velocity = Vector3.zero;
						BB.transform.SetParent(RightHand.transform, true);
						BB.transform.localPosition = new Vector3(-0.3f, 0f, 0f);
					}
				}
				else if (Can)
				{
					Can = false;
					BB.transform.SetParent(null, true);
				}


				if (Leftgrip && !Rightgrip)
				{
					if (Vector3.Distance(LeftHand.transform.position, BB.transform.position) < 3)
					{
						Can = true;
						BB.GetComponent<Rigidbody>().velocity = Vector3.zero;
						BB.transform.SetParent(LeftHand.transform, true);
						BB.transform.localPosition = new Vector3(0f, 0f, 0.3f);
					}
				}
				else if (Can)
				{
					Can = false;
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

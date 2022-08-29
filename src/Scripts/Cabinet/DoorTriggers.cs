using ObjectVelocityTracker;
using UnityEngine;
using System.Collections;

public class DoorTriggers : MonoBehaviour
{
    GameObject LDO;
    GameObject RDO;
    GameObject LDC;
    GameObject RDC;

    void Start()
    {
        LDO = GameObject.Find("Ball Cabinet(Clone)/Cabinet/LeftDoorOpen");
        RDO = GameObject.Find("Ball Cabinet(Clone)/Cabinet/RightDoorOpen");
        LDC = GameObject.Find("Ball Cabinet(Clone)/Cabinet/LeftDoorClose");
        RDC = GameObject.Find("Ball Cabinet(Clone)/Cabinet/RightDoorClose");

        gameObject.layer = 18;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<GorillaTriggerColliderHandIndicator>() == null)
            return;

        GorillaTagger.Instance.StartVibration(col.name == "LeftHandTriggerCollider", 0.15f, 0.1f);

        if (gameObject.transform.parent.name == "LeftDoorOpen")
        {
            LDO.SetActive(false);
            LDC.SetActive(true);
        }

        if (gameObject.transform.parent.name == "RightDoorOpen")
        {
            RDO.SetActive(false);
            RDC.SetActive(true);
        }

        if (gameObject.transform.parent.name == "LeftDoorClose")
        {
            LDO.SetActive(true);
            LDC.SetActive(false);
        }

        if (gameObject.transform.parent.name == "RightDoorClose")
        {
            RDO.SetActive(true);
            RDC.SetActive(false);
        }

    }
}
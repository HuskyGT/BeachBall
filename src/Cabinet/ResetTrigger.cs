using ObjectVelocityTracker;
using UnityEngine;
using System.Collections;

public class ResetTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.GetComponent<VelocityTracker>() != null)
        {
            print($"{gameObject.name} opened by {col.gameObject.name}");
            GameObject BeachBall = GameObject.Find("Beach Ball");
            if (BeachBall != null)
            {
                if (col.name == "LeftHandTriggerCollider")
                {
                    GorillaTagger.Instance.StartVibration(true, 0.3f, 0.1f);
                }
                if (col.name == "RightHandTriggerCollider")
                {
                    GorillaTagger.Instance.StartVibration(false, 0.3f, 0.1f);
                }
                // GorillaTagger.Instance.StartVibration
                BeachBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                BeachBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                BeachBall.transform.position = new Vector3(-68.5292f, 11.5086f, -84.2434f);
            }
        }
    }
}
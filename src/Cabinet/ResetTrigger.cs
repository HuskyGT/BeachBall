using ObjectVelocityTracker;
using UnityEngine;
using System.Collections;
using BeachBall;

public class ResetTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.GetComponent<VelocityTracker>() != null)
        {
            GameObject BeachBall = BeachBallMain.BB;
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
                BeachBall.transform.position = new Vector3(-63.8182f, 11.9579f, -84.8337f);
            }
        }
    }
}

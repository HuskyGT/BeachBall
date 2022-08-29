using UnityEngine;
using BeachBall;

public class ResetTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (BeachBallMain.instance.BB == null)
            return;

        GorillaTagger.Instance.StartVibration(col.name == "LeftHandTriggerCollider", 0.15f, 0.1f);

        BeachBallMain.instance.BB.GetComponent<Rigidbody>().velocity = Vector3.zero;
        BeachBallMain.instance.BB.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        BeachBallMain.instance.BB.transform.position = BeachBallMain.instance.originalPosition - new Vector3(0, BeachBallMain.instance.BB.transform.localScale.y * 1.35f, 0);
        BeachBallMain.instance.BB.transform.eulerAngles = BeachBallMain.instance.originalEulerAngles;
    }
}

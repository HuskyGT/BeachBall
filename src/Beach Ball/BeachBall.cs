using ObjectVelocityTracker;
using UnityEngine;
public class BeachBallStuff : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("LeftHandTriggerCollider").AddComponent<VelocityTracker>();
        GameObject.Find("RightHandTriggerCollider").AddComponent<VelocityTracker>();
        gameObject.transform.SetParent(null, true);
    }
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(0f, -2f, 0f);
    }
    void OnTriggerEnter(Collider col)
    {
            if (col.gameObject.GetComponent<VelocityTracker>() != null)
            {
                if (!gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    if (col.name == "LeftHandTriggerCollider")
                    {
                        GorillaTagger.Instance.StartVibration(true, 0.3f, 0.1f);
                    }
                    if (col.name == "RightHandTriggerCollider")
                    {
                        GorillaTagger.Instance.StartVibration(false, 0.3f, 0.1f);
                    }
                        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
                }
                print($"{col.gameObject.name} Hit Beach Ball with: {col.gameObject.GetComponent<VelocityTracker>().velocity} velocity");
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().AddExplosionForce(col.gameObject.GetComponent<VelocityTracker>().force, col.ClosestPoint(transform.position), 5);
            }
    }
}
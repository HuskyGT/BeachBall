using UnityEngine;
using ObjectVelocityTracker;

public class BeachBallStuff : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.SetParent(null, true);
    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(0f, -2f, 0f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<VelocityTracker>() == null)
            return;

        if (gameObject.GetComponent<AudioSource>().isPlaying)
            return;

        GorillaTagger.Instance.StartVibration(col.name == "LeftHandTriggerCollider", 0.15f, 0.1f);

        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().AddExplosionForce(col.gameObject.GetComponent<VelocityTracker>().force, col.ClosestPoint(transform.position), 8);
    }
}

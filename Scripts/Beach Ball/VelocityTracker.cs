using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectVelocityTracker
{
    public class VelocityTracker : MonoBehaviour
    {
        public float force;
        public Vector3 velocity;
        public Vector3 LastPos;

        void Start()
        {
            LastPos = Vector3.zero;
            velocity = transform.position;
        }
        void Update()
        {
            velocity = transform.position - LastPos / Time.deltaTime / 5;
            //velocity = transform.position - LastPos / Time.deltaTime / 15;
            LastPos = transform.position;
            force = velocity.magnitude;
        }
    }
}
    


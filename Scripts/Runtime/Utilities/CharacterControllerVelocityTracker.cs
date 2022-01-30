using System;
using UnityEngine;

namespace DoubTech.AnimalAI.Utilities
{
    public class CharacterControllerVelocityTracker : MonoBehaviour, IVelocityTracker
    {
        [SerializeField] private CharacterController characterController;
        
        private Quaternion rotationLast;

        private void OnValidate()
        {
            if(!characterController) characterController = GetComponent<CharacterController>();
        }

        public float Speed { get; private set; } = 0;
        public float Strafe { get; private set; } = 0;
        public float Turn { get; private set; } = 0;

        protected virtual void FixedUpdate()
        {
            Speed = Vector3.Dot(characterController.velocity, characterController.transform.forward);
            Strafe = Vector3.Dot(characterController.velocity, characterController.transform.right);
            
            
            Quaternion deltaRotation = characterController.transform.rotation * Quaternion.Inverse(rotationLast);
            rotationLast = characterController.transform.rotation;
 
            deltaRotation.ToAngleAxis(out var angle, out var axis);
 
            angle *= Mathf.Deg2Rad;
 
            var angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            Turn = angularVelocity.magnitude;
        }
    }
}

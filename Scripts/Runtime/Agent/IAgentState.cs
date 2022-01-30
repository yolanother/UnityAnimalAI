using System;
using DoubTech.AnimalAI.Animations;
using UnityEngine;

namespace DoubTech.AnimalAI.Agent
{
    public interface IAgentState
    {
        public AnimationStateController AnimationController
        {
            get;
            set;
        }
        public float RunSpeed { get; }
        public float WalkSpeed { get; }
        public Transform Transform { get; }
        
        public Vector3 Destination { get; set; }

        public bool ReachedDestination { get; }

        public float DistanceRemaining { get; }
        bool Stop { get; set; }

        public void CheckForValidDestination(Vector3 destination, Action<Vector3, bool> onDestinationCalculated);
    }
}

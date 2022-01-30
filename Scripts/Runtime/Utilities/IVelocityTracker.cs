using System;

namespace DoubTech.AnimalAI.Utilities
{
    public interface IVelocityTracker
    {
        public float Speed { get; }
        public float Strafe { get; }
        public float Turn { get; }
    }
}

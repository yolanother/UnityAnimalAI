using System;
using System.Collections.Generic;
using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Utilities;
using UnityEngine;

namespace DoubTech.AnimalAI.Animations
{
    public class AnimationStateController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private AnimatorOverrideController controller;
        
        private static readonly int AnimParamSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimParamStrafe = Animator.StringToHash("Strafe");
        private static readonly int AnimParamTurn = Animator.StringToHash("Turn");
        private static readonly int AnimParamIdle = Animator.StringToHash("Idle");
        private static readonly int AnimParamSleeping = Animator.StringToHash("Sleeping");
        private static readonly int AnimParamGoToSleep = Animator.StringToHash("GoToSleep");
        private static readonly int AnimParamEmote1 = Animator.StringToHash("Emote Slot 1");
        private static readonly int AnimParamEmote2 = Animator.StringToHash("Emote Slot 2");

        private IVelocityTracker velocityTracker;
        private IAgentState agentState;

        private Queue<CrossfadableAnimation> animationQueue = new Queue<CrossfadableAnimation>();
        
        private int currentIdleClip = 0;

        private AnimationClip idle1;
        private AnimationClip idle2;

        private CrossfadableAnimation emoteClip1;
        private CrossfadableAnimation emoteClip2;

        private int activeEmote = -1;
        private const int EMOTE1 = 0;
        private const int EMOTE2 = 1;

        public void OnStartedState(string tag)
        {
            switch (tag)
            {
                case "Emote1":
                    activeEmote = EMOTE1;
                    break;
                case "Emote2":
                    activeEmote = EMOTE2;
                    break;
            }
        }

        public void OnStoppedState(string tag)
        {
            switch (tag)
            {
                case "Emote1":
                    emoteClip1.onPlaybackFinished?.Invoke();
                    break;
                case "Emote2":
                    emoteClip2.onPlaybackFinished?.Invoke();
                    break;
            }

            if (animationQueue.Count > 0)
            {
                Emote(animationQueue.Dequeue());
            }
            else
            {
                activeEmote = -1;
            }
        }

        protected virtual void OnValidate()
        {
            if (!animator) animator = GetComponent<Animator>();
        }

        protected virtual void Awake()
        {
            agentState = GetComponentInParent<IAgentState>();
            controller = animator.runtimeAnimatorController as AnimatorOverrideController;
            velocityTracker = GetComponentInParent<IVelocityTracker>();
        }

        protected virtual void OnEnable()
        {
            agentState.AnimationController = this;
        }

        private float speed;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                animator.SetFloat(AnimParamSpeed, CalculateSpeed(value));
            }
        }

        private float strafe;
        public float Strafe
        {
            get => strafe;
            set
            {
                strafe = value;
                animator.SetFloat(AnimParamStrafe, CalculateSpeed(value));
            }
        }

        public float Turn
        {
            get => animator.GetFloat(AnimParamTurn);
            set => animator.SetFloat(AnimParamTurn, value);
        }

        private bool sleeping;
        public bool Sleep
        {
            get => sleeping;
            set
            {
                if (!sleeping)
                {
                    animator.SetTrigger(AnimParamGoToSleep);
                }

                sleeping = value;
                animator.SetBool(AnimParamSleeping, value);
            }
        }

        private void Emote(CrossfadableAnimation clip)
        {
            switch (activeEmote)
            {
                case EMOTE1:
                    emoteClip2 = clip;
                    controller["Emote Slot 2"] = clip.clip;
                    animator.CrossFade(AnimParamEmote2, clip.transition);
                    //animator.SetTrigger(AnimParamEmote2);
                    activeEmote = EMOTE2;
                    break;
                default:
                    emoteClip1 = clip;
                    controller["Emote Slot 1"] = clip.clip;
                    animator.CrossFade(AnimParamEmote1, clip.transition);
                    //animator.SetTrigger(AnimParamEmote1);
                    activeEmote = EMOTE1;
                    break;
            }
        }

        public AnimationClip Idle
        {
            get => currentIdleClip == 0 ? idle1 : idle2;
            set
            {
                if (currentIdleClip == 1)
                {
                    idle1 = value;
                    controller["Idle Slot 1"] = value;
                }
                else
                {
                    idle2 = value;
                    controller["Idle Slot 2"] = value;
                }
            }
        }

        private float CalculateSpeed(float value)
        {
            return value < agentState.WalkSpeed
                ? value / agentState.WalkSpeed
                : (value - agentState.WalkSpeed) / (agentState.RunSpeed - agentState.WalkSpeed) + 1;
        }

        private void FixedUpdate()
        {
            if (null != velocityTracker)
            {
                Speed = velocityTracker.Speed;
                Strafe = velocityTracker.Strafe;
                Turn = velocityTracker.Turn;
            }
        }

        public void QueueAnimation(CrossfadableAnimation animationClip)
        {
            animationQueue.Enqueue(animationClip);
            if (activeEmote == -1)
            {
                Emote(animationQueue.Dequeue());
            }
        }
    }

    public class CrossfadableAnimation
    {
        public float transition = .2f;
        public AnimationClip clip;
        public Action onPlaybackFinished;

        public CrossfadableAnimation(AnimationClip clip)
        {
            this.clip = clip;
        }
    }
}

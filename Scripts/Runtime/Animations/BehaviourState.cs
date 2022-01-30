using DoubTech.AnimalAI.Agent;
using UnityEngine;
using UnityEngine.Animations;

namespace DoubTech.AnimalAI.Animations
{
    public class BehaviourState : StateMachineBehaviour
    {
        [SerializeField] private string tag;
        private AnimationStateController stateController;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);
            OnStartState(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);
            OnEndState(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnEndState(animator);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            OnStartState(animator);
        }

        private void OnStartState(Animator animator)
        {
            if (!stateController)
            {
                stateController = animator.GetComponent<AnimationStateController>();
            }

            if (stateController)
            {
                stateController.OnStartedState(tag);
            }
        }

        private void OnEndState(Animator animator)
        {
            if (!stateController)
            {
                stateController = animator.GetComponent<AnimationStateController>();
            }

            if (stateController)
            {
                stateController.OnStoppedState(tag);
            }
        }
    }
}
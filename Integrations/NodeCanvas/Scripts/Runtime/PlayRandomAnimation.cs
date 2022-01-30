using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Animations;
using DoubTech.AnimalAI.Utilities;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.NodeCanvas
{
    [Category("DoubTech/Animal AI/Animations")]
    [Description("Makes the agent wander randomly within the navigation map")]
    public class PlayRandomAnimation : ActionTask<IAgentState>
    {
        public BBParameter<AnimationClip[]> actions;
        public BBParameter<int> maxRepetitions = 1;
        private bool playbackComplete;

        protected override void OnExecute()
        {
            base.OnExecute();

            playbackComplete = false;
            int repetitions = Random.Range(1, maxRepetitions.value + 1);
            for (int i = 0; i < repetitions; i++)
            {
                var anim = new CrossfadableAnimation(actions.value[Random.Range(0, actions.value.Length)]);
                if (i + 1 == repetitions)
                {
                    anim.onPlaybackFinished += () => playbackComplete = true;
                }
                agent.AnimationController.QueueAnimation(anim);
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if(playbackComplete) EndAction(true);
        }
    }
}

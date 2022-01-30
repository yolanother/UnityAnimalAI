using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Utilities;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.NodeCanvas
{
    [Category("DoubTech/Animal AI/Movement/Pathfinding")]
    [Description("Makes the agent wander randomly within the navigation map")]
    public class Wander : ActionTask<IAgentState>
    {
        public BBParameter<bool> run = false;
        public BBParameter<float> radius = 20;
        public BBParameter<float> wanderFailureTimeout = 5;

        private bool pathPending;
        private bool hasValidDestination;

        private Vector3 lastPosition;
        private float lastPositionChangeTime;
        private Terrain terrain;

        protected override void OnExecute()
        {
            base.OnExecute();
            pathPending = true;
            hasValidDestination = false;
            lastPosition = agent.Transform.position;
            lastPositionChangeTime = Time.realtimeSinceStartup;

            if(TerrainUtilities.GetRandomPositionOnTerrain(agent.Transform.position, radius.value, out var position, out terrain))
            {
                position.y = terrain.SampleHeight(position) + terrain.transform.position.y;
                agent.CheckForValidDestination(position, OnFoundPosition);
            }
        }

        private void OnFoundPosition(Vector3 position, bool found)
        {
            if (found)
            {
                agent.Destination = position;
                hasValidDestination = found;
                pathPending = false;
                lastPosition = agent.Transform.position;
                lastPositionChangeTime = Time.realtimeSinceStartup;
            }
            else
            {
                EndAction(false);
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!terrain || !terrain.gameObject.activeInHierarchy || !terrain.enabled)
            {
                EndAction(false);
            }

            if (!pathPending)
            {
                if (hasValidDestination && agent.ReachedDestination) 
                {
                    EndAction();
                    return;
                }
                else if(!hasValidDestination)
                {
                    EndAction(false);
                    return;
                }
            }

            var distance = Vector3.Distance(lastPosition, agent.Transform.position);
            if (distance < .01f && Time.realtimeSinceStartup - lastPositionChangeTime > wanderFailureTimeout.value)
            {
                EndAction(false);
                return;
            }

            if (distance > .01f)
            {
                lastPosition = agent.Transform.position;
                lastPositionChangeTime = Time.realtimeSinceStartup;
            }
        }
    }
}

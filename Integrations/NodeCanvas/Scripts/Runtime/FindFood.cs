using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Consumables;
using DoubTech.AnimalAI.Utilities;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.NodeCanvas
{
    [Category("DoubTech/Animal AI/Movement/Pathfinding")]
    [Description("Makes the agent wander randomly within the navigation map")]
    public class FindFood : ActionTask<IAgentState>
    {
        public BBParameter<string> tag;
        public BBParameter<LayerMask> layerMask;
        public BBParameter<bool> run = false;
        public BBParameter<float> radius = 20;
        public BBParameter<float> wanderFailureTimeout = 5;
        public BBParameter<float> stoppingDistanceToFood = 2;

        private bool pathPending;
        private bool hasValidDestination;

        private Vector3 lastPosition;
        private float lastPositionChangeTime;

        protected override void OnExecute()
        {
            base.OnExecute();


            pathPending = true;
            hasValidDestination = false;
            
            var hits = Physics.SphereCastAll(agent.Transform.position, radius.value, Vector3.forward, 0, layerMask.value);
            
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (string.IsNullOrEmpty(tag.value) || hit.collider.gameObject.CompareTag(tag.value))
                {
                    var consumable = hit.collider.GetComponent<IConsumable>();
                    if (null != consumable && !consumable.IsConsumed)
                    {
                        Debug.Log($"Found food {hit.collider.name} at {hit.collider.transform.position}");
                        agent.CheckForValidDestination(hit.collider.transform.position, OnFoundPosition);
                        return;
                    }
                }
            }
            
            EndAction(false);
        }

        private void OnFoundPosition(Vector3 position, bool found)
        {
            if (found)
            {
                agent.Destination = position;
                hasValidDestination = found;
                pathPending = false;
            }
            else
            {
                EndAction(false);
            }
        }

        protected override void OnUpdate()
        {
            if (!hasValidDestination) return;
            base.OnUpdate();
            
            if (agent.ReachedDestination || Vector3.Distance(agent.Transform.position, agent.Destination) < stoppingDistanceToFood.value)
            {
                EndAction(true);
            }
            else
            {
                var distance = Vector3.Distance(lastPosition, agent.Transform.position);
                if (distance < .01f &&
                    Time.realtimeSinceStartup - lastPositionChangeTime > wanderFailureTimeout.value)
                {
                    EndAction(false);
                }

                if (distance > .01f)
                {
                    lastPosition = agent.Transform.position;
                    lastPositionChangeTime = Time.realtimeSinceStartup;
                }
            }
        }

        public static Terrain GetClosestCurrentTerrain(Vector3 agentPosition)
        {
            //Get all terrain
            Terrain[] terrains = Terrain.activeTerrains;

            //Make sure that terrains length is ok
            if (terrains.Length == 0) return null;

            //If just one, return that one terrain
            if (terrains.Length == 1) return terrains[0];

            //Get the closest one to the player
            float lowDist = (terrains[0].GetPosition() - agentPosition).sqrMagnitude;
            var terrainIndex = 0;

            for (int i = 1; i < terrains.Length && terrains[i].enabled && terrains[i].gameObject.activeInHierarchy; i++)
            {
                Terrain terrain = terrains[i];
                Vector3 terrainPos = terrain.GetPosition();

                //Find the distance and check if it is lower than the last one then store it
                var dist = (terrainPos - agentPosition).sqrMagnitude;
                if (dist < lowDist)
                {
                    lowDist = dist;
                    terrainIndex = i;
                }
            }
            return terrains[terrainIndex];
        }
    }
}

using UnityEngine;

namespace DoubTech.AnimalAI.Utilities
{
    public class TerrainUtilities
    {
        public static bool GetRandomPositionOnTerrain(Vector3 center, float radius, out Vector3 position)
        {
            return GetRandomPositionOnTerrain(center, radius, out position, out var terrain);
        }

        public static bool GetRandomPositionOnTerrain(Vector3 center, float radius, out Vector3 position, out Terrain terrain)
        {
            var circlePos = Random.insideUnitCircle * radius;
            position = center;
            position.x += circlePos.x;
            position.z += circlePos.y;
            terrain = GetClosestCurrentTerrain(position);
            var validator = terrain.GetComponent<ITerrainValidator>();
            if (null != validator && validator.IsValid || null == validator && terrain)
            {
                position.y = terrain.SampleHeight(position) + terrain.transform.position.y;
                return true;
            }

            return false;
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

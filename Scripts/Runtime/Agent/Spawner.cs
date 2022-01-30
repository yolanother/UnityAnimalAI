using System;
using System.Collections.Generic;
using DoubTech.AnimalAI.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.AnimalAI.Agent
{
    public class Spawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] public GameObject[] prefabs;

        [Header("Spawner")]
        [SerializeField] public Vector2Int instanceCountRange;
        [SerializeField] public float spawnRadius;

        [Header("Spawner Player Detection")]
        [SerializeField] private Transform player;
        [SerializeField] private float spawnDistance = 500;
        [SerializeField] private float updateSeconds = 4;

        [Header("Gizmos")]
        [SerializeField] private bool showGizmos;
        [SerializeField] public Color spawnRadiusColor = Color.blue;
        [SerializeField] public Color spawnDistanceColor = Color.grey;

        private Camera mainCamera;

        private List<GameObject> instances = new List<GameObject>();
        private int instanceCount;

        private float lastUpdate;

        private void Update()
        {
            if (!player) return;
            
            if (Time.time - lastUpdate > updateSeconds)
            {
                lastUpdate = Time.time;
                if (Vector3.Distance(player.position, transform.position) < spawnDistance)
                {
                    if (instances.Count < instanceCount)
                    {
                        if (TerrainUtilities.GetRandomPosition(transform.position, spawnRadius,
                                out var position))
                        {
                            var rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            instances.Add(Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, rotation));
                        }
                    }
                }

                for (int i = 0; i < instances.Count; i++)
                {
                    instances[i].SetActive(Vector3.Distance(instances[i].transform.position, player.position) < spawnDistance);
                }
            }
        }

        private void OnEnable()
        {
            instanceCount = Random.Range(instanceCountRange.x, instanceCountRange.y + 1);

            if (!player) player = GameObject.FindWithTag("Player")?.transform;
            if (!player) player = Camera.current.transform;
        }

        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                Gizmos.color = spawnRadiusColor;
                Gizmos.DrawWireSphere(transform.position, spawnRadius);
                Gizmos.color = spawnDistanceColor;
                Gizmos.DrawWireSphere(transform.position, spawnDistance);
            }
        }
    }
}

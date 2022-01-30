using System;
using Pathfinding;
using UnityEngine;

namespace DoubTech.AnimalAI.Integrations.AStarPro
{
    public class PathfinderRecenter : MonoBehaviour
    {
        [SerializeField] private AstarPath pathfinder;
        [SerializeField] private Transform trackedObject;
        [SerializeField] private float rescanRange = 250;

        private Vector3 lastCenter;

        private void Awake()
        {
            Rescan();
        }

        private void OnEnable()
        {
            Rescan();
        }

        private void Update()
        {
            if (Vector3.Distance(lastCenter, trackedObject.position) > rescanRange)
            {
                Rescan();
            }
        }

        private void Rescan()
        {
            lastCenter = trackedObject.position;
            for (int i = 0; i < pathfinder.graphs.Length; i++)
            {
                if (pathfinder.graphs[i] is RecastGraph recast)
                {
                    recast.forcedBoundsCenter = trackedObject.position;
                }

                if (pathfinder.graphs[i] is GridGraph grid)
                {
                    grid.center = trackedObject.position;
                }

                AstarPath.active.Scan();
            }
        }
    }
}

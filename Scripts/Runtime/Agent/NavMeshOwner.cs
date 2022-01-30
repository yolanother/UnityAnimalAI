using UnityEngine;

namespace DoubTech.AnimalAI.Agent
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    public class NavMeshOwner : MonoBehaviour
    {
        public GameObject[] colliders = new GameObject[4];

        //default make it public so it can be changed in map magic
        public LayerMask navMeshLayers = 1;

        //[SerializeField] Vector3 upDirection = Vector3.up;
        public bool IsNavMeshReady = false;
        NavMeshDataInstance navMeshDataInstance;

        private void Awake()
        {
            //stop the player from entering a chunk that's not ready!
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = new GameObject();
                colliders[i].transform.parent = transform;
                colliders[i].AddComponent<BoxCollider>();
            }

            //colliders[0].name = "North";
            colliders[0].GetComponent<BoxCollider>().size = new Vector3(10, 10000, 1000);
            colliders[0].transform.localPosition = new Vector3(1000, transform.localPosition.y, 500);
            //colliders[1].name = "East";
            colliders[1].GetComponent<BoxCollider>().size = new Vector3(1000, 10000, 10);
            colliders[1].transform.localPosition = new Vector3(500, transform.localPosition.y, 1000);
            //colliders[2].name = "South";
            colliders[2].GetComponent<BoxCollider>().size = new Vector3(10, 10000, 1000);
            colliders[2].transform.localPosition = new Vector3(1000, transform.localPosition.y, 0);
            //colliders[3].name = "West";
            colliders[3].GetComponent<BoxCollider>().size = new Vector3(1000, 10000, 10);
            colliders[3].transform.localPosition = new Vector3(500, transform.localPosition.y, 0);
        }

        void OnEnable()
        {
            IsNavMeshReady = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].SetActive(true);
            }

            // Create new NavMesh whenever the script is enabled 
            //Task.Factory.StartNew(CreateNavMesh);
            CreateNavMesh();
        }

        void OnDisable()
        {
            // Delete current NavMesh whenever the script is disabled
            RemoveNavMesh();
        }

        public AsyncOperation ao;

        public void CreateNavMesh()
        {
            List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();

            NavMeshBuilder.CollectSources(transform, navMeshLayers, NavMeshCollectGeometry.RenderMeshes, 0,
                new List<NavMeshBuildMarkup>(), buildSources);

            NavMeshData navData = new NavMeshData();
            ao = NavMeshBuilder.UpdateNavMeshDataAsync(navData, NavMesh.GetSettingsByID(0), buildSources,
                new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000)));
            ao.completed += completed;
            navMeshDataInstance = NavMesh.AddNavMeshData(navData);
        }

        private void completed(AsyncOperation obj)
        {
            IsNavMeshReady = true;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i])
                    colliders[i].SetActive(false);
            }
        }

        public void RemoveNavMesh()
        {
            navMeshDataInstance.Remove();
        }
    }

}

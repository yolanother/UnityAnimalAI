using System;
using DoubTech.AnimalAI.Agent;
using DoubTech.AnimalAI.Animations;
using Pathfinding;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DefaultCompany.Com.Doubtech.Unity.Animalai.Integrations.AStarPro
{
    [RequireComponent(typeof(Seeker))]
    public class AgentState : MonoBehaviour, IAgentState
    {
        [Header("Agent Setup")] [SerializeField]
        private float runSpeed = 4;

        [SerializeField] private float walkSpeed = 2;
        [SerializeField] private bool run;

        private IAstarAI agent;
        private Seeker seeker;

        private void OnValidate()
        {
            if (null == agent) agent = GetComponent<IAstarAI>();
            if (null == seeker) seeker = GetComponent<Seeker>();
            Run = run;
        }

        private void Awake()
        {
            agent = GetComponent<IAstarAI>();
            seeker = GetComponent<Seeker>();
            Run = run;
        }

        private void OnEnable()
        {
            initialized = false;
        }

        private void Update()
        {
            if (!initialized)
            {
                initialized = true;
                agent.Teleport(transform.position, true);
            }
        }

        public Transform Transform => transform;

        public bool Run
        {
            get => run;
            set
            {
                run = value;
                agent.maxSpeed = run ? runSpeed : walkSpeed;
            }
        }

        public AnimationStateController AnimationController { get; set; } = null;
        public float RunSpeed => runSpeed;
        public float WalkSpeed => walkSpeed;

        private bool stop;
        public bool Stop
        {
            get => stop;
            set
            {
                stop = value;
                agent.isStopped = value;
            }
        }

        private Vector3 destination;
        private bool initialized;

        public Vector3 Destination
        {
            get => destination;
            set
            {
                if (destination != value)
                {
                    if (!initialized)
                    {
                        Debug.LogError("Not yet initialized.");
                        return;
                    }

                    if (stop) return;
                    destination = value;
                    agent.destination = value;
                }
            }
        }

        public bool ReachedDestination
        {
            get => agent.reachedDestination;
        }

        public float DistanceRemaining
        {
            get => agent.remainingDistance;
        }

        public void CheckForValidDestination(Vector3 destination, Action<Vector3, bool> onDestinationCalculated)
        {
            seeker.StartPath(transform.position, destination,
                (path) => onDestinationCalculated.Invoke(destination, !path.error));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(agent.destination, .25f);
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof (AgentState))]
    public class AgentStateEditor : Editor
    {
        [SerializeField] private Transform targetTransform;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                var agent = target as AgentState;
                GUILayout.Space(32);
                GUILayout.Label("Debugging", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                targetTransform =
                    (Transform) EditorGUILayout.ObjectField("Target Transform", targetTransform, typeof(Transform));
                if (targetTransform && targetTransform.position != agent.Destination && GUILayout.Button("Move"))
                {
                    agent.Destination = targetTransform.position;
                }
                GUILayout.EndHorizontal();
                
                agent.Destination = EditorGUILayout.Vector3Field("Target", agent.Destination);
                GUILayout.Label("Distance remaining: " + agent.DistanceRemaining);
            }
        }
    }
    #endif
}

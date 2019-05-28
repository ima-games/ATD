using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Follow the leader using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=14")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}LeaderFollowIcon.png")]
    public class LeaderFollow : Action
    {
        [Tooltip("The speed of the agents")]
        public SharedFloat speed;
        [Tooltip("Angular speed of the agents")]
        public SharedFloat angularSpeed;
        [Tooltip("Agents less than this distance apart are neighbors")]
        public SharedFloat neighborDistance = 10;
        [Tooltip("How far behind the leader the agents should follow the leader")]
        public SharedFloat leaderBehindDistance = 2;
        [Tooltip("The distance that the agents should be separated")]
        public SharedFloat separationDistance = 2;
        [Tooltip("The agent is getting too close to the front of the leader if they are within the aheadDistance")]
        public SharedFloat aheadDistance = 2;
        [Tooltip("The leader to follow")]
        public UnityEngine.AI.NavMeshAgent leader = null;
        [Tooltip("All of the agents that should be following")]
        public UnityEngine.AI.NavMeshAgent[] agents = null;

        // The transform of the leader
        private Transform leaderTransform;
        // The corresponding transforms of the agents
        private Transform[] agentTransforms;

        public override void OnAwake()
        {
            leaderTransform = leader.transform;
            agentTransforms = new Transform[agents.Length];
            // Cache the transform of the agents
            for (int i = 0; i < agents.Length; ++i) {
                agentTransforms[i] = agents[i].transform;
            }
        }

        public override void OnStart()
        {
            // Enable the nav mesh
            for (int i = 0; i < agents.Length; ++i) {
                agents[i].enabled = true;
                agents[i].speed = speed.Value;
                agents[i].angularSpeed = angularSpeed.Value;
            }
        }

        // The agents will always be following the leader so always return running
        public override TaskStatus OnUpdate()
        {
            var behindPosition = LeaderBehindPosition();
            // Determine a destination for each agent
            for (int i = 0; i < agents.Length; ++i) {
                // Get out of the way of the leader if the leader is currently looking at the agent and is getting close
                if (LeaderLookingAtAgent(i) && Vector3.SqrMagnitude(leaderTransform.position - agentTransforms[i].position) < aheadDistance.Value) {
                    agents[i].destination = transform.position + (transform.position - leaderTransform.position).normalized * aheadDistance.Value;
                } else {
                    // The destination is the behind position added to the separation vector
                    agents[i].destination = behindPosition + DetermineSeparation(i);
                }
            }
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            for (int i = 0; i < agents.Length; ++i) {
                if (agents[i] != null)
                    agents[i].enabled = false;
            }
        }

        private Vector3 LeaderBehindPosition()
        {
            // The behind position is the normalized inverse of the leader's velocity multiplied by the leaderBehindDistance
            return leaderTransform.position + (-leader.velocity).normalized * leaderBehindDistance.Value;
        }

        // Determine the separation between the current agent and all of the other agents also following the leader
        private Vector3 DetermineSeparation(int agentIndex)
        {
            var separation = Vector3.zero;
            int neighborCount = 0;
            var agentTransform = agentTransforms[agentIndex];
            // Loop through each agent to determine the separation
            for (int i = 0; i < agents.Length; ++i) {
                // The agent can't compare against itself
                if (agentIndex != i) {
                    // Only determine the parameters if the other agent is its neighbor
                    if (Vector3.SqrMagnitude(agentTransforms[i].position - agentTransform.position) < neighborDistance.Value) {
                        // This agent is the neighbor of the original agent so add the separation
                        separation += agentTransforms[i].position - agentTransform.position;
                        neighborCount++;
                    }
                }
            }

            // Don't move if there are no neighbors
            if (neighborCount == 0) {
                return Vector3.zero;
            }
            // Normalize the value
            return ((separation / neighborCount) * -1).normalized * separationDistance.Value;
        }

        // Use the dot product to determine if the leader is looking at the current agent
        public bool LeaderLookingAtAgent(int agentIndex)
        {
            return Vector3.Dot(leaderTransform.forward, agentTransforms[agentIndex].forward) < -0.5f;
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 10;
            angularSpeed = 10;
            neighborDistance = 10;
            leaderBehindDistance = 2;
            separationDistance = 2;
            aheadDistance = 2;
            leader = null;
            agents = null;
        }
    }
}
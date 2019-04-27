#if UNITY_2017_1 || UNITY_2017_2 || UNITY_2017_3 || UNITY_2017_4 || UNITY_2018_1 || UNITY_2018_2
using UnityEngine.Networking;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNetwork
{
    public class IsClient : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return NetworkClient.active ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
#endif
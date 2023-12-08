using UnityEngine;

namespace Tiger.Audio
{
    public class FollowTransform : MonoBehaviour
    {
        public Transform target;
        private void LateUpdate()
        {
            if (target) transform.position = target.position;
        }
    }
}

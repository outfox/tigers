using UnityEngine;

namespace Tiger.Util.Editor
{
    public class DrawScaleGizmo : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color32(0, 0, 128, 128);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}

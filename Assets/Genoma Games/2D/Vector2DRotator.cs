using UnityEngine;

namespace GenomaGames
{
    public class Vector2DRotator : MonoBehaviour
    {
        [SerializeField]
        private Vector2 vector;
        [SerializeField]
        private float rotation;

        private void OnDrawGizmos()
        {
            Vector2 from = transform.position;
            Vector2 to = from + vector;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(from, to);

            Vector2 toRotated = from + (Vector2)(Quaternion.AngleAxis(rotation, Vector3.forward) * vector);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, toRotated);
        }
    }
}
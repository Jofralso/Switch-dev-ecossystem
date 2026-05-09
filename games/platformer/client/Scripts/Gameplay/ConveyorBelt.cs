using UnityEngine;

namespace Game.Gameplay
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("Settings")]
        public Vector2 direction = Vector2.right;
        public float speed = 3f;
        public bool active = true;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!active) return;

            var rb = collision.rigidbody;
            if (rb == null) return;

            rb.velocity += direction * (speed * Time.fixedDeltaTime * 10f);
        }

        public void Activate() => active = true;
        public void Deactivate() => active = false;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            var box = GetComponent<Collider2D>()?.bounds;
            if (box.HasValue)
            {
                Vector3 center = box.Value.center;
                Vector3 end = center + (Vector3)direction.normalized * 2f;
                Gizmos.DrawLine(center, end);
                Gizmos.DrawSphere(end, 0.1f);
            }
        }
    }
}

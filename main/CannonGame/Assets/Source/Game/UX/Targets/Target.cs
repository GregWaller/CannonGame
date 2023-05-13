/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    using LRG.Master;

    public enum TargetType
    {
        Unassigned,
        SmallShip,
    }

    [RequireComponent(typeof(Collider))]
    public class Target : PooledObject<TargetType>
    {
        [SerializeField] private TargetType _targetType = TargetType.Unassigned;

        private Collider _collider = null;

        public override TargetType Key => _targetType;

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawSphere(transform.position, 0.05f);
        }

#endif
        public override void Init()
        {
            _collider = GetComponent<Collider>();
        }

        public override void Activate(bool active)
        {
            
        }

        public override void Initialize(Vector3 worldPosition)
        {
            
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile))
            {
                projectile.Despawn();
                gameObject.SetActive(false);
            }
        }
    }
}

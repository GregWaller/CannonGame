/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    using LRG.Master;

    public enum ProjectileType
    {
        Unassigned,
        Cannonball,
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : PooledObject<ProjectileType>
    {
        [SerializeField] private ProjectileType _projectileType = ProjectileType.Unassigned;
        [SerializeField] private float _timeToLive = 2.0f;
        [SerializeField] private float _despawnPlane = -1.0f;

        private Rigidbody _rigidbody = null;
        private float _aliveDuration = 0.0f;

        public override ProjectileType Key => _projectileType;
        public Cannon Source { get; set; } = null;

        public void Update()
        {
            if (!_active) return;

            _aliveDuration += Time.deltaTime;
            if (_aliveDuration >= _timeToLive || transform.position.y <= _despawnPlane)
                Despawn();
        }

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
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Reinitialize()
        {
            _aliveDuration = 0.0f;
            Source = null;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public override void Activate(bool active)
        {
            base.Activate(active);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void AddForce(Vector3 direction, float magnitude)
        {
            _rigidbody.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
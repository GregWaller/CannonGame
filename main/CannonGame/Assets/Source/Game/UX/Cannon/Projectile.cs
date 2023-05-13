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
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileType _projectileType = ProjectileType.Unassigned;
        [SerializeField] private float _timeToLive = 2.0f;

        private Rigidbody _rigidbody = null;
        private IObjectPool<Projectile> _pool = null;
        private float _aliveDuration = 0.0f;
        private bool _active = false;

        public ProjectileType ProjectileType => _projectileType;

        public void Update()
        {
            if (!_active) return;

            _aliveDuration += Time.deltaTime;
            if (_aliveDuration >= _timeToLive)
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

        public void Initialize(Vector3 position)
        {
            transform.position = position;
            _aliveDuration = 0.0f;
        }
        
        public void Activate(bool active)
        {
            _active = active;
            gameObject.SetActive(active);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void Despawn()
        {
            _pool.Reclaim(this);
        }

        public void RegisterPool(IObjectPool<Projectile> projectilePool)
        {
            _pool = projectilePool;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void AddForce(Vector3 direction, float magnitude)
        {
            _rigidbody.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
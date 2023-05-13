/* © 2023 - Greg Waller.  All rights reserved. */

using LRG.Master;

using UnityEngine;

namespace LRG.Game
{
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

        private Rigidbody _rb = null;
        private IObjectPool<Projectile> _pool = null;
        private float _aliveDuration = 0.0f;
        private bool _active = false;

        public ProjectileType ProjectileType => _projectileType;

        public void Update()
        {
            if (!_active) return;

            _aliveDuration += Time.deltaTime;
            if (_aliveDuration >= _timeToLive)
                _pool.Reclaim(this);
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
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        public void RegisterPool(IObjectPool<Projectile> projectilePool)
        {
            _pool = projectilePool;
            _rb = GetComponent<Rigidbody>();
        }
        
        public void AddForce(Vector3 direction, float magnitude)
        {
            _rb.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
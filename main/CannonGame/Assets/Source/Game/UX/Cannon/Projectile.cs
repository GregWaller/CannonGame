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
        public int Damage { get; set; } = 0;

        public void Update()
        {
            if (!_active) return;

            _aliveDuration += Time.deltaTime;

            if (transform.position.y <= _despawnPlane)
            {
                PooledEffect splash = VisualEffectFactory.Instance.Spawn(EffectType.Water_Splash);
                splash.SetPosition(transform.position);
                Despawn();

                return;
            }

            if (_aliveDuration >= _timeToLive)
            {
                Despawn();
                return;
            }
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawSphere(transform.position, 0.05f);
        }

#endif

        public override void Despawn()
        {
            base.Despawn();
            Source = null;
        }

        public override void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Activate(bool active)
        {
            base.Activate(active);
            gameObject.SetActive(active);
        }

        public override void Reinitialize()
        {
            base.Reinitialize();
            _aliveDuration = 0.0f;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public void AddForce(Vector3 direction, float magnitude)
        {
            _rigidbody.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
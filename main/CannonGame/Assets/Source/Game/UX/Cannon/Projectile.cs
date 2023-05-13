﻿/* © 2023 - Greg Waller.  All rights reserved. */

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
        private bool _active = false;

        public override ProjectileType Key => _projectileType;

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

        public override void Initialize(Vector3 position)
        {
            transform.position = position;
            _aliveDuration = 0.0f;
        }
        
        public override void Activate(bool active)
        {
            _active = active;
            gameObject.SetActive(active);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void AddForce(Vector3 direction, float magnitude)
        {
            _rigidbody.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
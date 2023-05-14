/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using System.Collections.Generic;

namespace LRG.Game
{
    using LRG.Master;

    [RequireComponent(typeof(Collider))]
    public class PlayerShip : MonoBehaviour
    {
        [Header("UX")]
        [SerializeField] private int _baseHull = 10;
        [SerializeField] private Cannon _cannon = null;
        [SerializeField] private int _damagePerRound = 1;
        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private int _initialAmmo = 100;

        [Header("Debugging")]
        [SerializeField] private Transform _targetPosition = null;
        [SerializeField] private float _rawAccuracy = 1.0f;
        [SerializeField] private bool _showAccuracyIndicator = false;

        private Collider _collider = null;
        private int _currentHull = 0;
        private int _currentGold = 0;
        private int _currentAmmo = 0;

        public Transform Target => _targetPosition;
        public int Hull => _currentHull;
        public int BaseHull => _baseHull;
        public int Gold => _currentGold;
        public int Ammo => _currentAmmo;

        public void Awake()
        {
            _collider = GetComponent<Collider>();
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (_targetPosition == null || !_showAccuracyIndicator) return;

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(_targetPosition.position, _rawAccuracy);
        }

#endif

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile) && projectile.Source != _cannon)
            {
                projectile.Despawn();
                _currentHull -= projectile.Damage;
            }
        }

        public void Reinitialize()
        {
            _currentHull = _baseHull;
            _currentGold = _initialGold;
            _currentAmmo = _initialAmmo;
        }

        public void Aim(Vector2 inputVector)
        {
            _cannon.Aim(inputVector);
        }

        public void Fire()
        {
            if (_currentAmmo > 0)
            {
                _cannon.Fire(_damagePerRound);
                _consume_ammo(1);
            }
        }

        public void Repair()
        {
            if (_currentHull < _baseHull)
            {
                _currentGold -= 100;
                _currentHull += 1;
            }
        }

        public void GainGold(int amount)
        {
            _currentGold += amount;
        }

        public void PurchaseAmmo()
        {
            _currentGold -= 100;
            _currentAmmo += 10;
        }

        private void _consume_ammo(int amount)
        {
            _currentAmmo -= amount;
        }
    }
}
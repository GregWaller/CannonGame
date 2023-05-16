/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using UnityEngine;

namespace LRG.Game
{
    [RequireComponent(typeof(Collider))]
    public class PlayerShip : MonoBehaviour
    {
        public event Action OnDestroyed = delegate { };
        public event Action<int> OnGoldGained = delegate { };
        public event Action OnDamaged = delegate { };

        [Header("Ship")]
        [SerializeField] private int _baseHull = 10;
        [SerializeField] private int _repairAmount = 1;
        [SerializeField] private Transform _targetPosition = null;

        [Header("Cannon")]
        [SerializeField] private Cannon _cannon = null;
        [SerializeField] private int _damagePerShot = 1;

        [Header("Ammo")]
        [SerializeField] private int _initialAmmo = 100;
        [SerializeField] private int _ammoPerPurchase = 10;

        [Header("Economy")]
        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private int _repairCost = 100;
        [SerializeField] private int _ammoCost = 100;

        [Header("Debugging")]
        [SerializeField] private float _rawAccuracy = 1.0f;
        [SerializeField] private bool _showAccuracyIndicator = false;

        private Collider _collider = null;
        private int _currentHull = 0;
        private int _currentGold = 0;
        private int _currentAmmo = 0;
        private bool _alive = false;

        public Transform Target => _targetPosition;
        public int Hull => _currentHull;
        public int BaseHull => _baseHull;
        public int Gold => _currentGold;
        public int Ammo => _currentAmmo;
        public Cannon Cannon => _cannon;
        public bool Alive => _alive;

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
                if (!_alive) return;

                _currentHull -= projectile.Damage;
                OnDamaged?.Invoke();

                if (_currentHull <= 0)
                {
                    _alive = false;
                    OnDestroyed?.Invoke();
                }
            }
        }

        public void Reinitialize()
        {
            _currentHull = _baseHull;
            _currentGold = _initialGold;
            _currentAmmo = _initialAmmo;
            _alive = true;
        }

        public void Aim(Vector2 inputVector)
        {
            _cannon.Aim(inputVector);
        }

        public void Fire()
        {
            if (_currentAmmo > 0)
            {
                _cannon.Fire(_damagePerShot);
                _consume_ammo(1);
            }
        }

        public void Repair()
        {
            if (_currentHull < _baseHull && _currentGold >= _repairCost)
            {
                _currentGold -= _repairCost;
                _currentHull += _repairAmount;
            }
        }

        public void GainGold(int amount)
        {
            _currentGold += amount;
            OnGoldGained?.Invoke(amount);
        }

        public void PurchaseAmmo()
        {
            if (_currentGold >= _ammoCost)
            {
                _currentGold -= _ammoCost;
                _currentAmmo += _ammoPerPurchase;
            }
        }

        public void Kill()
        {
            _alive = false;
        }

        private void _consume_ammo(int amount)
        {
            _currentAmmo -= amount;
        }
    }
}
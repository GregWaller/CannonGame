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

        [Header("Debugging")]
        [SerializeField] private Transform _targetPosition = null;
        [SerializeField] private float _rawAccuracy = 1.0f;
        [SerializeField] private bool _showAccuracyIndicator = false;

        private Collider _collider = null;
        private int _currentHull = 0;

        public Transform Target => _targetPosition;
        public int Hull => _currentHull;
        public Cannon Cannon => _cannon;

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

        public void Reinitialize()
        {
            _currentHull = _baseHull;
        }
    }
}
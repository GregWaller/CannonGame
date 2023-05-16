/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Collections.Generic;
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
    public abstract class Target : PooledObject<TargetType>
    {
        public event Action<Target> OnDestroyed = delegate { };

        private const float _WAYPOINT_THRESHOLD = 0.001f;
        protected abstract TargetType _targetType { get; }

        [Header("Movement")]
        [SerializeField] private float _minSpeed = 0.1f;
        [SerializeField] private float _maxSpeed = 0.5f;

        [Header("Sinking")]
        [SerializeField] private float _sinkDuration = 2.0f;
        [SerializeField] private float _sinkSpeedFactor = 0.1f;
        [SerializeField] private AnimationCurve _depthOverTime = new AnimationCurve();

        [Header("Enemy Data")]
        [SerializeField] private Cannon _cannon = null;
        [SerializeField] private int _roundsPerMinute = 60;
        [SerializeField] private float _accuracy = 1.0f;    // TODO: min accuracy is 1, max accuracy is 40
                                                            // this should be a percentage of that range, inverted.  
                                                            // for example, if a ship is 100% accurate, their resulting accuracy is 1, while a 50% accurate ship has an accuracy of 20

        [SerializeField] private int _damagePerRound = 1;
        [SerializeField] private int _goldValue = 100;

        private int _currentWaypointIDX = 0;
        private List<Vector3> _waypoints = null;
        private float _currentSpeed = 0.0f;
        private PlayerShip _playerShip = null;
        private float _firingDelay = 0.0f;
        private bool _sinking = false;
        private float _currentSinkTime = 0.0f;
        private Vector3 _destroyedPosition = Vector3.zero;
        private Vector3 _sinkLookPosition = Vector3.zero;

        public override TargetType Key => _targetType;
        public int GoldValue => _goldValue;
        private Vector3 _currentWaypoint => _waypoints[_currentWaypointIDX];
        private float _firingInterval => 60.0f / (float)_roundsPerMinute;

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawSphere(transform.position, 0.05f);
        }

#endif

        public void Update()
        {
            if (_sinking)
            {
                transform.LookAt(_sinkLookPosition);

                _currentSinkTime += Time.deltaTime;
                float t = _currentSinkTime / _sinkDuration;
                float sinkDepth = _depthOverTime.Evaluate(t);
                transform.position = new Vector3(transform.position.x, transform.position.y + (sinkDepth * _sinkSpeedFactor), transform.position.z);

                if (_currentSinkTime >= _sinkDuration)
                    _sink_complete();
                
                return;
            }

            // ---- Patrols
            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint, _currentSpeed);
            if (Vector3.Distance(transform.position, _currentWaypoint) < _WAYPOINT_THRESHOLD)
                _set_next_waypoint();

            // ---- Firing
            _firingDelay += Time.deltaTime;
            if (_firingDelay >= _firingInterval)
            {
                _firingDelay = 0.0f;
                _cannon.Fire(_damagePerRound);

                // TODO: select a new target, and track it, based on my accuracy
            }
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile) && projectile.Source != _cannon)
            {
                PooledEffect explosion = VisualEffectFactory.Instance.Spawn(EffectType.Target_Hit);
                explosion.SetPosition(projectile.transform.position);

                _sinking = true;
                _destroyedPosition = transform.position;
                _sinkLookPosition = _destroyedPosition + (transform.forward * 10.0f);
                _currentSinkTime = 0.0f;
                projectile.Despawn();
            }
        }

        public override void Init()
        {
            _waypoints = new List<Vector3>();
        }

        public override void Despawn()
        {
            _sinking = false;
            gameObject.SetActive(false);
            _waypoints.Clear(); 
            base.Despawn();
        }

        private void _sink_complete()
        {
            Despawn();
            OnDestroyed?.Invoke(this);
        }

        public void Patrol(List<Vector3> waypoints)
        {
            if (waypoints.Count < 2)
            {
                Debug.LogError("Error: Could not start target patrol -- Insufficient waypoints.");
                Despawn();
                return;
            }

            gameObject.SetActive(true);
            _waypoints.AddRange(waypoints);

            _currentWaypointIDX = 0;
            transform.position = _currentWaypoint;

            _currentSpeed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);

            _set_next_waypoint();
        }

        public void SetTarget(PlayerShip playerShip)
        {
            _playerShip = playerShip;
            _cannon.SetMode(TrackingMode.Automatic);
            _cannon.Track(_playerShip.Target);
        }

        private void _set_next_waypoint()
        {
            _currentWaypointIDX++;
            if (_currentWaypointIDX > _waypoints.Count - 1)
                _currentWaypointIDX = 0;

            transform.LookAt(_currentWaypoint);
        }
    }
}
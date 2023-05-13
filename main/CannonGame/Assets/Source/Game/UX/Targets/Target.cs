/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    using System.Collections.Generic;

    using LRG.Master;

    public enum TargetType
    {
        Unassigned,
        SmallShip,
    }

    // the type of ship determines
    //      its ability to fire on the player
    //      how much damage those shots deal
    //      how often it fires
    //      how fast it travels on the track
    // and that target will sail back and forth on that track until either the player or target is killed

    [RequireComponent(typeof(Collider))]
    public class Target : PooledObject<TargetType>
    {
        private const float _WAYPOINT_THRESHOLD = 0.001f;

        [SerializeField] private TargetType _targetType = TargetType.Unassigned;
        [SerializeField] private float _minSpeed = 0.1f;
        [SerializeField] private float _maxSpeed = 0.5f;

        private Collider _collider = null;
        private int _currentWaypointIDX = 0;
        private List<Vector3> _waypoints = null;
        private float _currentSpeed = 0.0f;

        public override TargetType Key => _targetType;
        public Vector3 _currentWaypoint => _waypoints[_currentWaypointIDX];

        public void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint, _currentSpeed);

            if (Vector3.Distance(transform.position, _currentWaypoint) < _WAYPOINT_THRESHOLD)
                _set_next_waypoint();
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawSphere(transform.position, 0.05f);
        }

#endif
        public override void Init()
        {
            _collider = GetComponent<Collider>();
            _waypoints = new List<Vector3>();
        }

        public override void Activate(bool active)
        {
            
        }

        public override void Reinitialize()
        {
            
        }

        public void Patrol(List<Vector3> waypoints)
        {
            if (waypoints.Count < 2)
            {
                Debug.LogError("Error: Could not start target patrol -- Insufficient waypoints.");
                return;
            }

            _waypoints.Clear();
            _waypoints.AddRange(waypoints);

            _currentWaypointIDX = 0;
            transform.position = _currentWaypoint;

            _currentSpeed = Random.Range(_minSpeed, _maxSpeed);

            _set_next_waypoint();

            gameObject.SetActive(true);
        }

        private void _set_next_waypoint()
        {
            _currentWaypointIDX++;
            if (_currentWaypointIDX > _waypoints.Count - 1)
                _currentWaypointIDX = 0;

            transform.LookAt(_currentWaypoint);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile))
            {
                projectile.Despawn();
                gameObject.SetActive(false);
                Despawn();
            }
        }
    }
}

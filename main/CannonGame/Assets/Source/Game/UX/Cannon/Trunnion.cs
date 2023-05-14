/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Collections.Generic;

using UnityEngine;

namespace LRG.Game
{
    public class Trunnion : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _barrel = null;
        [SerializeField] private GameObject _housing = null;

        [Header("Rotation")]
        [SerializeField] private float _maxRotationSpeed = 2.0f;
        [SerializeField] private float _rotationSmoothing = 0.01f;

        // ----- Tracking Modes
        private TrackingMode _trackingMode = TrackingMode.Manual;
        private Dictionary<TrackingMode, Action> _trackingCallbacks = null;

        // ----- Manual Tracking
        private Vector2 _inputVector = Vector2.zero;
        private float _currentYaw = 0.0f;
        private float _currentYawSpeed = 0.0f;
        private float _currentPitch = 0.0f;
        private float _currentPitchSpeed;
        
        // ----- Automatic Tracking
        private Transform _target = null;

        public Vector3 Trajectory => transform.forward;

        public void Awake()
        {
            _trackingCallbacks = new Dictionary<TrackingMode, Action>
            {
                { TrackingMode.Manual, _manual_tracking },
                { TrackingMode.Automatic, _auto_tracking }
            };
        }

        public void Update()
        {
            _trackingCallbacks[_trackingMode].Invoke();
        }

        public void SetMode(TrackingMode mode)
        {
            _trackingMode = mode;
        }

        #region Manual Targeting

        public void Aim(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        private void _manual_tracking()
        {
            // the trunnion's yaw is based on the inputVector x axis
            if (_inputVector.x == 0.0f)
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, 0.0f, _rotationSmoothing);
            else
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, _maxRotationSpeed, _rotationSmoothing);

            // the trunnion's pitch is based on the inputVector y axis
            if (_inputVector.y == 0.0f)
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, 0.0f, _rotationSmoothing);
            else
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, _maxRotationSpeed, _rotationSmoothing);

            _currentYaw += _inputVector.x * _currentYawSpeed;
            _currentPitch += _inputVector.y * _currentPitchSpeed;

            Quaternion targetRotation = Quaternion.Euler(_currentPitch, _currentYaw, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }

        #endregion

        #region Automatic Targeting

        public void Track(Transform target)
        {
            _target = target;
        }

        private void _auto_tracking()
        {
            transform.LookAt(_target.position, Vector3.up);
        }

        #endregion
    }
}

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

        [Header("Yaw")]
        [SerializeField] private float _maxYawSpeed = 2.0f;
        [SerializeField] private float _yawSmoothing = 0.01f;

        [Header("Pitch")]
        [SerializeField] private float _maxPitchSpeed = 2.0f;
        [SerializeField] private float _pitchSmoothing = 0.01f;

        // ----- Tracking Modes
        private TrackingMode _trackingMode = TrackingMode.Manual;

        // ----- Manual Tracking
        private Vector2 _inputVector = Vector2.zero;
        private float _currentYaw = 0.0f;
        private float _currentYawSpeed = 0.0f;
        private float _currentPitch = 0.0f;
        private float _currentPitchSpeed;
        
        // ----- Automatic Tracking
        private Transform _target = null;

        public Vector3 Trajectory => _barrel.transform.forward;

        public void Update()
        {
            if (_trackingMode == TrackingMode.Automatic)
                _auto_tracking();
        }

        public void LateUpdate()
        {
            if (_trackingMode == TrackingMode.Manual)
                _manual_tracking();
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
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, 0.0f, _yawSmoothing);
            else
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, _maxYawSpeed, _yawSmoothing);

            // the trunnion's pitch is based on the inputVector y axis
            if (_inputVector.y == 0.0f)
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, 0.0f, _pitchSmoothing);
            else
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, _maxPitchSpeed, _pitchSmoothing);

            _currentYaw += _inputVector.x * _currentYawSpeed;
            _currentPitch += _inputVector.y * _currentPitchSpeed;

            // rotate the entire trunnion based on the current yaw
            Quaternion targetBaseRotation = Quaternion.Euler(0.0f, _currentYaw, 0.0f); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetBaseRotation, Time.deltaTime);

            // then only rotate the barrel based on the current pitch
            Quaternion targetBarrelRotation = Quaternion.Euler(_currentPitch, 0.0f, 0.0f);
            _barrel.transform.localRotation = Quaternion.Lerp(_barrel.transform.localRotation, targetBarrelRotation, Time.deltaTime);
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

﻿/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    public class CannonGimbal : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _barrel = null;
        [SerializeField] private GameObject _housing = null;

        [Header("Yaw")]
        [SerializeField] private float _maxRotationSpeed = 2.0f;
        [SerializeField] private float _rotationSmoothing = 0.01f;

        private Vector2 _inputVector = Vector2.zero;
        private float _currentYaw = 0.0f;
        private float _currentYawSpeed = 0.0f;
        private float _currentPitch = 0.0f;
        private float _currentPitchSpeed;

        public void Update()
        {
            Rotate();
        }

        public void Aim(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        public void Rotate()
        {
            // the gimbal's yaw is based on the inputvector x axis
            if (_inputVector.x == 0.0f)
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, 0.0f, _rotationSmoothing);
            else
                _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, _maxRotationSpeed, _rotationSmoothing);
            
            _currentYaw += _inputVector.x * _currentYawSpeed;
            //transform.rotation = Quaternion.AngleAxis(_currentYaw, Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(_currentRotationAngle, Vector3.up), Time.deltaTime);

            // the gimbal's pitch is based on the inputvector y axis
            if (_inputVector.y == 0.0f)
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, 0.0f, _rotationSmoothing);
            else
                _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, _maxRotationSpeed, _rotationSmoothing);

            _currentPitch += _inputVector.y * _currentPitchSpeed;

            Quaternion targetRotation = Quaternion.Euler(_currentPitch, _currentYaw, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);

        }
    }
}
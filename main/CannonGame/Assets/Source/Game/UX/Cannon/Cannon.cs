/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{ 
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private CannonGimbal _gimbal = null;
        [SerializeField] private Transform _ballSpawn = null;

        public void Aim(Vector2 inputVector)
        {
            if (_gimbal == null) return;
            _gimbal.Aim(inputVector);
        }
    }
}
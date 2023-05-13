/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    using LRG.Master;

    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Trunnion _trunnion = null;
        [SerializeField] private Transform _ballSpawn = null;
        [SerializeField] private float _cannonForce = 50.0f;

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_ballSpawn.position, _ballSpawn.position + _trunnion.Trajectory);
            Gizmos.DrawSphere(_ballSpawn.position, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_ballSpawn.position + _trunnion.Trajectory, 0.05f);
        }

#endif

        public void Aim(Vector2 inputVector)
        {
            if (_trunnion == null) return;
            _trunnion.Aim(inputVector);
        }

        public void Fire()
        {
            try
            {
                ProjectileType projectileType = ProjectileType.Cannonball;
                Projectile projectile = ProjectileFactory.Instance.Spawn(projectileType);
                projectile.SetPosition(_ballSpawn.position);
                projectile.AddForce(_trunnion.Trajectory, _cannonForce);
            }
            catch (PooledObjectTypeNotFoundException typeNotFoundEx)
            {
                Debug.LogError(typeNotFoundEx.Message);
            }
        }
    }
}
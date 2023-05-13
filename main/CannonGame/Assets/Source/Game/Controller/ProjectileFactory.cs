/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    using LRG.Game;

    public interface IObjectPool<T>
    {
        void Reclaim(T obj);
    }

    public class ProjectileFactory : IObjectPool<Projectile>
    {
        private const string _PROJECTILE_RESOURCE_PATH = "Projectiles";

        #region Singleton

        private static ProjectileFactory _instance = null;
        public static ProjectileFactory Instance
        {
            get
            {
                _instance ??= new ProjectileFactory();
                return _instance;
            }
        }

        private ProjectileFactory() 
        {
            _pooledProjectiles = new Dictionary<ProjectileType, Queue<Projectile>>();
        }

        #endregion

        private Dictionary<ProjectileType, Projectile> _projectileTypeMap = null;
        private Dictionary<ProjectileType, Queue<Projectile>> _pooledProjectiles = null;

        public void Initialize()
        {
            _projectileTypeMap = new Dictionary<ProjectileType, Projectile>();
            Projectile[] projectiles = Resources.LoadAll<Projectile>(_PROJECTILE_RESOURCE_PATH);

            foreach(Projectile projectile in projectiles)
            {
                if (projectile.ProjectileType == ProjectileType.Unassigned)
                {
                    Debug.LogWarning($"A projectile prefab was discovered with an unassigned type (name = {projectile.name}).  It will be ignored.");
                    continue;
                }

                if (_projectileTypeMap.ContainsKey(projectile.ProjectileType))
                {
                    Debug.LogWarning($"A projectile of type {projectile.ProjectileType} has already been indexed (name = {_projectileTypeMap[projectile.ProjectileType].name}).  The projectile prefab named {projectile.name} will be ignored.");
                    continue;
                }

                _projectileTypeMap.Add(projectile.ProjectileType, projectile);
            }
        }

        public Projectile Spawn(ProjectileType requestedType, Vector3 worldPosition)
        {
            if (!_pooledProjectiles.ContainsKey(requestedType) && !_projectileTypeMap.ContainsKey(requestedType))
                throw new ProjectileTypeNotFoundException(nameof(requestedType), $"CRITICAL ERROR: The requested type of projectile ({requestedType}) has not been indexed or instantiated by this factory and cannot be spawned.");

            if (!_pooledProjectiles.ContainsKey(requestedType))
                _pooledProjectiles.Add(requestedType, new Queue<Projectile>());

            Projectile spawnedProjectile = _pooledProjectiles[requestedType].Count == 0
                ? _generate(requestedType)
                : _pooledProjectiles[requestedType].Dequeue();

            spawnedProjectile.Initialize(worldPosition);
            spawnedProjectile.Activate(true);

            return spawnedProjectile;
        }

        public void Reclaim(Projectile projectile)
        {
            _pooledProjectiles[projectile.ProjectileType].Enqueue(projectile);
            projectile.Activate(false);
        }

        private Projectile _generate(ProjectileType requestedType)
        {
            Projectile newProjectile = GameObject.Instantiate(_projectileTypeMap[requestedType]);
            newProjectile.RegisterPool(this);

            return newProjectile;
        }
    }
}
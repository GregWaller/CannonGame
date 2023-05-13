/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    using LRG.Game;

    public class ProjectileFactory : PooledObjectFactory<ProjectileType, Projectile>
    {
        protected override string _prefab_path => "Projectiles";

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
            : base() { }

        #endregion
    }
}
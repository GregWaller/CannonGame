/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace LRG.Master
{
    public interface IPooledObject<K>
    {
        event Action<IPooledObject<K>> OnDespawned;

        K Key { get; }

        void Init();
        void Initialize(Vector3 worldPosition);
        void Activate(bool active);
        void Despawn();
    }

    public abstract class PooledObject<K> : MonoBehaviour, IPooledObject<K>
    {
        public event Action<IPooledObject<K>> OnDespawned = delegate { };

        public abstract K Key { get; }

        public abstract void Init();
        public abstract void Initialize(Vector3 worldPosition);
        public abstract void Activate(bool active);

        private IPooledObjectFactory<K, IPooledObject<K>> _pool = null;

        public void Despawn()
        {
            OnDespawned?.Invoke(this);
        }
    }
}
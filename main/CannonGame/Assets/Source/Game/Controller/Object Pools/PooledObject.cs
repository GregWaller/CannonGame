/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using UnityEngine;

namespace LRG.Master
{
    public interface IPooledObject<K>
    {
        event Action<IPooledObject<K>> OnDespawned;

        K Key { get; }

        void Init();
        void Reinitialize();
        void Activate(bool active);
        void Despawn();
    }

    public abstract class PooledObject<K> : MonoBehaviour, IPooledObject<K>
    {
        public event Action<IPooledObject<K>> OnDespawned = delegate { };

        protected bool _active = false;

        public abstract K Key { get; }
        public bool Active => _active;

        public abstract void Init();
        public abstract void Reinitialize();

        public virtual void Activate(bool active)
        {
            _active = active;
        }

        public virtual void Despawn()
        {
            OnDespawned?.Invoke(this);
        }
    }
}
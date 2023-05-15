/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    public interface IPooledObjectFactory<K, T>
    {
        T Spawn(K key);
        void Reclaim(T managedObject);
        void DespawnAll();
    }

    public abstract class PooledObjectFactory<K, T> : IPooledObjectFactory<K, T> where T : PooledObject<K>
    {
        protected abstract string _prefab_path { get; }

        protected readonly Dictionary<K, Queue<T>> _pooledObjects = null;
        protected readonly Dictionary<K, List<T>> _activeObjects = null;
        protected readonly Dictionary<K, T> _objectPrefabMap = null;

        protected PooledObjectFactory()
        {
            _pooledObjects = new Dictionary<K, Queue<T>>();
            _activeObjects = new Dictionary<K, List<T>>();
            _objectPrefabMap = new Dictionary<K, T>();
        }

        public void Initialize()
        {
            T[] prefabs = Resources.LoadAll<T>(_prefab_path);

            foreach (T prefab in prefabs)
            {
                if (_objectPrefabMap.ContainsKey(prefab.Key))
                {
                    Debug.LogWarning($"An object of type {prefab.Key} has already been indexed (name = {_objectPrefabMap[prefab.Key].name}).  The object prefab named {prefab.name} will be ignored.");
                    continue;
                }

                _objectPrefabMap.Add(prefab.Key, prefab);
            }
        }

        public T Spawn(K key)
        {
            if (!_pooledObjects.ContainsKey(key) && !_objectPrefabMap.ContainsKey(key))
                throw new PooledObjectTypeNotFoundException(nameof(key), $"CRITICAL ERROR: The requested type of object ({key}) has not been indexed or instantiated by this factory and cannot be spawned.");

            if (!_pooledObjects.ContainsKey(key))
                _pooledObjects.Add(key, new Queue<T>());

            if (!_activeObjects.ContainsKey(key))
                _activeObjects.Add(key, new List<T>());

            T spawnedObject = _pooledObjects[key].Count == 0
                ? _generate(key)
                : _pooledObjects[key].Dequeue();

            spawnedObject.Reinitialize();
            spawnedObject.Activate(true);
            spawnedObject.OnDespawned += _object_despawned;
            _activeObjects[key].Add(spawnedObject);

            return spawnedObject;
        }

        private T _generate(K requestedType)
        {
            T generatedObj = GameObject.Instantiate(_objectPrefabMap[requestedType]);
            generatedObj.Init();
            return generatedObj;
        }

        public void Reclaim(T managedObject)
        {
            _pooledObjects[managedObject.Key].Enqueue(managedObject);
            _activeObjects[managedObject.Key].Remove(managedObject);
            managedObject.Activate(false);
        }

        public void DespawnAll()
        {
            List<T> toReclaim = new List<T>();

            foreach (K managedObjectType in _activeObjects.Keys)
                foreach (T managedObject in _activeObjects[managedObjectType])
                    toReclaim.Add(managedObject);

            foreach (T managedObject in toReclaim)
                Reclaim(managedObject);
        }

        private void _object_despawned(IPooledObject<K> obj)
        {
            obj.OnDespawned -= _object_despawned;
            Reclaim(obj as T);
        }
    }
}
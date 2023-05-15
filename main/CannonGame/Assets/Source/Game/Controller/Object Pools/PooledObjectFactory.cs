/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    public interface IPooledObjectFactory<K, T>
    {
        IEnumerator Initialize();
        T Spawn(K key);
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

        public IEnumerator Initialize()
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
                yield return null;
            }

            yield return null;
        }

        public T Spawn(K requestedType)
        {
            if (!_pooledObjects.ContainsKey(requestedType) && !_objectPrefabMap.ContainsKey(requestedType))
                throw new PooledObjectTypeNotFoundException(nameof(requestedType), $"CRITICAL ERROR: The requested type of object ({requestedType}) has not been indexed or instantiated by this factory and cannot be spawned.");

            if (!_pooledObjects.ContainsKey(requestedType))
                _pooledObjects.Add(requestedType, new Queue<T>());

            if (!_activeObjects.ContainsKey(requestedType))
                _activeObjects.Add(requestedType, new List<T>());

            T spawnedObject = _pooledObjects[requestedType].Count == 0
                ? _generate(requestedType)
                : _pooledObjects[requestedType].Dequeue();

            spawnedObject.Reinitialize();
            spawnedObject.Activate(true);
            spawnedObject.OnDespawned += _object_despawned;
            _activeObjects[requestedType].Add(spawnedObject);

            return spawnedObject;
        }

        public void DespawnAll()
        {
            List<T> toReclaim = new List<T>();

            foreach (K managedObjectType in _activeObjects.Keys)
                foreach (T managedObject in _activeObjects[managedObjectType])
                    toReclaim.Add(managedObject);

            foreach (T managedObject in toReclaim)
                _reclaim(managedObject);
        }

        private T _generate(K requestedType)
        {
            T generatedObj = GameObject.Instantiate(_objectPrefabMap[requestedType]);
            generatedObj.Init();
            return generatedObj;
        }

        private void _object_despawned(IPooledObject<K> managedObject)
        {
            managedObject.OnDespawned -= _object_despawned;
            _reclaim(managedObject as T);
        }

        private void _reclaim(T managedObject)
        {
            _pooledObjects[managedObject.Key].Enqueue(managedObject);
            _activeObjects[managedObject.Key].Remove(managedObject);
            managedObject.Activate(false);
        }
    }
}
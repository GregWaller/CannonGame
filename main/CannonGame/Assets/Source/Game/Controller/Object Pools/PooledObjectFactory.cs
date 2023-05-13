/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    public interface IPooledObjectFactory<K, T>
    {
        T Spawn(K key, Vector3 worldPosition);
        void Reclaim(T managedObject);
    }

    public abstract class PooledObjectFactory<K, T> : IPooledObjectFactory<K, T> where T : PooledObject<K>
    {
        protected abstract string _prefab_path { get; }

        protected readonly Dictionary<K, Queue<T>> _pooledObjects = null;
        protected readonly Dictionary<K, T> _objectPrefabMap = null;

        protected PooledObjectFactory()
        {
            _pooledObjects = new Dictionary<K, Queue<T>>();
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

        public T Spawn(K key, Vector3 worldPosition)
        {
            if (!_pooledObjects.ContainsKey(key) && !_objectPrefabMap.ContainsKey(key))
                throw new PooledObjectTypeNotFoundException(nameof(key), $"CRITICAL ERROR: The requested type of object ({key}) has not been indexed or instantiated by this factory and cannot be spawned.");

            if (!_pooledObjects.ContainsKey(key))
                _pooledObjects.Add(key, new Queue<T>());

            T spawnedObject = _pooledObjects[key].Count == 0
                ? _generate(key)
                : _pooledObjects[key].Dequeue();

            spawnedObject.Initialize(worldPosition);
            spawnedObject.Activate(true);
            spawnedObject.OnDespawned += _object_despawned;

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
            managedObject.Activate(false);
        }

        private void _object_despawned(IPooledObject<K> obj)
        {
            obj.OnDespawned -= _object_despawned;
            Reclaim(obj as T);
        }
    }
}
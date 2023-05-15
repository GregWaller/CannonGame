/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LRG.Game;

namespace LRG.Master
{
    [CreateAssetMenu(fileName = "GameLevel", menuName = "Long Road Games/GameLevel/Level", order = 0)]
    public class GameLevel : ScriptableObject
    {
        [SerializeField] private List<TargetType> _spawnableTypes = null;
        [SerializeField] private uint _spawnCount = 1;
        [SerializeField] private float _spawnDelay = 2.0f;
        [SerializeField] private int _completionAward = 1000;

        public float SpawnDelay => _spawnDelay;
        public int CompletionAward => _completionAward;

        public Queue<Target> GetTargets()
        {
            Queue<Target> targets = new Queue<Target>();

            try
            {
                for (int i = 0; i < _spawnCount; i++)
                {
                    TargetType targetType = _spawnableTypes.Random();
                    Target target = TargetFactory.Instance.Spawn(targetType);
                    target.gameObject.SetActive(false);
                    targets.Enqueue(target);
                }

                return targets;
            }
            catch (PooledObjectTypeNotFoundException typeNotFoundEx)
            {
                Debug.LogError(typeNotFoundEx.Message);
            }

            return targets;
        }
    }
}

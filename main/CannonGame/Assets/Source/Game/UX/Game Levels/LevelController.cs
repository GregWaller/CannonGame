/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LRG.Master
{
    using LRG.Data;
    using LRG.UI;
    using LRG.Game;
    using System;
    using System.Linq;

    public class LevelController : MonoBehaviour
    {
        [Header("Targets and AI")]
        [SerializeField] private PlayerShip _playerShip = null;
        [SerializeField] private List<TargetTrack> _targetTracks = null;

        [Header("Levels")]
        [SerializeField] private float _levelCountdown = 3.0f;
        [SerializeField] private List<GameLevel> _gameLevels = null;

        // ----- Game Data
        private uint _currentGold = 0;

        // ----- Level Data
        private int _currentLevel = 0;
        private bool _countdown = false;
        private float _currentCountdown = 0.0f;
        private int _targetCount = 0;
        private Coroutine _levelCoroutine = null;

        public PlayerShip Player => _playerShip;

        public void Update()
        {
            if (_countdown)
            {
                _currentCountdown -= Time.deltaTime;
                Debug.Log($"Countdown: {_currentCountdown} s");
                if (_currentCountdown <= 0.0f)
                {
                    _start_level(_currentLevel);
                    _countdown = false;
                }
            }
        }

        public void OnDestroy()
        {
            if (_levelCoroutine != null)
                StopCoroutine(_levelCoroutine);
        }

        public void Begin()
        {
            _currentGold = 0;

            _currentLevel = 0;
            _start_countdown();
        }

        private void _start_countdown()
        {
            _countdown = true;
            _currentCountdown = _levelCountdown;
        }

        private void _start_level(int currentLevelIDX)
        {
            GameLevel currentGameLevel = _gameLevels[currentLevelIDX];
            _levelCoroutine = StartCoroutine(_play_level(currentGameLevel));
        }

        private IEnumerator _play_level(GameLevel gameLevel)
        {
            Queue<Target> targets = gameLevel.GetTargets();
            _targetCount = targets.Count;

            while (_targetCount > 0)
            {
                if (targets.Count > 0)
                {
                    List<TargetTrack> availableTracks = _targetTracks.Where(t => t.Occupant == null).ToList();
                    if (availableTracks.Count > 0)
                    {
                        Target target = targets.Dequeue();
                        target.OnDespawned += _target_despawned;
                        target.SetTarget(_playerShip);

                        TargetTrack selectedTrack = availableTracks.Random();
                        selectedTrack.SetOccupant(target);
                    }
                }

                yield return new WaitForSeconds(gameLevel.SpawnDelay);
            }

            _next_level();
            yield return null;
        }

        private void _target_despawned(IPooledObject<TargetType> obj)
        {
            Target target = obj as Target;
            target.OnDespawned -= _target_despawned;

            TargetTrack occupiedTrack = _targetTracks.Where(t => t.Occupant == target).FirstOrDefault();
            occupiedTrack.SetOccupant(null);

            _targetCount--;
        }

        private void _next_level()
        {
            if (_currentLevel < _gameLevels.Count - 1) // we'll just repeat the last level in the list indefinitely
                _currentLevel++;  
            
            _start_countdown();
        }
    }
}
/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    using LRG.UI;
    using LRG.Game;

    public class LevelController : MonoBehaviour
    {
        public event Action OnGameExit = delegate { };

        private enum GameState
        {
            MainMenu,
            Play,
            GameOver,
        }

        [Header("Targets and AI")]
        [SerializeField] private PlayerShip _playerShip = null;
        [SerializeField] private List<TargetTrack> _targetTracks = null;

        [Header("Levels")]
        [SerializeField] private float _levelCountdown = 3.0f;
        [SerializeField] private List<GameLevel> _gameLevels = null;

        [Header("UI & Menus")]
        [SerializeField] private MainMenuUI _mainMenu = null;
        [SerializeField] private GameUI _gameScreen = null;
        [SerializeField] private GameOverUI _gameOver = null;

        // ----- Level Data
        private int _currentLevel = 0;
        private bool _countdown = false;
        private float _currentCountdown = 0.0f;
        private int _targetCount = 0;
        private Coroutine _levelCoroutine = null;
        private List<Target> _registeredTargets = null;
        private GameState _currentState = GameState.MainMenu;

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

        public void Cancel()
        {
            switch (_currentState)
            {
                case GameState.MainMenu:
                    _main_menu_quit_clicked();
                    break;

                case GameState.GameOver:
                    _game_over_quit_clicked();
                    break;

                case GameState.Play:
                    _game_over_menu_clicked();
                    break;
            }
        }

        public void Begin()
        {
            _currentState = GameState.MainMenu;
            _registeredTargets ??= new List<Target>();

            _mainMenu.SetActive(true);
            _gameScreen.SetActive(false);
            _gameOver.SetActive(false);

            _mainMenu.OnPlayClicked += _main_menu_play_clicked;
            _mainMenu.OnQuitClicked += _main_menu_quit_clicked;
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
            _registeredTargets.Clear();

            while (_targetCount > 0)
            {
                if (targets.Count > 0)
                {
                    List<TargetTrack> availableTracks = _targetTracks.Where(t => t.Occupant == null).ToList();
                    if (availableTracks.Count > 0)
                    {
                        Target target = targets.Dequeue();
                        target.OnDespawned += _target_despawned;
                        target.OnDestroyed += _target_destroyed;
                        target.SetTarget(_playerShip);
                        _registeredTargets.Add(target);

                        TargetTrack selectedTrack = availableTracks.Random();
                        selectedTrack.SetOccupant(target);
                    }
                }

                yield return new WaitForSeconds(gameLevel.SpawnDelay);
            }

            _next_level(gameLevel.CompletionAward);
            yield return null;
        }

        private void _target_destroyed(Target target)
        {
            target.OnDestroyed -= _target_destroyed;
            _playerShip.GainGold(target.GoldValue);
        }

        private void _target_despawned(IPooledObject<TargetType> obj)
        {
            Target target = obj as Target;
            target.OnDespawned -= _target_despawned;

            TargetTrack occupiedTrack = _targetTracks.Where(t => t.Occupant == target).FirstOrDefault();
            occupiedTrack.SetOccupant(null);

            _targetCount--;
        }

        private void _next_level(int award)
        {
            _playerShip.GainGold(award);

            if (_currentLevel < _gameLevels.Count - 1) // we'll just repeat the last level in the list indefinitely
                _currentLevel++;  

            _start_countdown();
        }

        private void _player_destroyed()
        {
            _playerShip.OnDestroyed -= _player_destroyed;

            _currentState = GameState.GameOver;
            StopCoroutine(_levelCoroutine);
            _levelCoroutine = null;

            _gameScreen.SetActive(false);
            _gameOver.SetActive(true);

            _gameOver.OnPlayClicked += _game_over_play_clicked;
            _gameOver.OnMainMenuClicked += _game_over_menu_clicked;
            _gameOver.OnQuitClicked += _game_over_quit_clicked;
        }

        private void _play_game()
        {
            _currentState = GameState.Play;

            _mainMenu.SetActive(false);
            _gameOver.SetActive(false);
            _gameScreen.SetActive(true);

            _playerShip.Reinitialize();
            _playerShip.OnDestroyed += _player_destroyed;
            
            _currentLevel = 0;
            _start_countdown();
        }

        private void _despawn_level()
        {
            foreach(Target target in _registeredTargets)
            {
                target.OnDestroyed -= _target_destroyed;
                target.OnDespawned -= _target_despawned;
            }

            TargetFactory.Instance.DespawnAll();
            ProjectileFactory.Instance.DespawnAll();

            _targetCount = 0;
            foreach (TargetTrack track in _targetTracks)
                track.SetOccupant(null);
        }

        private void _main_menu_play_clicked()
        {
            _mainMenu.OnPlayClicked -= _main_menu_play_clicked;
            _mainMenu.OnQuitClicked -= _main_menu_quit_clicked;

            _play_game();
        }

        private void _main_menu_quit_clicked()
        {
            _mainMenu.OnPlayClicked -= _main_menu_play_clicked;
            _mainMenu.OnQuitClicked -= _main_menu_quit_clicked;

            OnGameExit?.Invoke();
        }

        private void _game_over_play_clicked()
        {
            _gameOver.OnPlayClicked -= _game_over_play_clicked;
            _gameOver.OnMainMenuClicked -= _game_over_menu_clicked;
            _gameOver.OnQuitClicked -= _game_over_quit_clicked;

            _despawn_level();
            _play_game();
        }

        private void _game_over_menu_clicked()
        {
            _gameOver.OnPlayClicked -= _game_over_play_clicked;
            _gameOver.OnMainMenuClicked -= _game_over_menu_clicked;
            _gameOver.OnQuitClicked -= _game_over_quit_clicked;
            _despawn_level();

            Begin();
        }

        private void _game_over_quit_clicked()
        {
            _gameOver.OnPlayClicked -= _game_over_play_clicked;
            _gameOver.OnMainMenuClicked -= _game_over_menu_clicked;
            _gameOver.OnQuitClicked -= _game_over_quit_clicked;

            OnGameExit?.Invoke();
        }
    }
}
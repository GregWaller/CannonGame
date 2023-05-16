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

    public enum GameState
    {
        MainMenu,
        Play,
        GameOver,
    }

    public class LevelController : MonoBehaviour
    {
        public event Action OnGameExit = delegate { };

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

        [Header("UX")]
        [SerializeField] private bool _cameraShakeOnDamage = true;
        [SerializeField] private Camera _mainCamera = null;
        [SerializeField] private AnimationCurve _cameraShakeOverTime = new AnimationCurve();
        [SerializeField] private float _cameraShakeInterval = 0.5f;
        [SerializeField] private float _cameraShakeDuration = 1.5f;

        // ----- Level Data
        private int _currentLevelIDX = 0;
        private bool _countdown = false;
        private float _currentCountdown = 0.0f;
        private int _targetCount = 0;
        private Coroutine _levelCoroutine = null;
        private List<Target> _registeredTargets = null;
        private GameState _currentState = GameState.MainMenu;
        private int _currentLevel = 1;
        private int _currentScore = 0;

        // ----- UX
        private bool _cameraShake = false;
        private float _currentShakeInterval = 0.0f;
        private float _currentShakeDuration = 0.0f;
        private readonly Quaternion _defaultCameraRotation = Quaternion.Euler(35.0f, 0.0f, 0.0f);

        public PlayerShip Player => _playerShip;
        public bool IsCountingDown => _countdown;
        public int Countdown => (int)_currentCountdown;
        public int CurrentLevel => _currentLevel;
        public int CurrentScore => _currentScore;
        public GameState State => _currentState;

        public void Update()
        {
            if (_countdown)
            {
                _currentCountdown -= Time.deltaTime;
                if (_currentCountdown <= 0.0f)
                {
                    _start_level(_currentLevelIDX);
                    _countdown = false;
                }
            }

            if (_cameraShake)
            {
                _currentShakeDuration += Time.deltaTime;
                _currentShakeInterval += Time.deltaTime;
                if (_currentShakeDuration >= _cameraShakeDuration)
                {
                    _cameraShake = false;
                    _mainCamera.transform.rotation = _defaultCameraRotation;
                }
                else if (_currentShakeInterval >= _cameraShakeInterval)
                {
                    _currentShakeInterval = 0.0f;

                    float t = _currentShakeDuration / _cameraShakeDuration;
                    float magnitude = _cameraShakeOverTime.Evaluate(t);
                    float x = _defaultCameraRotation.eulerAngles.x + UnityEngine.Random.Range(-1.0f, 1.0f) * magnitude;
                    float y = _defaultCameraRotation.eulerAngles.y + UnityEngine.Random.Range(-1.0f, 1.0f) * magnitude;
                    float z = _defaultCameraRotation.eulerAngles.z + UnityEngine.Random.Range(-1.0f, 1.0f) * magnitude;
                    Quaternion newRotation = Quaternion.Euler(x, y, z);
                    _mainCamera.transform.rotation = newRotation;
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

            _currentLevel++;

            if (_currentLevelIDX < _gameLevels.Count - 1) // we'll just repeat the last level in the list indefinitely
                _currentLevelIDX++;  

            _start_countdown();
        }

        private void _player_destroyed()
        {
            _playerShip.OnDestroyed -= _player_destroyed;
            _playerShip.OnGoldGained -= _player_gained_gold;
            _playerShip.OnDamaged -= _player_damaged;

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
            _playerShip.OnGoldGained += _player_gained_gold;
            _playerShip.OnDamaged += _player_damaged;

            _currentLevelIDX = 0;
            _currentLevel = 1;
            _start_countdown();
        }

        private void _player_damaged()
        {
            if (!_cameraShakeOnDamage) return;

            _cameraShake = true;
            _currentShakeInterval = 0.0f;
            _currentShakeDuration = 0.0f;
        }

        private void _player_gained_gold(int amount)
        {
            _currentScore += amount;
        }

        private void _despawn_level()
        {
            if (_playerShip.Alive)
                _playerShip.Kill();

            if (_levelCoroutine != null)
                StopCoroutine(_levelCoroutine);

            foreach (Target target in _registeredTargets)
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
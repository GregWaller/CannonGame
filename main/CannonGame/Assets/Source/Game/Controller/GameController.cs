﻿/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using UnityEngine;
using UnityEditor;

namespace LRG.Master
{
    using LRG.Data;
    using LRG.UI;

    public class GameController : MonoBehaviour
    {
        public static string DevelopedBy = "Greg Waller";
        public static int CopyrightYear = 2023;

        public static VersionData CurrentVersion { get; private set; } = new VersionData(0, 0, 4, 0);

        [Header("UI/UX")]
        [SerializeField] private InputController _inputController = null;
        [SerializeField] private LevelController _levelController = null;

        public LevelController LevelController => _levelController;

        public virtual void Start()
        {
            Application.targetFrameRate = 120;
            StartCoroutine(_load_scene());
        }

        private IEnumerator _load_scene()
        {
            Debug.Log($"Initializing GameController v{CurrentVersion}...");

            yield return _init_factories();
            yield return _init_ui();
            yield return _init_audio();

            Debug.Log("Load complete.  Transferring control to player.");
            _levelController.Begin();

            yield return null;
        }

        private IEnumerator _init_factories()
        {
            Debug.Log("Initializing object factory pools...");
            yield return ProjectileFactory.Instance.Initialize();
            yield return TargetFactory.Instance.Initialize();
            yield return VisualEffectFactory.Instance.Initialize();
        }

        private IEnumerator _init_ui()
        {
            Debug.Log("Initializing User Interface...");
            Debug.Assert(_inputController != null, "InputController must be specified");
            Debug.Assert(_levelController != null, "LevelController must be specified");
            _inputController.RegisterGameController(this);
            _levelController.OnGameExit += _exit_application;

            yield return null;
        }

        private IEnumerator _init_audio()
        {
            Debug.Log("Initializing Audio...");
            //AudioLibrary.Instance.Initialize(this, InputController);
            //AudioLibrary.Play(SongID._02_otherworldly_dream);
            yield return null;
        }

        protected void _exit_application()
        {
            Debug.Log("Exiting application...");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
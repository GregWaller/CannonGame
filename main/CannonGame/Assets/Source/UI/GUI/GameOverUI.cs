/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LRG.UI
{
    using LRG.Game;
    using LRG.Master;

    public class GameOverUI : MonoBehaviour
    {
        public event Action OnPlayClicked = delegate { };
        public event Action OnMainMenuClicked = delegate { };
        public event Action OnQuitClicked = delegate { };

        [SerializeField] private Button _playButton = null;
        [SerializeField] private Button _menuButton = null;
        [SerializeField] private Button _quitButton = null;

        public void OnEnable()
        {
            _playButton.onClick.AddListener(_play_clicked);
            _menuButton.onClick.AddListener(_menu_clicked);
            _quitButton.onClick.AddListener(_quit_clicked);
        }

        public void OnDisable()
        {
            _playButton.onClick.RemoveListener(_play_clicked);
            _menuButton.onClick.RemoveListener(_menu_clicked);
            _quitButton.onClick.RemoveListener(_quit_clicked);
        }

        private void _play_clicked()
        {
            OnPlayClicked?.Invoke();
        }

        private void _menu_clicked()
        {
            OnMainMenuClicked?.Invoke();
        }
        
        private void _quit_clicked()
        {
            OnQuitClicked?.Invoke();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}

/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using TMPro;

namespace LRG.UI
{
    using System;

    using LRG.Master;

    public class GameUI : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController = null;
        [SerializeField] private GameObject _countdown = null;
        [SerializeField] private TextMeshProUGUI _countdownText = null;
        [SerializeField] private GoldPopupUI _goldPopup = null;

        public void Awake()
        {
            _levelController.OnTargetDestroyed += _on_player_gold_gained;
            _goldPopup.Hide();
        }

        public void OnDestroy()
        {
            _levelController.OnTargetDestroyed -= _on_player_gold_gained;
        }

        public void Update()
        {
            _countdown.SetActive(_levelController.IsCountingDown);

            if (_levelController.IsCountingDown)
                _countdownText.text = _levelController.Countdown.ToString();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void _on_player_gold_gained(Vector3 worldPosition, int goldAmount)
        {
            Vector2 screenPosition = _levelController.WorldPositionToScaledScreenPoint(worldPosition);
            _goldPopup.Show(screenPosition, goldAmount);
        }
    }
}
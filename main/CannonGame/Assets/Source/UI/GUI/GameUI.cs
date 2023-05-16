/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using TMPro;

namespace LRG.UI
{
    using LRG.Master;

    public class GameUI : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController = null;
        [SerializeField] private GameObject _countdown = null;
        [SerializeField] private TextMeshProUGUI _countdownText = null;

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
    }
}
/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LRG.UI
{
    using LRG.Game;

    public class ShipHealthUI : MonoBehaviour
    {
        [SerializeField] private PlayerShip _playerShip = null;
        [SerializeField] private Image _shipHP = null;
        [SerializeField] private TextMeshProUGUI _shipHPNumbers = null;
        [SerializeField] private GameObject _lowHullIndicator = null;
        [SerializeField] private int _lowHealthThreshold = 3;
        [SerializeField] private float _lowHealthFlashInterval = 0.2f;

        private float _flashing = 0.0f;
        private bool _isLowHull = false;

        public void Update()
        {
            if (_playerShip == null) return;

            _shipHPNumbers.text = $"{_playerShip.Hull}/{_playerShip.BaseHull}";
            _shipHP.fillAmount = (float)_playerShip.Hull / (float)_playerShip.BaseHull;

            if (_playerShip.Hull <= _lowHealthThreshold)
            {
                _flashing += Time.deltaTime;
                if (_flashing >= _lowHealthFlashInterval)
                {
                    _flashing = 0.0f;
                    _isLowHull = !_isLowHull;
                    _lowHullIndicator.SetActive(_isLowHull);
                }
            }
            else
            {
                _flashing = 0.0f;
                _lowHullIndicator.SetActive(false);
                _isLowHull = false;
            }
        }
    }
}
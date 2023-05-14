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

        public void Update()
        {
            if (_playerShip == null) return;

            _shipHPNumbers.text = $"{_playerShip.Hull}/{_playerShip.BaseHull}";
            _shipHP.fillAmount = (float)_playerShip.Hull / (float)_playerShip.BaseHull;
        }
    }
}
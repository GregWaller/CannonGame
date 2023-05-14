/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LRG.UI
{
    using System;

    using LRG.Game;
    using LRG.Master;

    public class GoldCounterUI : MonoBehaviour
    {
        [SerializeField] private PlayerShip _playerShip = null;
        [SerializeField] private TextMeshProUGUI _goldCounter = null;
        [SerializeField] private Button _btnRepairShip = null;

        public void Awake()
        {
            if (_btnRepairShip != null)
                _btnRepairShip.onClick.AddListener(_on_repair_clicked);
        }

        public void OnDestroy()
        {
            if (_btnRepairShip != null)
                _btnRepairShip.onClick.RemoveListener(_on_repair_clicked);
        }

        public void Update()
        {
            if (_playerShip == null) return;

            _goldCounter.text = $"{_playerShip.Gold}";
        }

        private void _on_repair_clicked()
        {
            _playerShip.Repair();
        }
    }
}

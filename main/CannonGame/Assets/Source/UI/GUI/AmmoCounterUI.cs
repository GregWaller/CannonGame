/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LRG.UI
{
    using LRG.Game;

    public class AmmoCounterUI : MonoBehaviour
    {
        [SerializeField] private PlayerShip _playerShip = null;
        [SerializeField] private TextMeshProUGUI _ammoCounter = null;
        [SerializeField] private Button _btnBuyAmmo = null;

        public void Awake()
        {
            if (_btnBuyAmmo != null)
                _btnBuyAmmo.onClick.AddListener(_on_purchase_clicked);
        }

        public void OnDestroy()
        {
            if (_btnBuyAmmo != null)
                _btnBuyAmmo.onClick.RemoveListener(_on_purchase_clicked);
        }

        public void Update()
        {
            if (_playerShip == null) return;

            _ammoCounter.text = $"{_playerShip.Ammo}";
        }

        private void _on_purchase_clicked()
        {
            _playerShip.PurchaseAmmo();
        }
    }
}

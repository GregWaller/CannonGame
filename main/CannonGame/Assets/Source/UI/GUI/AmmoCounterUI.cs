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
        [SerializeField] private GameObject _lowAmmoIndicator = null;
        [SerializeField] private int _lowAmmoThreshold = 30;
        [SerializeField] private float _lowAmmoFlashInterval = 0.2f;

        private float _flashing = 0.0f;
        private bool _isLowAmmo = false;

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

            if (_playerShip.Ammo <= _lowAmmoThreshold)
            {
                _flashing += Time.deltaTime;
                if (_flashing >= _lowAmmoFlashInterval)
                {
                    _flashing = 0.0f;
                    _isLowAmmo = !_isLowAmmo;
                    _lowAmmoIndicator.SetActive(_isLowAmmo);
                }
            }
            else
            {
                _flashing = 0.0f;
                _lowAmmoIndicator.SetActive(false);
                _isLowAmmo = false;
            }
        }

        private void _on_purchase_clicked()
        {
            _playerShip.PurchaseAmmo();
        }
    }
}

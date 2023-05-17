/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using TMPro;

namespace LRG.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class GoldPopupUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _goldText = null;
        [SerializeField] private float _fadeSpeed = 0.1f;
        [SerializeField] private float _animationSpeed = 0.1f;

        private RectTransform _rectTransform = null;
        private bool _visible = false;
        private float _currentAlpha = 0.0f;

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Update()
        {
            if (_visible)
            {
                float currentY = _rectTransform.anchoredPosition.y + _animationSpeed;
                _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, currentY);

                _currentAlpha -= Time.deltaTime * _fadeSpeed;
                _goldText.color = new Color(_goldText.color.r, _goldText.color.g, _goldText.color.b, _currentAlpha);
                if (_currentAlpha <= 0.0f)
                {
                    gameObject.SetActive(false);
                    _visible = false;
                    return;
                }
            }
        }

        public void Show(Vector2 screenPos, int goldAmount)
        {
            gameObject.SetActive(true);
            _goldText.color = Color.white;
            _goldText.text = $"+{goldAmount}";
            _rectTransform.anchoredPosition = screenPos;
            _visible = true;
            _currentAlpha = 1.0f;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

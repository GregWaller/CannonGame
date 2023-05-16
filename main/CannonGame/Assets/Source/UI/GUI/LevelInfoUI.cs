/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using TMPro;

namespace LRG.UI
{
    using LRG.Master;

    using UnityEngine.InputSystem.LowLevel;

    public class LevelInfoUI : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController = null;
        [SerializeField] private TextMeshProUGUI _level = null;
        [SerializeField] private TextMeshProUGUI _dubloons = null;

        public void Update()
        {
            _level.text = _levelController.CurrentLevel.ToString();
            _dubloons.text = _levelController.CurrentScore.ToString();
        }
    }
}

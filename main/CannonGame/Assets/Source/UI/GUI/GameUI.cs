/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.UI
{
    public class GameUI : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
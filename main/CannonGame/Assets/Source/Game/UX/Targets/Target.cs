/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;

namespace LRG.Game
{
    using LRG.Master;

    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        private Collider _collider = null;

        public void Awake()
        {
            _collider = GetComponent<Collider>();    
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile))
            {
                projectile.Despawn();
                gameObject.SetActive(false);
            }
        }
    }
}

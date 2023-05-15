/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace LRG.Game
{
    using LRG.Master;

    public enum EffectType
    {
        Unassigned,
        Cannon_Fire, 
        Cannon_Smoke,
        Water_Splash,
        Target_Hit
    };

    [RequireComponent(typeof(VisualEffect))]
    public abstract class PooledEffect : PooledObject<EffectType>
    {
        protected abstract EffectType _effectType { get; }

        private VisualEffect _vfx = null;
        private bool _awaitingFirstParticle = false;

        public override EffectType Key => _effectType;

        private void Update()
        {
            if (_awaitingFirstParticle && _vfx.aliveParticleCount > 0)
                _awaitingFirstParticle = false;
            else if (!_awaitingFirstParticle && _vfx.aliveParticleCount == 0)
                Despawn();
        }

        public override void Init()
        {
            _vfx = GetComponent<VisualEffect>();
        }

        public override void Activate(bool active)
        {
            base.Activate(active);
            gameObject.SetActive(active);
        }

        public override void Reinitialize()
        {
            base.Reinitialize();
            _awaitingFirstParticle = true;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            SetPosition(worldPosition, Vector3.zero);
        }

        public void SetPosition(Vector3 worldPosition, Vector3 lookAt)
        {
            transform.position = worldPosition;
            transform.LookAt(transform.position + lookAt);
        }
    }
}
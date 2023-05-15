/* © 2023 - Greg Waller.  All rights reserved. */

using System;
using UnityEngine;
using System.Collections.Generic;

namespace LRG.Game
{
    using LRG.Master;

    public class CannonFire_PooledEffect : PooledEffect
    {
        protected override EffectType _effectType => EffectType.Cannon_Fire;
    }
}
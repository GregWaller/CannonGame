/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using System.Collections.Generic;

namespace LRG.Game
{
    using LRG.Master;

    public class SmallShip_Target : Target
    {
        protected override TargetType _targetType => TargetType.SmallShip;
    }
}
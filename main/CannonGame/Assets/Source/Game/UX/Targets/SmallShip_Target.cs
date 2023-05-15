/* © 2023 - Greg Waller.  All rights reserved. */

namespace LRG.Game
{
    public class SmallShip_Target : Target
    {
        protected override TargetType _targetType => TargetType.SmallShip;

        public override void Reinitialize() { }
    }
}
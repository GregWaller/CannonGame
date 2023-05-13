/* © 2023 - Greg Waller.  All rights reserved. */

namespace LRG.Master
{
    using LRG.Game;

    public class TargetFactory : PooledObjectFactory<TargetType, Target>
    {
        protected override string _prefab_path => "Targets";

        #region Singleton

        private static TargetFactory _instance = null;
        public static TargetFactory Instance
        {
            get
            {
                _instance ??= new TargetFactory();
                return _instance;
            }
        }

        private TargetFactory()
            : base() { }

        #endregion
    }
}
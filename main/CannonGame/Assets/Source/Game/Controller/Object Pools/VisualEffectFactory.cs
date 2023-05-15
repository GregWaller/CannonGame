/* © 2023 - Greg Waller.  All rights reserved. */

namespace LRG.Master
{
    using LRG.Game;

    public class VisualEffectFactory : PooledObjectFactory<EffectType, PooledEffect>
    {
        protected override string _prefab_path => "Visual Effects";

        #region Singleton

        private static VisualEffectFactory _instance = null;
        public static VisualEffectFactory Instance
        {
            get
            {
                _instance ??= new VisualEffectFactory();
                return _instance;
            }
        }

        private VisualEffectFactory()
            : base() { }

        #endregion
    }
}

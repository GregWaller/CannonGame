/* © 2023 - Greg Waller.  All rights reserved. */

namespace LRG
{
    public class PooledObjectTypeNotFoundException : System.ArgumentOutOfRangeException
    {
        public PooledObjectTypeNotFoundException(string paramName, string message) : base(paramName, message) { }
    }
}
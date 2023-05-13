/* © 2023 - Greg Waller.  All rights reserved. */

namespace LRG
{
    public class ProjectileTypeNotFoundException : System.ArgumentOutOfRangeException
    {
        public ProjectileTypeNotFoundException(string paramName, string message) : base(paramName, message) { }
    }
}
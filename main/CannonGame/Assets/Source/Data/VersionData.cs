/* © 2020 - Greg Waller.  All rights reserved. */

using System;

namespace LRG.Data
{
    using LRG.Game;

    public class VersionData : IEquatable<VersionData>
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Patch { get; set; }

        public VersionData(int maj, int min, int bld, int pat)
        {
            Major = maj;
            Minor = min;
            Build = bld;
            Patch = pat;
        }

        public VersionData()
            : this(GameController.CurrentVersion) { }
        public VersionData(VersionData cpy)
            : this(cpy.Major, cpy.Minor, cpy.Build, cpy.Patch) { }

        public bool MatchBuild(VersionData other)
        {
            return
                Major == other.Major &&
                Minor == other.Minor &&
                Build == other.Build;
        }

        public bool MatchMinor(VersionData other)
        {
            return
                Major == other.Major &&
                Minor == other.Minor;
        }

        public bool MatchMajor(VersionData other)
        {
            return Major == other.Major;
        }

        public bool Equals(VersionData other)
        {
            return
                Major == other.Major &&
                Minor == other.Minor &&
                Build == other.Build &&
                Patch == other.Patch;
        }

        public static bool operator !=(VersionData lh, VersionData rh)
        {
            return !(lh == rh);
        }
        public static bool operator ==(VersionData lh, VersionData rh)
        {
            if (lh is null)
                return rh is null;

            return lh.Equals(rh);
        }
        public static bool operator <(VersionData lh, VersionData rh)
        {
            if (rh is null || lh is null)
                throw new ArgumentNullException("Critical Error: Cannot compare a VersionData object to null.");

            return lh.Major < rh.Major ||
                   lh.Minor < rh.Minor ||
                   lh.Build < rh.Build ||
                   lh.Patch < rh.Patch;
        }
        public static bool operator >(VersionData lh, VersionData rh)
        {
            if (rh is null || lh is null)
                throw new ArgumentNullException("Critical Error: Cannot compare a VersionData object to null.");

            return lh.Major > rh.Major ||
                   lh.Minor > rh.Minor ||
                   lh.Build > rh.Build ||
                   lh.Patch > rh.Patch;
        }
        public static bool operator <=(VersionData lh, VersionData rh)
        {
            return lh < rh || lh == rh;
        }
        public static bool operator >=(VersionData lh, VersionData rh)
        {
            return lh > rh || lh == rh;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Patch}";
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return ReferenceEquals(this, obj);
        }
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            return hash;
        }
    }
}

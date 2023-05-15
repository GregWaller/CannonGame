/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;
using UnityEngine;

namespace LRG.Master
{
    using LRG.Game;

    public class TargetTrack : MonoBehaviour
    {
        [SerializeField] private Transform _a = null;
        [SerializeField] private Transform _b = null; // TODO: Make this a list rather than an explicit pair.  Target traversal will also need to understand it as a list as well.

        private Target _occupant = null;

        public Target Occupant => _occupant;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_a.position, _b.position);
        }

#endif

        public void SetOccupant(Target occupant)
        {
            _occupant = occupant;
            if (occupant == null)
                return;

            List<Vector3> waypoints = new List<Vector3> { _a.position, _b.position };
            waypoints.Shuffle();
            occupant.Patrol(waypoints);
        }
    }
}

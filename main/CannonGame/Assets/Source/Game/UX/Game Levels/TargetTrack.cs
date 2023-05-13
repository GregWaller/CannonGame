/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LRG.Master
{
    using LRG.Data;
    using LRG.UI;
    using LRG.Game;

    public class TargetTrack : MonoBehaviour
    {
        [SerializeField] private Transform _a = null;
        [SerializeField] private Transform _b = null;

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

            // we'll select a random ordering of _a, _b or _b, _a
            // and give these points to the occupant for its patrol
            occupant.Patrol(new List<Vector3> { _a.position, _b.position });
        }
    }
}

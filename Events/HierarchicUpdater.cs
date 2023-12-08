//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
using Tiger.Util;
using UnityEngine;

namespace Tiger.Events
{
    [Icon("Assets/Tiger/Events/Editor/Icons/crown.png")]
    public sealed class HierarchicUpdater : MonoBehaviour
    {
        private enum UpdateMode
        {
            /// <summary>
            /// Updates the shallowest children first (unity order)
            /// </summary>
            TopDown = default,
            
            /// <summary>
            /// Updates the deepest children first (reverse unity order, in-order)
            /// </summary>
            BottomUp,

            /// <summary>
            /// Randomly shuffles the update order each frame.
            /// </summary>
            Shuffled,

            /// <summary>
            /// No update at the same sequence position as the previous frame.
            /// </summary>
            Deranged
        }

        [SerializeField] private UpdateMode updateMode;

        private readonly List<IHierarchicalUpdate> _children = new();

        private void Awake() => SetUpChildren();

        //FIXME: This doesn't work if this message is sent to a child instead.
        private void OnTransformChildrenChanged() => SetUpChildren(); 

        private void Update()
        {
            switch (updateMode)
            {
                case UpdateMode.Shuffled:
                    _children.Shuffle();
                    break;
                case UpdateMode.Deranged:
                    _children.Derange();
                    break;

                case UpdateMode.TopDown:
                case UpdateMode.BottomUp:
                default:
                    break;
            }
            
            UpdateChildren();
        }

        private void SetUpChildren()
        {
            GetComponentsInChildren(_children);
            if (updateMode == UpdateMode.BottomUp) _children.Reverse();
        }
        
        private void UpdateChildren()
        {
            foreach (var child in _children) child.HierarchicUpdate(Time.deltaTime);
        }
    }
}

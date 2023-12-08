//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Tiger.Events
{
    public abstract class SealableUpdateBehaviour : MonoBehaviour
    {
#if !UNITY_2018_1_OR_NEWER
        //In Unity, "private protected", which would be the correct access modifier, doesn't work.
        private
#endif
        protected abstract void Update();
    }
}
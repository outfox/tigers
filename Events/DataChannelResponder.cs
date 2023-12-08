//SPDX-License-Identifier: Unlicense

using UnityEngine;
using UnityEngine.Scripting;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Tiger.Events
{
    [Icon("Assets/Tiger/Events/Editor/Icons/responder.png")]
    [Preserve]
    public abstract class DataChannelResponder<TChannel, T> : SealableEnableDisableBehaviour where TChannel : DataChannel<T>
    {
        [SerializeField] protected TChannel channel;

        /// <summary>
        /// Called when events are emitted to the channel.
        /// </summary>
        protected abstract void OnEvent(T data);

        /// <summary>
        /// Override this if you want to have your own code happen on enable
        /// </summary>
        protected virtual void OnEnableOverride()
        {
        }

        /// <summary>
        /// Override this if you want to have your own code happen on disable.
        /// </summary>
        protected virtual void OnDisableOverride()
        {
        }

        #region Unity Events

        // Triggered when the event(s) happen.
        private void Trigger(T data)
        {
            OnEvent(data);
        }

        protected sealed override void OnEnable()
        {
            OnEnableOverride();
            channel.Subscribe(Trigger);
        }

        protected sealed override void OnDisable()
        {
            OnDisableOverride();
            if (channel) channel.Unsubscribe(Trigger);
        }

#if UNITY_EDITOR
#if TIGER_ALWAYS_SHOW_GIZMOS
        protected virtual void OnDrawGizmos()
#else
        protected virtual void OnDrawGizmosSelected()
#endif
        {
            if (channel)
            {
                UnityEditor.Handles.Label(UnityEditor.Handles.matrix * transform.position, $"→{channel.name}");
            }
            else
            {
                UnityEditor.Handles.Label(transform.position, "not subscribed");
            }
        }
#endif

        protected virtual void OnValidate()
        {
            if (!channel) Debug.LogWarning($"DataChannelResponder: Channel is not set on {this}", this);
        }

        #endregion
    }
}

/*
Written by Tiger Blue in 2021, 2023

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/
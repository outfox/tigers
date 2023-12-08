//SPDX-License-Identifier: Unlicense

using System;
using Loggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace Tiger.Events
{
    [CreateAssetMenu(menuName="Event/(void) Channel")]
    [Icon("Assets/Tiger/Events/Editor/Icons/channel.png")]
    [Preserve]
    public class Channel : AbstractChannel
    {
        [Header("Logs & Error Handling")]        
        [SerializeField] [Tooltip("Debug Settings Asset")]
        private DebugSettings debugSettings;
        
        [NonSerialized]
        private readonly UnityEvent _subscriptions = new();

        /// <summary>
        /// Subscribe to this channel.
        /// </summary>
        /// <param name="action">callback that will be invoked on Emit</param>
        public void Subscribe(UnityAction action)
        {
            _subscriptions.AddListener(action);
        }

        /// <summary>
        /// Unsubscribe from this channel.
        /// </summary>
        /// <param name="action">the callback to remove</param>
        public void Unsubscribe(UnityAction action)
        {
            _subscriptions.RemoveListener(action);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Emit a value on this channel. All subscriber actions will be invoked with the value.
        /// </summary>
        /// <param name="context">UnityEngine.Object that become the potential Debut console highlight cuplrit.</param>
        public void Emit(Object context = null)
        {
            if (debugSettings.enabled)
            {
                debugSettings.Log($"<b>EVENT</b> {name}", context != null ? context : this);
            }
            _subscriptions.Invoke();
        }

        protected internal override void Init()
        {
            if (debugSettings.enabled) debugSettings.Log($"<b>INIT</b> {name} (void)", this);
            _subscriptions.RemoveAllListeners();
        }
    }
}

/*
Written by Tiger Blue in 2021

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
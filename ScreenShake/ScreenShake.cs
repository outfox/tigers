// SPDX-License-Identifier: Unlicense
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tiger.ScreenShake
{
    public static class ScreenShake
    {
        public struct ShakeEvent
        {
            public Vector3 position;
            public float amplitudeHF;
            public float amplitudeLF;
        }

        [NonSerialized]
        internal static readonly UnityEvent<ShakeEvent> Shakes = new();

        /// <summary>
        /// Adds a screen shake effect, invoking the Event to notify the subscribers.
        /// </summary>
        /// <param name="position">World space position of the event.</param>
        /// <param name="amplitudeHF">High frequency strength of the shake.</param>
        /// <param name="amplitudeLF">Low frequency strength of the shake.</param>
        public static void Add(Vector3 position, float amplitudeHF, float amplitudeLF)
        {
            Shakes.Invoke(new ShakeEvent()
            {
                position = position,
                amplitudeHF = amplitudeHF,
                amplitudeLF = amplitudeLF
            });
        }
    }
}

/*
Written by Tiger Blue in 2017

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
//SPDX-License-Identifier: Unlicense
using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Loggers
{
    [CreateAssetMenu(menuName = "Debug/Debug Settings", fileName = "Debug Settings", order = 0)]
    public class DebugSettings : ScriptableObject
    {
        public static Level LogLevel;
        
        public bool enabled; 
        public Level level;
        public Color color = Color.cyan;

        private string ApplyColor(string input)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{input}</color>";
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Log(string message, UnityEngine.Object context = null)
        {
            if (!enabled || level > LogLevel) return;
            
            //TODO: color the message nicely (some format parsing perhaps).
            switch (level)
            {
                case Level.Info:
                    Debug.Log(ApplyColor(message), context);
                    break;
                case Level.Warn:
                    Debug.LogWarning(ApplyColor(message), context);
                    break;
                case Level.Error:
                    Debug.LogError(ApplyColor(message), context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public enum Level
        {
            Info,
            Warn,
            Error
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
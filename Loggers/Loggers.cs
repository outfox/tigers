//SPDX-License-Identifier: Unlicense OR CC0-1.0+
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
// ReSharper disable StaticMemberInGenericType
namespace Loggers
{
    /// <summary>
    /// Inherit from this class to get a logger that prepends the name of the class to every log message.
    /// <code>class Log : Loggers.Create&lt;Log&gt; {}</code>
    /// </summary>
    /// <remarks> 
    /// <para>Access methods statically: <c>Log.Info("Hello World");</c></para>
    /// <para>Derive multiple classes with different purposes, and then import them in the files that use them.</para>
    /// <para>You can also use a using declaration to name a specific logger for a class in the file you want to use it in.</para>
    /// <para>You may add optional code to the class to add additional functionality that is exposed only for that type.</para>
    ///</remarks>
    /// <example>     
    /// Derive a logger for a specific purpose:
    /// <code>class MyGame.CameraLogger : Loggers.Create&lt;CameraLogger&gt; {}</code>
    /// Import that specific logger:
    /// <code>using Log = MyGame.CameraLogger;</code>
    /// Define a Debug logger for a specific MonoBehaviour (can be done in the same file):
    /// <code>using Log = Logging.Create&lt;MyMonoBehaviour&gt;;</code>
    /// Shadowing UnityEngine.Debug, existing Debug.Log code calls the new logger:
    /// <code>using Debug = MyGame.AnotherLoggingClass;</code>
    /// </example>
    /// <typeparam name="T">class whose name will be prepended to the log messages, can be the derived class itself</typeparam>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class Create<T>
    {
        #region Public API

        /// <summary>
        /// Log a message to the Unity console at the Default Log Level.
        /// Equivalent to Debug.Log(...), but the name of the Logging class will be prepended.
        /// </summary>
        /// <param name="message">string to log</param>
        /// <param name="context">optional UnityEngine.Object that will be pinged when the user clicks the log message</param>
        [HideInCallstack]
        public static void Info(string message, Object context = null) =>
            Logger.Log(LogType.Log, Tag, message, context);

        /// <summary>
        /// Log a message to the Unity console at the Warning Log Level.
        /// Equivalent to Debug.LogWarning(...), but the name of the Logging class will be prepended.
        /// </summary>
        /// <param name="message">string to log</param>
        /// <param name="context">optional UnityEngine.Object that will be pinged when the user clicks the log message</param>
        [HideInCallstack]
        public static void Warn(string message, Object context = null) =>
            Logger.Log(LogType.Warning, Tag, message, context);

        /// <summary>
        /// Log a message to the Unity console at the Error Log Level.
        /// Equivalent to Debug.LogError(...), but the name of the Logging class will be prepended.
        /// </summary>
        /// <param name="message">string to log</param>
        /// <param name="context">optional UnityEngine.Object that will be pinged when the user clicks the log message</param>
        [HideInCallstack]
        public static void Error(string message, Object context = null) =>
            Logger.Log(LogType.Error, Tag,  message, context);

        /// <summary>
        /// Log an exception with stack trace to the Unity console.
        /// Direct equivalent to Debug.LogException(...)
        /// </summary>
        /// <param name="ex">exception to output</param>
        /// <param name="context">optional UnityEngine.Object that will be pinged when the user clicks the log message</param>
        [HideInCallstack]
        public static void Exception(Exception ex, Object context = null) => Logger.LogException(ex, context);

        #endregion

        #region Compatibility API

        /// <summary>
        /// Convenience accessor that mimics the Unity logger style.
        /// See <see cref="Info"/> for details.
        /// </summary>
        [HideInCallstack]
        public static void Log(string message, Object context = null) => Info(message, context);

        /// <summary>
        /// Convenience accessor that mimics the Unity logger style.
        /// See <see cref="Warn"/> for details.
        /// </summary>
        [HideInCallstack]
        public static void LogWarning(string message, Object context = null) => Warn(message, context);

        /// <summary>
        /// Convenience accessor that mimics the Unity logger style.
        /// See <see cref="Error"/> for details.
        /// </summary>
        [HideInCallstack]
        public static void LogError(string message, Object context = null) => Error(message, context);

        /// <summary>
        /// Convenience accessor that mimics the Unity logger style.
        /// See <see cref="Exception"/> for details.
        /// </summary>
        [HideInCallstack]
        public static void LogException(Exception ex, Object context = null) => Exception(ex, context);

        #endregion

        #region Private Fields and Intialization

        /// <summary>
        /// Constructor that ensures registration.
        /// </summary>
        static Create()
        {
            Registry.Entries[FullTag] = Logger;
        }

        /// <summary>
        /// Set this color to change the colour of the Tag / ShortTag (if in use)
        /// </summary>
        public static Color TagColor = Color.white;

        // We want this exact behaviour here - a new field for every specialized type
#if UNITY_EDITOR 
        private static string Tag => HyperTag;
#else
        private static string Tag => ShortTag;
#endif
        
        private static string ShortTag => $"<color=#{ColorUtility.ToHtmlStringRGB(TagColor)}>{typeof(T).Name}</color>";

        private static readonly string FullTag = typeof(T).FullName;
        
        // ReSharper disable once StaticMemberInGenericType
        /// <summary>
        /// Access to the logger for this type, allows to configure the log level and enable/disable logging and more.
        /// See <see cref="UnityEngine.ILogger"/> for details (advanced use cases only).
        /// </summary>
        public static readonly Logger Logger = new(Debug.unityLogger);

        #endregion

        public static string HyperTag
        {
            get
            {
                var st = new System.Diagnostics.StackTrace(true);
                var frame = st.GetFrame(3);
                var fileShort = Path.GetFileName(frame.GetFileName());
                var position = frame.GetFileName();
                var line = frame.GetFileLineNumber();
                const string PATTERN = @"(.*\\)(.*?)\\(Assets\\.*)";
                return Regex.Replace(position ?? "unknown", PATTERN, m =>
                {
                    var project = m.Groups[2].Value; // Group 2 is the project dir.
                    var filePath = m.Groups[3].Value; // Group 3 is the file asset path.

                    // Issue in Rider URL Protocol Handler, line numbers are off by one (or maybe they count from zero)
                    var file_and_line_rider = $"{filePath}:{Math.Max(line - 1, 0)}";  
                        
                    var encoded_file_path = Uri.EscapeDataString(file_and_line_rider);
                    var url = $"jetbrains://rider/navigate/reference?project={project}&path={encoded_file_path}";
                    return $"<a href=\"{url}\"><b>{ShortTag}</b>:{line}</a>";
                });
            }
        }
    }

    /// <summary>
    /// Gives access to all existing loggers, for runtime Loglevel configuration.
    /// </summary>
    public static class Registry
    {
        #region Public API

        /// <summary>
        /// A dictionary of all loggers, keyed by their full type name.
        /// Can be used to write a service that allows runtime configuration of log levels.
        /// </summary>
        /// <example>
        /// <code>
        /// Registry.All["MyGame.Log"].filterLogType = LogLevel.Debug;
        /// Registry.All["MyGame.Log"].logEnabled = false;
        /// </code>
        /// <remarks>
        /// You can use typeof(MyGame.Log).FullName to get the full type name in a refactoring-proof way.
        /// You can use Registry.All.Keys to get a list of all loggers, to display them in a UI, etc.
        /// </remarks>
        /// </example>
        public static IReadOnlyDictionary<string, ILogger> All => Entries;

        /// <summary>
        /// Internal use for Logging Framework.
        /// </summary>
        internal static readonly Dictionary<string, ILogger> Entries = new();

        #endregion
    }
}
    
/*
Written by Tiger Blue in 2023

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

/*
At your discretion, you may interchangeably treat this code as CC0 licensed.
https://creativecommons.org/publicdomain/zero/1.0/legalcode
*/
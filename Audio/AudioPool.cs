using System;
using System.Collections.Generic;
using System.Linq;
using Tiger.Util;
using UnityEngine;

namespace Tiger.Audio
{
    /// <summary>
    /// A basic component that creates audio sources on an object and serves as a point to access them.
    /// </summary>
    public class AudioPool : MonoBehaviour
    {
        [SerializeField]
        private int capacity = 10;
        
        public List<AudioSource> sources => sourceComponents;
        
        [SerializeField]
        private List<AudioSource> sourceComponents = new();
        
        /// <summary>
        /// Provides an AudioSource from the pool for purposes of holding it for a longer time for later use.
        /// Example use case: playing a sound on a loop, but then stopping it later.
        /// </summary>
        /// <remarks>
        /// Only allowed at Runtime.
        /// </remarks>
        /// <returns>The source</returns>
        public AudioSource Take()
        {
            if (!Application.isPlaying) throw new InvalidOperationException("AudioPool: Cannot take sources at edit time.");
            return sources.Take();   
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (capacity == sources.Count && sources.All(s => s)) return;
            UnityEditor.EditorApplication.delayCall += EnsureComponents;
        }

        private void EnsureComponents()
        {
            if (!this) return;
            
            sources.Clear();
            var components = GetComponents<AudioSource>();

            for (var i = 0; i < capacity && i < components.Length; i++)
            {
                sources.Add(components[i]);
            }
            
            for (var i = sources.Count; i < capacity; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                sources.Add(source);
            }

            for (var i = capacity; i < components.Length; i++)
            {
                DestroyImmediate(components[i], true);
            }
            
            sources.RemoveRange(capacity, sources.Count - capacity);
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}

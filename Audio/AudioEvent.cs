using System.Collections.Generic;
using Tiger.Util;
using UnityEngine;

namespace Tiger.Audio
{
    /// <inheritdoc />
    /// <summary>
    /// Contains the abstract capability of being a playable audio asset for a given AudioSource 
    /// </summary>
    [Icon("Assets/Tiger/Audio/Editor/Icons/sound.png")]
    public abstract class AudioEvent : ScriptableObject
    {
        /// <summary>
        /// Play this Audio.
        /// </summary>
        /// <param name="source">the AudioSource to play it on.</param>
        public abstract float Play(AudioSource source);
        
        /// <summary>
        /// Play this Audio on game object. The object needs to have an AudioSource component in its hierarchy.
        /// </summary>
        /// <remarks>
        /// This function is generally not recommended, as it is not clear which AudioSource will be used.
        /// Preferably, use the overloads that take an AudioSource or an AudioPool.
        /// </remarks>
        /// <param name="gameObject"></param>
        public void PlaySimple(GameObject gameObject) => Play(gameObject.GetComponent<AudioSource>());

        /// <summary>
        /// Play this Audio on the given ring buffer (IList) of audio sources.
        /// It will play it on the first one, then move that source to the end of the list.
        /// </summary>
        /// <param name="ringBuffer">the AudioSources to shift and then play it on.</param>
        public float Play(IList<AudioSource> ringBuffer)
        {
            var source = ringBuffer.Shift();
            if (source.isPlaying) Debug.LogWarning($"{GetType()}: {source} is already playing. It will be interrupted by AudioEvent {name}. The RingBuffer / AudioPool may be too small", this);
            return Play(source);
        }

        /// <summary>
        /// Play this Audio on a local AudioPool.
        /// </summary>
        /// <param name="pool">pool object that provides the sources</param>
        public float Play(AudioPool pool) => Play(pool.sources);

        /// <summary>
        /// Play this Audio on a centrally managed AudioSource.
        /// </summary>
        /// <param name="position">world position (if unattached) or local position (if attached)</param>
        /// <param name="attachTo">optional transform to follow (in </param>
        /// <returns>AudioSource that is playing the 1-shot.</returns>
        public AudioSource PlayOneShot(Vector3 position = default, Transform attachTo = null)
        {
            var oneShot = new GameObject();
            //TODO: Pull from ring buffer created in RuntimeInitializeOnLoadMethod
            //oneShot = ...                
            
            if (attachTo)
            {
                oneShot.AddComponent<FollowTransform>().target = attachTo;
                oneShot.transform.localPosition = position;
            }
            else
            {
                oneShot.transform.position = position;
            }

            var source = oneShot.AddComponent<AudioSource>();
            var delay = Play(source);

            if (!source.loop)
            {
                oneShot.name = $"\u266b 1-Shot ({name})";
                Destroy(oneShot, source.clip.length + delay + PRE_DESTROY_COOLDOWN);
            }
            else
            {
                oneShot.name = $"\u266b Loop ({name})";
            }

            return source;
        }

        /// <summary>
        /// Play this Audio, following the given transform.
        /// </summary>
        /// <param name="attachTo">transform to follow</param>
        public AudioSource PlayOneShot(Transform attachTo) => PlayOneShot(Vector3.zero, attachTo);

        private const float PRE_DESTROY_COOLDOWN = 0.1f;
    }
}

/*
Written by Moritz Voss in 2018, 2023

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
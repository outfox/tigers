//SPDX-License-Identifier: Unlicense

using System;
using System.Collections.Generic;
using Tiger.Attributes;
using Tiger.Util;
using UnityEngine;
using UnityEngine.Audio;

namespace Tiger.Audio
{
    [Icon("Assets/Tiger/Audio/Editor/sound.png")]
    public class SoundEffect : AudioEvent
    {
        [Header("Audio Palette")]
        public List<AudioClip> clips;

        [Header("Mixer")] 
        public AudioMixerGroup group;

        [Header("Variation Settings")]
        [MinMaxRange(0, 3)]
        public RangedFloat delay;

        public RangedFloat volume = new() {minimum = 0.9f, maximum = 1.0f};
        
        [MinMaxRange(0, 2)]
        public RangedFloat pitch = new() {minimum = 0.9f, maximum = 1.1f};
        
        [Header("Looping")]
        public bool loop;

        [Header("Spatial Parameters")]
        public SpatializerConfig spatializer = new();
        
        public override float Play(AudioSource source)
        {
            source.Stop();
            source.clip = clips.Pick();
            source.pitch = pitch.Random();
            source.volume = volume.Random();
            source.outputAudioMixerGroup = group;

            source.spatialize = spatializer.enabled;
            if (spatializer.enabled)
            {
#if UNITY_WEBGL
                //WebGL has a spatializer issue, case IN-62035
                source.spatialBlend = spatializer.spatialBlend * Mathf.Round(spatializer.spatialBlend);
#else
                source.spatialBlend = spatializer.spatialBlend;
#endif
                source.minDistance = spatializer.minDistance;
                source.maxDistance = spatializer.maxDistance;
                source.rolloffMode = spatializer.rolloffMode;
                source.spread = spatializer.spread;
                source.dopplerLevel = spatializer.dopplerLevel;
            }

            var wait = delay.Random();
            source.PlayDelayed(wait);
            return wait;
        }

        [Serializable]
        public class SpatializerConfig
        {
            public bool enabled = false;
            [Range(0, 1)] public float spatialBlend = 0.5f;
            [Range(0, 360)] public float spread = 90f;
            public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
            public float minDistance = 10f;
            public float maxDistance = 100f;
            public float dopplerLevel = 1f;
        }
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
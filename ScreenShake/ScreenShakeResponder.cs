// SPDX-License-Identifier: Unlicense

using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Tiger.ScreenShake
{
    public class ScreenShakeResponder : MonoBehaviour
    {
        [Header("Object to Shake (e.g. Camera)")]
        public Transform target;

        [Header("Observer to perceive trauma from (e.g. Player)")]
        public Transform perceiver;

        [Header("Shake Frequency")] 
        [Range(1f, 100f)] [Tooltip("Frequency of the noise used for shaking")] public float highFrequency = 30.0f;
        [Range(1f, 100f)] [Tooltip("Frequency of the noise used for shaking")] public float lowFrequency = 5.0f;

        [Header("Trauma ASDR")] 
        [Range(0f, 1f)] public float highFrequencyAttack = 0.0f;
        [Range(0f, 2f)] public float highFrequencySustain = 0.5f;

        [Range(0f, 1f)] public float lowFrequencyAttack = 0.2f;
        [Range(0f, 2f)] public float lowFrequencySustain = 0.8f;

        [Range(0.1f, 4f)] [Tooltip("Shape of the curve, trauma values are from 0..1 and are raised to this power.")]
        public float traumaHFIntensityExponent = 2f;

        [Range(0.1f, 4f)] [Tooltip("Shape of the curve, trauma values are from 0..1 and are raised to this power.")]
        public float traumaLFIntensityExponent = 2f;

        [FormerlySerializedAs("distanceUnitLength")]
        [Header("Distance Falloff")]
        [Tooltip("If your world has a specific scale, you can use this to make the distance falloff match.")]
        public float distanceUnitScale = 1f;
        
        [Range(0.0f, 4f)] [Tooltip("Falloff with distance, 0=no falloff, 2=inverse square law")] 
        public float highFrequencyDistanceExponent = 2f;

        [Range(0.0f, 4f)] [Tooltip("Falloff with distance, 0=no falloff, 2=inverse square law")]
        public float lowFrequencyDistanceExponent = 1f;

        [Header("Amplitudes in units (ShakeType.Positional) or degrees (ShakeType.Rotational)")]
        public Vector3 shakeAxisStrengthsHF = Vector3.one;
        public Vector3 shakeAxisStrengthsLF = Vector3.one;

        
        private float _traumaHF;
        private float _traumaLF;

        private float _traumaHFAttackDerivative;
        private float _traumaLFAttackDerivative;

        private float _traumaHFSustainDerivative;
        private float _traumaLFSustainDerivative;

        private float _traumaHFSmooth;
        private float _traumaLFSmooth;

        private Vector3 _noiseIndex;

        public ShakeType shakeType;

        public enum ShakeType
        {
            Rotational,
            Positional
        }

        private void OnEnable()
        {
            if (!perceiver) perceiver = target;
            _noiseIndex = Random.insideUnitSphere * 10f;
            ScreenShake.Shakes.AddListener(OnShake);
        }


        private void OnDisable()
        {
            ScreenShake.Shakes.RemoveListener(OnShake);
        }


        [ContextMenu("Test Shake Both")]
        public void TestShakeBoth()
        {
            ScreenShake.Add(Vector3.zero, 5, 5);
        }

        [ContextMenu("Test Shake Low")]
        public void TestShakeLow()
        {
            ScreenShake.Add(Vector3.zero, 0, 5);
        }

        [ContextMenu("Test Shake High")]
        public void TestShakeHigh()
        {
            ScreenShake.Add(Vector3.zero, 5, 0);
        }

        private void OnShake(ScreenShake.ShakeEvent shake)
        {
            var distance = Mathf.Max(1, (perceiver.position - shake.position).magnitude * distanceUnitScale);

            _traumaHF += shake.amplitudeHF / Mathf.Pow(distance, highFrequencyDistanceExponent);
            _traumaHF = Mathf.Clamp01(_traumaHF);

            _traumaLF += shake.amplitudeLF / Mathf.Pow(distance, lowFrequencyDistanceExponent);
            _traumaLF = Mathf.Clamp01(_traumaLF);
        }

        private void LateUpdate()
        {
            _traumaHFSmooth = Mathf.SmoothDamp(_traumaHFSmooth, _traumaHF, ref _traumaHFAttackDerivative, highFrequencyAttack);
            _traumaLFSmooth = Mathf.SmoothDamp(_traumaLFSmooth, _traumaLF, ref _traumaLFAttackDerivative, lowFrequencyAttack);

            //Determine the actual shake.
            var effectiveHF = CalculateOffset(Mathf.Pow(_traumaHFSmooth, traumaHFIntensityExponent), highFrequency, shakeAxisStrengthsHF);
            var effectiveLF = CalculateOffset(Mathf.Pow(_traumaLFSmooth, traumaLFIntensityExponent), lowFrequency, shakeAxisStrengthsLF);
            
            switch (shakeType)
            {
                case ShakeType.Rotational:
                    target.localRotation = Quaternion.Euler(effectiveHF + effectiveLF);
                    break;
                case ShakeType.Positional:
                    target.localPosition = effectiveHF + effectiveLF;
                    break;
            }

            _traumaHF = Mathf.SmoothDamp(_traumaHF, 0, ref _traumaHFSustainDerivative, highFrequencySustain);
            _traumaLF = Mathf.SmoothDamp(_traumaLF, 0, ref _traumaLFSustainDerivative, lowFrequencySustain);
        }

        private Vector3 CalculateOffset(float amplitude, float frequency, Vector3 strength)
        {
            return new Vector3()
            {
                x = strength.x * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.x, Time.time * frequency)),
                y = strength.y * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.y, Time.time * frequency)),
                z = strength.z * amplitude * (0.5f - Mathf.PerlinNoise(_noiseIndex.z, Time.time * frequency))
            };
        }
    }
}

/*
Written by Tiger Blue in 2017, 2023 (filters modernized, inspector modernized)

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
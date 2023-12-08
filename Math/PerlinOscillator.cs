using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Tiger.Math
{
    public struct PerlinOscillator
    {
        private float _timeScale;

        private float _amplitude;
        private float2 _amplitude2;
        private float3 _amplitude3;
        private float4 _amplitude4;

        private float _xSeed;
        private float _ySeed;

        public void Init(float scale, float amp)
        {
            _amplitude4 = new float4(amp, amp, amp, amp);
            _amplitude3 = _amplitude4.xyz;
            _amplitude2 = _amplitude4.xy;
            _amplitude = _amplitude4.x;
            _timeScale = scale;
            _xSeed = Random.Range(0, 10 * scale);
            _ySeed = Random.Range(0, 10 * scale);
        }

        public void Init(float scale, float2 amp)
        {
            _amplitude4 = new float4(amp.x, amp.y, math.length(amp), math.length(amp));
            _amplitude3 = _amplitude4.xyz;
            _amplitude2 = _amplitude4.xy;
            _amplitude = _amplitude4.x;
            _timeScale = scale;
            _xSeed = Random.Range(0, 10 * scale);
            _ySeed = Random.Range(0, 10 * scale);
        }

        public void Init(float scale, float3 amp)
        {
            _amplitude4 = new float4(amp.x, amp.y, amp.z, math.length(amp));
            _amplitude3 = _amplitude4.xyz;
            _amplitude2 = _amplitude4.xy;
            _amplitude = _amplitude4.x;
            _timeScale = scale;
            _xSeed = Random.Range(0, 10 * scale);
            _ySeed = Random.Range(0, 10 * scale);
        }

        public void Init(float scale, float4 amp)
        {
            _amplitude4 = amp;
            _amplitude3 = _amplitude4.xyz;
            _amplitude2 = _amplitude4.xy;
            _amplitude = _amplitude4.x;
            _timeScale = scale;
            _xSeed = Random.Range(0, 10 * scale);
            _ySeed = Random.Range(0, 10 * scale);
        }

        private float EvalInternal(float time, int index)
        {
            return noise.cnoise(new float2(_xSeed + time * _timeScale, _ySeed + index * 5));
        }

        public float Eval1D(float time, int index = 0)
        {
            return EvalInternal(time, index) * _amplitude;
        }

        public float2 Eval2D(float time, int index = 0)
        {
            return new float2(
                EvalInternal(time, index),
                EvalInternal(time, index + 1)
            ) * _amplitude2;
        }

        public float3 Eval3D(float time, int index = 0)
        {
            return new float3(
                EvalInternal(time, index),
                EvalInternal(time, index + 1),
                EvalInternal(time, index + 2)
            ) * _amplitude3;
        }

        public float4 Eval4D(float time, int index = 0)
        {
            return new float4(
                EvalInternal(time, index),
                EvalInternal(time, index + 1),
                EvalInternal(time, index + 2),
                EvalInternal(time, index + 3)
            ) * _amplitude4;
        }
    }
}
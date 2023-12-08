using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Tiger.Audio
{
    [RequireComponent(typeof(Slider))]
    [DisallowMultipleComponent]
    public class MixerGroupSlider : MonoBehaviour
    {
        public AudioMixerGroup group;

        [Tooltip("The minimum decibel value of the slider")]
        public float minDB = -80;

        [Tooltip("The maximum decibel value of the slider")]
        public float maxDB = 3;

        [Tooltip("Additionally scale the slider along a nonlinear curve to give more precision in the middle to top range.")]
        [Range(0.1f, 1f)]
        public float linearity = 0.5f;

        [SerializeField]
        [HideInInspector]
        private Slider slider;
        
        private string prefsKey => $"MixerGroup/{group.name}";

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }
    
        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void Start()
        {
            var value = PlayerPrefs.HasKey(prefsKey) ? PlayerPrefs.GetFloat(prefsKey) : ReadInitialValueFromMixer();
            OnValueChanged(value);
            slider.value = value;
        }
        
        private float ReadInitialValueFromMixer()
        {
            group.audioMixer.GetFloat(group.name, out var decibels);
            var normalized = math.saturate(math.remap(minDB, maxDB, 0, 1, decibels));
            var nonlinear = math.pow(normalized, 1f / linearity);
            var exponential = math.pow(10f, nonlinear);
            var remapped = math.remap(1, 10, 0, 1, exponential);
            return math.saturate(remapped);
        }

        private void OnValueChanged(float sliderValue)
        {
            var remapped = math.remap(0, 1, 1, 10, slider.value);
            var logarithmic = math.saturate(math.log10(remapped));
            var nonlinear = math.pow(logarithmic, linearity);
            var decibels = math.remap(0f, 1f, minDB, maxDB, nonlinear);
            group.audioMixer.SetFloat(group.name, decibels);
            PlayerPrefs.SetFloat(prefsKey, slider.value);
        }

        private void OnValidate()
        {
            if (!slider) slider = GetComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
        }
    }
}

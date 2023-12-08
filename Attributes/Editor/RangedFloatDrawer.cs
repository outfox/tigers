#if UNITY_EDITOR
using Tiger.Audio;
using UnityEditor;
using UnityEngine;

namespace Tiger.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(Util.RangedFloat), true)]
    public class RangedFloatDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            var minProp = property.FindPropertyRelative("minimum");
            var maxProp = property.FindPropertyRelative("maximum");

            var minValue = minProp.floatValue;
            var maxValue = maxProp.floatValue;

            float rangeMin = 0;
            float rangeMax = 1;

            bool step = false;

            var ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof (MinMaxRangeAttribute), true);
            if (ranges.Length > 0)
            {
                rangeMin = ranges[0].min;
                rangeMax = ranges[0].max;
                step = ranges[0].integer;
            }

            const float range_bounds_label_width = 40f;

            var rangeBoundsLabel1Rect = new Rect(position);
            rangeBoundsLabel1Rect.width = range_bounds_label_width;
            GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString("F2")));
            position.xMin += range_bounds_label_width;

            var rangeBoundsLabel2Rect = new Rect(position);
            rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - range_bounds_label_width;
            GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString("F2")));
            position.xMax -= range_bounds_label_width;

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
            if (EditorGUI.EndChangeCheck())
            {
                if (step)
                {
                    minValue = Mathf.Round(minValue);
                    maxValue = Mathf.Round(maxValue);
                }
            
                minProp.floatValue = minValue;
                maxProp.floatValue = maxValue;
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif

/*
Written by Tiger Blue in 2018

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
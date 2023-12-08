using System;
using System.Reflection;
using UnityEngine;

namespace Tiger.Attributes
{
    public class AutoBehaviour : MonoBehaviour
    {
        [AttributeUsage(AttributeTargets.Field)]
        protected class AutoAttribute : PropertyAttribute
        {
        }
        
        protected virtual void OnValidateCustom()
        {
        }

        private void OnValidate()
        {
            OnValidateCustom();
            var object_fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field_info in object_fields)
            {
                if (Attribute.GetCustomAttribute(field_info, typeof(AutoAttribute)) is not AutoAttribute) continue;

                var value = GetComponent(field_info.FieldType);
                field_info.SetValue(this, value);
            }
        }
    }
}
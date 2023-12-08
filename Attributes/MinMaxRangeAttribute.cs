using System;

namespace Tiger.Attributes
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute(float min, float max, bool integer = false)
        {
            this.min = min;
            this.max = max;
            this.integer = integer;
        }

        public float min { get; private set; }
        public float max { get; private set; }
        public bool integer { get; private set; }
    }
}
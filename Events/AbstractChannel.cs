using UnityEngine;
using UnityEngine.Scripting;

namespace Tiger.Events
{
    [Preserve]
    public abstract class AbstractChannel : ScriptableObject
    {
        protected internal abstract void Init();
    }
}
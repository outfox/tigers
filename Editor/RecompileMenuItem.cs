using UnityEditor;
using UnityEditor.Compilation;

namespace Tiger.Editor
{
    public abstract class RecompileMenuItem
    {
        [MenuItem("Tiger/Recompile")]
        public static void Recompile()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}

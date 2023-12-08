using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jovian
{
    public static class MeshMenu
    {
        [MenuItem("Tiger/Mesh/Smooth Normals")]
        public static void SmoothNormals()
        {
            //Select Mesh Asset, change import settings
            var mesh = Selection.activeObject as Mesh;
            if (mesh == null) return;
            var path = AssetDatabase.GetAssetPath(mesh);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer == null) return;
            importer.normalSmoothingAngle = 180;
            importer.importNormals = ModelImporterNormals.Calculate;
            importer.importTangents = ModelImporterTangents.CalculateMikk;
            importer.SaveAndReimport();
        }
    }
}

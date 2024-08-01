
using UnityEngine;
using UnityEditor;

namespace com.mitsukaki.expressive.editor
{
    public static class AssetUtility
    {
        public static GameObject AttatchPrefabFromGUID(
            Transform targetObject, string assetGUID
        )
        {
            // create an instance of the prefab on the target
            var templateAssetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(
                AssetDatabase.LoadAssetAtPath<GameObject>(templateAssetPath),
                targetObject
            ) as GameObject;

            // unpack the prefab
            PrefabUtility.UnpackPrefabInstance(
                prefabInstance,
                PrefabUnpackMode.Completely,
                InteractionMode.AutomatedAction
            );

            return prefabInstance;
        }

        public static Object CreateSerializedClone(
            Object original,
            string extension = "Asset"
        )
        {
            // clone the menu
            var clone = Object.Instantiate(original);

            CreateGeneratedDirectories();

            // write the menu to the disk
            var clonePath = "Assets/CellKit/Generated/" + RandomAssetName(extension);
            AssetDatabase.CreateAsset(clone, clonePath);

            return clone;
        }

        public static void CreateGeneratedDirectories()
        {
            if (!AssetDatabase.IsValidFolder("Assets/CellKit"))
                AssetDatabase.CreateFolder("Assets", "CellKit");

            if (!AssetDatabase.IsValidFolder("Assets/CellKit/Generated"))
                AssetDatabase.CreateFolder("Assets/CellKit", "Generated");
        }

        public static string RandomAssetName(string extension)
        {
            string ID = System.Guid.NewGuid().ToString();
            return extension + "Asset-" + ID + "." + extension.ToLower();
        }
    }
}
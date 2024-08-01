using UnityEngine;
using UnityEditor;

namespace com.mitsukaki.expressive.editor
{

    [CustomEditor(typeof(ExpressiveContainer))]
    public class ContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var container = (ExpressiveContainer)target;

            // Button to show the editor
            if (GUILayout.Button("Open Editor"))
            {
                ExpresssiveEditor.ShowWindow();
            }

            // Button to rebuild the animations
            if (GUILayout.Button("Rebuild"))
            {
                Assembler.Build(container);
            }

            base.OnInspectorGUI();

            // // Text input for the output path
            // var outputPath = EditorGUILayout.TextField("Output Path", container.OutputPath);
            // container.OutputPath = outputPath;
        }
    }

}
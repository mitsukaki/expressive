using UnityEngine;
using UnityEditor;

namespace com.mitsukaki.expressive.editor
{
    public class ExpresssiveEditor : EditorWindow
    {
        [MenuItem("Window/Expressive")]
        public static void ShowWindow()
        {
            GetWindow<ExpresssiveEditor>("Expressive");
        }

        private void OnGUI()
        {
            GUILayout.Label("Expressive", EditorStyles.boldLabel);

            // ExpressiveContainer input for user to enter the container
            var container = (ExpressiveContainer)EditorGUILayout.ObjectField(
                "Container", null, typeof(ExpressiveContainer), false
            );

            // Button to build the animations
            if (GUILayout.Button("Build"))
            {
                Assembler.Build(container);
            }
        }
    }

}
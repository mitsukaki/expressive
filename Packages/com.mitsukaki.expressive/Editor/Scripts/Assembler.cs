using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace com.mitsukaki.expressive.editor
{

    public class Assembler
    {
        public static bool Build(ExpressiveContainer container)
        {
            if (!Validate(container)) return false;

            CreatePath(container.OutputPath);

            // For every clip in the container
            foreach (ExpressionClip expClip in container.Clips)
            {
                // If the clip is null, skip it
                if (expClip.SourceClip == null)
                {
                    Debug.LogWarning("Found a null clip! skipping");
                    continue;
                }

                // If the clip does not have a duration set
                if (expClip.Duration <= 0)
                    // Set the clip's duration to the default duration
                    expClip.Duration = container.DefaultDuration;

                // Create a new animation clip
                string clipName = expClip.SourceClip.name + "_Expressive";
                AnimationClip clip = new AnimationClip
                {
                    name = clipName,
                    wrapMode = WrapMode.Loop,
                    frameRate = 60.0f
                };

                // Build the curves
                List<string> mappedBlendshapes = new List<string>();
                string headPath = GetClipPath(expClip.SourceClip);
                foreach (ExpressionTrack track in expClip.Tracks)
                {
                    if (track.blendshapes.Length == 0)
                    {
                        Debug.LogWarning("No blendshapes in track! skipping");
                        continue;
                    }

                    // Create a new curve
                    AnimationCurve trackCurve = CurveBuilder.MakeCurve(
                        track, expClip.Duration
                    );

                    // Add the curve to the clip
                    clip.SetCurve(
                        headPath, typeof(SkinnedMeshRenderer),
                        track.blendshapes[0], trackCurve
                    );

                    // Add the blendshape to the list of mapped blendshapes
                    mappedBlendshapes.Add(track.blendshapes[0]);
                }

                // Map all the remaining blendshapes to constant curves
                var sourceBindings = AnimationUtility.GetCurveBindings(expClip.SourceClip);
                foreach (var binding in sourceBindings)
                {
                    // If the blendshape is not in the list of mapped blendshapes
                    if (mappedBlendshapes.Contains(binding.propertyName))
                        continue;

                    Debug.LogWarning("Mapping blendshape: " + binding.propertyName + " to constant curve");

                    // Create a new curve
                    var curve = AnimationUtility.GetEditorCurve(expClip.SourceClip, binding);
                    AnimationCurve trackCurve = CurveBuilder.CreateConstantCurve(
                        curve.keys[0].value, expClip.Duration
                    );

                    // Add the curve to the clip
                    clip.SetCurve(
                        headPath, typeof(SkinnedMeshRenderer),
                        binding.propertyName, trackCurve
                    );
                }

                // Save the animation clip to the asset database
                AssetDatabase.CreateAsset(clip, GetClipPath(container.OutputPath, clipName));
                AssetDatabase.SaveAssets();
            }

            Debug.Log("Build complete!");

            return true;
        }

        private static void DumpClip(AnimationClip clip)
        {
            Debug.Log("Clip name: " + clip.name);
            Debug.Log("Clip length: " + clip.length);
            Debug.Log("Clip frame rate: " + clip.frameRate);
            Debug.Log("Clip wrap mode: " + clip.wrapMode);

            // Print all curves and bindings
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                Debug.Log("Binding path: " + binding.path);
                Debug.Log("Binding property name: " + binding.propertyName);
                Debug.Log("Binding type: " + binding.type);
                Debug.Log("Binding is PPTR: " + binding.isPPtrCurve);
            }
        }

        private static string GetClipPath(AnimationClip clip)
        {
            return AnimationUtility.GetCurveBindings(clip)[0].path;
        }

        private static string GetClipPath(string outputPath, string clipName)
        {
            return "Assets/" + outputPath + clipName + ".anim";
        }

        private static bool Validate(ExpressiveContainer container)
        {
            if (container == null)
            {
                Debug.LogError("Container is null!");
                return false;
            }

            if (container.Clips == null)
            {
                Debug.LogError("Container clips are null!");
                return false;
            }

            if (
                container.OutputPath == null ||
                container.OutputPath == ""
            )
            {
                Debug.LogError("Output path is null!");
                return false;
            }

            // if the output path does not end with a slash
            if (container.OutputPath[container.OutputPath.Length - 1] != '/')
            {
                // Add a slash to the end of the output path
                container.OutputPath += "/";
            }

            // TODO: Validate the output path to ensure it is a valid path

            // TODO: Validate the clips in the container

            return true;
        }

        private static void CreatePath(string path)
        {
            // Split the path into folders
            string[] folders = path.Split('/');

            // Initialize the current path
            string currentPath = "Assets";

            // For every folder in the path
            foreach (string folder in folders)
            {
                // If the folder is empty, skip it
                if (folder == "") continue;

                // Create the folder if it does not exist
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folder))
                    AssetDatabase.CreateFolder(currentPath, folder);

                // Append the folder to the current path
                currentPath += "/" + folder;
            }
        }
    }
}
#if UNITY_EDITOR
using UnityEngine;
using N;
using N.Tests;
using System;
using System.IO;
using System.Collections.Generic;

namespace N.Package.Data
{
    /// Drag this onto a component to generate manifest files
    [ExecuteInEditMode]
    public class ManifestBuilder : MonoBehaviour
    {
        public void Start()
        {
            ManifestBuilder.Run();
            this.RemoveComponents<ManifestBuilder>(true);
        }

        public static void Run()
        {
            ManifestBuilder.Run(null);
        }

        public static void Run(string restrictPath)
        {
            try
            {
                // Expand to specific target, if restricted
                var target = ".*Resources.*";
                if (restrictPath != null)
                {
                    target += restrictPath + ".*";
                }

                // Find all the folders and generate a manifest
                var resources = Project.AllDirs(target);
                foreach (var path in resources)
                {
                    if (!path.EndsWith("Resources"))
                    {
                        var files = Project.Files(path, ".*");
                        for (var i = 0; i < files.Length; ++i)
                        {
                            files[i] = Path.GetFileNameWithoutExtension(files[i]);
                        }
                        var dirs = Project.Dirs(path, ".*");
                        for (var i = 0; i < dirs.Length; ++i)
                        {
                            var value = dirs[i].Replace(path, "");
                            value = value.Replace("/", "");
                            value = value.Replace("\\", "");
                            dirs[i] = value;
                        }
                        var manifest = new Manifest() { files = files, folders = dirs };
                        var output = Json.Serialize(manifest);
                        System.IO.File.WriteAllText(Path.Combine(path, "manifest.json"), output);
                        N.Console.Log("Generated manifest for: {0}", path);
                    }
                }
            }
            catch (Exception err)
            {
                N.Console.Log("Failed to run manifest builder");
                N.Console.Error(err);
            }
        }
    }
}
#endif

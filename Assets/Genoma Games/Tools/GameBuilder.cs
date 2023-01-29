#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GameBuilder
{
    private static string buildsDirectory = "Builds";

    [MenuItem("Tools/Genoma Games/Build Game")]
    public static void Build()
    {
        if (BuildPipeline.isBuildingPlayer)
        {
            throw new UnityException("A build is already in progress.");
        }

        string gitCommitDescription = Git.Describe();

        string pattern = @"v(\d+\.\d+\.\d+)-(\d+)-(g[a-f0-9]{4,})(?:-(dirty))?";
        Regex regex = new Regex(pattern);


        if (!regex.IsMatch(gitCommitDescription))
        {
            throw new UnityException($"Can not parse version from git describe: {gitCommitDescription}");
        }

        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        string substitution = $@"$1.$2.{timestamp}";

        string version = regex.Replace(gitCommitDescription, substitution);

        Debug.Log($"Building version {version}");

        PlayerSettings.bundleVersion = version;

        string projectPath = Directory.GetCurrentDirectory();

        string buildsPath = Path.Combine(projectPath, buildsDirectory);

        string[] scenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions();

        options.scenes = scenePaths;
        options.target = BuildTarget.WebGL;
        options.locationPathName = Path.Combine(buildsPath, options.target.ToString(), $"{PlayerSettings.productName} v{version} ({options.target})");
        options.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            decimal sizeInMb = Math.Round((decimal)summary.totalSize / 1024 / 1024, 2);
            Debug.Log($"Build succeeded: {sizeInMb}MB");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
#endif

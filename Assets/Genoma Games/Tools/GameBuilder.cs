#if UNITY_EDITOR
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GameBuilder
{
    private static readonly string buildsDirectory = "Builds";

    [MenuItem("Tools/Genoma Games/Build Game (Development)")]
    public static void BuildDevelopment() => Build(isDevelopment: true);

    [MenuItem("Tools/Genoma Games/Build Game (Production)")]
    public static void BuildProduction() => Build(isDevelopment: false);

    private static void Build(bool isDevelopment)
    {
        bool isProduction = !isDevelopment;

        if (BuildPipeline.isBuildingPlayer)
        {
            throw new UnityException("A build is already in progress.");
        }

        string currentVersion = PlayerSettings.bundleVersion;

        PlayerSettings.bundleVersion = GetBuildVersion(isDevelopment);

        BuildReport report = BuildPlayers();
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            if (isProduction)
            {
                string zipPath = $@"{summary.outputPath}.zip";

                ZipFile.CreateFromDirectory(summary.outputPath, zipPath);
            }
        }

        if (isDevelopment || summary.result != BuildResult.Succeeded)
        {
            PlayerSettings.bundleVersion = currentVersion;
        }
    }

    private static string GetBuildVersion(bool isDevelopment)
    {
        string gitCommitDescription = Git.Describe();

        string parseGitCommitDescriptionPattern = @"v(?'Version'\d+\.\d+\.\d+)-(?'ExtraCommits'\d+)-(?'Hash'g[a-f0-9]{4,})(?:-(?'Dirty'dirty))?";

        Match match = Regex.Match(gitCommitDescription, parseGitCommitDescriptionPattern);

        if (!match.Success)
        {
            throw new UnityException($"Can not parse version from Git.Describe(): {gitCommitDescription}");
        }

        string buildVersion = $"{match.Groups["Version"].Value}";

        if (isDevelopment)
        {
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            buildVersion += $".{timestamp}";
        }
        else
        {
            if (match.Groups["Dirty"].Success)
            {
                throw new UnityException("There are uncommitted changes.");
            }

            if (int.Parse(match.Groups["ExtraCommits"].Value) != 0)
            {
                throw new UnityException("The current commit does not have a version tag.");
            }
        }

        return buildVersion;
    }

    private static BuildReport BuildPlayers()
    {
        Debug.Log($"Building version {PlayerSettings.bundleVersion}");

        string projectPath = Directory.GetCurrentDirectory();

        string buildsPath = Path.Combine(projectPath, buildsDirectory);

        string[] scenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

        BuildPlayerOptions webGLBuildOptions = new BuildPlayerOptions
        {
            scenes = scenePaths,
            target = BuildTarget.WebGL,
            locationPathName = Path.Combine(buildsPath, BuildTarget.WebGL.ToString(), $"{PlayerSettings.productName} v{PlayerSettings.bundleVersion} ({BuildTarget.WebGL})"),
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(webGLBuildOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            decimal sizeInMb = Math.Round((decimal)summary.totalSize / 1024 / 1024, 2);
            Debug.Log($"Build succeeded: {sizeInMb}MB");
        }
        else
        {
            Debug.LogError($"Build {summary.result}");
        }

        return report;
    }
}
#endif

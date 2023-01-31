#if UNITY_EDITOR
using System;
using System.Collections.Generic;
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

        Dictionary<BuildTarget, BuildReport> reports = BuildPlayers();

        if (reports.TryGetValue(BuildTarget.WebGL, out BuildReport webGLReport))
        {
            BuildSummary summary = webGLReport.summary;

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
        else
        {
            PlayerSettings.bundleVersion = currentVersion;

            throw new UnityException("Unable to obtain WebGL build report");
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

    private static Dictionary<BuildTarget, BuildReport> BuildPlayers()
    {
        string projectPath = Directory.GetCurrentDirectory();

        string buildsPath = Path.Combine(projectPath, buildsDirectory);

        string[] scenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

        BuildTarget[] buildTargets = new BuildTarget[]
        {
            BuildTarget.Android,
            BuildTarget.WebGL,
        };

        Dictionary<BuildTarget, BuildReport> reports = new Dictionary<BuildTarget, BuildReport>();

        foreach (BuildTarget buildTarget in buildTargets)
        {
            BuildReport report = BuildPlayer(buildTarget, scenePaths, buildsPath);

            reports.Add(buildTarget, report);
        }

        return reports;
    }

    private static BuildReport BuildPlayer(BuildTarget target, string[] scenePaths, string buildsPath)
    {
        BuildReport report;

        Debug.Log($"Building version {PlayerSettings.bundleVersion} for {target}");

        switch (target)
        {
            case BuildTarget.Android:
                BuildPlayerOptions androidBuildOptions = new BuildPlayerOptions
                {
                    scenes = scenePaths,
                    target = BuildTarget.Android,
                    locationPathName = Path.Combine(buildsPath, BuildTarget.Android.ToString(), $"{PlayerSettings.productName} v{PlayerSettings.bundleVersion} ({BuildTarget.Android}).apk"),
                    options = BuildOptions.None,
                };

                report = BuildPipeline.BuildPlayer(androidBuildOptions);
                break;
            case BuildTarget.WebGL:
                BuildPlayerOptions webGLBuildOptions = new BuildPlayerOptions
                {
                    scenes = scenePaths,
                    target = BuildTarget.WebGL,
                    locationPathName = Path.Combine(buildsPath, BuildTarget.WebGL.ToString(), $"{PlayerSettings.productName} v{PlayerSettings.bundleVersion} ({BuildTarget.WebGL})"),
                    options = BuildOptions.None,
                };

                report = BuildPipeline.BuildPlayer(webGLBuildOptions);
                break;
            default:
                throw new UnityException($"{target} is not supported.");
        }

        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            decimal sizeInMb = Math.Round((decimal)summary.totalSize / 1024 / 1024, 2);
            Debug.Log($"{target} build succeeded: {sizeInMb}MB");
        }
        else
        {
            throw new UnityException($"{target} build {summary.result}");
        }

        return report;
    }
}
#endif

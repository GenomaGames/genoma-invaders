#if UNITY_EDITOR
/**
 * Taken from
 * https://blog.redbluegames.com/version-numbering-for-games-in-unity-and-git-1d05fca83022
 */
using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

/// <summary>
/// GitException includes the error output from a Git.Run() command as well as the
/// ExitCode it returned.
/// </summary>
public class GitException : InvalidOperationException
{
    public GitException(int exitCode, string errors) : base(errors) =>
        this.ExitCode = exitCode;

    /// <summary>
    /// The exit code returned when running the Git command.
    /// </summary>
    public readonly int ExitCode;
}

public static class Git
{
    public static string Describe()
    {
        string describe = Run(@"describe --tags --long --dirty --broken");

        return describe;
    }

    /// <summary>
    /// Runs git.exe with the specified arguments and returns the output.
    /// </summary>
    public static string Run(string arguments)
    {
        int exitCode = ProcessRun(
            @"git",
            arguments,
            Application.dataPath,
            out var output,
            out var errors
        );
        
        if (exitCode == 0)
        {
            return output;
        }
        else
        {
            throw new GitException(exitCode, errors);
        }
    }

    private static int ProcessRun(
        string application,
        string arguments,
        string workingDirectory,
        out string output,
        out string errors
    )
    {
        using Process process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            FileName = application,
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };

        // Use the following event to read both output and errors output.
        var outputBuilder = new StringBuilder();
        var errorsBuilder = new StringBuilder();
        process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
        process.ErrorDataReceived += (_, args) => errorsBuilder.AppendLine(args.Data);

        // Start the process and wait for it to exit.
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        output = outputBuilder.ToString().TrimEnd();
        errors = errorsBuilder.ToString().TrimEnd();
        return process.ExitCode;
    }
}
#endif

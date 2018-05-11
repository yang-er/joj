using System;
using System.Collections.Generic;

namespace JudgeCore
{
    public interface ICompiler
    {
        List<string> IncludePath { get; }
        List<string> LibraryPath { get; }
        List<string> Options { get; }
        List<string> ToolchainPath { get; }
        string MasterPath { get; }
        string StandardError { get; }
        string StandardOutput { get; }
        int ExitCode { get; }
        bool Compile(string ans, Guid identify);
    }
}

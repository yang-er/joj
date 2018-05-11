using System;

namespace JudgeCore
{
    interface ICompiler
    {
        string IncludePath { get; }
        string Options { get; }
        string LibraryDirs { get; }
        string MasterPath { get; }
        string ExtraPath { get; }
        bool Compile(string ans, Guid identify);
    }
}

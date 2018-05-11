using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeCore.Compiler
{
    public class Msvc : ICompiler
    {
        private string VCVersion => "14.13.26128";
        private string WinVersion => "10.0.16299.0";
        private string VS2017dir => @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community";
        private string WinKit => @"C:\Program Files (x86)\Windows Kits\10\bin\{winv}";
        public string IncludePath => VS2017dir + @"\VC\Tools\MSVC\{vcv}\include;" + @"C:\Program Files (x86)\Windows Kits\10\Include\10.0.16299.0\ucrt;C:\Program Files (x86)\Windows Kits\10\bin\10.0.16299.0\x64;";
        public string Options => @"/DDEBUG;";
        public string LibraryDirs => MasterPath + @"\lib\x64;C:\Program Files (x86)\Windows Kits\10\Lib\10.0.16299.0\ucrt\x64;C:\Program Files (x86)\Windows Kits\10\Lib\10.0.16299.0\um\x64;";
        public string MasterPath => VS2017dir + @"\VC\Tools\MSVC\{vcv}";
        public string ExtraPath => VS2017dir + @"\Common7\IDE;C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\{vcv}\bin\Hostx64\x64;C:\Program Files (x86)\Windows Kits\10\bin\10.0.16299.0\x64;";

        public bool Compile(string ans, Guid identify)
        {
            throw new NotImplementedException();
        }
    }
}

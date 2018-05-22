using System;
using System.IO;
using System.Text;

namespace JudgeProblemCreate
{
    class Program
    {
        static readonly string prefix = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<problem>
	<id>1002</id>
	<title><![CDATA[Expression Calculate]]></title>
	<description><![CDATA[Calculate an expression.]]></description>
	<input><![CDATA[One line, which contains the expression, ended with =.]]></input>
	<output><![CDATA[Output the result (integer) in one line.]]></output>
	<author><![CDATA[fmgu2000]]></author>
	<time_limit>1000</time_limit>
	<memory_limit>32</memory_limit>

	<samples>
		<group>
			<input><![CDATA[(1+30)/3=
]]></input>
			<output><![CDATA[10
]]></output>
		</group>
	</samples>

	<test_cases>";
        static readonly string postfix = @"
	</test_cases>

	<hint><![CDATA[]]></hint>
</problem>
";

        static void Main(string[] args)
        {
            Console.Title = "JOJ Problem Creator";
            Console.WriteLine("Usage: dotnet.exe JudgeProblemCreate.dll");
            Console.WriteLine(" - Files like aaaa1.in, aaaa1.out, aaaa2.in, aaaa2.out, etc.");
            Console.WriteLine();
            Console.Write("The folder you want to read (like `F:\\Judge\\aaaa\\`) : ");
            var folder = Console.ReadLine();
            Console.Write("The problem appendix (like `aaaa` above) : ");
            var name = Console.ReadLine();
            Console.WriteLine();

            var sb = new StringBuilder();
            sb.Append(prefix);
            int p = 0;
            try
            {
                for (p = 1; ; p++)
                {
                    sb.Append("\r\n		<group>\r\n			<input><![CDATA[" +
                        File.ReadAllText($"{folder}\\{name}{p}.in") +
                        "]]></input>\r\n			<output><![CDATA[" +
                        File.ReadAllText($"{folder}\\{name}{p}.out") +
                        "]]></output>\r\n		</group>");
                }
            }
            catch { }
            sb.Append(postfix);

            Console.WriteLine($"{p} entries added to the file. \nPlease modify the descriptions and samples.");
            File.WriteAllText($"{folder}\\{name}.xml", sb.ToString());
        }
    }
}

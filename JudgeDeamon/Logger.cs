using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeDaemon
{
    class Logger : TextWriter
    {
        public StreamWriter inner;

        public Logger(string filename)
        {
            inner = new StreamWriter(new FileStream(filename, FileMode.Append));
            Encoding = Console.InputEncoding;
        }

        public Logger(StreamWriter sw)
        {
            inner = sw;
            Encoding = sw.Encoding;
        }

        public override Encoding Encoding { get; }

        public override void WriteLine()
        {
            inner.WriteLine();
            inner.Flush();
        }
        
        public override void WriteLine(string value)
        {
            inner.WriteLine(value);
            inner.Flush();
        }

        public override void Flush()
        {
            inner.Flush();
        }
        
        public static void SetTracer(bool daemon, Action clean, int id)
        {
            if (!daemon)
            {
                if (Console.IsErrorRedirected)
                {
                    Console.WriteLine("Debugger not trigged, putting into stderr..");
                    Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
                }
                else
                {
                    Console.WriteLine("Debugger trigged, using OutputDebugString..");
                }

                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("Ctrl-C detected.");
                    clean();
                    e.Cancel = true;
                };
            }
            else
            {
                Console.In.Close();
                var sw = new Logger($"joj_server{id}.log");
                Console.SetOut(sw);
                Console.SetError(sw);
                Trace.Listeners.Clear();
                Trace.Listeners.Add(new TextWriterTraceListener(sw));
            }
        }
    }
}

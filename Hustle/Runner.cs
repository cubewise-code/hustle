using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Cubewise.Hustle
{
    public class Runner
    {
        string[] _commands;
        int _threads = 0;
        int _maxThreads = 0;

        public Runner(string batchListPath, int maxThreads)
        {
            _maxThreads = maxThreads;
            try
            {
                _commands = File.ReadAllLines(batchListPath);
                int count = 0;
                foreach (string command in _commands)
                {
                    if (command.Trim() != "")
                        count++;
                }

                Program.Log("Lines to be executed: {0}", count);

                // Stop on infinite loop starting
                if (_maxThreads > count)
                {
                    _maxThreads = count;
                    Program.Log("Max threads changed to: {0}", count);
                }

                Program.Log("");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to get commands from file", ex);
            }
        }

        public void Run()
        {

            Program.Log("Starting processes...");
            Program.Log("");

            int row = 1;
            foreach (string command in _commands)
            {
                if (command.Trim() == "")
                    continue;

                while (_threads >= _maxThreads)
                {
                    System.Threading.Thread.Sleep(100);
                }

                RunCommand(command, string.Format("Row {0}", row));
                row++;
            }

            // Wait for all thread to complete
            while (_threads > 0)
            {
                System.Threading.Thread.Sleep(100);
            }
        }


        void RunCommand(object arg, string id)
        {

            string command = arg.ToString();

            try
            {
                _threads++;

                Program.Log("{0} ::: START", id);
                Process proc = new Process();

                int exe = command.IndexOf(".exe");
                if (exe == -1)
                {
                    Program.Log("The process requires an executable (exe)");
                    return;
                }

                int space = command.IndexOf(" ", exe + 4);
                string fileName = command.Substring(0, space);
                string args = command.Substring(space);
                //Program.Log("\tFile Name: " + fileName);
                //Program.Log("\tArguments: " + args);

                proc.StartInfo.FileName = fileName;
                proc.StartInfo.Arguments = args;

                // set up output redirection
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.CreateNoWindow = true;

                StringBuilder result = new StringBuilder();

                // add event handlers
                proc.OutputDataReceived += delegate(object sender1, DataReceivedEventArgs e1)
                {
                    if (string.IsNullOrEmpty(e1.Data) == false)
                    {
                        Program.Log("{0} ::: {1}", id, e1.Data);
                    }
                };

                proc.ErrorDataReceived += delegate(object sender1, DataReceivedEventArgs e1)
                {
                    if (string.IsNullOrEmpty(e1.Data) == false)
                    {
                        Program.Log("{0} ::: {1}", id, e1.Data);
                    }
                };

                proc.Exited += delegate(object sender, EventArgs e)
                {
                    Program.Log("{0} ::: COMPLETE", id);
                    _threads--;
                };

                proc.Start();

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

            }
            catch (Exception ex)
            {
                Program.Log("{0} ::: ERROR", id);
                Program.Log("\t", ex.Message);
                _threads--;
            }
        }

    }
}

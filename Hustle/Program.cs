using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cubewise.Hustle
{
    class Program
    {

        static string batchListPath;

        static int Main(string[] args)
        {

            Console.InputEncoding = Encoding.UTF8;

            if (args.Length != 2)
            {
                Console.WriteLine("2 Arguments must be provided: ");
                Console.WriteLine("\tThe file path to batch file list, eg. BatchList.txt");
                Console.WriteLine("\tThe number of threads to use, eg. 7");
                return 1;
            }


            batchListPath = args[0];
            int maxThreads = 0;
            try
            {
                maxThreads = int.Parse(args[1]);
            }
            catch (Exception)
            {
                Log("Max threads is not a valid number: {0}", args[1]);
                return 1;
            }

            Log("");
            Log("Hustle parameters: ");
            Log("\tFile path for batch is {0}", batchListPath);
            Log("\tMaximum number of threads is {0}", maxThreads);
            Log("");

            if (File.Exists(batchListPath) == false)
            {
                Log("ERROR: File {0} does not exist", batchListPath);
                return 1;
            }

            Runner runner = null;
            try
            {
                runner = new Runner(batchListPath, maxThreads);
            }
            catch (Exception ex)
            {
                Log("ERROR: Unable to create runner: " + ex.Message);
                return 1;
            }

            try
            {
                runner.Run();
            }
            catch (Exception ex)
            {
                Log("Unable to run: " + ex.Message);
                return 1;
            }        

            return 0;

        }

        public static void Log(string text)
        {
            Log(text, new object[]{});
        }

        public static void Log(string text, params object[] args)
        {
            Console.WriteLine(text, args);

        }
        
    }
}

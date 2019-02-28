using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HashCode2019
{
    class Program
    {
        private static string _inputFilepath = @"C:\HashCodes\2019\in\";
        private static string _outputFilepath = @"C:\HashCodes\2019\out\";
        private static string _inputFileName = @"e_shiny_selfies.txt";
        static void Main(string[] args)
        {
            var input = readInput(Path.Combine(_inputFilepath, _inputFileName));

            var processedResult = ProcessSolution.run(input);
            
            writeOutput(Path.Combine(_outputFilepath, "out.txt"), processedResult);

        }

        private static List<string> readInput(string filepath)
        {
            return new List<string>(File.ReadAllLines(filepath));
        }

        private static void writeOutput(string filepath, List<string> lines)
        {
            File.WriteAllLines(filepath, lines);
        }

    }
}

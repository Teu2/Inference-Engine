using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            bool fileNotValid = true;
            string fileName = string.Empty;

            while (fileNotValid)
            {
                Console.Write("Enter Test Case (include '.txt'): ");
                fileName = Console.ReadLine();

                if (File.Exists(fileName))
                {
                    Console.WriteLine();
                    fileNotValid = false;
                }
                else
                {
                    Console.WriteLine("File not found. \n");
                }
            }

            KnowledgeBase KB = new KnowledgeBase();
            KB.GetKnowledgeBase(fileName);

            Console.WriteLine($"KB: {KB.DisplayFullClause} \nAsk: {KB.Ask}\n");

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("1. Run Inference Engine (TT, FC & BC) \n2. Change Knowledge Base \n3. Change Ask Value");
                Console.Write("\nOption: "); 
                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1: TruthTable TT = new TruthTable(KB);
                            ForwardChaining FC = new ForwardChaining(KB);
                            BackwardChaining BC = new BackwardChaining(KB);
                            break;

                    case 2: KB.Clause.Clear();
                            KB.SingleClauses.Clear();
                            fileNotValid = true;
                            while (fileNotValid)
                            {
                                Console.Write("\nEnter new file e.g (test_HornKB.txt): ");
                                fileName = Console.ReadLine();

                                if (File.Exists(fileName))
                                {
                                    Console.WriteLine();
                                    fileNotValid = false;
                                }
                                else
                                {
                                    Console.WriteLine("File not found. \n");
                                }
                            }
                            KB.GetKnowledgeBase(fileName);
                            Console.WriteLine($"\nKB: {KB.DisplayFullClause} \nAsk: {KB.Ask}\n");
                            break;

                    case 3: Console.Write("\nEnter new ask: ");
                            KB.Ask = Console.ReadLine(); 
                            Console.WriteLine($"\nKB: {KB.DisplayFullClause} \nAsk: {KB.Ask}\n");
                            break;

                    default: Console.WriteLine("Please select a valid option."); break;
                }
            }
        }
    }
}

//foreach (var c in KB.Clause)
//{
//    foreach (var i in c.Body)
//    {
//        Console.WriteLine($"B:[{i}] H:[{c.Head}]");
//    }
//}

//foreach (var c in KB.SingleClauses)
//{
//    Console.WriteLine($"H:[{c}]");
//}



using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;

namespace STTranspiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                string inputPath = "../../ST_Programs/Tests";
                string outputPath = "../../../STPrograms/TranspiledPrograms";
                string fileName = "";
                string path = Path.Combine(Environment.CurrentDirectory, inputPath, "Test3.txt");
                string filePath = Path.Combine(path, fileName);
                string fileContent = File.ReadAllText(filePath);

                STListener extractor = TranspileSTProgram(fileContent);
                string content = extractor.content.ToString().Replace("{GLOBAL_VARIABLES}", "GLOBAL_VARIABLES.PrintGlobalVariables();");
                SaveToFile(extractor.programName, new string[] { content, extractor.externalContent.ToString(), GenerateGlobalVariablesClass(extractor.globalVariables) }, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                Console.ReadKey();
            }
        }

        private static STListener TranspileSTProgram(string fileContent)
        {
            AntlrInputStream inputStream = new AntlrInputStream(fileContent);
            STLexer lexer = new STLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            STParser parser = new STParser(tokens);
            IParseTree tree = parser.startRule();
            ParseTreeWalker walker = new ParseTreeWalker();
            STListener extractor = new STListener(parser);
            walker.Walk(extractor, tree);

            return extractor;
        }

        private static string GenerateGlobalVariablesClass(List<string> globalVariables)
        {
            string globalVariablesClass = "public static class GLOBAL_VARIABLES \n{";
            globalVariablesClass += string.Join("\n", globalVariables);

            globalVariablesClass += @"
                public static void PrintGlobalVariables()
                {
                    var type = typeof(GLOBAL_VARIABLES);
                    var properties = type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                    System.Console.WriteLine(""---------------- BEGIN GLOBAL_VARIABLES ----------------\n"");

                    foreach (System.Reflection.PropertyInfo property in properties)
                        {
                            System.Console.WriteLine(""{0} = {1}"", property.Name, property.GetValue(null).ToString());
                        }

                        System.Console.WriteLine(""\n---------------- END GLOBAL_VARIABLES ----------------\n\n"");
                    }";

            globalVariablesClass += "\n}";

            return globalVariablesClass;
        }

        private static void SaveToFile(string programName, string[] content, string path)
        {
            if (!string.IsNullOrWhiteSpace(programName))
            {
                var pathToFile = Path.Combine(Environment.CurrentDirectory, path, programName + ".cs");

                using (FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate))
                using (StreamWriter file = new StreamWriter(fs))
                {
                    foreach (var item in content)
                    {
                        file.Write(item);
                    }

                    file.Close();
                    fs.Close();
                }
            }
        }
    }
}
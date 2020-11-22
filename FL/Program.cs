using FL.Entity;
using FL.Serializer.Deserialize;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using FL.Service;

namespace FL
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText("C:/Users/kapli/source/repos/FL/FL/resources/lexer.json");
            Dictionary<string, DescriptionLexer> lexers = DescriptionLexerDeserializer.Deserialize(json);

            List<Lexer> listLexers = new List<Lexer>();
            foreach (KeyValuePair<string, DescriptionLexer> item in lexers)
            {
                string jsonItem = File.ReadAllText("C:/Users/kapli/source/repos/FL/FL/resources" + item.Value.path);
                List<Lexer> list = LexerDeserializer.Deserialize(jsonItem, int.Parse(item.Value.priority), item.Key);
                foreach (Lexer it in list)
                {
                    listLexers.Add(it);
                }
            }

            Console.WriteLine(LexerService.checkIntP(".")); // 0, false
            Console.WriteLine(LexerService.checkIntP(".1")); // 2, true
            Console.WriteLine(LexerService.checkIntP("123")); // 3, true
            Console.WriteLine(LexerService.checkIntP("a123")); // 0, false
            Console.WriteLine(LexerService.checkIntP("123.")); // 4, true
            Console.WriteLine(LexerService.checkIntP("12.3.")); // 4, true
            Console.WriteLine(LexerService.checkIntP("123.1")); // 5, true
            Console.WriteLine(LexerService.checkIntP("123.123")); // 7, true
            Console.WriteLine(LexerService.checkIntP("+123.123")); // 8, true
            Console.WriteLine(LexerService.checkIntP("-123.123")); // 8, true
            Console.WriteLine(LexerService.checkIntP("a123.123")); // 0, false
            Console.WriteLine(LexerService.checkIntP("a123.123", 1)); // 7, true
            Console.WriteLine(LexerService.checkIntP("a123.123", 4)); // 4, true
            Console.WriteLine(LexerService.checkIntP("a123.123", 5)); // 3, true
            Console.WriteLine();

            string textProgram = File.ReadAllText("C:/Users/kapli/source/repos/FL/FL/resources/input.txt");
            Console.WriteLine(LexerService.checkProgram(textProgram));
        }
    }
}

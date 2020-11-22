using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FL.Serializer.Deserialize;
using FL.Service;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace FL
{
    [TestFixture]
    class Tests
    {
        [Test]
        [TestCase("123", "Integer")]        
        [TestCase("123.7", "Integer")]        
        [TestCase("a.7", "Integer")]        
        [TestCase("_test", "Identifier")]        
        [TestCase("12test", "Identifier")]        
        [TestCase("test", "Identifier")]        
        [TestCase("true", "Bool")]        
        [TestCase("false", "Bool")]        
        [TestCase(">", "Operation")]        
        [TestCase(">=", "Operation")]        
        [TestCase("+=", "Operation")]        
        [TestCase("=", "Operation")]        
        [TestCase("9=", "Operation")]        
        [TestCase(" ", "Whitespace")]        
        [TestCase("", "Whitespace")]        
        [TestCase("s", "Whitespace")]        
        [TestCase("\n", "Whitespace")]        
        [TestCase("\r", "Whitespace")]        
        [TestCase("\t", "Whitespace")]        
        [TestCase("\tt", "Whitespace")]        
        [TestCase(";", "Simbols")]        
        [TestCase(".", "Simbols")]        
        [TestCase("{", "Simbols")]        
        [TestCase(")", "Simbols")]        
        [TestCase("&", "Simbols")]        
        [TestCase("||", "Simbols")]            
        [TestCase("123", "Double")]        
        [TestCase("123.", "Double")]        
        [TestCase("123.123", "Double")]        
        [TestCase(".123", "Double")]        
        [TestCase("d.123", "Double")]        
        public void TestOneElement(string textTest, string  type)
        {
            Assert.AreEqual((string)LexerService.checkProgramPortion(textTest)[2], type);
        }

        [Test]
        [TestCase("int a = 20 / 5; ss;")]
        public void TestAllText(string text){

            string result = LexerService.checkProgram(text);

            List<string> results = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                text = text.Substring(i);
                object[] res = LexerService.checkProgramPortion(text);
                i = (int)res[0] + 1;
                results.Add(text.Substring(0, i).Replace("\n", "\\n").Replace("\r", "\\r") + "\t<--->\t" + (string)res[2]);
            }
           
            Assert.AreEqual(result, string.Join("\n", result), "Success");
        }

        [Test]
        public void TestCreateLexer()
        {
            string path = File.ReadAllText("C:/Users/kapli/source/repos/FL/FL/resources/lexer/bool.json");
            int prioritet = 0;
            string name = "test";
            LexerDeserializer.Deserialize(path, prioritet, name);
        }
    }
}

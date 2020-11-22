using System;
using System.Collections.Generic;
using FL.Entity;
using FL.Serializer.Deserialize;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace FL.Service
{
    class LexerService
    {
        static List<Lexer> getLexer()
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
            return listLexers;
        }
        static Lexer getOneLexer(string path)
        {
            string jsonItem = File.ReadAllText(path);
            List<Lexer> list = LexerDeserializer.Deserialize(jsonItem, 0, "id");

            return list[0];
        }

        public static KeyValuePair<int, bool> checkInt(string text)
        {
            bool res = false;
            string path = "C:/Users/kapli/source/repos/FL/FL/resources/lexer/real.json";
            Lexer automat = getOneLexer(path);
            string state = automat.start[0];
            int j=-1;            

            for (int i =0; i<text.Length; i++)
            {
                bool transition = false;
                if (i>0 && state == "q0")
                {
                    break;
                }
                Dictionary<string, List<string>> q = automat.matrix[state];
                List<string> keys = new List<string>(q.Keys);
                keys.Sort();
                foreach (string item in keys)
                {
                    if(item == text[i].ToString())
                    {
                        state = q[item][0];
                        transition = true;
                        break;
                    }
                    else
                    {
                        if (automat.inputs.ContainsKey(item))
                        {
                            ParameterExpression x = Expression.Parameter(typeof(char), "x");
                            LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x }, null, automat.inputs[item]);
                            var c = e.Compile();
                            var result = c.DynamicInvoke((char)text[i]);
                            if((bool)result == true)
                            {
                                state = q[item][0];
                                transition = true;
                                break;
                            }
                        }
                        
                    }
                }

                if (transition == false) break;
                j = i;
                if (automat.finish.IndexOf(state) >= 0)
                {
                    res = true;
                }
                
            }

            return new KeyValuePair<int, bool>(j, res);
        }

        public static string checkIntP(string text, int position = 0)
        {
            KeyValuePair<int, bool> result = checkInt(text.Substring(position));
            if (result.Value==false)
            {
                return 0 + ", " + false;
            }else
            {
                return result.Key+1 + ", " + true;
            }
        }

        public static object[] checkProgramPortion(string text)
        {
            List<Lexer> automats = getLexer();
            automats.Sort();
            bool res = false;
            string type = "";
            int j = -1;
            int jOld = j;
            string typeOld = type;
            foreach (Lexer automat in automats)
            {
                if (j > jOld)
                {
                    jOld = j;
                    typeOld = type;
                }
                else
                    if (res) 
                    break;
                jOld = j;
                typeOld = type; 
                string state = automat.start[0];

                for (int i = 0; i < text.Length; i++)
                {
                    bool transition = false;
                    if (i > 0 && state == "q0" || !automat.matrix.ContainsKey(state))
                    {
                        break;
                    }
                    Dictionary<string, List<string>> q = automat.matrix[state];
                    List<string> keys = new List<string>(q.Keys);
                    keys.Sort();
                    foreach (string item in keys)
                    {
                        if (item == text[i].ToString())
                        {
                            state = q[item][0];
                            transition = true;
                            break;
                        }
                        else
                        {
                            if (automat.inputs != null && automat.inputs.ContainsKey(item))
                            {
                                ParameterExpression x = Expression.Parameter(typeof(char), "x");
                                LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x }, null, automat.inputs[item]);
                                var c = e.Compile();
                                var result = c.DynamicInvoke((char)text[i]);
                                if ((bool)result == true)
                                {
                                    state = q[item][0];
                                    transition = true;
                                    break;
                                }
                            }

                        }
                    }
                    if (transition == false) break;
                    j = i;
                    if (automat.finish.IndexOf(state) >= 0)
                    {
                        res = true;
                        type = automat.type;
                    }

                }
            }
            return new object[3] { jOld==-1?j:jOld, res, typeOld == "" ? type : typeOld };
        }


        public static string checkProgram(string text)
        {
            List<string> result = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                text = text.Substring(i);
                object[] res =  checkProgramPortion(text);
                i = (int)res[0]+1;
                result.Add(text.Substring(0,i).Replace("\n", "\\n").Replace("\r", "\\r") + "\t<--->\t" + (string)res[2]);
            }
            return string.Join("\n", result); 
        }
    }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace FL.Serializer.Deserialize
{
    class LexerDeserializer
    {
        public static List<Lexer> Deserialize(string json, int prioritet, string type)
        {
            JObject search = JObject.Parse(json);
            List<JToken> results = search["data"].Children().ToList();

            List<Lexer> searchResults = new List<Lexer>();
            foreach (JToken result in results)
            {
                Lexer searchResult = result.ToObject<Lexer>();
                searchResult.prioritet = prioritet;
                searchResult.type = type;
                searchResults.Add(searchResult);
            }
            return searchResults;
        }
    }
}

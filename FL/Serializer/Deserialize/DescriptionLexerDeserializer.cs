using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using FL.Entity;

namespace FL.Serializer.Deserialize
{
    class DescriptionLexerDeserializer
    { 
        public static Dictionary<string, DescriptionLexer> Deserialize(string json)
        {
            JObject search = JObject.Parse(json);
            List<JToken> results = search.Children().ToList();

            Dictionary<string,DescriptionLexer> descriptionLexer = new Dictionary<string, DescriptionLexer>();
            foreach (JToken result in results)
            {
                DescriptionLexer searchResult = result.First().ToObject<DescriptionLexer>();
                descriptionLexer.Add(result.Path, searchResult);
            }
            return descriptionLexer;

        }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace FL
{
    class Lexer: IComparable<Lexer>
    {
        public List<string> start { get; set; }
        public List<string> finish { get; set; }
        public Dictionary<string, string> inputs { get; set; }
        public Dictionary<string, Dictionary<string, List<string>>> matrix { get; set; }
        public int prioritet { get; set; }
        public string type { get; set; }

        int IComparable<Lexer>.CompareTo(Lexer other)
        {
            if (other == null) return 1;
            return -prioritet.CompareTo(other.prioritet);
        }
    }
}

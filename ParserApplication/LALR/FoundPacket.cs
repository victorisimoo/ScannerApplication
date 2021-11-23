using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApplication.LALR
{
    struct FoundPacket
    {
        public bool found;
        public string valuesent;
        public int comingfrom;
        public int actual;
        public TokenType typesent;

        public FoundPacket(bool result, string value, int from,int where,TokenType sent) {
            found = result;
            valuesent = value;
            comingfrom = from;
            actual = where;
            typesent = sent;
        }
    }
}

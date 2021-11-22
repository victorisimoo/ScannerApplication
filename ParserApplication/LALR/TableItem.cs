using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApplication.LALR
{
    class TableItem
    {
        public TokenType Typesent;
        public string valuesent;
        public int comingfrom;
        public Dictionary<string, int> Shifts = new Dictionary<string, int>();
        public Dictionary<string, int> Gotos = new Dictionary<string, int>();
        public List<ListadeTokens> EstadoProduction = new List<ListadeTokens>();

        public TableItem(List<ListadeTokens> Production,string entry,int anterior,TokenType type) {
            EstadoProduction = new List<ListadeTokens>(Production);
            valuesent = entry;
            comingfrom = anterior;
            Typesent = type;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApplication.LALR
{
    public class TableItem
    {
        public bool Accept;
        public TokenType Typesent;
        public string valuesent;
        public int comingfrom;
        public Dictionary<string, int> Shifts = new Dictionary<string, int>();
        public Dictionary<string, int> Gotos = new Dictionary<string, int>();
        public Dictionary<string, ListadeTokens> Reduce = new Dictionary<string, ListadeTokens>();
        public List<ListadeTokens> EstadoProduction = new List<ListadeTokens>();
        public TableItem(List<ListadeTokens> Production,string entry,int anterior,TokenType type) {
            EstadoProduction = new List<ListadeTokens>(Production);
            valuesent = entry;
            comingfrom = anterior;
            Typesent = type;
            if (TokenType.EOF == Typesent)
            {
                Accept = true;
            }
            else {
                Accept = false;
            }
        }

        public void SetLookAheads(List<FirstFollow> Follow) {
            foreach (var item in EstadoProduction) {
                foreach (var item2 in Follow)
                {
                    if (item.identifier == item2.token.Value) {
                        item.Lookaheads.AddRange(item2.lista);
                    }
                }
            }
        }

        public void SetReduce() {
            foreach (var item in EstadoProduction)
            {
                if (item.pos == item.listas.Count) {
                    //Hay un reduce
                    foreach (var item2 in item.Lookaheads)
                    {
                        if (!Reduce.ContainsKey(item2)) {
                            Reduce.Add(item2, item);
                            
                        }
                    }
                }
            }
        }
    }
}

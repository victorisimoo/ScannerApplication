using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApplication.TokenConstruction;

namespace ParserApplication.LALR
{
    public class ListadeTokens
    {
        public List<Token> listas;
        public Token idRule;
        string regla = ""; 
        public ListadeTokens(List<Token> lista)
        {
            idRule = lista[0];
            lista.Remove(lista[0]);
            listas = lista;
            
            foreach (var item in lista)
            {
                regla = regla + item.Value + " ";
            }
        }
    }
}

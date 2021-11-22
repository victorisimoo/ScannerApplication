using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApplication.TokenConstruction;

namespace ParserApplication.LALR
{
    public class Rules
    {
        public List<ListadeTokens> Reglas = new List<ListadeTokens>();
        public Token tokenid;
        List<Token> entradas = new List<Token>();
        public List<Token> ingresar = new List<Token>();
        public Rules(Token[] entrada)
        {
            entradas = entrada.ToList();

            for (int i = 0; i < entradas.Count; i++)
            {
                switch (entradas[i].Tag) {
                    case TokenType.igual:
                        break;
                    case TokenType.or:
                        tokenid = ingresar[0];
                        ListadeTokens mandar = new ListadeTokens(ingresar);
                        Reglas.Add(mandar);
                        ingresar = new List<Token>();
                        ingresar.Add(tokenid);
                        break;
                    case TokenType.puntoycoma:
                        ListadeTokens mandar2 = new ListadeTokens(ingresar);
                        Reglas.Add(mandar2);
                        ingresar = new List<Token>();
                        break;

                    default:
                        ingresar.Add(entradas[i]);
                        break;
                }                                         
            }
            
           
        }
        
    }
}

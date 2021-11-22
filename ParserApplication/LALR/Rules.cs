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
                if (entradas[i].Tag == TokenType.igual)
                {
                    i++;
                }
                if (entradas[i].Tag == TokenType.or)
                {
                    ListadeTokens mandar = new ListadeTokens(ingresar);
                    Reglas.Add(mandar);
                    tokenid = ingresar[0]; //lee id
                    ingresar = new List<Token>(); //vacia
                    ingresar.Add(tokenid);
                    i++;

                }
                if (entradas[i].Tag == TokenType.puntoycoma)
                {
                    ListadeTokens mandar = new ListadeTokens(ingresar);
                    Reglas.Add(mandar);
                    ingresar = new List<Token>();
                }
                else
                {
                    ingresar.Add(entradas[i]);
                }

            }
            TokenFollow enviar = new TokenFollow(Reglas);
          
        }
    }
}

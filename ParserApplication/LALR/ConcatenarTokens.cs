using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApplication.TokenConstruction;

namespace ParserApplication.LALR
{
    public class ConcatenarTokens
    {
        List<List<Token>> Reglas = new List<List<Token>>();
        Token tokenid;
        List<Token> entradas = new List<Token>();
        List<Token> ingresar = new List<Token>();
        public ConcatenarTokens(Token[] entrada)
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
                    Reglas.Add(ingresar);
                    tokenid = ingresar[0]; //lee id
                    ingresar = new List<Token>(); //vacia
                    ingresar.Add(tokenid);
                    i++;

                }
                if (entradas[i].Tag == TokenType.puntoycoma)
                {
                    Reglas.Add(ingresar);
                    ingresar = new List<Token>();
                }
                else
                {
                    ingresar.Add(entradas[i]);
                }

            }
            ingresar = null;
          
        }
    }
}

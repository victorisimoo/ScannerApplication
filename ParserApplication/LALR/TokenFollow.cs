using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApplication.TokenConstruction;


namespace ParserApplication.LALR
{
    public class TokenFollow
    {
        List<First> idfollow = new List<First>();
        First inicio = new First();
        public List<Token> firsts = new List<Token>();
        public List<ListadeTokens> Reglas = new List<ListadeTokens>();
        Token id;
        public int contador = 0;
        

        public TokenFollow(List<ListadeTokens> Regla)
        {
            Reglas = Regla;
            inicio.token = Reglas[0].idRule;
            FIRST(Reglas, 0, firsts, inicio);
        }

        public static void FIRST(List<ListadeTokens> Reglas, int reglas, List<Token> firsts, First inicio)
        {   
            for (int i = reglas; i < Reglas.Count; i++)
            {
                if (inicio.token.Value != Reglas[i].idRule.Value)
                {
                    if (Reglas[i].listas[0].Tag != TokenType.id)
                    {
                         firsts.Add(Reglas[i].listas[0]);
                    }
                    else
                    {
                        inicio.token = Reglas[i].idRule;
                        FIRST(Reglas, Reglas.Count, firsts, inicio);
                    }
                }
                else
                {
                    if (Reglas[i].listas[0].Tag != TokenType.id)
                    {
                        firsts.Add(Reglas[i].listas[0]);
                    }
                    else
                    {
                        inicio = new First();
                        FIRST(Reglas, Reglas.Count, firsts, inicio);
                    }
                }
                
               
            }
        }
        
    }
}

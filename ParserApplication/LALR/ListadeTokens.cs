﻿using System;
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
        public string identifier = "";
        public string regla = "";
        public int pos = 0;
        public List<string> Lookaheads = new List<string>();

        public ListadeTokens(ListadeTokens Kernel,bool check)
        {
            listas = Kernel.listas;
            idRule = Kernel.idRule;
            identifier = Kernel.identifier;
            regla = Kernel.regla;
            if (check)
            {
                pos = 0;
            }
            else {
                pos = Kernel.pos;
            }
            
        }
        public ListadeTokens(ListadeTokens Kernel) {
            listas = Kernel.listas;
            idRule = Kernel.idRule;
            identifier = Kernel.identifier;
            regla = Kernel.regla;
            pos = Kernel.pos + 1;
        }
        public ListadeTokens(List<Token> lista)
        {
            idRule = lista[0];
            identifier = idRule.Value;
            lista.Remove(lista[0]);
            listas = lista;
            
            foreach (var item in lista)
            {
                regla = regla + item.Value + " ";
            }
        }

        public ListadeTokens()
        { }
    }
}

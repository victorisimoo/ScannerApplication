using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApplication.TokenConstruction;

namespace ParserApplication.LALR
{
    class Graph
    {
        private List<TableItem> Grafo = new List<TableItem>();
        private List<ListadeTokens> _rulelist = new List<ListadeTokens>();
        private List<Token> first = new List<Token>();
        private First firsts = new First();
        private List<First> ListofFirst = new List<First>();
        int estado=0;

        public Graph(List<ListadeTokens> rules) {
            _rulelist = rules;
            for (int i = 0; i < rules.Count; i++)
            {
                if (!first.Contains(rules[i].idRule))
                {
                    first.Add(rules[i].idRule);
                    firsts = new First();
                    firsts.token = rules[i].idRule;
                    ListofFirst.Add(firsts);
                }
            }

        }

        public void BuildGraph()
        {
            _rulelist[0].listas.Add(new Token { Value = "", Tag = TokenType.EOF });

            //primer valor
            string kernel = _rulelist[0].regla;
            List<ListadeTokens> Production = new List<ListadeTokens>();
            Production.AddRange(GetRuleId(_rulelist[0].identifier));
            List<string> used = new List<string>();
            for (int i = 0; i < Production.Count; i++)
            {
                if (Production[i].listas[0].Tag == TokenType.id)
                {
                    if (!used.Contains(Production[i].listas[0].Value))
                    {
                        used.Add(Production[i].listas[0].Value);
                        Production.AddRange(GetRuleId(Production[i].listas[0].Value));
                    }

                }
                else if (Production[i].listas[0].Tag == TokenType.EOF) {
                //Accept
                }
                else
                {
                    //No hacer nada por ahora
                }
            }
            Grafo.Add(new TableItem(Production,"",-1,TokenType.Null));
            int estadoactual = estado;
            estado++;
            Dictionary<string, List<ListadeTokens>> Parejas = new Dictionary<string, List<ListadeTokens>>();
            foreach (var item in Grafo[0].EstadoProduction)
            {
                if (Parejas.ContainsKey(item.listas[0].Value))
                {
                    Parejas[item.listas[0].Value].Add(item);
                }
                else {
                    Parejas.Add(item.listas[0].Value,new List<ListadeTokens>());
                    Parejas[item.listas[0].Value].Add(item);
                }
                //BuildNode(item,estadoactual);
            }
            foreach (var item in Parejas)
            {
                BuildNode(item.Value, estadoactual, item.Key, Match(item.Key));
            }
            BuildTable();
            foreach (var item in _rulelist)
            {
                List<string> Firstlist = new List<string>();
                Firstlist.AddRange(First(item.identifier));
            }
        }
        public TokenType Match(string value) {
            foreach (var item in _rulelist)
            {
                foreach (var item2 in item.listas)
                {
                    if (item2.Value == value)
                        return item2.Tag;
                }
            }
            return TokenType.EOF;
        }
        public void BuildTable() {
            for (int i = 0; i < Grafo.Count; i++)
            {
                for (int j = i+1; j < Grafo.Count; j++)
                {
                    if (Grafo[j].comingfrom == i) {
                        if (Grafo[j].Typesent == TokenType.id)
                        {
                            Grafo[i].Gotos.Add(Grafo[j].valuesent, j);
                        }
                        else if (Grafo[j].Typesent == TokenType.term)
                        {
                            Grafo[i].Shifts.Add(Grafo[j].valuesent, j);
                        }
                    }
                }
            }
        }
        public void BuildNode(List<ListadeTokens> Kernel,int earlierstate,string consumed,TokenType consumedType) {
            List<ListadeTokens> Production = new List<ListadeTokens>();
            foreach (var item in Kernel)
            {
                Production.Add(new ListadeTokens(item));
            }
            //Production.AddRange(Kernel);
            List<string> used = new List<string>();
            
            for (int i = 0; i < Production.Count; i++)
            {
                if (Production[i].listas.Count > Production[i].pos)
                {
                    if (Production[i].listas[Production[i].pos].Tag == TokenType.id)
                    {
                        if (!used.Contains(Production[i].listas[Production[i].pos].Value))
                        {
                            used.Add(Production[i].listas[Production[i].pos].Value);
                            Production.AddRange(GetRuleId(Production[i].listas[Production[i].pos].Value));
                        }

                    }
                    else
                    {
                        //No hacer nada por ahora
                    }
                }
                else {
                    //Generar Reduce
                }
                
            }
            Grafo.Add(new TableItem(Production, consumed,earlierstate,consumedType));
            int estadoactual = estado;
            estado++;
            Dictionary<string, List<ListadeTokens>> Parejas = new Dictionary<string, List<ListadeTokens>>();
            foreach (var item in Grafo[estado-1].EstadoProduction)
            {

                if (item.pos < item.listas.Count) {
                    if (Parejas.ContainsKey(item.listas[item.pos].Value))
                    {
                        Parejas[item.listas[item.pos].Value].Add(item);
                    }
                    else
                    {
                        Parejas.Add(item.listas[item.pos].Value, new List<ListadeTokens>());
                        Parejas[item.listas[item.pos].Value].Add(item);
                    }
                }
            }

            foreach (var item in Parejas)
            {
                BuildNode(item.Value, estadoactual, item.Key, Match(item.Key));
            }
        }

        public List<string> First(string id) {
            List<string> Toadd = new List<string>();
            foreach (var item in _rulelist)
            {
                if (id == item.identifier)
                {
                    
                    if (id != item.listas[0].Value)
                    { 
                        if (item.listas[0].Tag == TokenType.term)
                        {
                            Toadd.Add(item.listas[0].Value);
                        }
                            else if (item.listas[0].Tag == TokenType.id)
                            { 
                                
                                Toadd.AddRange(First(item.listas[0].Value));
                            }
                    }
                }
            }
            int index = 0;
            foreach (var item in ListofFirst)
            {
                if (item.token.Value == id)
                {
                    index = ListofFirst.IndexOf(item);
                }
            }
            ListofFirst[index].listafirst = Toadd;
            return Toadd;
        }
            
        
        public List<ListadeTokens> GetRuleId(string id) {
            List<ListadeTokens> Toadd = new List<ListadeTokens>();
            foreach (var item in _rulelist)
            {
                if (id == item.identifier)
                {
                    Toadd.Add(item); 
                }
            }
            return Toadd;
        }
    }
}

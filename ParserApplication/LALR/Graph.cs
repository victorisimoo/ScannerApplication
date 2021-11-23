using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ParserApplication.TokenConstruction;

namespace ParserApplication.LALR
{
    class Graph
    {
        public List<TableItem> Grafo = new List<TableItem>();
        private List<ListadeTokens> _rulelist = new List<ListadeTokens>();
        private List<Token> first = new List<Token>();
        private FirstFollow firstfollow = new FirstFollow();
        private List<Token> follow = new List<Token>();
        private List<FirstFollow> ListofFirst = new List<FirstFollow>();
        private List<FirstFollow> ListofFollow = new List<FirstFollow>();
        int estado=0;

        public List<TableItem> getGrafo () {
            return Grafo;
        }

        public Graph(List<ListadeTokens> rules) {
            _rulelist = rules;
            for (int i = 0; i < rules.Count; i++)
            {
                if (!first.Contains(rules[i].idRule))
                {
                    first.Add(rules[i].idRule);
                    firstfollow = new FirstFollow();
                    firstfollow.token = rules[i].idRule;
                    ListofFirst.Add(firstfollow);
                    ListofFollow.Add(firstfollow);
                }
            }

        }

        public void BuildGraph()
        {
            _rulelist[0].listas.Add(new Token { Value = "$", Tag = TokenType.EOF });

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
            foreach (var item in ListofFirst)
            {
                First(item.token.Value);
            }
            foreach (var item in ListofFollow)
            { 
                Follow(item.token.Value);
            }
            foreach (var item in Grafo) {
                item.SetLookAheads(ListofFollow);
            }
            foreach (var item in Grafo)
            {
                item.SetReduce();
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
                            if(!Grafo[i].Gotos.ContainsKey(Grafo[j].valuesent))
                                Grafo[i].Gotos.Add(Grafo[j].valuesent, j);
                        }
                        else if (Grafo[j].Typesent == TokenType.term)
                        {
                            if (!Grafo[i].Shifts.ContainsKey(Grafo[j].valuesent))
                                    Grafo[i].Shifts.Add(Grafo[j].valuesent, j);
                        }
                        else if (Grafo[j].Typesent == TokenType.EOF) {
                            if (!Grafo[i].Shifts.ContainsKey(Grafo[j].valuesent))
                                Grafo[i].Shifts.Add(Grafo[j].valuesent, j);
                        }
                    }
                }
            }
        }
        public void BuildNode(List<ListadeTokens> Kernel,int earlierstate,string consumed,TokenType consumedType) {
            try {
                List<ListadeTokens> Production = new List<ListadeTokens>();
                foreach (var item in Kernel) {
                    Production.Add(new ListadeTokens(item));
                }
                //Production.AddRange(Kernel);
                List<string> used = new List<string>();

                for (int i = 0; i < Production.Count; i++) {
                    if (Production [i].listas.Count > Production [i].pos) {
                        if (Production [i].listas [Production [i].pos].Tag == TokenType.id) {
                            if (!used.Contains(Production [i].listas [Production [i].pos].Value)) {
                                used.Add(Production [i].listas [Production [i].pos].Value);
                                Production.AddRange(GetRuleId(Production [i].listas [Production [i].pos].Value));
                            }

                        } else {
                            //No hacer nada por ahora
                        }
                    } else {
                        //Generar Reduce
                    }

                }
                //Check if ya existe
                FoundPacket result=CheckExists(Production);
                if (result.found)
                {
                    if (result.typesent == TokenType.id)
                    {
                        if(!Grafo[earlierstate].Gotos.ContainsKey(result.valuesent))
                            Grafo[earlierstate].Gotos.Add(result.valuesent, result.actual);
                    }
                    else if (result.typesent == TokenType.term || result.typesent == TokenType.EOF) {
                        if (!Grafo[earlierstate].Shifts.ContainsKey(result.valuesent))
                            Grafo[earlierstate].Shifts.Add(result.valuesent, result.actual);
                    }
                    
                }
                else {
                    Grafo.Add(new TableItem(Production, consumed, earlierstate, consumedType));
                    int estadoactual = estado;
                    estado++;
                    Dictionary<string, List<ListadeTokens>> Parejas = new Dictionary<string, List<ListadeTokens>>();
                    foreach (var item in Grafo[estado - 1].EstadoProduction)
                    {

                        if (item.pos < item.listas.Count)
                        {
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

                
            }catch (Exception) {

                MessageBox.Show("Se ha producido un error en el procesamiento de la gramática",
                        "¡Error encontrado: BuildNode!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public FoundPacket CheckExists(List<ListadeTokens> aComparar) {
            bool valor = false;
            int contador = 0;
            foreach (var item in Grafo) {
                valor = Comparar(aComparar,item.EstadoProduction);
                if (valor)
                    return new FoundPacket(true,item.valuesent,item.comingfrom,contador,item.Typesent);
                contador++;
            }
            return new FoundPacket(false, "", -1,-1,TokenType.Empty);
        }

        public bool Comparar(List<ListadeTokens> lista1, List<ListadeTokens> lista2) {
            bool igual = true;
            if (lista1.Count == lista2.Count)
            {
                for (int i = 0; i < lista1.Count; i++)
                {
                    if (lista1[i].identifier == lista2[i].identifier && lista1[i].pos == lista2[i].pos && lista1[i].regla == lista2[i].regla)
                    {
                        igual = igual && igual;
                    }
                    else {
                        igual = false;
                    }
                }
                return igual;
            }
            else {
                return false;
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
            ListofFirst[index].lista = Toadd;
            return Toadd;
        }

        public List<string> Follow(string id)
        {
            List<string> Toadd = new List<string>();
            foreach (var item in _rulelist)
            {
                for (int i = 0; i < item.listas.Count; i++)
                {
                    if (id == item.listas[i].Value)
                    {
                        if (i + 1 < item.listas.Count)
                        {
                            if (item.listas[i + 1].Tag == TokenType.term)
                            {
                                Toadd.Add(item.listas[i + 1].Value);
                            }
                            else if (item.listas[i + 1].Tag == TokenType.id)
                            {
                                foreach (var items in ListofFirst)
                                {
                                    if (items.token.Value == item.listas[i + 1].Value)
                                    {
                                        Toadd.AddRange(items.lista);
                                    }
                                }
                            }
                            else
                            {
                                if (!Toadd.Contains("$"))
                                {
                                    Toadd.Add("$");
                                }
                                
                            }
                        }
                        else
                        {
                            if (id != item.identifier) {
                                Toadd.AddRange(Follow(item.identifier));
                            }
                            
                        }  
                    }
                }
            }
            int index = 0;
            foreach (var item in ListofFollow)
            {
                if (item.token.Value == id)
                {
                    index = ListofFollow.IndexOf(item);
                }
            }
            if (Toadd.Count == 0)
            {
                Toadd.Add("$");
                ListofFollow[index].lista = (Toadd);
            }
            else
            {
                ListofFollow[index].lista = (Toadd);
            }
                        
            return Toadd;
        }


        public List<ListadeTokens> GetRuleId(string id) {
            List<ListadeTokens> Toadd = new List<ListadeTokens>();
            foreach (var item in _rulelist)
            {
                if (id == item.identifier)
                {
                    Toadd.Add(new ListadeTokens(item,true)); 
                }
            }
            return Toadd;
        }
    }
}

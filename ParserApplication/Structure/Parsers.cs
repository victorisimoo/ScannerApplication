using ParserApplication.Structure;
using System;
using System.Collections.Generic;
using ParserApplication.LALR;

namespace ParserApplication.TokenConstruction {
    public class Parsers {
        Scanner scanner;
        Token token;
        List<TableItem> ParsingTable;
        Stack<Token> pilaT = new Stack<Token>();
        Stack<State> pilaE = new Stack<State>();
        Stack<int> pilaE2 = new Stack<int>();
        Queue<Token> entrada = new Queue<Token>();

        public Parsers(Queue<Token> entradas)
        {
            entrada = entradas;
        }

        public Parsers(Queue<Token> entradas, List<TableItem> Tabla)
        {
            entrada = entradas;
            ParsingTable = new List<TableItem>(Tabla);
        }

        public void Parse3() {
            int estadoactual = 0;
            pilaE2.Push(estadoactual);
            pilaT.Push(new Token() { Value = "inicio", Tag = TokenType.inicial });
            Start(estadoactual,false);
        }

        private void Start(int pos,bool justgoto) {
            int estadoactual = 0;
            if (ParsingTable[pos].Accept)
            {

            }
            else
            {
                if (ParsingTable[pos].Gotos.ContainsKey(pilaT.Peek().Value))
                {
                    if (!justgoto)
                    {
                        estadoactual = ParsingTable[pos].Gotos[pilaT.Peek().Value];
                        pilaE2.Push(estadoactual);
                        Start(estadoactual, true);
                    }
                    else {
                    
                    }
                } else if (ParsingTable[pos].Shifts.ContainsKey(entrada.Peek().Value))
                {
                    pilaT.Push(entrada.Dequeue());
                    pilaE2.Push(ParsingTable[pos].Shifts[pilaT.Peek().Value]);
                    Start(pilaE2.Peek(), false);
                }
                else if (ParsingTable[pos].Reduce.ContainsKey(entrada.Peek().Value))
                {
                    ListadeTokens LoadRule = new ListadeTokens(ParsingTable[pos].Reduce[entrada.Peek().Value], false);
                    while (LoadRule.pos > 0)
                    {
                        if (LoadRule.listas[LoadRule.pos - 1].Value == pilaT.Peek().Value)
                        {
                            pilaT.Pop();
                            pilaE2.Pop();
                        }
                        else
                        {
                            throw new Exception("Syntax Error");
                        }

                        LoadRule.pos--;
                    }
                    pilaT.Push(new Token() { Value = LoadRule.identifier, Tag = TokenType.id });
                    Start(pilaE2.Peek(), false);
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
        }

        void ReduceRule(int regla)
        {
            switch (regla)
            {
                case 1:
                    if (pilaT.Peek().Tag == TokenType.L)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { Value = "", Tag = TokenType.G });
                    }
                    break;
                case 2:
                    if (pilaT.Peek().Tag == TokenType.P)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        if (pilaT.Peek().Tag == TokenType.L)
                        {
                            pilaT.Pop();
                            pilaE.Pop();
                            pilaT.Push(new Token() { Value = "", Tag = TokenType.L });
                        }
                    }
                    break;
                case 3:
                    pilaT.Push(new Token() { Value = "", Tag = TokenType.L });
                    break;
                case 4:
                    if (pilaT.Peek().Tag == TokenType.puntoycoma)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        if (pilaT.Peek().Tag == TokenType.D)
                        {
                            pilaE.Pop();
                            pilaT.Pop();
                            if (pilaT.Peek().Tag == TokenType.C)
                            {
                                pilaE.Pop();
                                pilaT.Pop();
                                if (pilaT.Peek().Tag == TokenType.igual)
                                {
                                    pilaE.Pop();
                                    pilaT.Pop();
                                    if (pilaT.Peek().Tag == TokenType.id)
                                    {
                                        pilaE.Pop();
                                        pilaT.Pop();
                                        pilaT.Push(new Token() { Value = "", Tag = TokenType.P });
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 5:
                    if (pilaT.Peek().Tag == TokenType.V)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        if (pilaT.Peek().Tag == TokenType.C)
                        {
                            pilaE.Pop();
                            pilaT.Pop();
                            pilaT.Push(new Token() { Value = "", Tag = TokenType.C });
                        }
                    }
                    break;
                case 6:
                    pilaT.Push(new Token() { Value = "", Tag = TokenType.C });
                    break;
                case 7:
                    if (pilaT.Peek().Tag == TokenType.D)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        if (pilaT.Peek().Tag == TokenType.C)
                        {
                            pilaE.Pop();
                            pilaT.Pop();
                            if (pilaT.Peek().Tag == TokenType.or)
                            {
                                pilaT.Pop();
                                pilaE.Pop();
                                pilaT.Push(new Token() { Value = "", Tag = TokenType.D });
                            }
                        }
                    }
                    break;
                case 8:
                    pilaT.Push(new Token() { Value = "", Tag = TokenType.D });
                    break;
                case 9:
                    if (pilaT.Peek().Tag == TokenType.id)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { Value = "", Tag = TokenType.V });
                    }
                    break;
                case 10:
                    if (pilaT.Peek().Tag == TokenType.term)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { Value = "", Tag = TokenType.V });
                    }
                    break;
                default:
                    break;
            }
        }

        //private void Match(TokenType tag)
        //{
        //    if (tag != TokenType.EOF)
        //    {
        //        if (token.Tag == tag)
        //        {
        //            token = scanner.GetToken();
        //        }
        //        else
        //        {
        //            throw new Exception("Syntax Error");
        //        }
        //    }
        //    else
        //    {
        //        if (token.Tag == tag)
        //        {
        //        }
        //        else
        //        {
        //            throw new Exception("Syntax Error");
        //        }

        //    }

        //}
        private void I13() {
            switch (entrada.Peek().Tag)
            {
                case TokenType.puntoycoma:
                    ReduceRule(7);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        }
        private void I12() {
            switch (pilaT.Peek().Tag)
            {
                case TokenType.D:
                    pilaE.Push(State.I13);
                    Table();
                    break;
                case TokenType.V:
                    pilaE.Push(State.I8);
                    Table();
                    break;

                default:
                    switch (entrada.Peek().Tag)
                    {
                        case TokenType.id:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I10);
                            Table();
                            break;
                        case TokenType.term:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I11);
                            Table();
                            break;
                        case TokenType.puntoycoma:
                            ReduceRule(8);
                            Table();
                            break;
                        case TokenType.or:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I9);
                            Table();
                            break;
                        //case TokenType.EOF:
                        //    ReduceRule(6);
                        //    Table();
                        //    break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;
            }
        }
        private void I11() {
            switch (entrada.Peek().Tag)
            {
                case TokenType.id:
                case TokenType.term:
                case TokenType.puntoycoma:
                case TokenType.or:
                case TokenType.EOF:
                    ReduceRule(10);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        }
        private void I10() {
            switch (entrada.Peek().Tag)
            {
                case TokenType.id:
                case TokenType.term:
                case TokenType.puntoycoma:
                case TokenType.or:
                case TokenType.EOF:
                    ReduceRule(9);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        }
        private void I9() {
            switch (pilaT.Peek().Tag)
            {
                case TokenType.C:
                    pilaE.Push(State.I12);
                    Table();
                    break;
                default:
                    switch (entrada.Peek().Tag) {
                        case TokenType.id:
                        case TokenType.term:
                        case TokenType.puntoycoma:
                        case TokenType.or:
                        case TokenType.EOF:
                            ReduceRule(6);
                            Table();
                            break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;
            }
        }
        private void I8() {
            switch (entrada.Peek().Tag) {
                case TokenType.id:
                case TokenType.term:
                case TokenType.or:
                case TokenType.puntoycoma:

                case TokenType.EOF:
                    ReduceRule(5);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        }
        private void I7() {
            switch (entrada.Peek().Tag) {
                case TokenType.id:
                case TokenType.EOF:
                    ReduceRule(4);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        }
        private void I6() {
            switch (entrada.Peek().Tag) {
                case TokenType.puntoycoma:
                    pilaT.Push(entrada.Dequeue());
                    pilaE.Push(State.I7);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }
        
        }
        private void I5()
        {
            switch (pilaT.Peek().Tag) {
                case TokenType.D:
                    pilaE.Push(State.I6);
                    Table();
                    break;
                case TokenType.V:
                    pilaE.Push(State.I8);
                    Table();
                    break;
                default:
                    switch (entrada.Peek().Tag) {
                        case TokenType.id:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I10);
                            Table();
                            break;
                        case TokenType.term:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I11);
                            Table();
                            break;
                        case TokenType.puntoycoma:
                            ReduceRule(8);
                            Table();
                            break;
                        case TokenType.or:
                            pilaT.Push(entrada.Dequeue());
                            pilaE.Push(State.I9);
                            Table();
                            break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;
            }
        }
        private void I4()
        {
            switch (pilaT.Peek().Tag)
            {
                case TokenType.C:
                    pilaE.Push(State.I5);
                    Table();
                    break;
                default:
                    switch (entrada.Peek().Tag)
                    {
                        case TokenType.id:
                        case TokenType.term:
                        case TokenType.puntoycoma:
                        case TokenType.or:
                        case TokenType.EOF:
                            ReduceRule(6);
                            Table();
                            break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;
            }
        }
        private void I3()
        {
            switch (entrada.Peek().Tag)
            {
                case TokenType.igual:
                    pilaT.Push(entrada.Dequeue());
                    pilaE.Push(State.I4);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }

        }
        private void I2()
        {
            switch (entrada.Peek().Tag)
            {
                case TokenType.id:
                case TokenType.EOF:
                    ReduceRule(2);
                    Table();
                    break;
                default:
                    throw new Exception("Syntax Error");
                    break;
            }

        }

        private void I1()
        {
            switch (pilaT.Peek().Tag)
            {
                case TokenType.P:
                    pilaE.Push(State.I2);
                    Table();
                    break;
                default:
                    switch (entrada.Peek().Tag)
                    {
                        case TokenType.id:
                            pilaE.Push(State.I3);
                            pilaT.Push(entrada.Dequeue());
                            Table();
                            break;
                        case TokenType.EOF:
                            //Aceptar
                            break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;

            }
        }
        private void I0()
        {
            switch (pilaT.Peek().Tag)
            {
                case TokenType.L:
                    pilaE.Push(State.I1);
                    Table();
                    break;
                default:
                    switch (entrada.Peek().Tag)
                    {
                        case TokenType.inicial:
                        case TokenType.EOF:
                        case TokenType.id:
                            ReduceRule(3);
                            Table();
                            break;
                        default:
                            throw new Exception("Syntax Error");
                            break;
                    }
                    break;
            }
        }

        private void Table()
        {
            switch (pilaE.Peek())
            {
                case State.I0:
                    I0();
                    break;
                case State.I1:
                    I1();
                    break;
                case State.I2:
                    I2();
                    break;
                case State.I3:
                    I3();
                    break;
                case State.I4:
                    I4();
                    break;
                case State.I5:
                    I5();
                    break;
                case State.I6:
                    I6();
                    break;
                case State.I7:
                    I7();
                    break;
                case State.I8:
                    I8();
                    break;
                case State.I9:
                    I9();
                    break;
                case State.I10:
                    I10();
                    break;
                case State.I11:
                    I11();
                    break;
                case State.I12:
                    I12();
                    break;
                case State.I13:
                    I13();
                    break;
                default:
                    break;
            }
        }

        public void Parse2()
        {
            pilaE.Push(State.I0);
            pilaT.Push(new Token() { Value = "", Tag = TokenType.inicial });
            I0();
            //Cola aqui
        }

    }
}

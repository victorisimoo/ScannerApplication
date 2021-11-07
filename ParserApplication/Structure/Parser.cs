using ParserApplication.TokenConstruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParserApplication.Structure {
<<<<<<< HEAD
    public class Parser 
    {
=======
    public class Parser {
        string numerocadena = "";
        Scanner scanner;
        Token token;
        Stack<Token> entrada = new Stack<Token>();
        Stack<Token> pilaT = new Stack<Token>();
        Stack<State> pilaE = new Stack<State>();

        void ReduceRule(int regla)
        {
            switch (regla)
            {
                case 1:
                    if (pilaT.Peek().Tag == TokenType.L)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { value = "", Tag = TokenType.G });
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
                            pilaT.Push(new Token() { value = "", Tag = TokenType.L });
                        }
                    }
                    break;
                case 3:
                    pilaT.Push(new Token() { value = "", Tag = TokenType.L });
                    break;
                case 4:
                    if (pilaT.Peek().Tag == TokenType.PuntoyComa)
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
                                if (pilaT.Peek().Tag == TokenType.Dospuntos)
                                {
                                    pilaE.Pop();
                                    pilaT.Pop();
                                    if (pilaT.Peek().Tag == TokenType.id)
                                    {
                                        pilaE.Pop();
                                        pilaT.Pop();
                                        pilaT.Push(new Token() { value = "", Tag = TokenType.P });
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
                            pilaT.Push(new Token() { value = "", Tag = TokenType.C });
                        }
                    }
                    break;
                case 6:
                    pilaT.Push(new Token() { value = "", Tag = TokenType.L });
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
                            if (pilaT.Peek().Tag == TokenType.Pipe)
                            {
                                pilaT.Pop();
                                pilaE.Pop();
                                pilaT.Push(new Token() { value = "", Tag = TokenType.D });
                            }
                        }
                    }
                    break;
                case 8:
                    pilaT.Push(new Token() { value = "", Tag = TokenType.D });
                    break;
                case 9:
                    if (pilaT.Peek().Tag == TokenType.V)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { value = "", Tag = TokenType.id });
                    }
                    break;
                case 10:
                    if (pilaT.Peek().Tag == TokenType.V)
                    {
                        pilaE.Pop();
                        pilaT.Pop();
                        pilaT.Push(new Token() { value = "", Tag = TokenType.term });
                    }
                    break;
                default:
                    break;
            }
        }

        private void Match(TokenType tag)
        {
            if (tag != TokenType.EOF)
            {
                if (token.Tag == tag)
                {
                    token = scanner.GetToken();
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (token.Tag == tag)
                {
                }
                else
                {
                    throw new Exception("Syntax Error");
                }

            }

        }
        private void I15() { }
        private void I14() { }
        private void I13() { }
        private void I12() { }
        private void I11() { }
        private void I10() { }
        private void I9() { }
        private void I8() { }
        private void I7() { }
        private void I6() { }
        private void I5()
        {

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
                        case TokenType.PuntoyComa:
                        case TokenType.Pipe:
                        case TokenType.EOF:
                            ReduceRule(6);
                            break;
                        default:
                            //error
                            break;
                    }

                    break;
            }
        }
        private void I3()
        {
            switch (entrada.Peek().Tag)
            {
                case TokenType.Dospuntos:
                    pilaT.Push(entrada.Pop());
                    pilaE.Push(State.I4);
                    Table();
                    break;
                default:
                    //Error
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
                            pilaT.Push(entrada.Pop());
                            Table();
                            break;
                        case TokenType.EOF:
                            //Aceptar
                            break;
                        default:
                            //Error
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
                        case TokenType.EOF:
                        case TokenType.id:
                            ReduceRule(3);
                            Table();
                            break;
                        default:
                            //error
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
            I0();
        }
>>>>>>> 5565b6b8711cdd153a8ed6165fb8a01c053746d8
    }
}

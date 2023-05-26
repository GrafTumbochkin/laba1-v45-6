using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static laba1_v45_6.Token;

namespace laba1_v45_6
{
    internal class ClassLR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public ClassLR(List<Token> entertoken)
        {
            tokens = entertoken;
        }
        public Token GetLexems(int nextLex)
        {
            return tokens[nextLex];
        }
        //Lexems lexeme;

        public void Shift()
        {
            lexemStack.Push(GetLexems(nextLex));
            nextLex++;
        }
        public void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        private void Expr()
        {

            Expression expr1 = new Expression();
            while (GetLexems(nextLex).Type != TokenType.ENTER)
            {
                expr1.TakeToken(GetLexems(nextLex));
                Shift();
            }
            Token k = new Token(TokenType.EXPR);
            lexemStack.Push(k);
            expr1.Start();
        }
        public void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(Token.TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }
        void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            //успешное завершение разбора
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        case "<список_операторов>":
                            GoToState(1);
                            break;
                        case "<оператор>":
                            GoToState(2);
                            break;
                        case "<присвоение>":
                            GoToState(4);
                            break;
                        case "<условие>":
                            GoToState(3);
                            break;
                        case "<объявление>":
                            GoToState(5);
                            break;
                        default:
                            throw new Exception($"State0\nОжидалось  <список_операторов>,<оператор>,<присвоение>,<условие>,<объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                           
                        
                    }
                    break;
                
                case TokenType.IF:
                    GoToState(6);
                    break;
                case TokenType.DIM:
                    GoToState(7);
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"State0\nОжидалось терминал IDENTIFIER, IF,DIM но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    
            }
        }
        private void State1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_операторов>":
                            if(nextLex == tokens.Count)
                            {
                                Reduce(1, "<программа>");
                                break;
                            }
                            switch (GetLexems(nextLex).Type)
                            {
                                case TokenType.IF:
                                    Shift();
                                    break;
                                case TokenType.DIM:
                                    Shift();
                                    break;
                                case TokenType.IDENTIFIER:
                                    Shift();
                                    break;
                                default:
                                    throw new Exception();
                            }

                            break;
                       
                        case "<оператор>":
                            GoToState(9);
                            break;
                        case "<условие>":
                            GoToState(3);
                                break;
                        case "<присвоение>":
                            GoToState(4);
                                break;
                        case "<объявление>":
                            GoToState(5);
                            break;
                        default:
                            throw new Exception($"State1\nОжидалось  <программа>,<оператор>,<условие>,<присвоение>,<объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.IF:
                    GoToState(6);
                    break;
                case TokenType.DIM:
                    GoToState(7);
                    break;
                default:
                    throw new Exception($"State1\nОжидалось терминал IDENTIFIER, IF,DIM но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            Shift();
                            break;
                       
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(10);
                    break;
                default:
                    throw new Exception($"State2\nОжидалось терминал ENTER   но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                   
            }
        }
        void State3()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условие>")
                Reduce(1, "<оператор>");
            else
                throw new Exception($"State3\nОжидалось <оператор>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        void State4()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присвоение>")
                Reduce(1, "<оператор>");
            else
                throw new Exception($"State4\nОжидалось <оператор>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

        }


        void State5()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<объявление>")
                Reduce(1, "<оператор>");
            else
                throw new Exception($"State5\nОжидалось <оператор>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
           
        }
        void State6()
        {

            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<логическое_условие>":
                            GoToState(11);
                            break;

                        case "<операнд>":
                            GoToState(12);
                            break;
                        default:
                            throw new Exception($"State6\nОжидалось <логическое_условие>,<операнд> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(13);
                    break;
                case TokenType.LITERAL:
                    GoToState(14);
                    break;
                case TokenType.IF:
                    Shift();
                    break;
                default:
                    throw new Exception($"State6\nОжидалось терминал IDENTIFIER,LITERAL,IF   но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

            }
        }
        void State7()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.DIM:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            GoToState(15);
                            break;
                        default:
                            throw new Exception($"State7\nОжидалось <список_переменных>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");      
                    }
                    break;
               
                case TokenType.IDENTIFIER:
                    GoToState(16);
                    break; 
                default:
                    throw new Exception($"State7\nОжидалось терминал DIM,IDENTIFIER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    
            }
        }
        void State8()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IDENTIFIER:
                    Shift();
                    break;

                case TokenType.EQUAL:
                    GoToState(17);
                    break;

                default:
                    throw new Exception($"State8\nОжидалось терминал EQUAL   но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

            }
        }
        void State9()
        {
             switch (lexemStack.Peek().Type)
             {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            Shift();
                            break;
                       
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(18);
                    break;
                default:
                    throw new Exception($"State9\nОжидалось терминал ENTER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                   
             }
        }

        void State10()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ENTER:
                    Reduce(2, "<список_операторов>");
                         break;
                default:
                    throw new Exception($"State10\nОжидалось <список_операторов> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }

        }



        void State11()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<логическое_условие>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.THEN:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"State11\nОжидалось терминал THEN но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

            }
        }
        void State12()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                        case "<логический_знак>":
                            GoToState(20);
                            break;
                        default:
                            throw new Exception($"State12\nОжидалось <логический_знак> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
                case TokenType.MORE:
                    GoToState(21);
                    break;
                case TokenType.LESS:
                    GoToState(22);
                    break;
                case TokenType.EQUAL:
                    GoToState(23);
                    break;
                default:
                    throw new Exception($"State12\nОжидалось терминал MORE,LESS,EQUAL но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");

            }
        }
        void State13()
        {
            switch (lexemStack.Peek().Type)
            {   
                case TokenType.IDENTIFIER:
                    Reduce(1, "<операнд>");
                    break;
                default:
                    throw new Exception($"State13\nОжидалось <операнд> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State14()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LITERAL:
                    Reduce(1, "<операнд>");
                    break;
                default:
                    throw new Exception($"State14\nОжидалось <операнд> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State15()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            Shift();
                            break;    
                    }
                    break;
                case TokenType.AS:
                    GoToState(26);
                    break;
                case TokenType.COMMA:
                    GoToState(27);
                    break;
                default:
                    throw new Exception($"State15\nОжидалось терминал AS,COMMA  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State16()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IDENTIFIER:
                    Reduce(1, "<список_переменных>");
                    break;
                default:
                    throw new Exception($"State16\nОжидалось <список_переменных> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State17()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    Expr();
                    break;
                case TokenType.EXPR:
                    GoToState(28);
                    break;
                default:
                    throw new Exception($"State17\nОжидалось терминал EXPR но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State18()
        {
           
             
                switch (lexemStack.Peek().Type)
                {
                  case TokenType.ENTER:
                    Reduce(3, "<список_операторов>");
                    break;
                   default:
                    throw new Exception($"State18\nОжидалось <список_операторов>   но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                }
        
        }
        
        void State19()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.THEN:
                    Shift();
                    break;
                case TokenType.ENTER:
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"State19\nОжидалось терминал ENTER  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State20()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<логический_знак>":
                            Shift();
                            break;
                        case "<операнд>":
                            GoToState(30);
                            break;
                        default:
                            throw new Exception($"State20\nОжидалось <операнд>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
                
                case TokenType.IDENTIFIER:
                    GoToState(13);
                    break;
                case TokenType.LITERAL:
                    GoToState(14);
                    break;
                default:
                    throw new Exception($"State20\nОжидалось терминал IDENTIFIER,LITERAL  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State21()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MORE:
                    Reduce(1, "<логический_знак>"); 
                    break;
                default:
                    throw new Exception($"State21\nОжидалось <логический_знак>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State22()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LESS:
                    Reduce(1, "<логический_знак>");
                    break;
                default:
                    throw new Exception($"State22\nОжидалось <логический_знак>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State23()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    Reduce(1, "<логический_знак>");
                    break;
                default:
                    throw new Exception($"State23\nОжидалось <логический_знак>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State24()
        {
            
        }
        void State25()
        {
            
        }
        void State26()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        
                        case "<тип>":
                            GoToState(31);
                            break;
                        default:
                            throw new Exception($"State26\nОжидалось  <тип> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
                case TokenType.AS:
                    Shift();
                    break;
                case TokenType.INTEGER:
                    GoToState(32);
                    break;
                case TokenType.BOOL:
                    GoToState(33);
                    break;
                case TokenType.STRING:
                    GoToState(34);
                    break;
                default:
                    throw new Exception($"State26\nОжидалось терминал INTEGER,BOOL,STRING но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State27()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COMMA:
                    Shift();
                    break;
                    case TokenType.IDENTIFIER:
                        GoToState(35);
                    break;
                default:
                    throw new Exception($"State27\nОжидалось терминал IDENTIFIER  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        
        void State28()
        {
            switch (lexemStack.Peek().Type)
            {
               
                case TokenType.EXPR:
                     Reduce(3, "<присвоение>");
                    break;
                default:
                    throw new Exception($"State28\nОжидалось <присвоение>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State29()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {

                        case "<список_операторов>":
                            GoToState(36);
                            break;
                        case "<оператор>":
                            GoToState(2);
                            break;
                        case "<условие>":
                            GoToState(3);
                            break;
                        case "<присвоение>":
                            GoToState(4);
                            break;
                        case "<объявление>":
                            GoToState(5);

                            break;
                        default:
                            throw new Exception($"State29\nОжидалось  <список_операторов>,<оператор>,<условие>,<присвоение>,<объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
                case TokenType.ENTER:
                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(6);
                    break;
                case TokenType.DIM:
                    GoToState(7);
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"State29\nОжидалось терминал IF,DIM,IDENTIFIER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State30()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {

                        case "<операнд>":
                            Reduce(3, "<логическое_условие>");
                            break;
                       
                        default:
                            throw new Exception($"State30\nОжидалось <логическое_условие> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
            }
        }

        void State31()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {

                        case "<тип>":
                            Reduce(4, "<объявление>");
                            break;

                        default:
                            throw new Exception($"State31\nОжидалось <объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
            }
        }
        void State32()
        {
            switch (lexemStack.Peek().Type)
            {
                
                case TokenType.INTEGER:
                    Reduce(1, "<тип>");
                    break;
                default:
                    throw new Exception($"State32\nОжидалось <тип>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State33()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.BOOL:
                    Reduce(1, "<тип>");
                    break;
                default:
                    throw new Exception($"State33\nОжидалось <тип> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State34()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.STRING:
                    Reduce(1, "<тип>");
                    break;
                default:
                    throw new Exception($"State34\nОжидалось <тип> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State35()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.IDENTIFIER:
                    Reduce(3, "<список_переменных>");
                    break;
                default:
                    throw new Exception($"State35\nОжидалось <список_переменных>  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State36()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_операторов>":
                            Shift();
                            break;
                        case "<оператор>":
                            GoToState(9);
                            break;
                        case "<условие>":
                            GoToState(3);
                            break;
                        case "<присвоение>":
                            GoToState(4);
                            break;
                        case "<объявление>":
                            GoToState(5);
                            break;
                        default:
                            throw new Exception($"State36\nОжидалось <оператор>,<условие>,<присвоение>,<объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                    break;
                case TokenType.END:
                    GoToState(37);
                    break;
                case TokenType.ELSE:
                    GoToState(38);
                    break;
                case TokenType.IF:
                    GoToState(6);
                    break;
                case TokenType.DIM:
                    GoToState(7);
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"State36\nОжидалось терминал END,ELSE,IF,DIM,IDENTIFIER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(39);
                    break;
                default:
                    throw new Exception($"State37\nОжидалось терминал IF но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State38()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ELSE:
                    Shift();
                    break;
                case TokenType.ENTER:
                    GoToState(40);
                    break;
                default:
                    throw new Exception($"State38\nОжидалось терминал ENTER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
        void State39()
        {
            switch (lexemStack.Peek().Type)
            {


                case TokenType.IF:
                    Reduce(7, "<условие>");
                    break;
                default:
                    throw new Exception($"State39\nОжидалось <условие> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
            }
        }
            void State40()
            {
                switch (lexemStack.Peek().Type)
                {
                    case TokenType.NETERM:
                        switch (lexemStack.Peek().Value)
                        {
                            case "<список_операторов>":
                                GoToState(41);
                                break;
                            case "<оператор>":
                                GoToState(2);
                                break;
                            case "<условие>":
                                GoToState(3);
                                break;
                            case "<присвоение>":
                                GoToState(4);
                                break;
                            case "<объявление>":
                                GoToState(5);
                                break;
                            default:
                                throw new Exception($"State40\nОжидалось <список_операторов>, <оператор>,<условие>,<присвоение>,<объявление> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                        }
                        break;
                    case TokenType.ENTER:
                        Shift();
                        break;
                    case TokenType.IF:
                        GoToState(6);
                        break;
                    case TokenType.DIM:
                        GoToState(7);
                        break;
                    case TokenType.IDENTIFIER:
                        GoToState(8);
                        break;
                    default:
                        throw new Exception($"State40\nОжидалось терминал IF,DIM,IDENTIFIER но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                }
            }
                void State41()
                {
                    switch (lexemStack.Peek().Type)
                    {
                        case TokenType.NETERM:
                            switch (lexemStack.Peek().Value)
                            {
                                case "<список_операторов>":
                                    Shift();
                                    break;
                                
                            }
                            break;
                        
                        case TokenType.END:
                            GoToState(42);
                            break;
                        default:
                            throw new Exception($"State41\nОжидалось терминал END но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                }
                void State42()
                {
                    switch (lexemStack.Peek().Type)
                    {
                       
                        case TokenType.END:
                            Shift();
                            break;
                        case TokenType.IF:
                            GoToState(43);
                            break;
                        default:
                            throw new Exception($"State42\nОжидалось терминал IF но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                }
                void State43()
                {
                    switch (lexemStack.Peek().Type)
                    {
                       
                        case TokenType.IF:
                            Reduce(10, "<условие>");
                            break;
                        default:
                            throw new Exception($"State43\nОжидалось <условие> но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value} {stateStack.Count}");
                    }
                }
             



                public void Start()
                {
                    try
                    {
                        stateStack.Push(0);
                        while (isEnd != true)
                        {
                            switch (state)
                            {
                                case 0:
                                    State0();
                                    break;
                                case 1:
                                    State1();
                                    break;
                                case 2:
                                    State2();
                                    break;
                                case 3:
                                    State3();
                                    break;
                                case 4:
                                    State4();
                                    break;
                                case 5:
                                    State5();
                                    break;
                                case 6:
                                    State6();
                                    break;
                                case 7:
                                    State7();
                                    break;
                                case 8:
                                    State8();
                                    break;
                                case 9:
                                    State9();
                                    break;
                                case 10:
                                    State10();
                                    break;
                                case 11:
                                    State11();
                                    break;
                                case 12:
                                    State12();
                                    break;
                                case 13:
                                    State13();
                                    break;
                                case 14:
                                    State14();
                                    break;
                                case 15:
                                    State15();
                                    break;
                                case 16:
                                    State16();
                                    break;
                                case 17:
                                    State17();
                                    break;
                                case 18:
                                    State18();
                                    break;
                                case 19:
                                    State19();
                                    break;
                                case 20:
                                    State20();
                                    break;
                                case 21:
                                    State21();
                                    break;
                                case 22:
                                    State22();
                                    break;
                                case 23:
                                    State23();
                                    break;
                                case 24:
                                    State24();
                                    break;
                                case 25:
                                    State25();
                                    break;
                                case 26:
                                    State26();
                                    break;
                                case 27:
                                    State27();
                                    break;
                                case 28:
                                    State28();
                                    break;
                                case 29:
                                    State29();
                                    break;
                                case 30:
                                    State30();
                                    break;
                                case 31:
                                    State31();
                                    break;
                                case 32:
                                    State32();
                                    break;
                                case 33:
                                    State33();
                                    break;
                                case 34:
                                    State34();
                                    break;
                                case 35:
                                    State35();
                                    break;
                                case 36:
                                    State36();
                                    break;
                                case 37:
                                    State37();
                                    break;
                                case 38:
                                    State38();
                                    break;
                                case 39:
                                    State39();
                                    break;
                                case 40:
                                    State40();
                                    break;
                                case 41:
                                    State41();
                                    break;
                                case 42:
                                    State42();
                                    break;
                                case 43:
                                    State43();
                                    break;
                                //case 44:
                                //    State44();
                                //    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        MessageBox.Show($"Errror! {ex.Message}");
                    }

                }
            }
        }
using laba1_v45_6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace laba1_v45_6
{
    internal class Expression
    {
        List<Token> logicExprStack = new List<Token>();
        Stack<string> stackOfOperations = new Stack<string>();
        Stack<int> priorityStack = new Stack<int>();
        int index = 0;
        string output = null;
        Dictionary<string, int> priority = new Dictionary<string, int>()
         {
                     {"+", 0}, {"-", 0},
                     {"*", 1}, {"/", 1}
         };
        public void TakeToken(Token token)
        {
            logicExprStack.Add(token);
        }
        public void Start()
        {
            Dijkstra();
            ReversePolishNotation();
        }
        private void PushesOutOperationsHighestPriority(string operation)
        {
            int count = stackOfOperations.Count();
            Stack<string> temp = new Stack<string>();
            Stack<int> priorityTemp = new Stack<int>();
            for (int i = 0; i < count; i++)//Цикл выталкивания
            {
                if (priorityStack.Peek() >= priority[operation])
                {
                    output += stackOfOperations.Pop();
                    priorityStack.Pop();
                }
                else
                {
                    temp.Push(stackOfOperations.Pop());
                    priorityTemp.Push(priorityStack.Pop());
                }
            }
            temp.Reverse();
            priorityTemp.Reverse();
            int countTemp = temp.Count();
            for (int i = 0; i < countTemp; i++)
            {
                stackOfOperations.Push(temp.Pop());
                priorityStack.Push(priorityTemp.Pop());
            }
            stackOfOperations.Push(logicExprStack[index].Value);
            priorityStack.Push(priority[operation]);
        }
        private void Dijkstra()//Метод Дейкстры
        {
            if (logicExprStack[index].Type == Token.TokenType.IDENTIFIER || logicExprStack[index].Type == Token.TokenType.LITERAL)
            {
                priorityStack.Push(0);
             
            while (index != logicExprStack.Count())
                {
                    if (logicExprStack[index].Type == Token.TokenType.LITERAL || logicExprStack[index].Type == Token.TokenType.IDENTIFIER)
                    {
                        output += logicExprStack[index].Value + " ";
                        index++;
                    }
                    else if (logicExprStack[index].Type == Token.TokenType.PLUS)
                    {
                        string operation = "+";
                      
                    if ((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() ==
0)
                        {
                            stackOfOperations.Push(logicExprStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    else if (logicExprStack[index].Type == Token.TokenType.MINUS)
                    {
                        string operation = "-";
                        if ((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() ==
                       0)
                        {
                            stackOfOperations.Push(logicExprStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    else if (logicExprStack[index].Type == Token.TokenType.MULTIPLY)
                    {
                        string operation = "*";
                        if ((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() ==
                       0)
                        {
                            stackOfOperations.Push(logicExprStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    else if (logicExprStack[index].Type == Token.TokenType.DIVISION)
                    {
                        string operation = "/";
                        if ((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() ==
                       0)
                         
                    {
                            stackOfOperations.Push(logicExprStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    
                  
                     else
                    {
                        throw new Exception("Неверно составлено логическое выражение.");
                    }
                }
                int countOperations = stackOfOperations.Count();
                for (int i = 0; i < countOperations; i++)//Выталкивание всех оставшихся операций в стеке
            {
                    output += stackOfOperations.Pop();
                }
            }
            else
                throw new Exception("Неверно составлено логическое выражение.");
        }
            public void ReversePolishNotation()//Метод выполняющий преобразование обратную польскую нотацию в матричный вид
              {
                 Dictionary<int, string> M = new Dictionary<int, string>(); // словарь содержащий ключ и стринговое значение
                        Stack<string> stackOperand = new Stack<string>();
                        int key = 1;
                 for (int i = 0; i<output.Count(); i++)
                 {
                 char currentChar = output[i];
                 switch (currentChar)
                 {

                     case ('+'):
                     {
                        M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "+");
                        stackOperand.Push("M" + key.ToString());
                        key++;
                        break;
                     }
                     case ('-'):
                     {
                        M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "-");
                        stackOperand.Push("M" + key.ToString());
                        key++;
                        break;
                     }
                     case ('*'):
                     {
                        M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "*");
                        stackOperand.Push("M" + key.ToString());
                        key++;
                        break;
                     }
                     case ('/'):
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "/");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                                break;
                            }

                         default:
                         {
                            if (Regex.IsMatch(currentChar.ToString(), "^[a-zA-Z]+$") || Regex.IsMatch(currentChar.ToString(), "^[0-9]+$"))
                            {
                                string temp = null;
                                while (output[i] != ' ')
                                {
                                    temp += output[i].ToString();
                                    i++;
                                }
                                stackOperand.Push(temp);
                            }
                                        else if (currentChar == ' ')
                                        {
                                        }
                                        else
                                        {
                                            throw new System.Exception("Ошибка перевода в матричный вид");
                                        }
                                       break;
                         }
                  }
            }
                 Form1._Form1.update("Обратная польская нотация:");
                Form1._Form1.update(output);
                Form1._Form1.update("Матричный вид:");
                int countOutput = stackOperand.Count;
                for (int i = 0; i < countOutput; i++)
                {
                    Form1._Form1.update(stackOperand.Pop());
                }
                Form1._Form1.update(" ");
                int countM = M.Count;
                for (int i = 1; i < countM + 1; i++)
                {
                    Form1._Form1.update("M" + i + ":" + M[i]);
                }
        }
    }
}

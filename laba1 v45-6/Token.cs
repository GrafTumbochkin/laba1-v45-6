using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace laba1_v45_6
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);
        }
        public enum TokenType
        {
            AS,DIM,INTEGER, MAIN, THEN, INT, BOOL, LITERAL, IDENTIFIER, CURLYBRACECLOSE,
            IF, ELSE, TRUE, FALSE, PLUS, MORE, LESS, SEMICOLON, CURLYBRACEOPEN, EXPR,
            MINUS, EQUAL, MULTIPLY, RPAR, LPAR, ENTER, DIVISION, COMMA, STRING, NETERMINAL,NETERM, OR, AND, TO, END
        }

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
             { "Dim",  TokenType.DIM },
             { "if",   TokenType.IF },
             { "integer", TokenType.INTEGER },
             { "else", TokenType.ELSE },
             { "as",   TokenType.AS },
             { "then", TokenType.THEN },
             { "end", TokenType.END },
             { "and", TokenType.AND },
             { "or", TokenType.OR },
             { "to", TokenType.TO },
        };

        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return SpecialWords.ContainsKey(word);
        }

        public static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
             { '#', TokenType.ENTER },
             { '(', TokenType.LPAR },
             { ')', TokenType.RPAR },
             { '+', TokenType.PLUS },
             { '-', TokenType.MINUS },
             { '=', TokenType.EQUAL },
             { '>', TokenType.MORE },
             { '<', TokenType.LESS },
             { '*', TokenType.MULTIPLY },
             { '/', TokenType.DIVISION },
             { ',', TokenType.COMMA },
             { ';', TokenType.SEMICOLON },
             { '{', TokenType.CURLYBRACEOPEN },
             { '}', TokenType.CURLYBRACECLOSE }
        };

        public static bool IsSpecialSymbol(char ch)
        {
            return SpecialSymbols.ContainsKey(ch);
        }

        public static void PrintTokens(RichTextBox box3, List<Token> list, bool check)
        {
            int i = 0;

            if (check == false)
            {
                foreach (var t in list)
                {
                    i++;

                    box3.Text += $"{i} {t} ";
                    box3.Text += Environment.NewLine;
                }
                Recognizer recognizer = new Recognizer(list);
                recognizer.Start();
            }


            if (check == true)
            {
                foreach (var t in list)
                {
                    i++;

                    box3.Text += $"{i} {t} ";
                    box3.Text += Environment.NewLine;
                }
                ClassLR classLR = new ClassLR(list);
                classLR.Start();
            }
        }
    }
}

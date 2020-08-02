using Algebra.Operations;
using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Algebra.Parsing
{
    public class Tokenizer
    {
        private readonly TextReader reader;
        private readonly ICollection<string> variables;
        private readonly ICollection<string> functions;

        public Token Token { get; private set; }
        public Rational Number { get; private set; }
        public string TokenSignature { get; private set; }

        private char currentChar;

        public Tokenizer(TextReader reader, ICollection<string> variables, ICollection<string> functions)
        {
            this.reader = reader;
            this.variables = variables;
            this.functions = functions;
            NextChar();
            NextToken();
        }

        void NextChar()
        {
            int c = reader.Read();
            currentChar = c < 0 ? '\0' : char.ToLower((char)c);
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(currentChar))
            {
                NextChar();
            }

            switch (currentChar)
            {
                case '\0':
                    Token = Token.EOF;
                    return;

                case '+':
                    NextChar();
                    Token = Token.Add;
                    return;

                case '-':
                    NextChar();
                    Token = Token.Subtract;
                    return;

                case '*':
                    NextChar();
                    Token = Token.Multiply;
                    return;

                case '/':
                    NextChar();
                    Token = Token.Divide;
                    return;

                case '^':
                    NextChar();
                    Token = Token.Exponent;
                    return;

                case '(':
                    NextChar();
                    Token = Token.OpenBrace;
                    return;

                case ')':
                    NextChar();
                    Token = Token.CloseBrace;
                    return;

                case ',':
                    NextChar();
                    Token = Token.Comma;
                    return;
            }

            if (char.IsDigit(currentChar))
            {
                StringBuilder stringBuilder = new StringBuilder();

                bool haveDecimalPoint = false;
                bool haveDenominator = false;

                while (char.IsDigit(currentChar)
                    || (!haveDecimalPoint && !haveDenominator && currentChar == '.')
                    || (!haveDecimalPoint && !haveDenominator && currentChar == '/'))
                {
                    stringBuilder.Append(currentChar);
                    haveDecimalPoint = haveDecimalPoint || currentChar == '.';
                    haveDenominator = haveDenominator || currentChar == '/';
                    NextChar();
                }

                if (!haveDenominator)
                {
                    Number = Rational.ParseDecimal(stringBuilder.ToString());
                }
                else
                {
                    Number = Rational.Parse(stringBuilder.ToString());
                }
                Token = Token.Decimal;
                return;
            }

            if (char.IsLetter(currentChar))
            {
                StringBuilder stringBuilder = new StringBuilder();
                while (char.IsLetter(currentChar))
                {
                    stringBuilder.Append(currentChar);
                    NextChar();
                }

                string identifier = stringBuilder.ToString();
                if (variables.Contains(identifier))
                {
                    TokenSignature = identifier;
                    Token = Token.Variable;
                    return;
                }
                else if (functions.Contains(identifier))
                {
                    TokenSignature = identifier;
                    Token = Token.Function;
                    return;
                }
                else
                {
                    throw new InvalidDataException($"Unknown identifier: '{identifier}'");
                }
            }

            throw new InvalidDataException($"Unexpected token '{currentChar + reader.ReadToEnd()}'");
        }
    }
}
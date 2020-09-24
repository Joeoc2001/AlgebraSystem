using Algebra.Atoms;
using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Algebra.Parsing
{
    public class Tokenizer
    {
        private readonly TextReader _reader;
        private readonly ICollection<string> _functions;
        private readonly ICollection<string> _namedConstants;

        public Token Token { get; private set; }
        public Rational Number { get; private set; }
        public string TokenSignature { get; private set; }

        private char _currentChar;

        public Tokenizer(TextReader reader, ICollection<string> functions, ICollection<string> namedConstants)
        {
            _reader = reader;
            _functions = functions;
            _namedConstants = namedConstants;
            NextChar();
            NextToken();
        }

        void NextChar()
        {
            int c = _reader.Read();
            _currentChar = c < 0 ? '\0' : (char)c;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }

            switch (_currentChar)
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

            if (char.IsDigit(_currentChar))
            {
                StringBuilder stringBuilder = new StringBuilder();

                bool haveDecimalPoint = false;
                bool haveDenominator = false;

                while (char.IsDigit(_currentChar)
                    || (!haveDecimalPoint && !haveDenominator && _currentChar == '.')
                    || (!haveDecimalPoint && !haveDenominator && _currentChar == '/'))
                {
                    stringBuilder.Append(_currentChar);
                    haveDecimalPoint = haveDecimalPoint || _currentChar == '.';
                    haveDenominator = haveDenominator || _currentChar == '/';
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

            if (char.IsLetter(_currentChar))
            {
                StringBuilder stringBuilder = new StringBuilder();
                while (char.IsLetter(_currentChar))
                {
                    stringBuilder.Append(_currentChar);
                    NextChar();
                }

                string identifier = stringBuilder.ToString();
                if (_functions.Contains(identifier))
                {
                    TokenSignature = identifier;
                    Token = Token.Function;
                    return;
                }
                else if (_namedConstants.Contains(identifier))
                {
                    TokenSignature = identifier;
                    Token = Token.NamedConstant;
                    return;
                }
                else
                {
                    TokenSignature = identifier;
                    Token = Token.Variable;
                    return;
                }
            }

            throw new InvalidDataException($"Unexpected token '{_currentChar + _reader.ReadToEnd()}'");
        }
    }
}
using Algebra.Functions;
using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algebra.Parsing
{
    public class Parser
    {
        private readonly Tokenizer _tokenizer;
        private readonly FunctionGeneratorSet _functions;
        private readonly NamedConstantSet _namedConstants;

        private Parser(Tokenizer tokenizer, FunctionGeneratorSet functions, NamedConstantSet namedConstants)
        {
            _tokenizer = tokenizer;
            _functions = functions;
            _namedConstants = namedConstants;
        }

        public static Expression Parse(string s, FunctionGeneratorSet functions = null, NamedConstantSet namedConstants = null)
        {
            functions = functions ?? FunctionGeneratorSet.DefaultFunctionGenerators;
            namedConstants = namedConstants ?? NamedConstantSet.DefaultConstants;

            // Create objects
            Tokenizer t = new Tokenizer(new StringReader(s), functions.Names, namedConstants.Names);
            Parser p = new Parser(t, functions, namedConstants);

            // Parse
            return p.Parse();
        }

        public Expression Parse()
        {
            var expr = ParseAddSubtract();

            // Check everything was consumed
            if (_tokenizer.Token != Token.EOF)
            {
                throw new SyntaxException("Unexpected characters at end of expression");
            }

            return expr;
        }

        // Parse an sequence of add/subtract operators
        Expression ParseAddSubtract()
        {
            // Collate all terms into a list
            List<Expression> terms = new List<Expression>
        {
            ParseMultiply()
        };

            bool subtractNext;
            while (true)
            {
                if (_tokenizer.Token == Token.Add)
                {
                    subtractNext = false;
                }
                else if (_tokenizer.Token == Token.Subtract)
                {
                    subtractNext = true;
                }
                else
                {
                    return Sum.Add(terms);
                }

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseMultiply();
                if (subtractNext)
                {
                    if (rhs is RationalConstant constant)
                    {
                        rhs = RationalConstant.ConstantFrom(-constant.GetValue());
                    }
                    else
                    {
                        rhs *= -1;
                    }
                }
                terms.Add(rhs);
            }
        }

        // Parse an sequence of multiply operators
        Expression ParseMultiply()
        {
            // Collate all terms into a list
            List<Expression> terms = new List<Expression>
            {
                ParseDivision()
            };

            while (true)
            {
                if (_tokenizer.Token != Token.Multiply)
                {
                    return Product.Multiply(terms);
                }

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseDivision();
                terms.Add(rhs);
            }
        }

        // Parse an sequence of Division operators
        Expression ParseDivision()
        {
            Expression lhs = ParseExponent();

            while (true)
            {
                if (_tokenizer.Token != Token.Divide)
                {
                    return lhs;
                }

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseExponent();
                lhs /= rhs;
            }
        }

        // Parse an sequence of exponent operators
        Expression ParseExponent()
        {
            Expression lhs = ParseLeaf();

            while (true)
            {
                if (_tokenizer.Token != Token.Exponent)
                {
                    return lhs;
                }

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseLeaf();
                lhs = Expression.Pow(lhs, rhs);
            }
        }

        // Parse a leaf node (Variable, Constant or Function)
        Expression ParseLeaf()
        {
            switch (_tokenizer.Token)
            {
                case Token.Subtract:
                    return ParseNegate();
                case Token.Decimal:
                    return ParseDecimal();
                case Token.Variable:
                    return ParseVariable();
                case Token.NamedConstant:
                    return ParseNamedConstant();
                case Token.OpenBrace:
                    return ParseOpenBrace();
                case Token.Function:
                    return ParseFunction();
                default:
                    break;
            }

            throw new SyntaxException($"Unexpected leaf token: {_tokenizer.Token}");
        }

        Expression ParseNegate()
        {
            _tokenizer.NextToken();
            return -1 * ParseLeaf();
        }

        Expression ParseDecimal()
        {
            Expression node = Expression.ConstantFrom(_tokenizer.Number);
            _tokenizer.NextToken();
            return node;
        }

        Expression ParseVariable()
        {
            string name = _tokenizer.TokenSignature;
            Expression node = new Variable(name);
            _tokenizer.NextToken();
            return node;
        }

        Expression ParseNamedConstant()
        {
            string name = _tokenizer.TokenSignature;

            if (!_namedConstants.TryGetValue(name, out IConstant constant))
            {
                throw new SyntaxException($"Tokenizer reported a Named Constant but none with the name {name} is present in the named constant set");
            }

            Expression node = constant.ToExpression();
            _tokenizer.NextToken();
            return node;
        }

        Expression ParseOpenBrace()
        {
            _tokenizer.NextToken();

            Expression node = ParseAddSubtract();

            if (_tokenizer.Token != Token.CloseBrace)
            {
                throw new SyntaxException("Missing close parenthesis");
            }

            _tokenizer.NextToken();

            return node;
        }

        Expression ParseFunction()
        {
            string functionName = _tokenizer.TokenSignature;

            _tokenizer.NextToken();

            List<Expression> nodes;
            if (_tokenizer.Token == Token.OpenBrace)
            {
                _tokenizer.NextToken();
                nodes = new List<Expression>()
                    {
                        ParseAddSubtract()
                    };

                while (_tokenizer.Token == Token.Comma)
                {
                    _tokenizer.NextToken();
                    nodes.Add(ParseAddSubtract());
                }

                if (_tokenizer.Token != Token.CloseBrace)
                {
                    throw new SyntaxException("Missing close parenthesis");
                }

                _tokenizer.NextToken();
            }
            else
            {
                nodes = new List<Expression>()
                    {
                        ParseLeaf()
                    };
            }

            // Create the function
            FunctionGenerator factory = _functions[functionName];
            try
            {
                return factory.CreateExpression(nodes);
            }
            catch (ArgumentException e)
            {
                throw new SyntaxException($"Incorrect arguments on function {functionName} - {e.Message}");
            }
        }
    }
}
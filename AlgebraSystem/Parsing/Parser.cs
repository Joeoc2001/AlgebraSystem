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

        public Parser(Tokenizer tokenizer, FunctionGeneratorSet functions)
        {
            this._tokenizer = tokenizer;
            this._functions = functions;
        }

        public static Expression Parse(string s)
        {
            HashSet<string> variables = new HashSet<string>() { "x", "y", "z" };
            return Parse(s, variables);
        }

        public static Expression Parse(string s, ICollection<string> variables)
        {
            FunctionGeneratorSet functions = FunctionGeneratorSet.DefaultFunctions;
            return Parse(s, variables, functions);
        }

        public static Expression Parse(string s, ICollection<string> variables, FunctionGeneratorSet functions)
        {
            // Ensure that all identifiers are in lower case
            HashSet<string> variablesLower = new HashSet<string>();
            foreach (string variable in variables)
            {
                variablesLower.Add(variable.ToLower());
            }

            // Create objects
            Tokenizer t = new Tokenizer(new StringReader(s), variablesLower, functions.Names);
            Parser p = new Parser(t, functions);

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
                    if (rhs is Constant constant)
                    {
                        rhs = Constant.ConstantFrom(-constant.GetValue());
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
            if (_tokenizer.Token == Token.Subtract)
            {
                _tokenizer.NextToken();
                return -1 * ParseLeaf();
            }

            if (_tokenizer.Token == Token.Decimal)
            {
                Expression node = Constant.ConstantFrom(_tokenizer.Number);
                _tokenizer.NextToken();
                return node;
            }

            if (_tokenizer.Token == Token.Variable)
            {
                string name = _tokenizer.TokenSignature;
                Expression node = new Variable(name);
                _tokenizer.NextToken();
                return node;
            }

            if (_tokenizer.Token == Token.OpenBrace)
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

            if (_tokenizer.Token == Token.Function)
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

            throw new SyntaxException($"Unexpected leaf token: {_tokenizer.Token}");
        }
    }
}
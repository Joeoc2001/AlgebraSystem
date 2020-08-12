using Algebra.Functions;
using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algebra.Parsing
{
    public class Parser
    {
        private readonly Tokenizer tokenizer;
        private readonly IDictionary<string, IFunctionGenerator> functions;

        public Parser(Tokenizer tokenizer, IDictionary<string, IFunctionGenerator> functions)
        {
            this.tokenizer = tokenizer;
            this.functions = functions;
        }

        public static Expression Parse(string s)
        {
            HashSet<string> variables = new HashSet<string>() { "x", "y", "z" };
            return Parse(s, variables);
        }

        public static Expression Parse(string s, ICollection<string> variables)
        {
            IDictionary<string, IFunctionGenerator> functions = FunctionGenerator.DefaultFunctions;
            return Parse(s, variables, functions);
        }

        public static Expression Parse(string s, ICollection<string> variables, IDictionary<string, IFunctionGenerator> functions)
        {
            // Ensure that all identifiers are in lower case
            HashSet<string> variablesLower = new HashSet<string>();
            foreach (string variable in variables)
            {
                variablesLower.Add(variable.ToLower());
            }
            Dictionary<string, IFunctionGenerator> functionsLower = new Dictionary<string, IFunctionGenerator>();
            foreach (string function in functions.Keys)
            {
                functionsLower.Add(function.ToLower(), functions[function]);
            }

            // Create objects
            Tokenizer t = new Tokenizer(new StringReader(s), variablesLower, functionsLower.Keys);
            Parser p = new Parser(t, functionsLower);

            // Parse
            return p.Parse();
        }

        public Expression Parse()
        {
            var expr = ParseAddSubtract();

            // Check everything was consumed
            if (tokenizer.Token != Token.EOF)
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
            ParseMultiplyDivide()
        };

            bool subtractNext;
            while (true)
            {
                if (tokenizer.Token == Token.Add)
                {
                    subtractNext = false;
                }
                else if (tokenizer.Token == Token.Subtract)
                {
                    subtractNext = true;
                }
                else
                {
                    return Sum.Add(terms);
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseMultiplyDivide();
                if (subtractNext)
                {
                    if (rhs is Constant constant)
                    {
                        rhs = Constant.From(-constant.GetValue());
                    }
                    else
                    {
                        rhs *= -1;
                    }
                }
                terms.Add(rhs);
            }
        }

        // Parse an sequence of multiply/divide operators
        Expression ParseMultiplyDivide()
        {
            // Collate all terms into a list
            List<Expression> terms = new List<Expression>
        {
            ParseExponent()
        };

            bool reciprocalNext;
            while (true)
            {
                if (tokenizer.Token == Token.Multiply)
                {
                    reciprocalNext = false;
                }
                else if (tokenizer.Token == Token.Divide)
                {
                    reciprocalNext = true;
                }
                else
                {
                    return Product.Multiply(terms);
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseExponent();
                if (reciprocalNext)
                {
                    rhs = Expression.Pow(rhs, -1);
                }
                terms.Add(rhs);
            }
        }

        // Parse an sequence of exponent operators
        Expression ParseExponent()
        {
            Expression lhs = ParseLeaf();

            while (true)
            {
                if (tokenizer.Token != Token.Exponent)
                {
                    return lhs;
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                Expression rhs = ParseLeaf();
                lhs = Expression.Pow(lhs, rhs);
            }
        }

        // Parse a leaf node (Variable, Constant or Function)
        Expression ParseLeaf()
        {
            if (tokenizer.Token == Token.Subtract)
            {
                tokenizer.NextToken();
                return -1 * ParseLeaf();
            }

            if (tokenizer.Token == Token.Decimal)
            {
                Expression node = Constant.From(tokenizer.Number);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.Variable)
            {
                string name = tokenizer.TokenSignature;
                Expression node = new Variable(name);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.OpenBrace)
            {
                tokenizer.NextToken();

                Expression node = ParseAddSubtract();

                if (tokenizer.Token != Token.CloseBrace)
                {
                    throw new SyntaxException("Missing close parenthesis");
                }

                tokenizer.NextToken();

                return node;
            }

            if (tokenizer.Token == Token.Function)
            {
                string functionName = tokenizer.TokenSignature;

                tokenizer.NextToken();

                List<Expression> nodes;
                if (tokenizer.Token == Token.OpenBrace)
                {
                    tokenizer.NextToken();
                    nodes = new List<Expression>()
                    {
                        ParseAddSubtract()
                    };

                    while (tokenizer.Token == Token.Comma)
                    {
                        tokenizer.NextToken();
                        nodes.Add(ParseAddSubtract());
                    }

                    if (tokenizer.Token != Token.CloseBrace)
                    {
                        throw new SyntaxException("Missing close parenthesis");
                    }

                    tokenizer.NextToken();
                }
                else
                {
                    nodes = new List<Expression>()
                    {
                        ParseLeaf()
                    };
                }

                // Create the function
                IFunctionGenerator factory = functions[functionName];
                try
                {
                    return factory.CreateExpression(nodes);
                }
                catch (ArgumentException e)
                {
                    throw new SyntaxException($"Incorrect arguments on function {functionName} - {e.Message}");
                }
            }

            throw new SyntaxException($"Unexpected leaf token: {tokenizer.Token}");
        }
    }
}
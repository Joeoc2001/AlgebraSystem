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

        public static IExpression Parse(string s)
        {
            HashSet<string> variables = new HashSet<string>() { "x", "y", "z" };
            return Parse(s, variables);
        }

        public static IExpression Parse(string s, ICollection<string> variables)
        {
            IDictionary<string, IFunctionGenerator> functions = FunctionGenerator.DefaultFunctions;
            return Parse(s, variables, functions);
        }

        public static IExpression Parse(string s, ICollection<string> variables, IDictionary<string, IFunctionGenerator> functions)
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

        public IExpression Parse()
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
        IExpression ParseAddSubtract()
        {
            // Collate all terms into a list
            List<IExpression> terms = new List<IExpression>
        {
            ParseMultiply()
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
                IExpression rhs = ParseMultiply();
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
        IExpression ParseMultiply()
        {
            // Collate all terms into a list
            List<IExpression> terms = new List<IExpression>
            {
                ParseDivision()
            };

            while (true)
            {
                if (tokenizer.Token != Token.Multiply)
                {
                    return Product.Multiply(terms);
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                IExpression rhs = ParseDivision();
                terms.Add(rhs);
            }
        }

        // Parse an sequence of Division operators
        IExpression ParseDivision()
        {
            IExpression lhs = ParseExponent();

            while (true)
            {
                if (tokenizer.Token != Token.Divide)
                {
                    return lhs;
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                IExpression rhs = ParseExponent();
                lhs /= rhs;
            }
        }

        // Parse an sequence of exponent operators
        IExpression ParseExponent()
        {
            IExpression lhs = ParseLeaf();

            while (true)
            {
                if (tokenizer.Token != Token.Exponent)
                {
                    return lhs;
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                IExpression rhs = ParseLeaf();
                lhs = Expression.Pow(lhs, rhs);
            }
        }

        // Parse a leaf node (Variable, Constant or Function)
        IExpression ParseLeaf()
        {
            if (tokenizer.Token == Token.Subtract)
            {
                tokenizer.NextToken();
                return -1 * ParseLeaf();
            }

            if (tokenizer.Token == Token.Decimal)
            {
                IExpression node = Constant.ConstantFrom(tokenizer.Number);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.Variable)
            {
                string name = tokenizer.TokenSignature;
                IExpression node = new Variable(name);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.OpenBrace)
            {
                tokenizer.NextToken();

                IExpression node = ParseAddSubtract();

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

                List<IExpression> nodes;
                if (tokenizer.Token == Token.OpenBrace)
                {
                    tokenizer.NextToken();
                    nodes = new List<IExpression>()
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
                    nodes = new List<IExpression>()
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
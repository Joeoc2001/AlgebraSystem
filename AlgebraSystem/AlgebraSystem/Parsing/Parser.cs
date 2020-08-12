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
        private readonly IDictionary<string, FunctionIdentity> functions;

        public Parser(Tokenizer tokenizer, IDictionary<string, FunctionIdentity> functions)
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
            IDictionary<string, FunctionIdentity> functions = FunctionIdentity.DefaultFunctions;
            return Parse(s, variables, functions);
        }

        public static Expression Parse(string s, ICollection<string> variables, IDictionary<string, FunctionIdentity> functions)
        {
            // Ensure that all identifiers are in lower case
            HashSet<string> variablesLower = new HashSet<string>();
            foreach (string variable in variables)
            {
                variablesLower.Add(variable.ToLower());
            }
            Dictionary<string, FunctionIdentity> functionsLower = new Dictionary<string, FunctionIdentity>();
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

                return MakeFunction(nodes, functionName);
            }

            throw new SyntaxException($"Unexpected leaf token: {tokenizer.Token}");
        }

        private Expression MakeFunction(List<Expression> nodes, string functionName)
        {
            FunctionIdentity factory = functions[functionName];
            return factory.CreateExpression(nodes);
            //int requiredParameters;
            //Func<IList<Expression>, Expression> constructor;
            //switch (functionName)
            //{
            //    case "log":
            //    case "ln":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.LnOf(ns[0]);
            //        break;
            //    case "sign":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.SignOf(ns[0]);
            //        break;
            //    case "abs":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.Abs(ns[0]);
            //        break;
            //    case "min":
            //        requiredParameters = 2;
            //        constructor = ns => Expression.Min(ns[0], ns[1]);
            //        break;
            //    case "max":
            //        requiredParameters = 2;
            //        constructor = ns => Expression.Max(ns[0], ns[1]);
            //        break;
            //    case "sin":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.SinOf(ns[0]);
            //        break;
            //    case "cos":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.CosOf(ns[0]);
            //        break;
            //    case "tan":
            //        requiredParameters = 1;
            //        constructor = ns => Expression.TanOf(ns[0]);
            //        break;
            //    default:
            //        throw new SyntaxException($"Unknown function name: {tokenizer.FunctionName}");
            //}

            //if (nodes.Count != requiredParameters)
            //{
            //    throw new SyntaxException($"Incorrect number of parameters for {tokenizer.FunctionName}: {nodes.Count}");
            //}

            //return constructor(nodes);
        }
    }
}
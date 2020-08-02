using Algebra.Functions;
using Algebra.Operations;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algebra.Parsing
{
    public class Parser
    {
        private readonly Tokenizer tokenizer;
        private readonly IDictionary<string, FunctionFactory> functions;

        public Parser(Tokenizer tokenizer, IDictionary<string, FunctionFactory> functions)
        {
            this.tokenizer = tokenizer;
            this.functions = functions;
        }

        public static Equation Parse(string s)
        {
            HashSet<string> variables = new HashSet<string>() { "x", "y", "z" };
            return Parse(s, variables);
        }

        public static Equation Parse(string s, ICollection<string> variables)
        {
            IDictionary<string, FunctionFactory> functions = FunctionFactory.DefaultFunctions;
            return Parse(s, variables, functions);
        }

        public static Equation Parse(string s, ICollection<string> variables, IDictionary<string, FunctionFactory> functions)
        {
            // Ensure that all identifiers are in lower case
            HashSet<string> variablesLower = new HashSet<string>();
            foreach (string variable in variables)
            {
                variablesLower.Add(variable.ToLower());
            }
            Dictionary<string, FunctionFactory> functionsLower = new Dictionary<string, FunctionFactory>();
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

        public Equation Parse()
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
        Equation ParseAddSubtract()
        {
            // Collate all terms into a list
            List<Equation> terms = new List<Equation>
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
                Equation rhs = ParseMultiplyDivide();
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
        Equation ParseMultiplyDivide()
        {
            // Collate all terms into a list
            List<Equation> terms = new List<Equation>
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
                Equation rhs = ParseExponent();
                if (reciprocalNext)
                {
                    rhs = Equation.Pow(rhs, -1);
                }
                terms.Add(rhs);
            }
        }

        // Parse an sequence of exponent operators
        Equation ParseExponent()
        {
            Equation lhs = ParseLeaf();

            while (true)
            {
                if (tokenizer.Token != Token.Exponent)
                {
                    return lhs;
                }

                // Skip the operator
                tokenizer.NextToken();

                // Parse the next term in the expression
                Equation rhs = ParseLeaf();
                lhs = Equation.Pow(lhs, rhs);
            }
        }

        // Parse a leaf node (Variable, Constant or Function)
        Equation ParseLeaf()
        {
            if (tokenizer.Token == Token.Subtract)
            {
                tokenizer.NextToken();
                return -1 * ParseLeaf();
            }

            if (tokenizer.Token == Token.Decimal)
            {
                Equation node = Constant.From(tokenizer.Number);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.Variable)
            {
                string name = tokenizer.TokenSignature;
                Equation node = new Variable(name);
                tokenizer.NextToken();
                return node;
            }

            if (tokenizer.Token == Token.OpenBrace)
            {
                tokenizer.NextToken();

                Equation node = ParseAddSubtract();

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

                List<Equation> nodes;
                if (tokenizer.Token == Token.OpenBrace)
                {
                    tokenizer.NextToken();
                    nodes = new List<Equation>()
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
                    nodes = new List<Equation>()
                    {
                        ParseLeaf()
                    };
                }

                return MakeFunction(nodes, functionName);
            }

            throw new SyntaxException($"Unexpected leaf token: {tokenizer.Token}");
        }

        private Equation MakeFunction(IList<Equation> nodes, string functionName)
        {
            FunctionFactory factory = functions[functionName];
            return factory.CreateEquation(nodes);
            //int requiredParameters;
            //Func<IList<Equation>, Equation> constructor;
            //switch (functionName)
            //{
            //    case "log":
            //    case "ln":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.LnOf(ns[0]);
            //        break;
            //    case "sign":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.SignOf(ns[0]);
            //        break;
            //    case "abs":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.Abs(ns[0]);
            //        break;
            //    case "min":
            //        requiredParameters = 2;
            //        constructor = ns => Equation.Min(ns[0], ns[1]);
            //        break;
            //    case "max":
            //        requiredParameters = 2;
            //        constructor = ns => Equation.Max(ns[0], ns[1]);
            //        break;
            //    case "sin":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.SinOf(ns[0]);
            //        break;
            //    case "cos":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.CosOf(ns[0]);
            //        break;
            //    case "tan":
            //        requiredParameters = 1;
            //        constructor = ns => Equation.TanOf(ns[0]);
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
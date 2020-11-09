using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algebra.Compilation
{
    public abstract class HeapCompiler<ReturnType, Compiled> : Compiler<ReturnType>
    {
        public class ExecutionOrderException : Exception
        {
            public ExecutionOrderException(string message) : base(message)
            {
            }
        }

        private class CompileTraverser : IMapping<int>
        {
            private readonly HeapCompiler<ReturnType, Compiled> _compiler;

            private readonly List<(Compiled instr, int lastUsed)> _instructions = new List<(Compiled, int)>();
            private readonly Dictionary<Expression, int> _resultLocation = new Dictionary<Expression, int>();
            private readonly Dictionary<string, int> _seenVariables = new Dictionary<string, int>();

            public CompileTraverser(HeapCompiler<ReturnType, Compiled> compiler)
            {
                _compiler = compiler;
            }

            public Compiled[] GetCompiled()
            {
                return _instructions.Select(i => i.instr).ToArray();
            }

            public int[] GetLastUses()
            {
                return _instructions.Select(i => i.lastUsed).ToArray();
            }

            public Dictionary<string, int> GetSeenVariables()
            {
                return _seenVariables;
            }

            private int EvaluateArgument(Expression arg)
            {
                if (!_resultLocation.TryGetValue(arg, out int loc))
                {
                    loc = arg.Map(this);
                    _resultLocation.Add(arg, loc);
                }

                return loc;
            }

            private void UpdateLastUsed(int argLoc, int index)
            {
                (Compiled instr, int _) = _instructions[argLoc];
                int lastUsed = index;
                _instructions[argLoc] = (instr, lastUsed);
            }

            private int EvaluateMonad(Expression arg, Func<int, int, Compiled> gen)
            {
                int argLoc = EvaluateArgument(arg);

                int resultLoc = _instructions.Count;
                UpdateLastUsed(argLoc, resultLoc);

                Compiled instr = gen(argLoc, resultLoc);
                _instructions.Add((instr, resultLoc));

                return resultLoc;
            }

            public int EvaluateArcsin(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _compiler.EvaluateArcsin);
            }

            public int EvaluateArctan(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _compiler.EvaluateArctan);
            }

            public int EvaluateLn(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _compiler.EvaluateLn);
            }

            public int EvaluateSign(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _compiler.EvaluateSign);
            }

            public int EvaluateSin(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _compiler.EvaluateSin);
            }

            public int EvaluateConstant(IConstant value)
            {
                int resultLoc = _instructions.Count;

                Compiled instr = _compiler.EvaluateConstant(value, resultLoc);
                _instructions.Add((instr, resultLoc));

                return resultLoc;
            }

            public int EvaluateVariable(IVariable value)
            {
                if (!_seenVariables.ContainsKey(value.GetName()))
                {
                    _seenVariables.Add(value.GetName(), _seenVariables.Count);
                }

                int resultLoc = _instructions.Count;

                Compiled instr = _compiler.EvaluateVariable(value, _seenVariables, resultLoc);
                _instructions.Add((instr, resultLoc));

                return resultLoc;
            }

            private int EvaluateSequential(ICollection<Expression> expressions, Func<int, int, int, Compiled> gen)
            {
                if (expressions.Count == 0)
                {
                    throw new IndexOutOfRangeException("Empty sequential not supported");
                }

                var enumerator = expressions.GetEnumerator();
                enumerator.MoveNext();

                int resultLoc = EvaluateArgument(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    int argLoc = EvaluateArgument(enumerator.Current);

                    int newResultLoc = _instructions.Count;
                    UpdateLastUsed(resultLoc, newResultLoc);
                    UpdateLastUsed(argLoc, newResultLoc);

                    Compiled compiled = gen(resultLoc, argLoc, newResultLoc);
                    _instructions.Add((compiled, newResultLoc));

                    resultLoc = newResultLoc;
                }
                return resultLoc;
            }

            public int EvaluateSum(ICollection<Expression> expressions)
            {
                return EvaluateSequential(expressions, _compiler.EvaluateSum);
            }

            public int EvaluateProduct(ICollection<Expression> expressions)
            {
                return EvaluateSequential(expressions, _compiler.EvaluateProduct);
            }

            public int EvaluateExponent(Expression baseExpression, Expression powerExpression)
            {
                int argLoc1 = EvaluateArgument(baseExpression);
                int argLoc2 = EvaluateArgument(powerExpression);

                int resultLoc = _instructions.Count;
                UpdateLastUsed(argLoc1, resultLoc);
                UpdateLastUsed(argLoc2, resultLoc);

                Compiled instr = _compiler.EvaluateExponent(argLoc1, argLoc2, resultLoc);
                _instructions.Add((instr, resultLoc));

                return resultLoc;
            }

            public int EvaluateFunction(Function function)
            {
                if (_compiler._supportedFunctions.Contains(function.GetIdentity()))
                {
                    List<int> args = new List<int>();
                    foreach (Expression param in function.GetParameterList())
                    {
                        args.Add(EvaluateArgument(param));
                    }
                    int resultLoc = _instructions.Count;

                    Compiled compiled = _compiler.EvaluateFunction(function.GetIdentity(), args, resultLoc);
                    _instructions.Add((compiled, resultLoc));

                    foreach (int arg in args)
                    {
                        UpdateLastUsed(arg, resultLoc);
                    }

                    return resultLoc;
                }
                else
                {
                    return function.GetAtomicBodiedExpression().Map(this);
                }
            }
        }

        public HeapCompiler(ICollection<FunctionIdentity> supportedFunctions)
            : base(supportedFunctions)
        {

        }

        protected abstract ICompiledFunction<ReturnType> CreateCompiled(Expression expression, Compiled[] instructions, IEnumerable<string> variables, int[] indirectionTable, int cellCount);

        public override ICompiledFunction<ReturnType> Compile(Expression expression, IEnumerable<string> parameterOrdering = null, int simplificationAggressiveness=3)
        {
            expression = expression.Simplify(metric:_simplificationMetric, depth:simplificationAggressiveness, equivalencies:_paths);

            // Compile and extract features
            CompileTraverser traverser = new CompileTraverser(this);
            expression.Map(traverser);
            Compiled[] instructions = traverser.GetCompiled();
            IEnumerable<string> seenVariables = traverser.GetSeenVariables().OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();
            int[] lastUses = traverser.GetLastUses();

            // Create indirection table
            int[] indirectionTable = GenerateIndirectionTable(lastUses);
            int cellCount = indirectionTable.Max() + 1;

            // Calculate variables
            IEnumerable<string> variables = CompareVariables(parameterOrdering, seenVariables);

            return CreateCompiled(expression, instructions, variables, indirectionTable, cellCount);
        }

        private static int[] GenerateIndirectionTable(int[] lastUses)
        {
            HashSet<int> free = new HashSet<int>();
            int next = 0;

            Dictionary<int, List<int>> freeEvents = new Dictionary<int, List<int>>();
            int[] mapping = new int[lastUses.Length];

            for (int i = 0; i < lastUses.Length; i++)
            {
                // Free all memory locations freed by this operation
                if (freeEvents.TryGetValue(i, out List<int> freedByThis))
                {
                    free.UnionWith(freedByThis);
                    freeEvents.Remove(i);
                }

                // Get the new location for the result of this operation
                int newLoc;
                if (free.Count == 0)
                {
                    newLoc = next;
                    next += 1; 
                }
                else
                {
                    newLoc = free.First();
                    free.Remove(newLoc);
                }

                // Set it
                mapping[i] = newLoc;

                // Add update to future last use operation
                int lastUse = lastUses[i];
                if (!freeEvents.TryGetValue(lastUse, out List<int> freed))
                {
                    freed = new List<int>();
                    freeEvents.Add(lastUse, freed);
                }
                freed.Add(newLoc);
            }

            return mapping;
        }

        protected abstract Compiled EvaluateArcsin(int arg, int dest);
        protected abstract Compiled EvaluateArctan(int arg, int dest);
        protected abstract Compiled EvaluateConstant(IConstant value, int dest);
        protected abstract Compiled EvaluateExponent(int arg1, int arg2, int dest);
        protected abstract Compiled EvaluateLn(int arg, int dest);
        protected abstract Compiled EvaluateSign(int arg, int dest);
        protected abstract Compiled EvaluateSin(int arg, int dest);
        protected abstract Compiled EvaluateProduct(int arg1, int arg2, int dest);
        protected abstract Compiled EvaluateSum(int arg1, int arg2, int dest);
        protected abstract Compiled EvaluateVariable(IVariable value, Dictionary<string, int> seenVariables, int dest);
        protected abstract Compiled EvaluateFunction(FunctionIdentity function, List<int> args, int dest);
    }
}

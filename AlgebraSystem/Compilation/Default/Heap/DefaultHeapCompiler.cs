using Algebra.Compilation.Default.Stack;
using Algebra.Functions;
using Algebra.Functions.FunctionIdentities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal class DefaultHeapCompiler : HeapCompiler<double, DefaultHeapInstruction>
        {
            private static readonly Dictionary<FunctionIdentity, DefaultOpcode> _defaultFunctionMap = new Dictionary<FunctionIdentity, DefaultOpcode>()
            {
                { AbsIdentity.Instance,         DefaultOpcode.ABS     },
                { ArccosIdentity.Instance,      DefaultOpcode.ARCCOS  },
                { ArcoshIdentity.Instance,      DefaultOpcode.ARCOSH  },
                { ArsinhIdentity.Instance,      DefaultOpcode.ARSINH  },
                { ArtanhIdentity.Instance,      DefaultOpcode.ARTANH  },
                { CoshIdentity.Instance,        DefaultOpcode.COSH    },
                { CosIdentity.Instance,         DefaultOpcode.COS     },
                { DivIdentity.Instance,         DefaultOpcode.DIVIDE  },
                { LogIdentity.Instance,         DefaultOpcode.LOG     },
                { MaxIdentity.Instance,         DefaultOpcode.MAX     },
                { MinIdentity.Instance,         DefaultOpcode.MIN     },
                { SinhIdentity.Instance,        DefaultOpcode.SINH    },
                { SqrtIdentity.Instance,        DefaultOpcode.SQRT    },
                { TanhIdentity.Instance,        DefaultOpcode.TANH    },
                { TanIdentity.Instance,         DefaultOpcode.TAN     },
            };

            public static DefaultHeapCompiler Instance = new DefaultHeapCompiler();

            private DefaultHeapCompiler()
                : base(_defaultFunctionMap.Keys)
            {
            }

            protected override ICompiledFunction<double> CreateCompiled(Expression expression, DefaultHeapInstruction[] instructions, Dictionary<string, int> seenVariables, int[] indirectionTable, int cellCount)
            {
                // Map instructions using indirection table
                for (int i = 0; i < instructions.Length; i++)
                {
                    instructions[i] = instructions[i].IndirectByTable(indirectionTable); 
                }

                return new DefaultHeapCompiledFunction(instructions, cellCount, seenVariables.ToDictionary(x => x.Value, x => x.Key));
            }

            protected override DefaultHeapInstruction EvaluateArcsin(int arg, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.ARCSIN, arg, dest);
            }

            protected override DefaultHeapInstruction EvaluateArctan(int arg, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.ARCTAN, arg, dest);
            }

            protected override DefaultHeapInstruction EvaluateConstant(IConstant value, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.CONSTANT, value.GetDoubleApproximation(), dest);
            }

            protected override DefaultHeapInstruction EvaluateExponent(int arg1, int arg2, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.EXPONENT, arg1, arg2, dest);
            }

            protected override DefaultHeapInstruction EvaluateFunction(FunctionIdentity function, List<int> args, int dest)
            {
                if (args.Count == 0 || args.Count > 2)
                {
                    throw new ArgumentException($"Heap function cannot have {args.Count} arguments");
                }

                if (args.Count == 1)
                {
                    return new DefaultHeapInstruction(_defaultFunctionMap[function], args[0], dest);
                }

                return new DefaultHeapInstruction(_defaultFunctionMap[function], args[0], args[1], dest);
            }

            protected override DefaultHeapInstruction EvaluateLn(int arg, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.LN, arg, dest);
            }

            protected override DefaultHeapInstruction EvaluateProduct(int arg1, int arg2, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.MULTIPLY, arg1, arg2, dest);
            }

            protected override DefaultHeapInstruction EvaluateSign(int arg, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.SIGN, arg, dest);
            }

            protected override DefaultHeapInstruction EvaluateSin(int arg, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.SIN, arg, dest);
            }

            protected override DefaultHeapInstruction EvaluateSum(int arg1, int arg2, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.ADD, arg1, arg2, dest);
            }

            protected override DefaultHeapInstruction EvaluateVariable(IVariable value, Dictionary<string, int> seenVariables, int dest)
            {
                return new DefaultHeapInstruction(DefaultOpcode.VARIABLE, seenVariables[value.GetName()], dest);
            }
        }
    }
}

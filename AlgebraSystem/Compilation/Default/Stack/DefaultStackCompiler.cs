using Algebra.Compilation.Default.Stack;
using Algebra.Functions;
using Algebra.Functions.FunctionIdentities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultStackCompiler : StackCompiler<double, IDefaultStackInstruction>
        {
            private static readonly Dictionary<FunctionIdentity, IDefaultStackInstruction> _defaultFunctionMap = new Dictionary<FunctionIdentity, IDefaultStackInstruction>()
            {
                { AbsIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.ABS)     },
                { ArccosIdentity.Instance,      new DefaultStackInstruction(DefaultOpcode.ARCCOS)  },
                { ArcoshIdentity.Instance,      new DefaultStackInstruction(DefaultOpcode.ARCOSH)  },
                { ArsinhIdentity.Instance,      new DefaultStackInstruction(DefaultOpcode.ARSINH)  },
                { ArtanhIdentity.Instance,      new DefaultStackInstruction(DefaultOpcode.ARTANH)  },
                { CoshIdentity.Instance,        new DefaultStackInstruction(DefaultOpcode.COSH)    },
                { CosIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.COS)     },
                { DivIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.DIVIDE)  },
                { LogIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.LOG)     },
                { MaxIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.MAX)     },
                { MinIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.MIN)     },
                { SelectIdentity.Instance,      new DefaultStackInstruction(DefaultOpcode.SELECT)  },
                { SinhIdentity.Instance,        new DefaultStackInstruction(DefaultOpcode.SINH)    },
                { SqrtIdentity.Instance,        new DefaultStackInstruction(DefaultOpcode.SQRT)    },
                { TanhIdentity.Instance,        new DefaultStackInstruction(DefaultOpcode.TANH)    },
                { TanIdentity.Instance,         new DefaultStackInstruction(DefaultOpcode.TAN)     },
            };

            public static DefaultStackCompiler Instance = new DefaultStackCompiler();

            private DefaultStackCompiler()
                : base(_defaultFunctionMap.Keys)
            {
            }

            protected override ICompiledFunction<double> CreateCompiled(Expression expressions, IDefaultStackInstruction[] instructions, string[] variables)
            {
                return new DefaultStackCompiledFunction(instructions, variables);
            }

            protected override IDefaultStackInstruction EvaluateArcsin()
            {
                return new DefaultStackInstruction(DefaultOpcode.ARCSIN);
            }

            protected override IDefaultStackInstruction EvaluateArctan()
            {
                return new DefaultStackInstruction(DefaultOpcode.ARCTAN);
            }

            protected override IDefaultStackInstruction EvaluateConstant(IConstant value)
            {
                return new DefaultStackLoadConst((double)value.GetDoubleApproximation());
            }

            protected override IDefaultStackInstruction EvaluateExponent()
            {
                return new DefaultStackInstruction(DefaultOpcode.EXPONENT);
            }

            protected override IDefaultStackInstruction EvaluateFunction(FunctionIdentity function)
            {
                return _defaultFunctionMap[function];
            }

            protected override IDefaultStackInstruction EvaluateLn()
            {
                return new DefaultStackInstruction(DefaultOpcode.LN);
            }

            protected override IDefaultStackInstruction EvaluateProduct()
            {
                return new DefaultStackInstruction(DefaultOpcode.MULTIPLY);
            }

            protected override IDefaultStackInstruction EvaluateSign()
            {
                return new DefaultStackInstruction(DefaultOpcode.SIGN);
            }

            protected override IDefaultStackInstruction EvaluateSin()
            {
                return new DefaultStackInstruction(DefaultOpcode.SIN);
            }

            protected override IDefaultStackInstruction EvaluateSum()
            {
                return new DefaultStackInstruction(DefaultOpcode.ADD);
            }

            protected override IDefaultStackInstruction EvaluateVariable(IVariable value)
            {
                return new DefaultStackLoadVar(value.GetName());
            }
        }
    }
}

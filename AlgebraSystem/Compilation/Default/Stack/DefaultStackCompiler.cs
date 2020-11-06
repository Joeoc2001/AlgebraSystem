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
        internal class DefaultStackCompiler : StackCompiler<double, IDefaultInstruction>
        {
            private static readonly Dictionary<FunctionIdentity, IDefaultInstruction> _defaultFunctionMap = new Dictionary<FunctionIdentity, IDefaultInstruction>()
            {
                { AbsIdentity.Instance,         new DefaultInstruction(DefaultOpcode.ABS)     },
                { ArccosIdentity.Instance,      new DefaultInstruction(DefaultOpcode.ARCCOS)  },
                { ArcoshIdentity.Instance,      new DefaultInstruction(DefaultOpcode.ARCOSH)  },
                { ArsinhIdentity.Instance,      new DefaultInstruction(DefaultOpcode.ARSINH)  },
                { ArtanhIdentity.Instance,      new DefaultInstruction(DefaultOpcode.ARTANH)  },
                { CoshIdentity.Instance,        new DefaultInstruction(DefaultOpcode.COSH)    },
                { CosIdentity.Instance,         new DefaultInstruction(DefaultOpcode.COS)     },
                { DivIdentity.Instance,         new DefaultInstruction(DefaultOpcode.DIVIDE)  },
                { LogIdentity.Instance,         new DefaultInstruction(DefaultOpcode.LOG)     },
                { MaxIdentity.Instance,         new DefaultInstruction(DefaultOpcode.MAX)     },
                { MinIdentity.Instance,         new DefaultInstruction(DefaultOpcode.MIN)     },
                { SelectIdentity.Instance,      new DefaultInstruction(DefaultOpcode.SELECT)  },
                { SinhIdentity.Instance,        new DefaultInstruction(DefaultOpcode.SINH)    },
                { SqrtIdentity.Instance,        new DefaultInstruction(DefaultOpcode.SQRT)    },
                { TanhIdentity.Instance,        new DefaultInstruction(DefaultOpcode.TANH)    },
                { TanIdentity.Instance,         new DefaultInstruction(DefaultOpcode.TAN)     },
            };

            public static DefaultStackCompiler Instance = new DefaultStackCompiler();

            private DefaultStackCompiler()
                : base(_defaultFunctionMap.Keys)
            {
            }

            protected override ICompiledFunction<double> CreateCompiled(Expression expression, IVariableInputSet<double> variables, IDefaultInstruction[] instructions)
            {
                return new DefaultStackCompiledFunction(variables, instructions);
            }

            protected override IDefaultInstruction EvaluateArcsin()
            {
                return new DefaultInstruction(DefaultOpcode.ARCSIN);
            }

            protected override IDefaultInstruction EvaluateArctan()
            {
                return new DefaultInstruction(DefaultOpcode.ARCTAN);
            }

            protected override IDefaultInstruction EvaluateConstant(IConstant value)
            {
                return new DefaultLoadConst((double)value.GetDoubleApproximation());
            }

            protected override IDefaultInstruction EvaluateExponent()
            {
                return new DefaultInstruction(DefaultOpcode.EXPONENT);
            }

            protected override IDefaultInstruction EvaluateFunction(FunctionIdentity function)
            {
                return _defaultFunctionMap[function];
            }

            protected override IDefaultInstruction EvaluateLn()
            {
                return new DefaultInstruction(DefaultOpcode.LN);
            }

            protected override IDefaultInstruction EvaluateProduct()
            {
                return new DefaultInstruction(DefaultOpcode.MULTIPLY);
            }

            protected override IDefaultInstruction EvaluateSign()
            {
                return new DefaultInstruction(DefaultOpcode.SIGN);
            }

            protected override IDefaultInstruction EvaluateSin()
            {
                return new DefaultInstruction(DefaultOpcode.SIN);
            }

            protected override IDefaultInstruction EvaluateSum()
            {
                return new DefaultInstruction(DefaultOpcode.ADD);
            }

            protected override IDefaultInstruction EvaluateVariable(IVariable value, IVariableInputSet<double> variables)
            {
                return new DefaultLoadVar(variables.Get(value.GetName()), value.GetName());
            }
        }
    }
}

using Algebra.Functions;
using Algebra.Functions.FunctionIdentities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default
    {
        internal class DefaultCompiler : Compiler<double, IDefaultInstruction>
        {
            private static readonly Dictionary<FunctionIdentity, Func<List<int>, IDefaultInstruction>> _defaultFunctionMap = new Dictionary<FunctionIdentity, Func<List<int>, IDefaultInstruction>>()
            {
                { AbsIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.ABS)     },
                { ArccosIdentity.Instance,      i => new DefaultInstruction(DefaultOpcode.ARCCOS)  },
                { ArcoshIdentity.Instance,      i => new DefaultInstruction(DefaultOpcode.ARCOSH)  },
                { ArsinhIdentity.Instance,      i => new DefaultInstruction(DefaultOpcode.ARSINH)  },
                { ArtanhIdentity.Instance,      i => new DefaultInstruction(DefaultOpcode.ARTANH)  },
                { CoshIdentity.Instance,        i => new DefaultInstruction(DefaultOpcode.COSH)    },
                { CosIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.COS)     },
                { DivIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.DIVIDE)  },
                { LogIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.LOG)     },
                { MaxIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.MAX)     },
                { MinIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.MIN)     },
                { SelectIdentity.Instance,      i => new DefaultInstruction(DefaultOpcode.SELECT)  },
                { SinhIdentity.Instance,        i => new DefaultInstruction(DefaultOpcode.SINH)    },
                { SqrtIdentity.Instance,        i => new DefaultInstruction(DefaultOpcode.SQRT)    },
                { TanhIdentity.Instance,        i => new DefaultInstruction(DefaultOpcode.TANH)    },
                { TanIdentity.Instance,         i => new DefaultInstruction(DefaultOpcode.TAN)     },
            };

            public static DefaultCompiler Instance = new DefaultCompiler();

            private DefaultCompiler()
                : base(_defaultFunctionMap)
            {
            }

            protected override ICompiledFunction<double> CreateCompiled(Expression expression, IVariableInputSet<double> variables, IDefaultInstruction[] instructions)
            {
                return new DefaultCompiledFunction(variables, instructions);
            }

            protected override IDefaultInstruction EvaluateArcsin(int arg)
            {
                return new DefaultInstruction(DefaultOpcode.ARCSIN);
            }

            protected override IDefaultInstruction EvaluateArctan(int arg)
            {
                return new DefaultInstruction(DefaultOpcode.ARCTAN);
            }

            protected override IDefaultInstruction EvaluateConstant(IConstant value)
            {
                return new DefaultLoadConst((double)value.GetDoubleApproximation());
            }

            protected override IDefaultInstruction EvaluateExponent(int baseIndex, int powerIndex)
            {
                return new DefaultInstruction(DefaultOpcode.EXPONENT);
            }

            protected override IDefaultInstruction EvaluateLn(int arg)
            {
                return new DefaultInstruction(DefaultOpcode.LN);
            }

            protected override IDefaultInstruction EvaluateProduct(int argument1, int argument2)
            {
                return new DefaultInstruction(DefaultOpcode.MULTIPLY);
            }

            protected override IDefaultInstruction EvaluateSign(int arg)
            {
                return new DefaultInstruction(DefaultOpcode.SIGN);
            }

            protected override IDefaultInstruction EvaluateSin(int arg)
            {
                return new DefaultInstruction(DefaultOpcode.SIN);
            }

            protected override IDefaultInstruction EvaluateSum(int argument1, int argument2)
            {
                return new DefaultInstruction(DefaultOpcode.ADD);
            }

            protected override IDefaultInstruction EvaluateVariable(IVariable value, IVariableInputSet<double> variables)
            {
                return new DefaultLoadVar(variables.Get(value.GetName()));
            }
        }
    }
}

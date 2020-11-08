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
        internal class DefaultHeapCompiler : HeapCompiler<double, DefaultHeapInstruction<int>>
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

        }
    }
}

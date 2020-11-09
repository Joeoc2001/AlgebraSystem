using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal class LambdaHeapCompiler : CustomHeapCompiler
        {
            public static readonly LambdaHeapCompiler Instance = new LambdaHeapCompiler();

            protected override ICompiledFunction<double> CreateCompiled(DefaultHeapInstruction[] instructions, int cellCount, string[] variables)
            {
                return new LambdaHeapCompiledFunction(instructions, cellCount, variables);
            }
        }
    }
}

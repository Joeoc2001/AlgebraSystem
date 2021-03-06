﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal class DefaultHeapCompiler : CustomHeapCompiler
        {
            public static readonly DefaultHeapCompiler Instance = new DefaultHeapCompiler();

            protected override ICompiledFunction<double> CreateCompiled(DefaultHeapInstruction[] instructions, int cellCount, string[] variables)
            {
                return new DefaultHeapCompiledFunction(instructions, cellCount, variables);
            }
        }
    }
}

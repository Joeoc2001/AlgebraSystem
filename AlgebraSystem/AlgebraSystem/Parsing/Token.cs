﻿using System.Collections;
using System.Collections.Generic;

namespace Algebra.Parsing
{
    public enum Token
    {
        EOF,
        Add,
        Subtract,
        Multiply,
        Divide,
        Exponent,
        Decimal,
        Variable,
        OpenBrace,
        CloseBrace,
        Function,
        Comma
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace RPNCalculatorXam
{
    public static class OpList
    {
        public static string[] ArithOpList { get => new string[]{ "+", "-", "*", "/", "^" }; }
        public static string[] FuncOpList
        {
            get => new string[]{ "sin", "cos", "tan", "asin", "acos", "atan", "log", "ln", "√", "floor", "ceil",
            "sinh", "cosh", "tanh", "asinh", "acosh", "atanh", "negate" };
        }
    }
}

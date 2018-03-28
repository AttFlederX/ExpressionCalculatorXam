using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RPNCalculatorXam.Services
{
    class RPNCalculator
    {
        /// <summary>
        /// Calculates the value of an expression written in the reverse Polish notation.
        /// The return type is nullable to enable the function to return null if the calculation has failed.
        /// angleCoef determines the unit in which to calculate trigonometric functions
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="angleCoef"></param>
        /// <returns></returns>
        public static double? Calculate(List<string> exp, double angleCoef = 1)
        {
            double res = 0;
            double conv;
            double op1;
            double op2;

            Stack<double> operandStack = new Stack<double>();

            try
            {
                foreach (string token in exp)
                {
                    if (double.TryParse(token, out conv)) // if token is a real number
                    {
                        operandStack.Push(conv);
                    }
                    else if (OpList.ArithOpList.Contains(token)) // if token is an arithmetic operator
                    {
                        op1 = operandStack.Pop();
                        op2 = operandStack.Pop();

                        switch (token)
                        {
                            case "+":
                                operandStack.Push(op2 + op1);
                                break;
                            case "-":
                                operandStack.Push(op2 - op1);
                                break;
                            case "*":
                                operandStack.Push(op2 * op1);
                                break;
                            case "/":
                                if (op1 == 0) { return null; }
                                operandStack.Push(op2 / op1);
                                break;
                            case "^":
                                operandStack.Push(Math.Pow(op2, op1));
                                break;
                        }
                    }
                    else if (OpList.FuncOpList.Contains(token)) // if token is a function operator
                    {
                        op1 = operandStack.Pop();

                        switch (token)
                        {
                            case "sin":
                                operandStack.Push(Math.Sin(angleCoef * op1));
                                break;
                            case "cos":
                                operandStack.Push(Math.Cos(angleCoef * op1));
                                break;
                            case "tan":
                                if (Math.Cos(op1) == 0) { return null; }
                                operandStack.Push(Math.Tan(angleCoef * op1));
                                break;
                            case "asin":
                                if (op1 < -1 || op1 > 1) { return null; }
                                operandStack.Push(Math.Asin(op1) / angleCoef);
                                break;
                            case "acos":
                                if (op1 < -1 || op1 > 1) { return null; }
                                operandStack.Push(Math.Acos(op1) / angleCoef);
                                break;
                            case "atan":
                                operandStack.Push(Math.Atan(op1) / angleCoef);
                                break;

                            case "log":
                                if (op1 == 0) { return null; }
                                operandStack.Push(Math.Log10(op1));
                                break;
                            case "ln":
                                if (op1 == 0) { return null; }
                                operandStack.Push(Math.Log(op1));
                                break;

                            case "sqrt":
                                if (op1 < 0) { return null; }
                                operandStack.Push(Math.Sqrt(op1));
                                break;

                            case "floor":
                                operandStack.Push(Math.Floor(op1));
                                break;
                            case "ceil":
                                operandStack.Push(Math.Ceiling(op1));
                                break;

                            case "sinh":
                                operandStack.Push(Math.Sinh(op1));
                                break;
                            case "cosh":
                                operandStack.Push(Math.Cosh(op1));
                                break;
                            case "tanh":
                                operandStack.Push(Math.Tanh(op1));
                                break;
                            // no built-in methods for inverse hyperbolic functions
                            case "asinh":
                                operandStack.Push(Math.Log(op1 + Math.Sqrt(Math.Pow(op1, 2) + 1)));
                                break;
                            case "acosh":
                                if (op1 < 1) { return null; }
                                operandStack.Push(Math.Log(op1 + Math.Sqrt(Math.Pow(op1, 2) - 1)));
                                break;
                            case "atanh":
                                if (op1 < -1 || op1 > 1) { return null; }
                                operandStack.Push(0.5 * Math.Log((1 + op1) / (1 - op1)));
                                break;

                            case "negate":
                                operandStack.Push(-1 * op1);
                                break;
                        }
                    }
                }

                res = operandStack.Pop();
                if (operandStack.Count > 0)
                {
                    return null;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

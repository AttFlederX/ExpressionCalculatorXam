using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using RPNCalculatorXam.Services;

namespace RPNCalculatorXam
{
	public partial class MainPage : ContentPage
	{
        //private readonly Button[] trigFuncButtons; // for trigonometric/hyperbolic modes

        bool isOperatorLastReceived = true; // prevents operators from being typed in a row 
        bool isDecimalPointReceived = false; // prevents more than one decimal point from appearing in a single number
        bool initialState = true;
        bool resultState = false;

        double memValue = 0; // memory storage

        public MainPage()
		{
			InitializeComponent();

            ResetState();
            ClearInput();
        }

        /// <summary>
        /// Resets the textboxes if the program is in its initial state or result display state
        /// </summary>
        private void ResetState()
        {
            if (initialState || resultState) // if the textbox contains the default value
            {
                inputEntry.Text = string.Empty;
                initialState = false;
                resultState = false;
            }
        }

        /// <summary>
        /// Resets the program to its initial state
        /// </summary>
        private void ClearInput()
        {
            inputEntry.Text = "0.";

            isOperatorLastReceived = true;
            isDecimalPointReceived = false;
            initialState = true;
            resultState = false;
        }

        /// <summary>
        /// Updates the isOperatorLastReceived condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            // anything other than a digit, a right parethesis or a decimal point is an operator 
            if (inputEntry.Text.Length > 0)
            {
                // isOperatorLastReceived = (!char.IsDigit(inputTextBox.Text[inputTextBox.Text.Length - 1]) &&
                //    !inputTextBox.Text.EndsWith(".") && !inputTextBox.Text.EndsWith(" )"));
                isOperatorLastReceived = inputEntry.Text.EndsWith(" "); // an operator is always followed by a space
            }
            else
            {
                isOperatorLastReceived = true;
            }
        }

        /// <summary>
        /// Handles the 1 through 9 digit buttons 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            // if the first entered digit was not zero
            if (!inputEntry.Text.EndsWith(" 0") && !(inputEntry.Text.EndsWith("0") && inputEntry.Text.Length == 1))
            {
                ResetState();
                inputEntry.Text += b.Text;
            }
        }

        /// <summary>
        /// Handles the +, -, * and / operators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            if (!isOperatorLastReceived && !resultState && !inputEntry.Text.EndsWith("( ") &&
                inputEntry.Text.Last() != '.') // can't have two operators in a row
            {

                inputEntry.Text += string.Format(" {0} ", b.Text); // frame the operator into spaces for Split function
                //exp.Append(b.Text);
                isDecimalPointReceived = false;
            }
        }

        /// <summary>
        /// Equals button handling(result calculation & display)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EqualsButton_Click(object sender, EventArgs e)
        {
            // converts the expression into reverse Polish notation and immidiately calculates the result
            double? result = RPNCalculator.Calculate(RPNConverter.Convert(inputEntry.Text.Split(' ')));
            // MessageBox.Show(string.Join(string.Empty, RPNConverter.Convert(inputTextBox.Text.Split(' ')).ToArray()));

            if (result == null)
            {
                inputEntry.Text = "Error";
            }
            else
            {
                inputEntry.Text = result.ToString();
                // add to history
                // historyListBox.Items.Add(string.Format("{0} = {1}", inputTextBox.Text, result.ToString()));
                // resultHistory.Add(result);
                // clearHistoryButton.Enabled = true;
                // historyListBox.SelectedIndex = historyListBox.Items.Count - 1; // select the last item
            }
            resultState = true;
        }

        /// <summary>
        /// Handles all function expressions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncButton_Click(object sender, EventArgs e)
        {
            Button b = (sender as Button);

            if (isOperatorLastReceived)
            {
                ResetState();
                inputEntry.Text += string.Format("{0} ( ", b.Text);
            }
        }

        /// <summary>
        /// Negation function (has to be put before entering the number)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlusMinusButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived || inputEntry.Text.EndsWith("negate ( "))
            {
                ResetState();

                if (inputEntry.Text.EndsWith("negate ( ")) // changes back to positive
                {
                    inputEntry.Text = inputEntry.Text.Remove(inputEntry.Text.Length - 9);
                    if (inputEntry.Text.Length == 0) { ClearInput(); }
                }
                else
                {
                    inputEntry.Text += "negate ( ";
                }
            }
        }

        /// <summary>
        /// Puts the pi constant into the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PiButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputEntry.Text += Math.PI;
            }
        }

        /// <summary>
        /// Puts the e constant into the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputEntry.Text += Math.E;
            }
        }

        /// <summary>
        /// Left parenthesis handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftParButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived || inputEntry.Text.EndsWith("( "))
            {
                ResetState();

                inputEntry.Text += "( ";
                //isOperatorLastReceived = false;
            }
        }

        /// <summary>
        /// Right parenthesis handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightParButton_Click(object sender, EventArgs e)
        {
            if (!isOperatorLastReceived && inputEntry.Text.Contains("(") && inputEntry.Text[inputEntry.Text.Length - 1] != '.')
            {
                inputEntry.Text += " )";
            }
        }

        /// <summary>
        /// Decimal point handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecimalButton_Click(object sender, EventArgs e)
        {
            // can't have multiple decimal points in a single number or in the beginning of a number 
            if (!isOperatorLastReceived && !inputEntry.Text.EndsWith("( ") && !isDecimalPointReceived)
            {
                inputEntry.Text += ".";
                isDecimalPointReceived = true;
            }
        }

        /// <summary>
        /// '0' handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZeroButton_Click(object sender, EventArgs e)
        {
            //if the first digit of a number was not 0
            if (!inputEntry.Text.EndsWith(" 0") && !(inputEntry.Text.EndsWith("0") && inputEntry.Text.Length == 1))
            {
                ResetState();

                inputEntry.Text += "0";
            }
        }

        /// <summary>
        /// Input & output clearing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        /// <summary>
        /// Backspace button handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            if (!initialState && inputEntry.Text.Length > 0)
            {
                if (inputEntry.Text.EndsWith("( ") || inputEntry.Text.EndsWith(" )"))
                {
                    inputEntry.Text = inputEntry.Text.Remove(inputEntry.Text.Length - 2);
                }
                else if (inputEntry.Text.EndsWith(".") || inputEntry.Text.EndsWith("-"))
                {
                    inputEntry.Text = inputEntry.Text.Remove(inputEntry.Text.Length - 1);
                    isDecimalPointReceived = false;
                }
                else if (isOperatorLastReceived)
                {
                    while (inputEntry.Text.Length > 0 && !char.IsDigit(inputEntry.Text.Last()) &&
                        !inputEntry.Text.EndsWith(" )") && !inputEntry.Text.EndsWith("( ")) // remove the operator and the spaces
                    {
                        inputEntry.Text = inputEntry.Text.Remove(inputEntry.Text.Length - 1);
                    }
                }
                else
                {
                    inputEntry.Text = inputEntry.Text.Remove(inputEntry.Text.Length - 1);
                }

                if (inputEntry.Text.Length == 0) // if the input field is empty after the backspace
                {
                    ClearInput();
                }
            }
        }

        #region Memory buttons handling
        /// <summary>
        /// Memory reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemClearButton_Click(object sender, EventArgs e)
        {
            memValue = 0;
        }

        /// <summary>
        /// Memory read button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemRecallButton_Click(object sender, EventArgs e)
        {
            if (isOperatorLastReceived)
            {
                ResetState();

                inputEntry.Text += memValue;
            }
        }

        /// <summary>
        /// Memory addition button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemPlusButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue += Convert.ToDouble(inputEntry.Text);
            }
        }

        /// <summary>
        /// Memory subtraction button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemMinusButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue -= Convert.ToDouble(inputEntry.Text);
            }
        }

        /// <summary>
        /// Memory overwrite button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemStoreButton_Click(object sender, EventArgs e)
        {
            if (resultState)
            {
                memValue = Convert.ToDouble(inputEntry.Text);
            }
        }
        #endregion
    }
}

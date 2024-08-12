using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Cal
{
    public partial class MainWindow : Window
    {
        private double firstNumber;
        private double secondNumber;
        private string operation;
        private bool lastEqualPressed; // 마지막 연산 기억
        private double memory; 
        private List<double> memoryList = new List<double>(); // 메모리 리스트

        public MainWindow()
        {
            InitializeComponent();
            Clear(); // Initialize display on startup
        }

        private void Clear()
        {
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            lastEqualPressed = false;
            ExpressionDisplay.Text = "";
            ResultDisplay.Text = "";
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            // Reset display if '=' was last pressed
            if (lastEqualPressed)
            {
                ClearDisplayAfterEqual();
                ResultDisplay.Text = ""; // Clear the result display
            }

            // Handle decimal point appropriately
            if (number == "." && !ResultDisplay.Text.Contains("."))
            {
                ResultDisplay.Text += number;
            }
            else if (ResultDisplay.Text == "0" || ResultDisplay.Text == "")
            {
                ResultDisplay.Text = number;
            }
            else
            {
                ResultDisplay.Text += number;
            }

            ExpressionDisplay.Text += number; // Update expression display
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            operation = button.Content.ToString();

            // Reset display if '=' was last pressed
            if (lastEqualPressed)
            {
                ClearDisplayAfterEqual();
            }

            try
            {
                firstNumber = double.Parse(ResultDisplay.Text);
                ResultDisplay.Text = ""; // Clear result display for next input
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }

            ExpressionDisplay.Text += operation; // Update expression display
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!lastEqualPressed)
                {
                    // First '=' press after an operation
                    secondNumber = double.Parse(ResultDisplay.Text);
                }

                switch (operation)
                {
                    case "+":
                        firstNumber = firstNumber + secondNumber;
                        break;
                    case "-":
                        firstNumber = firstNumber - secondNumber;
                        break;
                    case "*":
                        firstNumber = firstNumber * secondNumber;
                        break;
                    case "/":
                        if (secondNumber == 0)
                        {
                            MessageBox.Show("Division by zero is not allowed.");
                            return;
                        }
                        firstNumber = firstNumber / secondNumber;
                        break;
                    default:
                        MessageBox.Show("Invalid operator.");
                        return;
                }

                ResultDisplay.Text = firstNumber.ToString();
                ExpressionDisplay.Text += " = " + ResultDisplay.Text;

                lastEqualPressed = true; // Mark that '=' was pressed
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        // Method to clear display after '=' is pressed and new input begins
        private void ClearDisplayAfterEqual()
        {
            ExpressionDisplay.Text = ""; // Clear the expression display
            lastEqualPressed = false; // Reset the flag
        }

        // Event handler for the '%' button
        private void PercentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = double.Parse(ResultDisplay.Text) / 100;
                ResultDisplay.Text = value.ToString();
                ExpressionDisplay.Text += "%";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the 'CE' button
        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            ResultDisplay.Text = "0";
        }

        // Event handler for the 'Back' button
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultDisplay.Text.Length > 0)
            {
                ResultDisplay.Text = ResultDisplay.Text.Substring(0, ResultDisplay.Text.Length - 1);
            }

            if (ResultDisplay.Text.Length == 0)
            {
                ResultDisplay.Text = "0";
            }
        }

        // Event handler for the '1/x' button
        private void ReciprocalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = 1 / double.Parse(ResultDisplay.Text);
                ResultDisplay.Text = value.ToString();
                ExpressionDisplay.Text = "1/(" + ResultDisplay.Text + ")";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Cannot divide by zero.");
            }
        }

        // Event handler for the 'x²' button
        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = Math.Pow(double.Parse(ResultDisplay.Text), 2);
                ResultDisplay.Text = value.ToString();
                ExpressionDisplay.Text = "sqr(" + ResultDisplay.Text + ")";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the '√x' button
        private void SquareRootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = Math.Sqrt(double.Parse(ResultDisplay.Text));
                ResultDisplay.Text = value.ToString();
                ExpressionDisplay.Text = "√(" + ResultDisplay.Text + ")";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the '+/-' button
        private void NegateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = double.Parse(ResultDisplay.Text);
                value = -value;
                ResultDisplay.Text = value.ToString();
                ExpressionDisplay.Text = "negate(" + ResultDisplay.Text + ")";
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the 'MC' button
        private void MemoryClearButton_Click(object sender, RoutedEventArgs e)
        {
            memory = 0;
            MessageBox.Show("Memory Cleared");
        }

        // Event handler for the 'MR' button
        private void MemoryRecallButton_Click(object sender, RoutedEventArgs e)
        {
            ResultDisplay.Text = memory.ToString();
        }

        // Event handler for the 'M+' button
        private void MemoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory += double.Parse(ResultDisplay.Text);
                MessageBox.Show("Added to Memory");
                memoryList.Add(memory); // Add updated memory to the list
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the 'M-' button
        private void MemorySubtractButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory -= double.Parse(ResultDisplay.Text);
                MessageBox.Show("Subtracted from Memory");
                memoryList.Add(memory); // Add updated memory to the list
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the 'MS' button
        private void MemoryStoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory = double.Parse(ResultDisplay.Text);
                MessageBox.Show("Value Stored in Memory");
                memoryList.Add(memory); // Add stored memory to the list
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        // Event handler for the 'M목록' button
        private void MemoryListButton_Click(object sender, RoutedEventArgs e)
        {
            if (memoryList.Count == 0)
            {
                MessageBox.Show("No memory stored.");
                return;
            }

            string memoryListString = string.Join("\n", memoryList);
            MessageBox.Show(memoryListString, "Memory List");
        }
    }
}

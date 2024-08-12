using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Cal
{
    public partial class MainWindow : Window
    {
        private string _input = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            _input += button.Content.ToString();
            Display.Text = _input;
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.ToString() == "C")
            {
                _input = string.Empty;
                Display.Text = _input;
            }
            else
            {
                _input += " " + button.Content.ToString() + " ";
                Display.Text = _input;
            }
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = new DataTable().Compute(_input, null);
                Display.Text = result.ToString();
                _input = result.ToString(); // 결과를 다음 계산을 위해 저장
            }
            catch
            {
                Display.Text = "Error";
                _input = string.Empty; // 오류 발생 시 입력 초기화
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _input = string.Empty;
            Display.Text = _input;
        }
    }
}

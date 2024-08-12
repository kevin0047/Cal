using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Cal
{
    public partial class MainWindow : Window
    {
        private string _input = string.Empty;
        private bool _isResultDisplayed = false; // 결과가 표시된 상태를 추적하는 변수

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            // 결과가 표시된 후 숫자 버튼을 누르면 초기화
            if (_isResultDisplayed)
            {
                _input = string.Empty; // 초기화
                _isResultDisplayed = false; // 결과 표시 상태 리셋
            }

            _input += button.Content.ToString(); // 숫자를 입력
            ResultDisplay.Text = _input; // 큰 텍스트박스에 숫자 표시
            ExpressionDisplay.Text = _input; // 작은 텍스트박스에 연산식 표시
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.ToString() == "C")
            {
                _input = string.Empty;
                ResultDisplay.Text = _input;
                ExpressionDisplay.Text = string.Empty; // 연산식 초기화
                _isResultDisplayed = false; // 결과 표시 상태 리셋
            }
            else
            {
                // 연산자를 추가하고 연산식 표시
                if (!string.IsNullOrWhiteSpace(_input)) // 입력이 있을 때만 연산자 추가
                {
                    ExpressionDisplay.Text = _input + " " + button.Content.ToString() + " "; // 연산식 업데이트
                    _input += " " + button.Content.ToString() + " "; // 연산자를 _input에 추가
                    ResultDisplay.Text = string.Empty; // 결과 표시 초기화
                }
            }
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = new DataTable().Compute(_input, null);
                ResultDisplay.Text = result.ToString(); // 결과를 큰 텍스트박스에 표시
                _input = result.ToString(); // 결과를 다음 계산을 위해 저장
                ExpressionDisplay.Text = string.Empty; // 연산식 초기화
                _isResultDisplayed = true; // 결과 표시 상태 설정
            }
            catch
            {
                ResultDisplay.Text = "Error";
                _input = string.Empty; // 오류 발생 시 입력 초기화
                ExpressionDisplay.Text = string.Empty; // 연산식 초기화
                _isResultDisplayed = false; // 결과 표시 상태 리셋
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _input = string.Empty;
            ResultDisplay.Text = _input;
            ExpressionDisplay.Text = string.Empty; // 연산식 초기화
            _isResultDisplayed = false; // 결과 표시 상태 리셋
        }
    }
}

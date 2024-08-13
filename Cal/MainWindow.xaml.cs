using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Cal
{
    public partial class MainWindow : Window
    {
        private double firstNumber; // 첫 번째 숫자 저장
        private double secondNumber; // 두 번째 숫자 저장
        private double thirdNumber; // 세번째 숫자 , ResultDisplay.Text값 저장
        private string operation; // 연산자 저장
        private bool lastOperPressed;
        private bool lastEqualPressed; // 마지막에 '=' 버튼이 눌렸는지 확인
        private string newOper ;
        private double memory; // 메모리 저장
        private List<double> memoryList = new List<double>(); // 메모리 리스트

        public MainWindow()
        {
            InitializeComponent();//MainWindow.xaml에 정의된 UI 요소들을 메모리에 로드
            Clear(); // 시작 시 디스플레이 초기화
        }

        // 디스플레이를 초기화하는 메소드
        private void Clear()
        {
            firstNumber = 0;
            secondNumber = 0;
            thirdNumber = 0;
            operation = "";
            newOper = "";
            lastEqualPressed = false;
            lastOperPressed = false;
            ExpressionDisplay.Text = "";
            ResultDisplay.Text = "";
        }

        // 숫자 버튼 클릭 시 호출되는 메소드
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            // 마지막에 '=' 버튼이 눌린 상태 (lastEqualPressed = true;)에는 다음연산을 위해 디스플레이를 초기화
            if (lastEqualPressed)
            {
                ClearDisplayAfterEqual(); // ExpressionDisplay 초기화와  lastEqualPressed=flase;로 변경
                
                ResultDisplay.Text = ""; // 결과 디스플레이를 초기화
            }

            // 소수점 처리
            if (number == "." && !ResultDisplay.Text.Contains(".")) //.버튼도 NumberButton_Click 발생, .이 눌러졌나? 그리고 ResultDisplay에 .이 없나? 
            {
                ResultDisplay.Text += number;
            }
            else if (ResultDisplay.Text == "0" || ResultDisplay.Text == "")// ResultDisplay가 0이거나 빈칸일때 숫자를 누르면 대체되게 처리 01이 되지않도록
            {
                ResultDisplay.Text = number;
            }
            else
            {
                ResultDisplay.Text += number;
            }

            ExpressionDisplay.Text += number; // 수식 디스플레이 업데이트
        }

        // 연산자 버튼 클릭 시 호출되는 메소드
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            operation = button.Content.ToString();
            
            


            try
            {
                if (lastEqualPressed)
                {
                    thirdNumber = double.Parse(ResultDisplay.Text); // 연속연산시 ResultDisplay 값을
                    ClearDisplayAfterEqual(); // display 초기화 하고
                    ExpressionDisplay.Text = thirdNumber.ToString(); // ExpressionDisplay로 넘김 
                    firstNumber = thirdNumber;
                    thirdNumber = 0;
                    
                    ResultDisplay.Text = ""; // 다음 입력을 위해 결과 디스플레이를 초기화
                    ExpressionDisplay.Text += operation;
                    newOper = operation;
                    
                }
                else
                {
                    // 첫 번째 연산 버튼 클릭 시
                    if (!lastOperPressed)
                    {
                        firstNumber = double.Parse(ResultDisplay.Text);
                        ResultDisplay.Text = ""; // 다음 입력을 위해 결과 디스플레이를 초기화
                        ExpressionDisplay.Text += operation; // 수식 디스플레이 업데이트
                        newOper = operation;
                        lastOperPressed = true; // 연산 버튼이 눌렸음을 표시
                    }
                    else
                    {
                        // 연속 연산 진행 시
                        // 이전 결과를 첫 번째 숫자로 사용
                        
                        OperCalculation();
                        newOper = operation;
                        ResultDisplay.Text = firstNumber.ToString();
                        ExpressionDisplay.Text = $"{firstNumber}"; // 연산식과 현재 결과로 업데이트
                        ExpressionDisplay.Text += operation; // 수식 디스플레이 업데이트
                        ClearDisplayAfterOper();
                        lastOperPressed = true;
                    }




                    
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 연산자를 입력하세요.");
            }

            
        }

        // '=' 버튼 클릭 시 호출되는 메소드
        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 첫 번째 '=' 버튼 클릭 시
                if (!lastEqualPressed)
                {
                    // 현재 디스플레이의 값을 두 번째 숫자로 사용
                    secondNumber = double.Parse(ResultDisplay.Text);

                    PerformCalculation();

                    ResultDisplay.Text = firstNumber.ToString();
                    ExpressionDisplay.Text += " = " + ResultDisplay.Text;

                    lastEqualPressed = true; // '=' 버튼이 눌렸음을 표시
                }
                else
                {
                    // 연속 '=' 버튼 클릭 시
                    // 이전 결과를 첫 번째 숫자로 사용
                    firstNumber = double.Parse(ResultDisplay.Text); // 현재 결과를 첫 번째 숫자로 사용

                    PerformCalculation();
                    ClearDisplayAfterEqual();
                    ResultDisplay.Text = firstNumber.ToString();
                    ExpressionDisplay.Text = $"{firstNumber} {operation} {secondNumber} ="; // 연산식과 현재 결과로 업데이트
                    lastEqualPressed = true;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }


        private void OperCalculation()
        {
            switch (newOper)
            {
                case "+":
                    firstNumber = firstNumber + double.Parse(ResultDisplay.Text);
                    break;
                case "-":
                    firstNumber = firstNumber - double.Parse(ResultDisplay.Text);
                    break;
                case "*":
                    firstNumber = firstNumber * double.Parse(ResultDisplay.Text);
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("0으로 나눌 수 없습니다.");
                        return;
                    }
                    firstNumber = firstNumber / double.Parse(ResultDisplay.Text);
                    break;
                default:
                    MessageBox.Show("유효하지 않은 연산자입니다.");
                    break;
            }
        }
        private void PerformCalculation()
        {
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
                        MessageBox.Show("0으로 나눌 수 없습니다.");
                        return;
                    }
                    firstNumber = firstNumber / secondNumber;
                    break;
                default:
                    MessageBox.Show("유효하지 않은 연산자입니다.");
                    break;
            }
        }

        // 'C' 버튼 클릭 시 호출되는 메소드
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        // '=' 버튼이 눌린 후 새로운 입력이 시작될 때 디스플레이를 초기화하는 메소드
        private void ClearDisplayAfterEqual()
        {
            ExpressionDisplay.Text = ""; // 수식 디스플레이를 초기화
            lastEqualPressed = false; // 플래그 리셋
    
            
        }
        private void ClearDisplayAfterOper()
        {
            ResultDisplay.Text = "";
            lastEqualPressed = false; // 플래그 리셋
            lastOperPressed = false;

        }

        // '%' 버튼 클릭 시 호출되는 메소드
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
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // 'CE' 버튼 클릭 시 호출되는 메소드
        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            ResultDisplay.Text = "0";
        }

        // 'Back' 버튼 클릭 시 호출되는 메소드
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

        // '1/x' 버튼 클릭 시 호출되는 메소드
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
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("0으로 나눌 수 없습니다.");
            }
        }

        // 'x²' 버튼 클릭 시 호출되는 메소드
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
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // '√x' 버튼 클릭 시 호출되는 메소드
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
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // '+/-' 버튼 클릭 시 호출되는 메소드
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
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // 'MC' 버튼 클릭 시 호출되는 메소드
        private void MemoryClearButton_Click(object sender, RoutedEventArgs e)
        {
            memory = 0;
            MessageBox.Show("메모리 삭제됨");
        }

        // 'MR' 버튼 클릭 시 호출되는 메소드
        private void MemoryRecallButton_Click(object sender, RoutedEventArgs e)
        {
            ResultDisplay.Text = memory.ToString();
        }

        // 'M+' 버튼 클릭 시 호출되는 메소드
        private void MemoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory += double.Parse(ResultDisplay.Text);
                MessageBox.Show("메모리에 추가됨");
                memoryList.Add(memory); // 업데이트된 메모리를 리스트에 추가
            }
            catch (FormatException)
            {
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // 'M-' 버튼 클릭 시 호출되는 메소드
        private void MemorySubtractButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory -= double.Parse(ResultDisplay.Text);
                MessageBox.Show("메모리에서 빼기 완료");
                memoryList.Add(memory); // 업데이트된 메모리를 리스트에 추가
            }
            catch (FormatException)
            {
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // 'MS' 버튼 클릭 시 호출되는 메소드
        private void MemoryStoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                memory = double.Parse(ResultDisplay.Text);
                MessageBox.Show("메모리에 값이 저장됨");
                memoryList.Add(memory); // 저장된 메모리를 리스트에 추가
            }
            catch (FormatException)
            {
                MessageBox.Show("유효하지 않은 입력입니다. 올바른 숫자를 입력하세요.");
            }
        }

        // 'M목록' 버튼 클릭 시 호출되는 메소드
        private void MemoryListButton_Click(object sender, RoutedEventArgs e)
        {
            if (memoryList.Count == 0)
            {
                MessageBox.Show("저장된 메모리가 없습니다.");
                return;
            }

            string memoryListString = string.Join("\n", memoryList);
            MessageBox.Show(memoryListString, "메모리 목록");
        }
    }
}

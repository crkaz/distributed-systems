using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Threading;

namespace Lab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<int> primeNumbers;
        public MainWindow()
        {
            InitializeComponent();
            primeNumbers = new List<int>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.Content = "Running";
            btn.IsEnabled = false;
            ParameterizedThreadStart ts = new ParameterizedThreadStart(FindPrimeNumbers);
            ts.BeginInvoke(20000, new AsyncCallback(FindPrimesFinishedCallback), null);
        }

        private void FindPrimesFinishedCallback(IAsyncResult iar)
        {
            Dispatcher.Invoke(() =>
            {
                outputTextBox.Text = primeNumbers[19999].ToString();
                btn.IsEnabled = true;
                btn.Content = "Find Primes";
            });
        }

        private void UpdateTextBox(int number)
        {
            outputTextBox.Text = number.ToString();
        }

        private void FindPrimeNumbers(object param)
        {
            int numberOfPrimesToFind = (int)param;
            //
            int primeCount = 0; int currentPossiblePrime = 1;
            while (primeCount < numberOfPrimesToFind)
            {
                currentPossiblePrime++; int possibleFactor = 2; bool isPrime = true;
                while ((possibleFactor <= currentPossiblePrime / 2) && (isPrime == true))
                {
                    int possibleFactor2 = currentPossiblePrime / possibleFactor;
                    if (currentPossiblePrime == possibleFactor2 * possibleFactor)
                    {
                        isPrime = false;
                    }
                    possibleFactor++;
                }
                if (isPrime)
                {
                    primeCount++;
                    primeNumbers.Add(currentPossiblePrime);
                    Dispatcher.Invoke(() =>
                    {
                        outputTextBox.Text = currentPossiblePrime.ToString();
                    });
                }
            }
        }
    }
}

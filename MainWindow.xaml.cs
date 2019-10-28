using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nummernmerker
{
    public partial class MainWindow : Window
    {
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public MainWindow()
        {
            InitializeComponent();
        }
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // CALCULATE
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void ButtonCalc_Click(object sender, RoutedEventArgs e)
        {
            Number = TextBoxNumber.Text.Replace("-", "");

            Digits = new int[Number.Length];
            Breaks = new int[Number.Length];
            BestZeros = Number.Length + 1;

            for(int I = 0; I < Number.Length; I++)
            {
                int Digit;
                if(!int.TryParse(Number[I].ToString(), out Digit))
                {
                    MessageBox.Show("Ungültige Eingabe!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Digits[I] = Digit;
            }

            string Out = String.Empty;
            if (Number.Length <= 4)
            {
                // If Number is smaller than biggest possible segment (4) return the number
                Out = Number;

                // Check for zero at first digit
                if(Digits[0] == 0)
                {
                    BestZeros = 1;
                }
                else
                {
                    BestZeros = 0;
                }
            }
            else
            {
                // Use backtracking to find the best solution
                Jump(0, 0, 0);

                // Build output string
                for (int I = 0; I < Number.Length; I++)
                {
                    if (I > 0 && BestBreaks.Contains(I))
                    {
                        Out += "-";
                    }
                    Out += Digits[I];
                }
            }

            // Display output
            TextBoxOutput.Text = String.Empty;
            MessageBox.Show("Eine optimale Lösung mit " + BestZeros + " Nullen wurde gefunden!", "Lösung gefunden", MessageBoxButton.OK, MessageBoxImage.Information);
            TextBoxOutput.Text = Out;
        }
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // GLOBAL VARIABLES
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Store basic data
        private string Number;
        private int[] Digits;

        // Store current solution
        private int[] Breaks;

        // Store best solution
        private int[] BestBreaks;
        private int BestZeros;
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // BACKTRACKING
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void Jump(int Step, int Index, int Zeros)
        {
            // Abort if index is above length of number - overshoot
            if (Index > Number.Length)
            {
                return;
            }

            // A solution is found if the index equals exactly the length of the number
            if (Index == Number.Length)
            {
                BestZeros = Zeros;
                BestBreaks = new int[Breaks.Length];
                Array.Copy(Breaks, 0, BestBreaks, 0, Step);
                return;
            }

            // Add current index to break array
            Breaks[Step] = Index;

            // Increase zero counter if digit is zero
            if (Digits[Index] == 0)
            {
                Zeros++;
            }

            // Abort if zero counter is worse than the current best
            if (Zeros > BestZeros)
            {
                return;
            }

            // Jump further
            Step++;
            Jump(Step, Index + 2, Zeros); // Segement of length 2
            Jump(Step, Index + 3, Zeros); // Segement of length 3
            Jump(Step, Index + 4, Zeros); // Segement of length 4
        }
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
}

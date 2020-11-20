using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringCalculatorConsole
{
    public static class StringCalculator
    {
        public static int Add(string numbersText)
        {
            int result = 0;
            StringBuilder errorMessage;
            if (!string.IsNullOrWhiteSpace(numbersText))
            {
                var numbers = Regex.Matches(numbersText, @"-?\d+")
                .Cast<Match>()
                .Select(m => int.Parse(m.Value))
                .ToArray();

                if (!IsValidString(numbersText, numbers, out errorMessage))
                {
                    throw new Exception(errorMessage.ToString());
                }
                char[] delimiters = GetDelimiters(numbersText);
                if(!IsValidStringWithDelimiters(numbersText, delimiters, out errorMessage))
                {
                    throw new Exception(errorMessage.ToString());
                }

                result = numbers.Where((n) => n <= 1000).Sum();
            }
            return result;
        }

        static bool IsValidString(string numbersText, int[] numbers, out StringBuilder errorMessage)
        {
            errorMessage = new StringBuilder();
            if (numbers.Length == 0)
            {
                errorMessage.AppendLine("Invalid input");
                errorMessage.AppendLine("Input string doesn't contain number(s).");
                return false;
            }
            if (HasNegativeNumbers(numbers, out errorMessage))
            {
                return false;
            }
            if (!HasValidLineBreaks(numbersText, out errorMessage))
            {
                return false;
            }
            return true;
        }

        static bool HasNegativeNumbers(int[] numbers, out StringBuilder errorMessage)
        {
            errorMessage = new StringBuilder();
            StringBuilder negativeNumbers = new StringBuilder();
            foreach (int number in numbers)
            {
                if (number < 0)
                {
                    negativeNumbers.AppendLine(number.ToString());
                }
            }

            if (negativeNumbers.Length > 0)
            {
                errorMessage.AppendLine("Negatives not allowed");
                errorMessage.AppendLine(negativeNumbers.ToString());
                return true;
            }
            return false;
        }

        static bool HasValidLineBreaks(string numbersText, out StringBuilder errorMessage)
        {
            errorMessage = new StringBuilder();
            int i = numbersText.StartsWith("//") ? 1 : 0;
            string[] lines = numbersText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (; i < lines.Length; i++)
            {
                string line = lines[i];
                char lastChar = line[line.Length - 1];
                if (!char.IsDigit(lastChar))
                {
                    errorMessage.AppendLine("Invalid input");
                    errorMessage.AppendLine("Line should be ended with number.");
                    return false;
                }
            }
            return true;
        }

        static char[] GetDelimiters(string numbersText)
        {
            if (numbersText.StartsWith("//") && numbersText.Contains("\n"))
            {
                string[] lines = numbersText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                return lines[0].Substring(2).ToCharArray();
            }
            return new char[] { ',' };
        }

        static bool IsValidStringWithDelimiters(string numbersText,char[] delimiters, out StringBuilder errorMessage) 
        {
            errorMessage = new StringBuilder();
            string stringToVerify = numbersText.Replace("\n", "");
            if (stringToVerify.StartsWith("//"))
            {
                stringToVerify = stringToVerify.Substring(2);
            }
            string nonNumericValue = string.Concat(stringToVerify.Where(c => !Char.IsDigit(c)));
            string nonDelimiters = string.Concat(nonNumericValue.Where(c => !delimiters.Contains(c)));

            if (!string.IsNullOrEmpty(nonDelimiters))
            {
                errorMessage.AppendLine("Input string has invlid characters");
                return false;
            }
            return true;
        }
    }
}

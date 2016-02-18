using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TestStringCalculator
{
    [TestFixture]
    public class TestStringCalculator
    {
        [Test]
        public void Add_GivenEmptyString_ShouldReturn0()
        {
            //---------------Set up test pack-------------------
            const string input = "";
            const int expected = 0;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Add_GivenSingleDigitString_ShouldReturnItself()
        {
            //---------------Set up test pack-------------------
            const string input = "1";
            const int expected = 1;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenCommaDelimitedStringOfNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2";
            const int expected = 3;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenNewLineDelimitedStringOfNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2\n3";
            const int expected = 6;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenCustomDelimitedStringOfNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//;\n1;2;3";
            const int expected = 6;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenStringOfNegativeNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,-50,-55";
            const string expected = "Negative numbers not allowed -50-55";
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<Exception>(()=> calculate.Add(input));
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, ex.Message);
        }

        [Test]
        public void Add_GivenStringOfNumbersGreaterThan1000_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1000,2500";
            const int expected = 1000;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenMultiLenghtCustiomDelimiter_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[***]\n500***250***250";
            const int expected = 1000;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_GivenMultiCustiomDelimiter_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[*][%]\n500%250*250";
            const int expected = 1000;
            var calculate = GetCalculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        private static Calculate GetCalculate()
        {
            var calculate = new Calculate();
            return calculate;
        }
    }

    public class Calculate  
    {
        public  int Add(string input)
        {
            var defaultDelimiters = DefaultDelimiters();    
            if (!defaultDelimiters.Any(input.Contains)) return string.IsNullOrEmpty(input) ? 0 : ConvertToInteger(input);
            input = ExtractDelimiter(input, defaultDelimiters);
            var listOfNumbers = SplitIntoListOfNumbers(input,defaultDelimiters);
            var filterNegativeNumbers = FilterNegativeNumbers(listOfNumbers);
            ThrowExceptionWhenNumberIsLessThan0(filterNegativeNumbers);
            var list= FilterNumbersGreaterThanOneThousand(listOfNumbers);
            listOfNumbers = SetListOfNumbers(listOfNumbers, list);
            var sum = Sum(listOfNumbers);
            return sum;
        }

        private static List<int> SetListOfNumbers(List<int> listOfNumbers, List<int> list)
        {
            if (listOfNumbers == null) throw new ArgumentNullException(nameof(listOfNumbers));
            listOfNumbers = list;
            return listOfNumbers;
        }

        private static string ExtractDelimiter(string input, List<string> defaultDelimiters)
        {
            if (!StartsWith(input)) return input;
            var splitString = SplitOnNewLine(input);
            var delimiterPart = GetDelimiterPart(splitString);
            var customeDelimiter = GetCustomeDelimiter(delimiterPart);
            defaultDelimiters.Add(customeDelimiter);
            ExtractCustomDelimiters(input, defaultDelimiters, delimiterPart);
            input = GetNumberStringPart(splitString);
            return input;
        }

        private static string GetNumberStringPart(string[] splitString)
        {
            if (splitString == null) throw new ArgumentNullException(nameof(splitString));
            return splitString.Last();
        }

        private static string GetDelimiterPart(string[] splitString)
        {
            if (splitString == null) throw new ArgumentNullException(nameof(splitString));
            return splitString.First();
        }

        private static void ExtractCustomDelimiters(string input, List<string> defaultDelimiters, string firstPart)
        {
            if (!ContainsCustomDelimiters(input)) return;
            var delimiter = GetDelimiter(firstPart);
            defaultDelimiters.AddRange(delimiter);
        }

        private static bool ContainsCustomDelimiters(string input)
        {
            return input.Contains("[") && input.Contains("]");
        }

        private static bool StartsWith(string input)
        {
            return input.StartsWith("//");
        }

        private static IEnumerable<string> GetDelimiter(string extractedDelimiterPart)
        {
            var trimedStart = extractedDelimiterPart.Substring(2,extractedDelimiterPart.Length-2);
            var splitString = trimedStart.Split('[',']').ToList();
            return splitString;
        }

        private static string[] SplitOnNewLine(string input)
        {
            return input.Split('\n');
        }

        private static List<int> FilterNumbersGreaterThanOneThousand(List<int> listOfNumbers)
        {
            for (var i = 0; i < listOfNumbers.Count; i++)
            {
                if (listOfNumbers[i]>1000)
                {
                    listOfNumbers[i] = 0;
                }
            }
            return listOfNumbers;
        }

        private static void ThrowExceptionWhenNumberIsLessThan0(List<int> filterNegativeNumbers)
        {
            if (filterNegativeNumbers == null) throw new ArgumentNullException(nameof(filterNegativeNumbers));
            var negativeValue = "";
            negativeValue = ConcatenateNegativeValues(filterNegativeNumbers, negativeValue);
            if (filterNegativeNumbers.Count > 0)
            {
                throw new Exception("Negative numbers not allowed " + negativeValue);

            }
        }

        private static string ConcatenateNegativeValues(IEnumerable<int> numberLessThan0, string negativeValue)
        {
            return numberLessThan0.Aggregate(negativeValue, (current, t) => current + t);
        }

        private static List<int> FilterNegativeNumbers(IEnumerable<int> listOfNumbers)
        {
            return listOfNumbers.Where(i => i < 0).ToList();
        }

        private static string GetCustomeDelimiter(string firstPart)
        {
             return firstPart.Last().ToString();
        }

        private static List<int> SplitIntoListOfNumbers(string input, List<string> defaultDelimiters)
        {
            var listOfNumbers = input.Split(defaultDelimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var converToList = ConverToList(listOfNumbers);
            return converToList;
        }

        private static List<int> ConverToList(string[] listOfNumbers)
        {
            return listOfNumbers.Select(int.Parse).ToList();
        }

        private static int Sum(IEnumerable<int> listOfNumbers)
        {
            return listOfNumbers.Sum();
        }
       private static List<string> DefaultDelimiters()
        {
                return new List<string> {",","\n"};
        }

        private static int ConvertToInteger(string input)
        {
            return int.Parse(input);
        }
    }
}   
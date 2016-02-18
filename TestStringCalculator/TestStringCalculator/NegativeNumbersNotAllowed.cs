using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TestStringCalculator
{
    [Serializable]
    public class NegativeNumbersNotAllowed : Exception
    {
        public NegativeNumbersNotAllowed()
        {
        }

        public List<int> NegativeNumbers { get; private set; }

        public override string Message
        {
            get { return "Negative numbers not allowed"; }
        }

        public NegativeNumbersNotAllowed(string message) : base(message)
        {
        }

        public NegativeNumbersNotAllowed(List<int> negativeNumbers)
        {
            NegativeNumbers = negativeNumbers;
        }

        public NegativeNumbersNotAllowed(string message, Exception inner) : base(message, inner)
        {
        }

        protected NegativeNumbersNotAllowed(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
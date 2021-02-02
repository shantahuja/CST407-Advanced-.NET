using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1.Tests
{
    [TestClass()]
    public class Lab1Tests
    {
        [TestMethod()]
        public void HelloWorldTest()
        {
            string expectedString = "Hello from Lab1!";

            Lab1 testClass = new Lab1();

            string testString;

            testString = testClass.HelloWorld();

            Console.Write(testString);

            Assert.AreEqual(expectedString, testString);
        }

        [TestMethod()]
        public void FibonacciSequenceTest()
        {
            int[] expected = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 };

            Lab1 testClass = new Lab1();

            IEnumerable<int> result = testClass.FibonacciSequence(10);

            foreach (int number in result)
            {
                Console.WriteLine(number);
            }

            // Assert.AreEqual(expected, result.ToArray());

            CollectionAssert.AreEqual(expected, result.ToArray());
        }

        [TestMethod()]
        public void EveryOtherTest()
        {
            string[] originalList = { "red", "green", "blue", "cyan", "magenta", "yellow", "black" };

            string[] evenExpected = { "red", "blue", "magenta", "black" };

            string[] oddExpected = { "green", "cyan", "yellow" };

            int[] originalListNumbers = { 0, 1, 2, 3, 4, 5 };

            int[] evenExpectedNumbers = { 0, 2, 4 };

            int[] oddExpectedNumbers = { 1, 3, 5 };

            Lab1 testClass = new Lab1();

            IEnumerable<int> resultFibonacciNumbers = testClass.FibonacciSequence(10);

            int[] evenFibonacciExpectedNumbers = { 0, 1, 3, 8, 21 }; 

            int[] oddFibonacciExpectedNumbers = { 1, 2, 5, 13, 34 };

            
            IEnumerable<string> resultEven = originalList.EveryOther(true);

            IEnumerable<string> resultOdd = originalList.EveryOther(false);

            IEnumerable<int> resultEvenNumbers = originalListNumbers.EveryOther(true);

            IEnumerable<int> resultOddNumbers = originalListNumbers.EveryOther(false);

            IEnumerable<int> resultFibonacciEvenNumbers = resultFibonacciNumbers.EveryOther(true);

            IEnumerable<int> resultFibonacciOddNumbers = resultFibonacciNumbers.EveryOther(false);


            CollectionAssert.AreEqual(resultEven.ToArray(), evenExpected);

            CollectionAssert.AreEqual(resultOdd.ToArray(), oddExpected);

            CollectionAssert.AreEqual(resultEvenNumbers.ToArray(), evenExpectedNumbers);

            CollectionAssert.AreEqual(resultOddNumbers.ToArray(), oddExpectedNumbers);

            CollectionAssert.AreEqual(resultEvenNumbers.ToArray(), evenExpectedNumbers);

            CollectionAssert.AreEqual(resultOddNumbers.ToArray(), oddExpectedNumbers);

            CollectionAssert.AreEqual(resultFibonacciEvenNumbers.ToArray(), evenFibonacciExpectedNumbers);

            CollectionAssert.AreEqual(resultFibonacciOddNumbers.ToArray(), oddFibonacciExpectedNumbers);

        }

        [TestMethod()]
        public void WhereTest()
        {
            Lab1 testClass = new Lab1();

            IEnumerable<int> resultFibonacciNumbers = testClass.FibonacciSequence(10);

            IEnumerable<int> resultFibonacciEvenNumbers = resultFibonacciNumbers.Where(fibonacciNumber => fibonacciNumber % 2 == 0);

            int[] evenFibonacciExpectedNumbers = { 0, 2, 8, 34 };

            CollectionAssert.AreEqual(resultFibonacciEvenNumbers.ToArray(), evenFibonacciExpectedNumbers);
        }

        [TestMethod()]
        public void GroupByTest()
        {
            Lab1 testClass = new Lab1();

            IEnumerable<int> resultFibonacciNumbers = testClass.FibonacciSequence(10);

            IEnumerable<IGrouping<string, int>> resultFibonacciEvenAndOddNumbers = resultFibonacciNumbers.GroupBy(fibonacciNumber => fibonacciNumber % 2 == 0 ? "Even" : "Odd");

            int[] evenFibonacciExpectedNumbers = { 0, 2, 8, 34 }; 

            int[] oddFibonacciExpectedNumbers = { 1, 1, 3, 5, 13, 21 };

            int[] evenFibonacciResultNumbers = resultFibonacciEvenAndOddNumbers.ToArray()[0].ToArray();

            int[] oddFibonacciResultNumbers = resultFibonacciEvenAndOddNumbers.ToArray()[1].ToArray();

            CollectionAssert.AreEqual(evenFibonacciResultNumbers.ToArray(), evenFibonacciExpectedNumbers);

            CollectionAssert.AreEqual(oddFibonacciResultNumbers.ToArray(), oddFibonacciExpectedNumbers);
        }
    }
}
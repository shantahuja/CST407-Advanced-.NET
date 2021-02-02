using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class Lab1
    {
        public string HelloWorld()
        {
            return "Hello from Lab1!";
        }

        public IEnumerable<int> FibonacciSequence(int max)
        {
			if (max > 0) yield return 0;

			int second = 1;
			int first = 0;

			for (int i = 1; i < max; i++)
			{
				yield return second;
				int temp = second;
				second += first;
				first = temp;
			}
		}

		static string IsEven(int n)
		{
			return n % 2 == 0 ? "Even" : "Odd";
		}


	}

	public static class ExtensionMethods
	{
		public static IEnumerable<T> EveryOther<T>(this IEnumerable<T> originalList, bool evenOrOdd)
		{
			int i = 0;

			foreach (T word in originalList)
			{
				if (evenOrOdd == true && i%2 == 0)
				{
					yield return word;
				}

				if (evenOrOdd == false && i%2 == 1)
				{
					yield return word;
				}
				i++;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PrimeCompetition
{
	public abstract class GeneratorBase
	{
		/// <summary>
		/// Implement me.
		/// </summary>
		public abstract string Description { get; }

		/// <summary>
		/// DoWork should stop as soon as this flag is set.
		/// </summary>
		protected bool Stop { get; private set; }

		/// <summary>
		/// Starts the generation process.
		/// </summary>
		public void Run()
		{
			try
			{
				ICollection<uint> primes = null;

				Stopwatch stopwatch = new Stopwatch();

				using (MMTimer timer = new MMTimer())
				{
					timer.Timer += (sender, e) =>
					{
						Stop = true;

						Console.WriteLine();
						Console.WriteLine();
						Console.WriteLine("Stop flag set after {0} ms", stopwatch.ElapsedMilliseconds);
					};

					stopwatch.Start();
					timer.Start(10000, false);

					Console.Write("Running...");

					primes = DoWork();

					stopwatch.Stop();

					Console.WriteLine("DoWork returned after {0} ms", stopwatch.ElapsedMilliseconds);
					Console.WriteLine("Largest prime found: {0}", primes.LastOrDefault());
				}

				Console.WriteLine();

				TestForOrder(primes);
				TestForInvalidPrimes(primes);
				TestForMissingPrimes(primes);

				Console.WriteLine();
				Console.WriteLine("Complete");
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR ({0})", e.ToString());
			}
		}

		/// <summary>
		/// Implement me.
		/// </summary>
		protected abstract ICollection<uint> DoWork();

		/// <summary>
		/// Feel free to use this method in your solution, it’s not too bad.
		/// </summary>
		protected bool IsPrime(uint number)
		{
			if (number % 2 == 0)
			{
				return (number == 2);
			}

			uint boundary = (uint)Math.Floor(Math.Sqrt(number));

			for (uint i = 3; i <= boundary; i += 2)
			{
				if (number % i == 0) return false;
			}

			return number != 1;
		}

		/// <summary>
		/// Ensure all numbers are in order.
		/// </summary>
		private void TestForOrder(ICollection<uint> primes)
		{
			Console.Write("Testing for order...");

			for (int i = 1; i < primes.Count; i++)
			{
				uint firstNumber = primes.ElementAt(i - 1);
				uint secondNumber = primes.ElementAt(i);

				if (secondNumber <= firstNumber)
				{
					Console.WriteLine("FAILED ({0} and {1} are not in sequential order)", firstNumber, secondNumber);
					return;
				}
			}

			Console.WriteLine("PASSED");
		}

		/// <summary>
		/// Take a random sampling of list items, and ensure each number is prime.
		/// If after two seconds, all sampled numbers are prime, test passes.
		/// </summary>
		private void TestForInvalidPrimes(IEnumerable<uint> primes)
		{
			Console.Write("Testing for invalid primes...");

			Random r = new Random();

			Stopwatch s = new Stopwatch();
			s.Start();

			while (s.ElapsedMilliseconds < 2000)
			{
				int randomIndex = r.Next(0, primes.Count() - 1);
				uint number = primes.ElementAt(randomIndex);

				if (!IsPrime(number))
				{
					Console.WriteLine("FAILED ({0} is not prime)", number);
					return;
				}
			}

			s.Stop();

			Console.WriteLine("PASSED");
		}

		/// <summary>
		/// Take a random sampling of adjacent list items, and ensure there are no missing primes in between.
		/// If after two seconds, no missing primes are found, test passes.
		/// </summary>
		private void TestForMissingPrimes(IEnumerable<uint> primes)
		{
			Console.Write("Testing for missing primes...");

			Random r = new Random();

			Stopwatch s = new Stopwatch();
			s.Start();

			while (s.ElapsedMilliseconds < 2000)
			{
				int randomIndex = r.Next(0, primes.Count() - 2);

				uint firstNumber = primes.ElementAt(randomIndex);
				uint secondNumber = primes.ElementAt(randomIndex + 1);

				uint numberToTest = firstNumber + 1;
				while (numberToTest < secondNumber)
				{
					if (IsPrime(numberToTest))
					{
						Console.WriteLine("FAILED (valid prime {0} is missing between {1} and {2})", numberToTest, firstNumber, secondNumber);
						return;
					}

					numberToTest++;
				}
			}

			s.Stop();

			Console.WriteLine("PASSED");
		}
	}
}

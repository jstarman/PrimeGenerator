using System;

namespace PrimeCompetition
{
	public class Program
	{
		private static void Main(string[] args)
		{
			// TODO: Implement your own Generator
			var generator = new GeneratorExample();

			Console.WriteLine(generator.Description);
			Console.WriteLine();

            generator.Run();	

            Console.WriteLine("Press any key to exit");
			Console.ReadLine();
		}
	}
}

using System;

namespace PrimeCompetition
{
	public class Program
	{
		private static void Main(string[] args)
		{
			// TODO: Implement your own Generator
			var generator = new GeneratorExample();
            var original = new OriginalGeneratorExample();
            var jtc = new JoeToddChadGenerator();

			Console.WriteLine(generator.Description);
			Console.WriteLine();

            generator.Run();

            Console.WriteLine(original.Description);
            Console.WriteLine();

            original.Run();

            Console.WriteLine(jtc.Description);
            Console.WriteLine();

            jtc.Run();

            Console.WriteLine("Press any key to exit");
			Console.ReadLine();
		}
	}
}

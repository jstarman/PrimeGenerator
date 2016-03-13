using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCompetition
{
    /// <summary>
    /// Example implementation.
    /// 
    /// Requirements:
    ///
    /// 1) Extend GeneratorBase.
    ///	2) Implement the Description property.
    ///	3) Implement the DoWork method.
    ///		3a) DoWork should generates primes as fast as possible.
    ///		3b) When the GeneratorBase.Stop flag is set, DoWork should immediately stop all prime generation activities.
    ///		3c) Within 100ms of the GeneratorBase.Stop flag being set, DoWork should return an IEnumerable<uint> containing
    ///			all found primes.  The returned set should begin with 2, and include all primes between 2 and the highest
    ///			found prime, in sequential order. For example:
    ///				Valid - [2, 3, 5] - includes all primes between 2 and 5, in ascending order
    ///				Valid - [2, 3, 5, 7, 11] - includes all primes between 2 and 11, in ascending order
    ///				Invalid - [2, 3, 5, 7, 9, 11] - includes a non-primes
    ///				Invalid - [2, 5, 3, 11, 7] - incorect order
    ///				Invalid - [2, 3, 5, 11] - missing a prime
    /// </summary>
    public class OriginalGeneratorExample : GeneratorBase
    {
        public override string Description
        {
            get { return "Tim's Example Generator"; }
        }

        protected override ICollection<uint> DoWork()
        {
            List<uint> primes = new List<uint>();

            // 2 is a prime number
            primes.Add(2);

            // Start by checking 3
            uint i = 3;

            // Do work until the GeneratorBase.Stop flag is set
            while (!Stop)
            {
                // You may use the included GeneratorBase.IsPrime() method, but you are not required to
                if (IsPrime(i))
                {
                    // Add found prime number to the Primes list
                    primes.Add(i);
                }

                // Only check odd numbers
                i += 2;
            }

            return primes;
        }
    }
}

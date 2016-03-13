using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GeneratorExample : GeneratorBase
    {
        public override string Description
        {
            get { return "Not Tim's Example Generator"; }
        }

        private const int segmentCount = 10000;
        private const uint segmentSize = 15000;
        private const int threadCount = 8;
        private IEnumerable<Segment> _segments;

        public GeneratorExample()
        {
            _segments = GetSegments(segmentCount);
        }
        protected override ICollection<uint> DoWork()
        {
            List<uint> primes = new List<uint>();
            primes.Add(2);

            var currentIndex = 0;

            while (!Stop)
            {
                var tasks = _segments.Skip(currentIndex).Take(threadCount).Select(s => FindAsync(s));
                var waiter = Task.WhenAll(tasks);
                waiter.Wait();

                primes.AddRange(tasks
                .OrderBy(t => t.Result.Id)
                .TakeWhile(t => t.Result.IsCompleted)
                .SelectMany(t => t.Result.Primes));

                currentIndex = currentIndex + threadCount;
            }

            return primes.ToList();
        }

        private IEnumerable<Task<CompletedSegment>> StartCurrentGroup(int skip, int take)
        {
            var starts = _segments.Skip(skip).Take(take).Select(s => FindAsync(s));
            return starts;
        }

        private CompletedSegment FindPrimesInSegment(Segment s)
        {
            List<uint> primes = new List<uint>();

            // Start by checking 3
            uint i = s.Min;

            // Do work until the GeneratorBase.Stop flag is set
            while (i < s.Max && !Stop)
            //while (i < max)
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

            return new CompletedSegment(s.Id, i == s.Max + 1, primes);
        }

        private async Task<CompletedSegment> FindAsync(Segment s)
        {
            return await Task.Run(() => FindPrimesInSegment(s)).ConfigureAwait(false);
        }

        private IEnumerable<Segment> GetSegments(int count)
        {
            var segments = new List<Segment>();
            segments.Add(new Segment(0, 3, segmentSize));
            for (int i = 1; i < count; i++)
            {
                segments.Add(new Segment(i, (uint)i * segmentSize + 1, (uint)i * segmentSize + segmentSize));
            }

            return segments;
        }

        private class Segment
        {
            public Segment(int id, uint min, uint max)
            {
                Id = id;
                Min = min;
                Max = max;
            }
            public int Id;
            public uint Min;
            public uint Max;
        }

        private class CompletedSegment
        {
            public CompletedSegment(int id, bool isCompleted, List<uint> primes)
            {
                Id = id;
                IsCompleted = isCompleted;
                Primes = primes;
            }
            public int Id;
            public bool IsCompleted;
            public List<uint> Primes;
        }
    }
}


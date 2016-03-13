using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeCompetition
{
    public class GeneratorExample : GeneratorBase
    {
        public override string Description
        {
            get { return "Not Tim's Example Generator"; }
        }

        private const int segmentCount = 10000;
        private const int segmentSize = 15000;
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
            List<Task<CompletedSegment>> tasks = new List<Task<CompletedSegment>>(segmentCount);
            while (!Stop)
            {
                var t = _segments.Skip(currentIndex).Take(threadCount).Select(s => FindAsync(s));
                var waiter = Task.WhenAll(t);
                waiter.Wait();
                tasks.AddRange(t);
                currentIndex = currentIndex + threadCount;
            }

            primes.AddRange(tasks
                .OrderBy(t => t.Result.Id)
                .TakeWhile(t => t.Result.IsCompleted)
                .SelectMany(t => t.Result.Primes));
            return primes.ToList();
        }

        private IEnumerable<Task<CompletedSegment>> StartCurrentGroup(int skip, int take)
        {
            var starts = _segments.Skip(skip).Take(take).Select(s => FindAsync(s));
            return starts;
        }

        private CompletedSegment FindPrimesInSegment(Segment s)
        {
            List<uint> primes = new List<uint>(segmentSize);

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


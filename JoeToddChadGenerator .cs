using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCompetition
{
    public class JoeToddChadGenerator : GeneratorBase
    {
        public const int NumThreads = 32;

        public override string Description
        {
            get { return "Something Cool That Todd Comes Up With"; }
        }

        protected override ICollection<uint> DoWork()
        {
            var tasks = new List<Task>(NumThreads);
            for (int i = 0; i < NumThreads; i++)
            {
                tasks.Add(Task.Factory.StartNew(FindPrimes));
            }

            // Do work until the GeneratorBase.Stop flag is set
            while (!Stop)
            {
            }

            return BuildList2().ToList();
        }

        private IEnumerable<uint> BuildList2()
        {
            var result = new List<uint>(SegmentData.Last.BatchId * 5000);
            var segment = SegmentData.First;
            while (segment != null && segment.Complete)
            {
                result.AddRange(segment.Primes);
                segment = segment.Next;
            }

            return result;
        }

        private void FindPrimes()
        {
            while (!Stop)
            {
                var segmentData = SegmentData.GetNextSegment();
                segmentData.Primes = CrunchPrimeSegment(segmentData.Min, segmentData.Max);
                segmentData.Complete = true;
            }
        }

        public List<uint> CrunchPrimeSegment(uint min, uint max)
        {
            List<uint> results = new List<uint>(100000);
            for (uint i = min; i <= max; i++)
            {
                if (IsPrime(i))
                {
                    results.Add(i);
                }
            }
            return results;
        }

    }

    public class SegmentData
    {
        private static uint m_batchSize = 50000;
        private static object m_lock = new object();

        public bool Complete { get; set; }
        public uint Min { get; set; }
        public uint Max { get; set; }
        public int BatchId { get; set; }
        public List<uint> Primes { get; set; }
        public SegmentData Next { get; set; }


        public static SegmentData First { get; private set; }
        public static SegmentData Last { get; private set; }

        public static SegmentData GetNextSegment()
        {
            lock (m_lock)
            {
                if (First == null)
                {
                    var next = new SegmentData
                    {
                        BatchId = 0,
                        Min = 2,
                        Max = m_batchSize
                    };
                    First = next;
                    Last = next;
                }
                else
                {
                    var next = new SegmentData
                    {
                        BatchId = Last.BatchId + 1,
                        Min = Last.Max,
                        Max = Last.Max + m_batchSize
                    };
                    Last.Next = next;
                    Last = next;
                }
            }

            return Last;
        }

    }
}

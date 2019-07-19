using System;
using System.Collections.Generic;
using System.Linq;
using TestletBuilder.Model.Interfaces;

namespace TestletBuilder.Model
{
    public class TestBuilder
    {
        ITestBankRepository testBank = null;
        public TestBuilder(ITestBankRepository testBankRepo)
        {
            this.testBank = testBankRepo;
        }

        private Testlet CreateTestlet(IEnumerable<TestItem> pretestItems, 
                                      IEnumerable<TestItem> operationalItems)
        {
            var newTestlet = new Testlet();
            newTestlet.Questions.AddRange(pretestItems.Take(2));

            var remainingRange =
                pretestItems.Skip(2).Concat(operationalItems).OrderBy(i => Guid.NewGuid());
            newTestlet.Questions.AddRange(remainingRange);

            return newTestlet;
        }

        public Testlet AssembleSingleTestlet()
        {
            // get Pretest and Operational questions
            var pretestItems = testBank.GetRandomizedPretestItems(4);
            var operationalItems = testBank.GetRandomizedOperationalItems(6);

            return CreateTestlet(pretestItems, operationalItems);
        }

        public IEnumerable<Testlet> AssembleMultipleTestlets(int numTests)
        {
            var totalNumPretest = numTests * 4;
            var totalNumOperational = numTests * 6;
            var pretestPool = testBank.GetRandomizedPretestItems(totalNumPretest).ToList();
            var operationalPool = testBank.GetRandomizedOperationalItems(totalNumOperational).ToList();

            var result = 
                Enumerable.Range(0, numTests)
                      .Aggregate(new Tuple<int, int, List<Testlet>>(0, 0, new List<Testlet>()),
                                (agg, i) =>
                                {
                                    var preItems = pretestPool.Skip(agg.Item1).Take(4);
                                    var opItems = operationalPool.Skip(agg.Item2).Take(6);
                                    agg.Item3.Add(CreateTestlet(preItems, opItems));
                                    return new Tuple<int, int, List<Testlet>>(
                                             agg.Item1 + 4, agg.Item2 + 6,
                                             agg.Item3);
                                });
            return result.Item3;
        }
    }
}

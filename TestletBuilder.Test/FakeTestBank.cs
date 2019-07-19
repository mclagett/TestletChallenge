using System;
using System.Collections.Generic;
using System.Linq;
using TestletBuilder.Model;
using TestletBuilder.Model.Interfaces;

namespace TestletBuilder.Test
{
    public class FakeTestBank : ITestBankRepository
    {
        List<TestItem> pretestItems = new List<TestItem>();
        List<TestItem> operationalItems = new List<TestItem>();

        public FakeTestBank(int initialPretestCount, int initialOperationalCount)
        {
            // Generate Test Bank With 1000 Pretest Items and 10,000 Operational Items
            var pretestItems = Enumerable.Range(0, initialPretestCount)
                                      .Select(i => new TestItem { Id = i, IsPretest = true });
            AddPretestItems(pretestItems);
            var operationalItems = Enumerable.Range(initialPretestCount, initialOperationalCount)
                                         .Select(i => new TestItem { Id = i, IsPretest = false });
            AddOperationalItems(operationalItems);
        }

        public List<TestItem> PretestItems { get => pretestItems; }
        public List<TestItem> OperationalItems { get => operationalItems; }

        public void AddPretestItems(IEnumerable<TestItem> items)
        {
            pretestItems.AddRange(items);
        }

        public void AddOperationalItems(IEnumerable<TestItem> items)
        {
            operationalItems.AddRange(items);
        }

        public IEnumerable<TestItem> GetSequentialPretestItems(int count = 1, int start = 0)
        {
            return PretestItems.Skip(start).Take(count);
        }

        public IEnumerable<TestItem> GetRandomizedPretestItems(int count = 1)
        {
            var distinctCount = PretestItems.Distinct().Count();
            var distinctCount2 = PretestItems.Concat(OperationalItems).Distinct().Count();
            return PretestItems.OrderBy(i => Guid.NewGuid()).Take(count);
        }

        public IEnumerable<TestItem> GetSequentialOperationalItems(int count = 1, int start=0)
        {
            return OperationalItems.Skip(start).Take(count);
        }

        public IEnumerable<TestItem> GetRandomizedOperationalItems(int count = 1)
        {
            var distinctCount = OperationalItems.Distinct().Count();
            return OperationalItems.OrderBy(i => Guid.NewGuid()).Take(count);
        }
    }
}

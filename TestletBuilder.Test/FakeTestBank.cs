using System;
using System.Collections.Generic;
using System.Linq;
using TestletBuilder.Model;
using TestletBuilder.Model.Interfaces;

namespace TestletBuilder.Test
{
    public class FakeTestBank 
    {
        List<TestItem> pretestItems = new List<TestItem>();
        List<TestItem> operationalItems = new List<TestItem>();

        public FakeTestBank(int initialPretestCount, int initialOperationalCount)
        {
            // Generate Test Bank With initialPretestCount Pretest Items and 
            // initialOperationalCount Operational Items
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

        public IEnumerable<TestItem> GetSequentialPretestItems(int count , int start )
        {
            return PretestItems.Skip(start).Take(count);
        }

        public IEnumerable<TestItem> GetRandomizedPretestItems(int count)
        {
            return PretestItems.OrderBy(i => Guid.NewGuid()).Take(count);
        }

        public IEnumerable<TestItem> GetSequentialOperationalItems(int count, int start)
        {
            return OperationalItems.Skip(start).Take(count);
        }

        public IEnumerable<TestItem> GetRandomizedOperationalItems(int count )
        {
            return OperationalItems.OrderBy(i => Guid.NewGuid()).Take(count);
        }

        public TestletQuestionSet GetTestletQuestionSetRandomly()
        {
            return new TestletQuestionSet()
            {
                PretestQuestions = this.GetRandomizedPretestItems(4),
                OperationalQuestions = GetRandomizedOperationalItems(6)
            };
        }

        public TestletQuestionSet GetTestletQuestionSetSequentially(int pretestStart = 0, int operationalStart = 0)
        {
            return new TestletQuestionSet()
            {
                PretestQuestions = this.GetSequentialPretestItems(4, pretestStart),
                OperationalQuestions = GetSequentialOperationalItems(6,operationalStart)
            };
        }
    }
}

using System.Collections.Generic;
using Xunit;
using TestletBuilder.Model;
using TestletBuilder.Model.Interfaces;
using System.Linq;

namespace TestletBuilder.Test
{

    public class TestletBuilderTests
    {
        ITestBankRepository testBank = null;
        TestBuilder testBuilder = null;

        public TestletBuilderTests()
        {
            // we can reuse test bank across all tests
            testBank = new FakeTestBank(1000, 10000);
            testBuilder = new TestBuilder(testBank);
        }

        [Fact]
        public void AssembledTestletHasPretestForFirstTwoItems()
        {
            // first two items are always Pretest items

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.True(testlet.Questions.Take(2).All(q => q.IsPretest == true));
        }

        [Fact]
        public void AssembledTestletLastEightItemsMixtureOfTwoPretestAndSixOperational()
        {
            // last eight items include two pretests and six operational

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.True(testlet.Questions.Skip(2).Where(i => i.IsPretest).Count() == 2);
            Assert.True(testlet.Questions.Skip(2).Where(i => !i.IsPretest).Count() == 6);
        }

        [Fact]
        public void AssembledTestletHasTotalOfTenItems()
        {
            // testlet has no more and no less than ten items

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.True(testlet.Questions.Count == 10);
        }

        [Fact]
        public void NoDuplicateItemsExistInTestlet()
        {
            // all ten items in test are distinct

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.Equal(testlet.Questions.Count(), testlet.Questions.Distinct().Count());
        }

        [Fact]
        public void NoConsecutivePretestItemsInLastEight()
        {
            // in last eight questions the two preset items do not appear consecutively

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            var numConsecutivePretests =
                testlet.Questions
                       .Skip(2)
                       .Aggregate(0, ((agg, i) => agg == 2 
                                                    ? agg 
                                                    : i.IsPretest ? agg + 1 : 0));
            Assert.True(numConsecutivePretests < 2);
        }

        [Fact]
        public void SixTestletItemsAreOperationalAndFourArePretest()
        {
            // a run of the testbuilder will produce a total of ten items, four of which 
            // are pretest, and six of which are operational

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.True(testlet.Questions.Where(i => i.IsPretest).Count() == 4);
            Assert.True(testlet.Questions.Where(i => !i.IsPretest).Count() == 6);
        }

        [Fact]
        public void SingleTestletSatisfiesAllConstraints()
        {
            // single testlet satisfies all constraints above

            // SUT is testBuilder.AssembleSingleTestlet();
            Testlet testlet = testBuilder.AssembleSingleTestlet();
            Assert.True(testlet.Questions.Take(2).All(q => q.IsPretest == true));
            Assert.True(testlet.Questions.Skip(2).Where(i => i.IsPretest).Count() == 2);
            Assert.True(testlet.Questions.Skip(2).Where(i => !i.IsPretest).Count() == 6);
            Assert.True(testlet.Questions.Count == 10);
            Assert.Equal(testlet.Questions.Count(), testlet.Questions.Distinct().Count());

            var numConsecutivePretests =
                testlet.Questions
                       .Skip(2)
                       .Aggregate(0, ((agg, i) => agg == 2
                                                    ? agg
                                                    : i.IsPretest ? agg + 1 : 0));
            Assert.True(numConsecutivePretests < 2);
            Assert.True(testlet.Questions.Where(i => i.IsPretest).Count() == 4);
            Assert.True(testlet.Questions.Where(i => !i.IsPretest).Count() == 6);
        }

        [Fact]
        public void TestletItemsAreRandomlyOrdered()
        {
            // several successive runs of testbuilder will produce different orders

            // SUT is testBuilder.AssembleSingleTestlet();
            var testlets = Enumerable.Range(0,100).Select(n => testBuilder.AssembleSingleTestlet().Questions.Select(q => q.Id)).ToArray();
            for (int i = 0; i < 100; i++)
            {
                var testletToCompare = testlets[i];

                for (int j = i+1; j < 100; j++ )
                {
                    Assert.False(testletToCompare.SequenceEqual(testlets[j]));
                }
            }
        }

        [Fact]
        public void SystemCanGenerateMultipleTestletsWithNonDuplicateItems()
        {
            // SUT is testBuilder.AssembleMultipleTestlets();
            var testlets = testBuilder.AssembleMultipleTestlets(100);
            var allQuestions = testlets.SelectMany(t => t.Questions.Select(q => q.Id));
            var questionCount = allQuestions.Count();
            var distinctQuestionCount = allQuestions.Distinct().Count();
            Assert.Equal(questionCount, distinctQuestionCount);
        }
    }
}

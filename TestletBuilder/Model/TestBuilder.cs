using System;
using System.Collections.Generic;
using System.Linq;
using TestletBuilder.Model.Interfaces;

namespace TestletBuilder.Model
{
    public class TestBuilder
    {
        private Testlet CreateTestlet(TestletQuestionSet questionSet)
        {
            // for a given set of four pretest and eight operational questions
            // begin the testlet with a random two of the four pretest and then 
            // the remaining eight ordered randomly.

            List<TestItem> randomizedPretestItems = null;
            List<TestItem> randomizedRemainingRange = null;
            int numConsecutivePretests = 0;
            var newTestlet = new Testlet();

            // randomize pretestItems
            randomizedPretestItems = questionSet.PretestQuestions.OrderBy(i => Guid.NewGuid()).ToList();
            newTestlet.Questions.AddRange(randomizedPretestItems.Take(2));

            do
            {
                randomizedRemainingRange =
                    randomizedPretestItems.Skip(2).Concat(questionSet.OperationalQuestions).OrderBy(i => Guid.NewGuid()).ToList();

                numConsecutivePretests =
                    randomizedRemainingRange.Aggregate(0, ((agg, i) => agg == 2
                                                                       ? agg
                                                                       : i.IsPretest ? agg + 1 : 0));
            } while (numConsecutivePretests == 2);
            newTestlet.Questions.AddRange(randomizedRemainingRange);

            return newTestlet;
        }

        public Testlet AssembleTestlet(TestletQuestionSet questionSet)
        {
            return CreateTestlet(questionSet);
        }
    }
}

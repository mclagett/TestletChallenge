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

            // ensure that incoming questionSet has four pretest itmes and six operational items
            if ((questionSet.PretestQuestions.Count() != 4) || (questionSet.OperationalQuestions.Count() != 6))
            {
                throw new IncorrectNumberOfItemsException("A testlet needs to include exactly four Pretest and six Operational questions");
            }

            var newTestlet = new Testlet();

            // randomize pretestItems
            var randomizedPretestItems = questionSet.PretestQuestions.OrderBy(i => Guid.NewGuid()).ToList();
            newTestlet.Questions.AddRange(randomizedPretestItems.Take(2));

            var randomizedRemainingRange =
                randomizedPretestItems.Skip(2).Concat(questionSet.OperationalQuestions).OrderBy(i => Guid.NewGuid()).ToList();
            newTestlet.Questions.AddRange(randomizedRemainingRange);

            return newTestlet;
        }

        public Testlet AssembleTestlet(TestletQuestionSet questionSet)
        {
            return CreateTestlet(questionSet);
        }
    }
}

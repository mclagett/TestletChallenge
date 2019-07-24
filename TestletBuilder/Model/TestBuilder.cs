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

            var newTestlet = new Testlet();

            // randomize pretestItems
            var randomizedPretestItems = questionSet.PretestQuestions.OrderBy(i => Guid.NewGuid());
            newTestlet.Questions.AddRange(randomizedPretestItems.Take(2));

            var randomizedRemainingRange =
                randomizedPretestItems.Skip(2).Concat(questionSet.OperationalQuestions).OrderBy(i => Guid.NewGuid());
            newTestlet.Questions.AddRange(randomizedRemainingRange);

            return newTestlet;
        }

        public Testlet AssembleTestlet(TestletQuestionSet questionSet)
        {
            return CreateTestlet(questionSet);
        }
    }
}

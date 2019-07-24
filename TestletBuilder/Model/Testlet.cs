using System.Collections.Generic;

namespace TestletBuilder.Model
{
    public class Testlet
    {
        List<TestItem> questions = new List<TestItem>();

        public List<TestItem> Questions { get => questions; }
    }
}
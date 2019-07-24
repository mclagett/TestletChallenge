using System.Collections;

namespace TestletBuilder.Model
{
    public class TestItem : IEqualityComparer
    {
        int id;
        public int Id { get => id; set => id = value; }

        bool isPretest;
        public bool IsPretest { get => isPretest; set => isPretest = value; }

        // a real test item would of course have additional fields to represent 
        // question, answer choices, difficulty, plus a host of other attributes
        // these, however, are not relevant to the correct working of the algorithm

        // IEqualityComparer
        bool IEqualityComparer.Equals(object x, object y)
        {
            return (((TestItem) x).Id == ((TestItem) y).Id);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}

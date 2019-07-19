using System;
using System.Collections.Generic;
using System.Text;

namespace TestletBuilder.Model.Interfaces
{
    public interface ITestBankRepository
    {
        IEnumerable<TestItem> GetSequentialPretestItems(int count = 1, int start = 0);
        IEnumerable<TestItem> GetRandomizedPretestItems(int count = 1);
        IEnumerable<TestItem> GetSequentialOperationalItems(int count = 1, int start = 0);
        IEnumerable<TestItem> GetRandomizedOperationalItems(int count = 1);
    }
}

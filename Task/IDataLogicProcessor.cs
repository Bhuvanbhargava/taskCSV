using System.Collections.Generic;
using System.IO;

namespace CSVFileHandler
{
    public interface IDataLogicProcessor
    {
        List<string> SortAddressByAlphabet(List<string> addressItem);
        List<string> SortNameByFrequencyAndAlphabet(List<string> frequencyItems);
    }
}
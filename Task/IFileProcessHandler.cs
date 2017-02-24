using System;
using System.Collections.Generic;
using System.IO;
using SystemWrapper.IO;

namespace CSVFileHandler
{
    public interface IFileProcessHandler
    {
        void CreateFile(string directoryName,string filename, string[] lines);
        MemoryStream GetMemoryStremFromFile();
        Tuple<List<string>, List<string>> ProcessStreamData(MemoryStream stream);
        
    }
}
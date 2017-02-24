using System;
using System.Collections.Generic;
using System.IO;
using SystemWrapper.IO;

namespace CSVFileHandler
{
    public class FileProcessHandler : IFileProcessHandler
    {
        
        private readonly IFileWrap _file;
        private readonly IFileInfoWrap _fileInfo;
        private IPathWrap _path;
        public FileProcessHandler(IFileWrap file,IPathWrap path, IFileInfoWrap fileInfo)
        {
            _file = file;
            _fileInfo = fileInfo;
            _path = path;
        }

        public MemoryStream GetMemoryStremFromFile()
        {

            IFileStreamWrap fileStream = _fileInfo.OpenRead();
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.SetLength(fileStream.Length);
            fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
            return memoryStream;
        }

        public Tuple<List<string>,List<string>> ProcessStreamData(MemoryStream stream)
        {
            if (stream.Length == 0)
            {
                throw new ArgumentException("No data found to process");
            }
            var frequencyItem = new List<string>();
            var addressItem = new List<string>();
            StreamReader sr = new StreamReader(stream);
            sr.ReadLine();//skip the first line as header
            while (!sr.EndOfStream)
            {
                var linetext = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(linetext))
                {
                    var arr = linetext.Split(',');
                    frequencyItem.Add(string.Format(arr[0]).Trim().ToLower());
                    frequencyItem.Add(string.Format(arr[1]).Trim().ToLower());
                    addressItem.Add(string.Format(arr[2].Trim().ToLower()));
                }
            }
            return new Tuple<List<string>, List<string>>(frequencyItem, addressItem);
          
        }

        public void CreateFile(string directoryName,string filename, string[] lines)
        {
             var fullFilename = _path.Combine(directoryName, filename);
            _file.WriteAllLines(fullFilename, lines);

        }
    }
}

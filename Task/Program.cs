using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemWrapper.IO;
using Microsoft.Practices.Unity;

namespace CSVFileHandler
{
    public class Program
    {
        private static string fileLocation = null;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("This project using the embeded data.csv file in case no file or location provided");
            Console.WriteLine("*****************************************************");
            DirectoryInfo dirinfo =null;
            FileInfo[] files = null;
            bool isCSVFilesExist = false;
            while (!isCSVFilesExist)
            {
                dirinfo = AskFileLocation();
                if (!dirinfo.Exists)
                {
                    Console.WriteLine("Directory {0} does not exist.", dirinfo.FullName);
                    Console.WriteLine();
                    continue;
                }
                files = CheckForCSVFilesInDirectory(dirinfo);
                if (files.Length == 0)
                {
                    Console.WriteLine("no csv file fount at {0}", dirinfo.FullName);
                    Console.WriteLine();
                }
                else { 
                    isCSVFilesExist= true;
                }
            }
            bool isValidFileSelected = false;
            int selectedFileId=-1;
            while (!isValidFileSelected)
            {
                int counter = 1;
                Console.WriteLine("Please enter a number in front of the corrospoding file to select");
                foreach (var file in files)
                {
                    Console.WriteLine("{0} - {1}", counter++, file.Name);

                }
                --counter;//decrease the additonl counter to maintain the index;
               
                int.TryParse(Console.ReadLine(), out selectedFileId);
                if (selectedFileId > counter || selectedFileId <= 0)
                {
                    continue;
                }
                isValidFileSelected = true;
            }
            
            var selectedFile = files[--selectedFileId];
            IFileInfoWrap wrapperFile = new FileInfoWrap(selectedFile);

            Console.WriteLine("Ok! processing ({0}) file.", selectedFile);
            fileLocation = dirinfo.FullName;

            //IOC Resolver
            var container = UnityResolver.UnityRegisterTypes();
            IFileProcessHandler fileHandler = container.Resolve<IFileProcessHandler>(new ParameterOverride("fileInfo", wrapperFile));
            IDataLogicProcessor datalogic = container.Resolve<IDataLogicProcessor>();

            var result = ProcessFile(fileHandler, datalogic);
            PrintResultOnCosole(result);
            CreateFile("FrequencyFile.txt", result.FrequencyItem,  fileHandler);
            CreateFile("AddressFile.txt", result.AddressItem, fileHandler);
            Console.WriteLine();
            Console.WriteLine("********Files are saved at ({0}).**********", fileLocation);

            Console.ReadLine();
        }
        public static ResultViewModel ProcessFile(IFileProcessHandler fileHandler, IDataLogicProcessor datalogic)
        {
            var stream = fileHandler.GetMemoryStremFromFile();
            var result = fileHandler.ProcessStreamData(stream);
            var frequencyresult = datalogic.SortNameByFrequencyAndAlphabet(result.Item1);
            var addressresult = datalogic.SortAddressByAlphabet(result.Item2);
            return new ResultViewModel()
            {
                FrequencyItem = frequencyresult,
                AddressItem = addressresult

            };
        }
       

        public static void CreateFile(string filename, List<string> result, IFileProcessHandler fh)
        {
            fh.CreateFile(fileLocation , filename, result.ToArray());
        }

        private static void PrintResultOnCosole(ResultViewModel result)
        {
            //Print the FrequencyResult
            Console.WriteLine();
            Console.WriteLine("Order name and lastname by frequency than alphabet");
            foreach (var item in result.FrequencyItem)
            {
                Console.WriteLine(item);
            }
            ;
            // Print the AddressResult
            Console.WriteLine();
            Console.WriteLine("Order address by alphabet");
            foreach (var item in result.AddressItem)
            {
                Console.WriteLine(item);
            }
        }

        private static FileInfo[] CheckForCSVFilesInDirectory(DirectoryInfo dirinfo)
        {
            FileInfo[] files = dirinfo.GetFiles("*.csv");
            return files;
           
        }

        private static DirectoryInfo AskFileLocation()
        {
            Console.WriteLine("Please specify file location or press enter to use default embedded file");

            var filePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = @"..\..\Files";
            }
           var dirinfo = new DirectoryInfo(filePath);
           return dirinfo;
        }

      
    }
}

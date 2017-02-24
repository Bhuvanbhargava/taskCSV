using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using SystemWrapper.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSVFileHandler.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FileProcessHandlerUnitTest
    {
        private Mock<IDataLogicProcessor> _mockDatalogic;
        private Mock<IFileInfoWrap> _mockFileInfo;
        private Mock<IFileStreamWrap> _mockFileStream;
        private Mock<IFileWrap> _mockFile;
        private Mock<IPathWrap> _mockPath;
        private FileProcessHandler fh;
        private string _rawData;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockFileInfo = new Mock<IFileInfoWrap>();
            _mockFileStream = new Mock<IFileStreamWrap>();
            _mockFile = new Mock<IFileWrap>();
            _mockPath = new Mock<IPathWrap>();

            _mockFileInfo.Setup(a => a.OpenRead()).Returns(_mockFileStream.Object);
            fh = new FileProcessHandler(_mockFile.Object, _mockPath.Object, _mockFileInfo.Object );

            _rawData =
           @"FirstName,LastName,Address,PhoneNumber
                        Joseph,Sharma,102 Long Lane,29384857
                        CALVIN,Omkar,65 Ambling Way,31214788";

        }



        [TestMethod]
        public void GetMemoryStremFromFile_test()
        {
            var stream=fh.GetMemoryStremFromFile();
            Assert.IsNotNull(stream);
            _mockFileStream.Verify(a=>a.Read(It.IsAny<byte[]>(),It.IsAny<int>(),It.IsAny<int>()),Times.Once);
        }

        [TestMethod]
        public void StartFileProcessing_ProcessFile_CallsDataLogic_returnResult()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(_rawData)))
            {
                var result = fh.ProcessStreamData(stream);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Item1.Any());
                Assert.IsTrue(result.Item2.Any());
                
            }

        }

        [TestMethod]
        public void CreateFile_CreateFile_asExpected()
        {
            string response = null;
            string[] actualArr = null;
            string[] expectedArr = { "text" };
            string inputFileLocation = "filelocation";
            string inputFilename = "test.txt";
            string fullFileName = @"filelocation\test.txt";
            _mockPath.Setup(a => a.Combine(inputFileLocation, inputFilename))
                .Returns(fullFileName);
            _mockFile.Setup(a => a.WriteAllLines(fullFileName, expectedArr))
               .Callback<string, string[]>((name, text) =>
                 {
                     response = string.Format("file {0} Created", fullFileName);
                     actualArr = expectedArr;

                 });

            fh.CreateFile(inputFileLocation, inputFilename, expectedArr);
            Assert.AreEqual(string.Format("file {0} Created", fullFileName), response);
            Assert.AreEqual(expectedArr, actualArr);
            _mockFile.Verify(a => a.WriteAllLines(fullFileName, expectedArr), Times.Once);
            _mockPath.Verify(a=>a.Combine(inputFileLocation, inputFilename));

        }

    }
}

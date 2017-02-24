using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSVFileHandler.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProgramUnitTest
    {
        private Mock<IFileProcessHandler> _mockFileHandler;
        private Mock<IDataLogicProcessor> _mockDatalogic;

        [TestMethod]
        public void ProcessFile_CallsFileHandlerAndSortLogic_returnResult()
        {
             var result= Program.ProcessFile(_mockFileHandler.Object, _mockDatalogic.Object);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FrequencyItem.Any());
            Assert.IsTrue(result.AddressItem.Any());
            _mockFileHandler.Verify(a => a.GetMemoryStremFromFile(), Times.Once);
            _mockFileHandler.Verify(a => a.ProcessStreamData(It.IsAny<MemoryStream>()), Times.Once);
            _mockDatalogic.Verify(a => a.SortNameByFrequencyAndAlphabet(It.IsAny<List<string>>()), Times.Once);
            _mockDatalogic.Verify(a => a.SortAddressByAlphabet(It.IsAny<List<string>>()), Times.Once);
            
        }


        [TestMethod]
        public void CreateFile_CallsFileHandlertoCreateFile()
        {
            var textList = new List<string>() {"abc", "pqr"};
            var filename = "myFile.txt";
            Program.CreateFile(filename, textList,_mockFileHandler.Object);
            _mockFileHandler.Verify(a => a.CreateFile(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string[]>()), Times.Once);

        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockFileHandler = new Mock<IFileProcessHandler>();
            _mockDatalogic = new Mock<IDataLogicProcessor>();

            _mockFileHandler.Setup(a => a.GetMemoryStremFromFile())
              .Returns(new MemoryStream(Encoding.UTF8.GetBytes("testData")));
            _mockFileHandler.Setup(a => a.ProcessStreamData(It.IsAny<MemoryStream>()))
                .Returns(new Tuple<List<string>, List<string>>(new List<string>(), new List<string>()));

            _mockDatalogic.Setup(a => a.SortNameByFrequencyAndAlphabet(It.IsAny<List<string>>()))
               .Returns(new List<string>()
               {
                  {"test"},
               });
            _mockDatalogic.Setup(a => a.SortAddressByAlphabet(It.IsAny<List<string>>()))
               .Returns(new List<string>()
               {
                  {"Address"}
               });
        }

    }
}

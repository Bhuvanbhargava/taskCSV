using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSVFileHandler.UnitTest
{
    [TestClass]
    public class DataLogicProcessorUnitTest
    {
        private DataLogicProcessor _logicHandler;
        
        [TestMethod]
        public void SortAddressByAlphabet_ValidList_returnAddressInAlphabetOrder()
        {
            List<string> unsortedList = new List<string>()
            {
                "102 long lane",
                "12 howard st",
                "78 short lane",
                "23-4 5 you at street",
                "82 stewart st",
                "65 ambling way",
                "94 roland st",
                "8 crimson rd",
                "49 sutherland st"
            };

            var result = _logicHandler.SortAddressByAlphabet(unsortedList);
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Count);
            List<string> actual = result.ToList();
            Assert.IsTrue(varifyAddressOrder(actual));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SortAddressByAlphabet_NullList_throwArgumentNullException()
        {
             _logicHandler.SortAddressByAlphabet(null);
        }

        [TestMethod]
        public void SortNameByFrequencyAndAlphabet_ValidList_returnNameInFrequencyOrder()
        {
            List<string> unsortedList = new List<string>()
            {
                "sharma","jagtap","jerry","bob",
                "calvin","CALVIN","omkar","sharma",
                "bob","hue","bhuvan","jerry","sharma",
                "hue","bhuvan","joseph","omkar","bhuvan",

            };

            var result = _logicHandler.SortNameByFrequencyAndAlphabet(unsortedList);
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Count);
            List<string> actual = result.ToList();
            Assert.IsTrue(varifyFrequencyOrder(actual));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SortNameByFrequencyAndAlphabet_NullParam_throwArgumentNullException()
        {
            _logicHandler.SortNameByFrequencyAndAlphabet(null);
        }

        private bool varifyFrequencyOrder(List<string> actual)
        {
            return (actual.IndexOf("bhuvan") == 0 &&
                    actual.IndexOf("sharma") == 1 &&
                    actual.IndexOf("bob") == 2 &&
                    actual.IndexOf("calvin") == 3 &&
                    actual.IndexOf("hue") == 4 &&
                    actual.IndexOf("jerry") == 5 &&
                    actual.IndexOf("omkar") == 6 &&
                    actual.IndexOf("jagtap") == 7 &&
                    actual.IndexOf("joseph") == 8);

        }

        private bool varifyAddressOrder(List<string> actual)
        {
            return (actual.IndexOf("65 ambling way") == 0 &&
            actual.IndexOf("8 crimson rd") == 1 &&
            actual.IndexOf("12 howard st") == 2 &&
            actual.IndexOf("102 long lane") == 3 &&
            actual.IndexOf("94 roland st") == 4 &&
            actual.IndexOf("78 short lane") == 5 &&
            actual.IndexOf("82 stewart st") == 6 &&
            actual.IndexOf("49 sutherland st") == 7 &&
            actual.IndexOf("23-4 5 you at street") == 8);


        }

        [TestInitialize]
        public void TestInitialize()
        {
            _logicHandler = new DataLogicProcessor();
        }
    }
}

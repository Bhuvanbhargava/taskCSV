using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVFileHandler
{
    public  class DataLogicProcessor : IDataLogicProcessor
    {
        public List<string> SortAddressByAlphabet(List<string> listItems)
        {
            if (listItems == null) { 
                throw new ArgumentNullException("listItems");
            }

            List<AddressModel> resultList = new List<AddressModel>();

            listItems.ForEach(a =>
            {
                int index = 0;
                char[] arr = a.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (Char.IsLetter(arr[i]))
                    {
                        index = i;
                        break;
                    }
                }
                resultList.Add(new AddressModel()
                {
                    Address = a,
                    SortString = a.Substring(index)
                });
            });

            return resultList.OrderBy(a => a.SortString).Select(a=>a.Address).ToList();
        }


        public List<string> SortNameByFrequencyAndAlphabet(List<string> listItems)
        {
            if (listItems == null)
            {
                throw new ArgumentNullException("listItems");
            }
            listItems =listItems.ConvertAll(d => d.ToLower());
            List<FrequencyModel> model = new List<FrequencyModel>();
            foreach (var listItem in listItems)
            {
                if (model.Any(a=>a.Item == listItem))
                {
                   var itemfrq = model.FirstOrDefault(a => a.Item == listItem).Frequency;
                    model.FirstOrDefault(a => a.Item == listItem).Frequency = ++itemfrq;
                }
                else
                {                    
                    model.Add(new FrequencyModel { Item = listItem, Frequency = 1 });
                }
            }

            return model.OrderByDescending(a=>a.Frequency).ThenBy(a=>a.Item).Select(a=>a.Item).ToList();
            
        }
    
    }
}

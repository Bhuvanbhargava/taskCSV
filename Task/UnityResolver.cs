using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;
using Microsoft.Practices.Unity;

namespace CSVFileHandler
{
    public static class UnityResolver
    {
        public static UnityContainer UnityRegisterTypes()
        {
            var container = new UnityContainer();
            container.RegisterType<IDataLogicProcessor, DataLogicProcessor>();
            container.RegisterType<IFileProcessHandler, FileProcessHandler>();
            container.RegisterType<IFileWrap, FileWrap>();
            container.RegisterType<IFileInfoWrap, FileInfoWrap>();
            container.RegisterType<IPathWrap, PathWrap>(); 




            return container;
        }
    }
}

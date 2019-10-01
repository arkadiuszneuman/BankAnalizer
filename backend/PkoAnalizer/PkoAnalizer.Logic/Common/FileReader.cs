using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PkoAnalizer.Logic.Common
{
    public interface IFileReader
    {
        IEnumerable<string> ReadLines(string path) => ReadLines(path, Encoding.Default);
        IEnumerable<string> ReadLines(string path, Encoding encoding);
    }

    public class FileReader : IFileReader
    {
        static FileReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return File.ReadLines(path, encoding);
        }
    }
}

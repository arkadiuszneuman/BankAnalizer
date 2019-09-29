using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PkoAnalizer.Logic.Common
{
    public class FileReader
    {
        static FileReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public virtual IEnumerable<string> ReadLines(string path)
        {
            return ReadLines(path, Encoding.Default);
        }

        public virtual IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return File.ReadLines(path, encoding);
        }
    }
}

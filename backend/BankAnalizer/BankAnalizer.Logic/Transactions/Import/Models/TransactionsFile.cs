using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Import.Models
{
    public class TransactionsFile
    {
        private readonly byte[] byteArray;

        private TransactionsFile(byte[] byteArray)
        {
            this.byteArray = byteArray;
        }

        public static async Task<TransactionsFile> CreateFromStream(Stream stream)
        {
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return new TransactionsFile(memoryStream.ToArray());
        }

        public string ReadFromWindows1250Encoding() => Encoding.GetEncoding(1250).GetString(byteArray);
        public string ReadFromUtf8() => Encoding.UTF8.GetString(byteArray);
    }
}
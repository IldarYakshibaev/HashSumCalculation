using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashSumCalculation
{
    public class Flow
    {
        public static void FlowRecord(Object parameter)
        {
            foreach (string file_name in parameter as string[])
            {
                try
                {

                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(file_name))
                        {
                            byte[] checkSum = md5.ComputeHash(stream);
                            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);

                            Record.RecordInDB(Path.GetDirectoryName(file_name), Path.GetFileName(file_name), result, "Ok");
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Record.RecordInDB(Path.GetDirectoryName(file_name), Path.GetFileName(file_name), "", "Не получилось получить доступ к " + file_name);
                }
            }
        }
    }
}

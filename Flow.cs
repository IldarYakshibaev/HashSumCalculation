using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashSumCalculation
{
    public class Flow
    {
        public static Semaphore sem = new Semaphore(5, 5);
        static object locker = new object();
        public static void FlowCalculationHashSum(Object parameterCalculation)
        {
            List<HashSum> hashSums = new List<HashSum>();
            sem.WaitOne();
            foreach (string file_name in parameterCalculation as string[])
            {
                try
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(file_name))
                        {
                            byte[] checkSum = md5.ComputeHash(stream);
                            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                            Console.WriteLine(Path.GetDirectoryName(file_name) + " " + Path.GetFileName(file_name) + " " + result + "Ok");
                            hashSums.Add(new HashSum
                            {
                                Path = Path.GetDirectoryName(file_name),
                                Title = Path.GetFileName(file_name),
                                HashSumFile = result,
                                TypeError = "Ok"
                            });
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    hashSums.Add(new HashSum
                    {
                        Path = Path.GetDirectoryName(file_name),
                        Title = Path.GetFileName(file_name),
                        HashSumFile = "",
                        TypeError = "Не получилось получить доступ к " + file_name
                    });
                }
            }
            sem.Release();
            ParameterizedThreadStart parameterRecord = new ParameterizedThreadStart(Flow.FlowRecordInDb);

            Thread thread = new Thread(parameterRecord);
            thread.Start(hashSums);
        }



        public static void FlowRecordInDb(Object parameterRecord)
        {
            lock (locker)
            {
                sem.WaitOne();
                foreach (HashSum hashSum in parameterRecord as List<HashSum>)
                {
                    Record.RecordInDB(hashSum);
                }
                sem.Release();
            }
        }
    }
}

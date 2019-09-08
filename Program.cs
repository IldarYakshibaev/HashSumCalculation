using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashSumCalculation
{
    class Program
    {
        public List<int> list_of_number = new List<int>();

        static void Main(string[] args)
        {
            string start_directory = "";

            while (Directory.Exists(start_directory) == false)
            {
                Console.WriteLine("Введите каталог откуда начинать\n");

                start_directory = Console.ReadLine();
            }

            string[] all_directory = Directory.GetDirectories(start_directory);
            string[] file_list = Directory.GetFiles(start_directory);

            while (all_directory.Length != 0)
            {
                ParameterizedThreadStart parameter = new ParameterizedThreadStart(Flow.FlowRecord);

                Thread thread = new Thread(parameter);
                thread.Start(file_list);
                start_directory = all_directory[0];

                try
                {
                    file_list = Directory.GetFiles(start_directory);
                    all_directory = all_directory.Skip(1).ToArray();
                    all_directory = all_directory.Concat(Directory.GetDirectories(start_directory)).ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    Record.RecordInDB(Path.GetDirectoryName(start_directory), start_directory, "", "Не получилось получить доступ к " + start_directory);
                    all_directory = all_directory.Skip(1).ToArray();
                }
            }
            Console.WriteLine("Программа закончила работу");
            Console.ReadKey();
        }
    }
}

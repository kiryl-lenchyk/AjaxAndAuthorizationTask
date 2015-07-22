using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = new[]
            {"Ivan", "Petr", "Vasiliy", "Vladislav", "Aleksandr", "Oleg", "Iliya"};

            string[] surnames = new[] { "Ivanov", "Petrov", "Sidorov", "Tleckiy", "Vladimirov", "Aleksandrov", "Chromov" };

            string[][] positions = new[]
            {
                new[] {"Belarus", "Minsk"}, new[] {"Belarus", "Homel"}, new[] {"Belarus", "Vitebsk"},
                new[] {"Belarus", "Grodno"}, new[] {"USA", "Boston"}, new[] {"USA", "New-Your"},
                new[] {"USA", "Washington"}, new[] {"Russia", "Moskow"}, new[] {"Russia", "Kasan"}
            };

            var random = new Random();

            using (StreamWriter sw = new StreamWriter("json.json"))
            {
                sw.Write("[\r\n");
                for (int i = 0; i < 100; i++)
                {
                    string[] possition = positions[random.Next(positions.Length)];

                    sw.Write("\t{{" + "\r\n" +
                             "\t\t\"name\" : \"{0}\",\r\n" +
                             "\t\t\"surname\" : \"{1}\",\r\n" +
                             "\t\t\"age\" : {2},\r\n" +
                             "\t\t\"address\" : {{\"country\" : \"{3}\", \"city\" : \"{4}\" }},\r\n" +
                             "\t}}," +
                             "\r\n", names[random.Next(names.Length)],
                        surnames[random.Next(surnames.Length)], random.Next(20, 60), possition[0],
                        possition[1]);
                }
                sw.Write("]");
            }

        }
    }
}

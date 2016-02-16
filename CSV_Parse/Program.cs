using System;

namespace CSV_Parse
{
    class Program
    {
        static void Main()
        {
            string path = "file.csv";
            var separator = ';';

            var serializer = new CsvSerializer(separator);

            var movies = serializer.Read<Movie>(path);

            serializer.Write("newFile.csv", movies);

            Console.ReadKey();
        }
    }
}

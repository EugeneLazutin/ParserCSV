using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CSV_Parse
{
    public class CsvSerializer
    {
        private char _separator;

        public char Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        public CsvSerializer(char separator)
        {
            _separator = separator;
        }

        public T[] Read<T>(string path) where T : new()
        {
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                return Serialize<T>(lines);
            }
            throw new FileNotFoundException(path);
        }

        public T[] Serialize<T>(string[] lines) where T : new()
        {
            string[] title = lines[0].Split(_separator);
            var collection = CreateCollection<T>(lines.Length - 1);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] rowItems = lines[i].Split(_separator);
                for (int j = 0; j < rowItems.Length; j++)
                {
                    PropertyInfo info = typeof(T).GetProperty(title[j]);
                    info.SetValue(collection[i - 1], Convert.ChangeType(rowItems[j], info.PropertyType));
                }
            }

            return collection;
        }

        private T[] CreateCollection<T>(int length) where T : new()
        {
            var collection = new T[length];
            for (int i = 0; i < length; i++)
            {
                collection[i] = new T();
            }
            return collection;
        }

        public void Write<T>(string path, T[] collection) where T : new()
        {
            var lines = Deserialize(collection);
            File.WriteAllLines(path,lines);
        }

        public string[] Deserialize<T>(T[] collection) where T : new()
        {
            var title = CreateTitle(typeof(T));
            Type type = typeof (T);

            var index = 0;
            var lines = new string[collection.Length + 1];
            lines[index++] = string.Join(_separator.ToString(), title);

            foreach (var item in collection)
            {
                string[] line = new string[title.Length];
                for (int i = 0; i < title.Length; i++)
                {
                    line[i] = type.GetProperty(title[i]).GetValue(item).ToString();
                }
                lines[index++] = string.Join(_separator.ToString(), line);
            }

            return lines;
        }

        private string[] CreateTitle(Type type)
        {
            var properties = type.GetProperties().Where(prop => prop.IsDefined(typeof(CsvAttribute), false)).ToArray();
            var title = new string[properties.Length];

            foreach (var propertyInfo in properties)
            {
                var attr = (CsvAttribute)propertyInfo.GetCustomAttribute(typeof(CsvAttribute));
                try
                {
                    title[attr.Index] = propertyInfo.Name;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Incorrectly defined indexes!");
                    throw;
                }
            }

            return title;
        }
    }
}

using System;

namespace CSV_Parse
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvAttribute : Attribute
    {
        public int Index { get; private set; }

        public CsvAttribute(int index)
        {
            Index = index;
        }
    }
}

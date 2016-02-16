namespace CSV_Parse
{
    public class Movie
    {
        public Movie(){}

        public Movie(string title, int year, string genre)
        {
            Title = title;
            Year = year;
            Genre = genre;
        }

        [Csv(1)]
        public string Title { get; set; }
        [Csv(2)]
        public int Year { get; set; }
        [Csv(0)]
        public string Genre { get; set; }
    }
}

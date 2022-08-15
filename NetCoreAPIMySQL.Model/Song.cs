
namespace NetCoreAPIMySQL.Model
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string Year { get; set; }
        public string Gender { get; set; }
        public int Id_favorite { get; set; }
    }
}

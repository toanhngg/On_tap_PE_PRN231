using System.Text.Json.Serialization;

namespace Q1_PE3.DTO
{
    public class DirectorDTO
    {

        [JsonIgnore]
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool Male { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; } = null!;
        public string Description { get; set; } = null!;

    }
}

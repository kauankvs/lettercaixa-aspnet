using Newtonsoft.Json;

namespace LettercaixaAPI.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? PosterPath { get; set; }
    }
}

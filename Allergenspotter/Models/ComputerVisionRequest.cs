using Newtonsoft.Json;

namespace Allergenspotter.Models
{
    public class ComputerVisionRequest
    {
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
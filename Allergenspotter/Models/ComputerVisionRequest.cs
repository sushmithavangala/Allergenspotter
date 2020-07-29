using Newtonsoft.Json;

namespace Allergenspotter.Models
{
    public class ComputerVisionRequest
    {
        [JsonProperty("base64Image")]
        public string Base64Image { get; set; }
    }
}
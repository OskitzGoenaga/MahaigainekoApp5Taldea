using System.Text.Json.Serialization;

namespace MahaigainekoAppTransakzioak.Models
{
    public class PistaModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("izena")]
        public string Izena { get; set; }

        [JsonPropertyName("kirola")]
        public string Kirola { get; set; }
    }
}
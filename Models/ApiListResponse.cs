using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MahaigainekoAppTransakzioak.Models
{
    public class ApiListResponse<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        // Mapeamos la propiedad "dataList" que devuelve tu backend
        [JsonPropertyName("dataList")]
        public List<T> DataList { get; set; }
    }
}
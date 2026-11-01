using MahaigainekoAppTransakzioak.Dto;
using MahaigainekoAppTransakzioak.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MahaigainekoAppTransakzioak
{
    public static class ApiClient
    {
        private static readonly HttpClient http = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5126/")
        };

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<List<PistaModel>> GetPistakAktiboakAsync()
        {
            var response = await http.GetAsync("api/Pistak");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiListResponse<PistaModel>>(json, options);
            return result?.DataList ?? new List<PistaModel>();
        }

        public static async Task<List<PistaModel>> GetPistakEzabatuakAsync()
        {
            var response = await http.GetAsync("api/Pistak/ezabatuak");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiListResponse<PistaModel>>(json, options);
            return result?.DataList ?? new List<PistaModel>();
        }

        public static async Task<List<BazkideModel>> GetBazkideakAsync()
        {
            var response = await http.GetAsync("api/Bazkideak");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiListResponse<BazkideModel>>(json, options);
            return result?.DataList ?? new List<BazkideModel>();
        }

        public static async Task<HttpResponseMessage> PostErreserbaAsync(ErreserbaCreateDto dto)
        {
            string json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await http.PostAsync("api/Erreserbak", content);
        }

        public static async Task<HttpResponseMessage> SoftDeletePistaAsync(int id)
            => await http.PutAsync($"api/Pistak/softDelete/{id}", null);

        // RUTA CLAVE: Cambiado 'restore' a 'berreskuratu' para coincidir con la API
        public static async Task<HttpResponseMessage> RestorePistaAsync(int id)
            => await http.PutAsync($"api/Pistak/berreskuratu/{id}", null);

        public static async Task<HttpResponseMessage> HardDeletePistaAsync(int id)
            => await http.DeleteAsync($"api/Pistak/hardDelete/{id}");
    }
}
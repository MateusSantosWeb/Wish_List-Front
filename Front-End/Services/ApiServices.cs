using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WishListWeb.Models;

namespace WishListWeb.Services
{
    public class ApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private void SetAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ===== AUTH ENDPOINTS =====
        
        public async Task<ApiResponse<LoginResponse>> Login(LoginViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                Email = model.Email, 
                Senha = model.Senha 
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/auth/login", content);
            
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), _jsonOptions);
                return new ApiResponse<LoginResponse> { Success = true, Data = result };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<LoginResponse> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<string>> Register(RegisterViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                Email = model.Email, 
                Senha = model.Senha,
                Nome = model.Nome,
                Role = model.Role,
                CodigoNamorada = model.CodigoNamorada
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/auth/register", content);
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Cadastro realizado com sucesso!" };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }

        // ===== WISH ENDPOINTS =====
        
        public async Task<List<WishViewModel>> GetWishes()
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/wish");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<WishViewModel>>(content, _jsonOptions) ?? new List<WishViewModel>();
            }
            
            return new List<WishViewModel>();
        }

        public async Task<WishViewModel?> GetWish(int id)
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/wish/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WishViewModel>(content, _jsonOptions);
            }
            
            return null;
        }

        public async Task<ApiResponse<WishViewModel>> CreateWish(WishViewModel model)
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Categoria = model.Categoria,
                Prioridade = model.Prioridade,
                Link = model.Link,
                ImagemUrl = model.ImagemUrl
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/wish", content);
            
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<WishViewModel>(await response.Content.ReadAsStringAsync(), _jsonOptions);
                return new ApiResponse<WishViewModel> { Success = true, Data = result };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<WishViewModel> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<WishViewModel>> UpdateWish(int id, WishViewModel model)
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Categoria = model.Categoria,
                Prioridade = model.Prioridade,
                Link = model.Link,
                ImagemUrl = model.ImagemUrl
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/wish/{id}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<WishViewModel>(await response.Content.ReadAsStringAsync(), _jsonOptions);
                return new ApiResponse<WishViewModel> { Success = true, Data = result };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<WishViewModel> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<string>> UpdateWishStatus(int id, string status)
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                Status = status
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/wish/{id}/status", content);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Status atualizado com sucesso!" };
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<string>> DeleteWish(int id)
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/wish/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Wish removido com sucesso!" };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }

        // ===== PROGRESS ENDPOINTS (para o namorado) =====
        
        public async Task<List<ProgressViewModel>> GetAllProgress()
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/progress");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProgressViewModel>>(content, _jsonOptions) ?? new List<ProgressViewModel>();
            }
            
            return new List<ProgressViewModel>();
        }

        public async Task<ApiResponse<string>> IniciarRealizacao(int wishId)
        {
            SetAuthHeader();
            var response = await _httpClient.PutAsync($"api/progress/{wishId}/iniciar", null);
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Marcado como 'Vou Realizar'!" };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<string>> MarcarRealizando(int wishId, string notaPrivada)
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                NotaPrivada = notaPrivada 
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/progress/{wishId}/realizando", content);
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Status atualizado para 'Realizando'!" };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }

        public async Task<ApiResponse<string>> RealizarWish(int wishId, string notaRealizacao, string? fotosRealizacao)
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(new 
            { 
                NotaRealizacao = notaRealizacao,
                FotosRealizacao = fotosRealizacao
            }), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/progress/{wishId}/realizar", content);
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<string> { Success = true, Message = "Desejo realizado! Ela pode ver agora 💙" };
            }
            
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<string> { Success = false, Message = errorMessage };
        }
    }
}

namespace WishListWeb.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
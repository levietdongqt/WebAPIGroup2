using System.Net;

namespace WebAPIGroup2.Models.DTO
{
    public class ResponseDTO<T>
    {
        public ResponseDTO()
        {

        }
        public ResponseDTO(HttpStatusCode status, string? message, string? token, T data)
        {
            Status = status;
            Message = message;
            Token = token;
            this.data = data;
        }

        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }

        public T? data { get; set; }
    }
}

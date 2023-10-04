using System;

namespace WealthBridge.Core.Models
{

    public class ApiClientResponse<T>
    {
        public int StatusCode { get; set; }
        
        public T Response { get; set; }
        public ApiClientResponseError Error { get; set; }
    }
}

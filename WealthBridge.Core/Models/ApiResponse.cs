using System;
using System.Runtime.Serialization;

namespace WealthBridge.Core.Models
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember(Name="errorCode")]
        public int ErrorCode { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "data")]
        public object Data { get; set; }
    }
}

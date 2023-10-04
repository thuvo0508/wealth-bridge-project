using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WealthBridge.Core.Models
{
    [DataContract]
    public class ApiClientResponseError
    {
        [DataMember(Name= "error_description")]
        public string Description { get; set; }
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }
}

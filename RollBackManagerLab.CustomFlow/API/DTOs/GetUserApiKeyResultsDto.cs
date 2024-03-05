using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class GetUserApiKeyResultsDto
    {

        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name = "errorCode")]
        public int ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "result")]
        public List<ApiKeyDto> Result { get; set; }
    }
}

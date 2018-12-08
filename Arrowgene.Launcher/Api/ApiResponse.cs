using System.Runtime.Serialization;

namespace Arrowgene.Launcher.Api
{
    [DataContract]
    public class Response<T>
    {
        [DataMember(Name = "isArrowgeneApi")]
        public bool IsArrowgeneApi { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "isError")]
        public bool IsError { get; set; }
        [DataMember(Name = "content")]
        public T Content { get; set; }
    }
}

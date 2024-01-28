using System.Text.Json.Serialization;

namespace Cube.Application.Services
{
    public class Response<TResult, TEnum> 
        where TResult : class
        where TEnum : Enum
    {
        public TResult Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TEnum ResponseResult { get; set; }

        public List<string> Messages { get; set; }
    }
}

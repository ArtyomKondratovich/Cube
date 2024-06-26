﻿using System.Text.Json.Serialization;

namespace Cube.Business.Services
{
    public class Response<TResult, TEnum>
        where TEnum : Enum
    {
        public TResult Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TEnum ResponseResult { get; set; }

        public List<string> Messages { get; set; } = new();
    }
}

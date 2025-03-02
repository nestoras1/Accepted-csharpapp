using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos
{
    public sealed class AuthResponseDto
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
    }
}

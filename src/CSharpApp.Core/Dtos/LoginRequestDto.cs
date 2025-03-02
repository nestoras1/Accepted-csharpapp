using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos
{
    public sealed class LoginRequestDto
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}

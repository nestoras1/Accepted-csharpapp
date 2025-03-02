using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos
{
    public sealed class CreateCategoryDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("image")]
        public required string image { get; set; }
    }
}

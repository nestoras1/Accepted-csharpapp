using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos
{
    public sealed class CreateProductDto
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("price")]
        public required int Price { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("categoryId")]
        public required int CategoryId { get; set; }

        [JsonPropertyName("images")]
        public required List<string> Images { get; set; }
    }
}

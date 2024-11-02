using System;
using System.Collections.Generic;

namespace Q1_PE1.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
    }
}

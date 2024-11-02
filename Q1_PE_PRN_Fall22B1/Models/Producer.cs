using System;
using System.Collections.Generic;

namespace Q1_PE_PRN_Fall22B1.Models
{
    public partial class Producer
    {
        public Producer()
        {
            Movies = new HashSet<Movie>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; }
    }
}

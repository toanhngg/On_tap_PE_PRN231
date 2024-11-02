using System;
using System.Collections.Generic;

namespace Q1_SpringB1_Script.Models
{
    public partial class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string? Role { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}

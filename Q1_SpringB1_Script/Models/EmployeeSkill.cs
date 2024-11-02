using System;
using System.Collections.Generic;

namespace Q1_SpringB1_Script.Models
{
    public partial class EmployeeSkill
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
        public string? ProficiencyLevel { get; set; }
        public DateTime? AcquiredDate { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Skill Skill { get; set; } = null!;
    }
}

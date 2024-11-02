﻿using System;
using System.Collections.Generic;

namespace Q1_SpringB1_Script.Models
{
    public partial class Skill
    {
        public Skill()
        {
            EmployeeSkills = new HashSet<EmployeeSkill>();
        }

        public int SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    }
}

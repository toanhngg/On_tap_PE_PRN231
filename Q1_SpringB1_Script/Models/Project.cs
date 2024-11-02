using System;
using System.Collections.Generic;

namespace Q1_SpringB1_Script.Models
{
    public partial class Project
    {
        public Project()
        {
            EmployeeProjects = new HashSet<EmployeeProject>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}

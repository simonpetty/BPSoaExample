using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLT.BenefitCosts.Messages.Events
{
    public class EmployeeStartedEmployment
    {
        public Guid EmployeeId { get; set; }

        public Guid CompanyId { get; set; }
    }
}

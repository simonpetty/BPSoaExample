using System;

namespace JLT.PensionsEntitlement.Messages.Events
{
    public class EmployeeStartedEmployment
    {
        public Guid EmployeeId { get; set; }

        public Guid CompanyId { get; set; }
    }
}

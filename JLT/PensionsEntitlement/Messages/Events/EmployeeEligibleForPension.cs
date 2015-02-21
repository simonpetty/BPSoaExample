using System;
using System.Collections.Generic;

namespace JLT.PensionsEntitlement.Messages.Events
{
    public class EmployeeEligibleForPension
    {
        public Guid EmployeeId { get; set; }

        public Guid PensionProfileId { get; set; }

        public List<Guid> PensionSchemeIds { get; set; }
    }
}

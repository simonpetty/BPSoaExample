﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLT.BenefitCosts.Messages.Commands
{
    public class CreateTestData
    {
        public Guid EmployeeId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CoverLevelId { get; set; }

        public Guid BenefitSchemeId { get; set; }

        public string CostFormula { get; set; }
    }
}

namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseService : IBaseService
    {
        public bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            // Non-existing parameter in the URL
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            bool isGuidValid = Guid.TryParse(id, out parsedGuid);

            // Invalid parameter in the URL
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}

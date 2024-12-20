﻿namespace DentalManagementSystem.Services.Data
{
    using System;

    using DentalManagementSystem.Services.Data.Interfaces;

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

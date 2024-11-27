namespace DentalManagementSystem.Services.Data.Interfaces
{
    using System;

    public interface IBaseService
    {
        bool IsGuidValid(string? id, ref Guid parsedGuid);
    }
}

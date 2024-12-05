namespace DentalManagementSystem.Web.ViewModels.Procedure
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Mapping;

    public class ProcedureDetailsViewModel : IMapFrom<Procedure>
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;
    }
}

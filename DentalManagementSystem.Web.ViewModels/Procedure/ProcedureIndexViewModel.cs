namespace DentalManagementSystem.Web.ViewModels.Procedure
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Mapping;

    public class ProcedureIndexViewModel : IMapFrom<Procedure>
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}

namespace DentalManagementSystem.Web.ViewModels.Procedure
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Mapping;

    public class ProcedureIndexViewModel : IMapFrom<Procedure>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}

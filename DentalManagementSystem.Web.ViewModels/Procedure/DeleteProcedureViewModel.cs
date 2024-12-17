namespace DentalManagementSystem.Web.ViewModels.Procedure
{
    public class DeleteProcedureViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;
    }
}
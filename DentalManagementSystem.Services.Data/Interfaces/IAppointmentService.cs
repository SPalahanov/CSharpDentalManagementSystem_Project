namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Appointment;

    public interface IAppointmentService
    {
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync();

        Task<CreateAppointmentViewModel> GetCreateAppointmentModelAsync();
        Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProceduresAsync();
        Task<bool> CreateAppointmentAsync(CreateAppointmentViewModel model);

        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id);
    }
}

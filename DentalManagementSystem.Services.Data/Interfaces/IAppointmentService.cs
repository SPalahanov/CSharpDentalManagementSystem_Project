namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Appointment;

    public interface IAppointmentService
    {
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync(AllAppointmentsFilterViewModel inputModel);
        Task<int> GetAppointmentsCountByFilterAsync(AllAppointmentsFilterViewModel inputModel);
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByPatientIdAsync(Guid patientId);
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByDentistIdAsync(Guid patientId);

        Task<CreateAppointmentViewModel> GetCreateAppointmentModelAsync();
        Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProceduresAsync();
        Task<bool> CreateAppointmentAsync(CreateAppointmentViewModel model);

        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id);

        Task<EditAppointmentFormModel?> GetAppointmentDataForEditAsync(Guid id);
        Task<IEnumerable<PatientAppointmentViewModel>> GetPatientListAsync();
        Task<IEnumerable<DentistAppointmentViewModel>> GetDentistListAsync();
        Task<IEnumerable<AppointmentTypeViewModel>> GetAppointmentTypeListAsync();
        Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProcedureListAsync();
        Task<EditAppointmentFormModel?> GetAppointmentForEditByIdAsync(Guid id);
        Task<bool> EditAppointmentAsync(EditAppointmentFormModel model);

        Task<DeleteAppointmentViewModel?> GetAppointmentForDeleteByIdAsync(Guid id);
        Task<bool> SoftDeleteAppointmentAsync(Guid id);
    }
}

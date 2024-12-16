namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Appointment;

    public interface IAppointmentService
    {
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync(AllAppointmentsFilterViewModel inputModel);

        Task<int> GetAppointmentsCountByFilterAsync(AllAppointmentsFilterViewModel inputModel);

        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByPatientIdAsync(Guid patientId, int currentPage, int entitiesPerPage);

        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByDentistIdAsync(Guid dentistId, int currentPage, int entitiesPerPage);

        Task<CreateAppointmentViewModel> GetCreateAppointmentModelAsync(string userId, bool isPatient, bool isDentist);

        Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProceduresAsync();

        Task<bool> CreateAppointmentAsync(CreateAppointmentViewModel model);

        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id);

        Task<IEnumerable<PatientAppointmentViewModel>> GetPatientListAsync();

        Task<IEnumerable<DentistAppointmentViewModel>> GetDentistListAsync();

        Task<IEnumerable<AppointmentTypeViewModel>> GetAppointmentTypeListAsync();

        Task<EditAppointmentFormModel?> GetAppointmentForEditByIdAsync(Guid id);

        Task<bool> EditAppointmentAsync(EditAppointmentFormModel model);

        Task<DeleteAppointmentViewModel?> GetAppointmentForDeleteByIdAsync(Guid id);

        Task<bool> SoftDeleteAppointmentAsync(Guid id);
    }
}

namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Common.Enums;
    using DentalManagementSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllAppointmentsIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string AppointmentDate { get; set; } = null!;

        public string AppointmentStatus { get; set; } = null!;
    }
}

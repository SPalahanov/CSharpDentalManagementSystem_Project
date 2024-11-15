﻿namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PatientAppointmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
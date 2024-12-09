﻿namespace DentalManagementSystem.Web.ViewModels.Patient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllPatientsSearchViewModel
    {
        public IEnumerable<AllPatientsIndexViewModel>? Patients { get; set; }

        public string? SearchQuery { get; set; }

        public int? CurrentPage { get; set; } = 1;

        public int? EntitiesPerPage { get; set; } = 7;

        public int? TotalPages { get; set; }
    }
}

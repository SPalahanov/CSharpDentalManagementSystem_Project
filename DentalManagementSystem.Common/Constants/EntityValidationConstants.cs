using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalManagementSystem.Common.Constants
{
    public static class EntityValidationConstants
    {
        public class Procedure
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;
        }

        public class AppointmentType
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public class Dentist
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 40;

            public const int PhoneNumberMinLength = 7;
            public const int PhoneNumberMaxLength = 15;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 70;

            public const int LicenseNumberMinLength = 5;
            public const int LicenseNumberMaxLength = 20;
        }

        public class Patient
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 40;

            public const int PhoneNumberMinLength = 7;
            public const int PhoneNumberMaxLength = 15;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 70;
        }

        public class Prescription
        {
            public const int MedicationNameMinLength = 2;
            public const int MedicationNameMaxLength = 30;
        }
    }
}

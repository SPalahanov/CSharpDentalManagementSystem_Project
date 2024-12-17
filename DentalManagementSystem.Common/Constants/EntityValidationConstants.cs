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
            public const int IdMinLength = 36;
            public const int IdMaxLength = 36;

            public const int NameMinLength = 2;
            public const int NameMaxLength = 40;

            public const int PhoneNumberMinLength = 7;
            public const int PhoneNumberMaxLength = 15;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 70;

            public const string DateOfBirthFormat = "dd/MM/yyyy";

            public const int LicenseNumberMinLength = 9;
            public const int LicenseNumberMaxLength = 9;
            public const string LicenseNumberRegex = "^[A-Z]{2}-\\d{3}-[A-Z]{2}$";
        }

        public class Patient
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 40;

            public const int PhoneNumberMinLength = 7;
            public const int PhoneNumberMaxLength = 15;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 70;

            public const string DateOfBirthFormat = "dd/MM/yyyy";
        }

        public class Appointment 
        {
            public const int IdMinLength = 36;
            public const int IdMaxLength = 36;

            public const string YearFilterRangeRegex = "^(\\d{4})\\s*\\-\\s*(\\d{4})$";
        }

        public class AppointmentProcedures
        {
            public const int IdMinLength = 36;
            public const int IdMaxLength = 36;
        }

        public class Prescription
        {
            public const int IdMinLength = 36;
            public const int IdMaxLength = 36;

            public const int MedicationNameMinLength = 2;
            public const int MedicationNameMaxLength = 45;

            public const int MedicationDescriptionMinLength = 2;
            public const int MedicationDescriptionMaxLength = 350;
        }
    }
}

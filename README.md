# C# Dental Management System

This is a **Dental Clinic Management System** project built using **.NET Core MVC**. The system is designed to simplify the management of dental clinic operations, including handling patient information, dentist schedules, appointments, and procedures. It provides functionalities for different roles, such as managers and receptionists, as well as options for patients and dentists.

## Features

- **Patient Management:** Add, view, update, and manage patient information.
- **Dentist Management:** Register dentists, manage their schedules, and assign them to appointments.
- **Appointment Scheduling:**
  - Fixed time slots for dentist schedules (e.g., 6 slots per day).
  - Patients can select available appointment times.
- **Role Management:**
  - Separate functionalities for managers, receptionists, dentists, and patients.
  - "Become a Dentist" feature for users to apply as dentists via a form.
- **Procedure Management:** Manage dental procedures associated with appointments.
- **User-Friendly Interface:** Simplified navigation with clear separation of roles and functionalities.


## Project Structure

The project follows a clean architecture with the following key components:

- **Controllers:** Handle HTTP requests and responses.
- **Services:** Implement business logic and interact with repositories.
- **Repositories:** Abstract database operations.
- **View Models:** Simplify data exchange between views and controllers.
- **Views:** HTML and Razor pages for the frontend.

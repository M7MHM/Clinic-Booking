namespace Clinic.Notification.Api.Messaging
{
    public class AppointmentCreatedMessage
    {
        public Guid AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
    }
}

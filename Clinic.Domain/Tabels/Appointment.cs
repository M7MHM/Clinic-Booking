using Clinic.Domain.Enum;

public class Appointment : CommonBase
{
    public Guid DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public DateTime AppointmentDate { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public string? Notes { get; private set; }
    private Appointment() { }

    public Appointment( Guid doctorId,Guid patientId,string title,DateTime appointmentDate,string? notes)
    {
        Id = Guid.NewGuid();
        DoctorId = doctorId;
        PatientId = patientId;
        Title = title;
        AppointmentDate = appointmentDate;
        Notes = notes;
        Status = AppointmentStatus.Pending;
    }

    public void Confirm()
    {
        Status = AppointmentStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = AppointmentStatus.Cancelled;
    }

    public void Complete()
    {
        Status = AppointmentStatus.Completed;
    }
}
using Clinic.Domain.Tables;

namespace Clinic.Domain.Tables;
public class Doctor : CommonBase
{
    public string Name { get; private set; } = string.Empty;
    public int Age { get; private set; }
    public string Specialization { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public List<Appointment> Appointments { get; private set; } = new();

    private Doctor() { }

    public Doctor(string name, int age,  string specialization, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
        Specialization = specialization;
        Email = email;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
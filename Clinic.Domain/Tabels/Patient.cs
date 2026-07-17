public class Patient : CommonBase
{
    public string Name { get; private set; } = string.Empty;

    public int Age { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public List<Appointment> Appointments { get; private set; } = new();

    private Patient() { }

    public Patient(string name,int age,string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
        Email = email;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
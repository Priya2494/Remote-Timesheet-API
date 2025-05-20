namespace TimesheetAPI.Models
{
    public class TimesheetEntry
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double HoursWorked { get; set; }
        public string? Description { get; set; }
    }
}

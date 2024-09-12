namespace PayrollManager
{

abstract class Employee(int ID, FullName name, GenderType gender)
{
    public int ID { get; } = ID;

    public FullName Name { get; set; } = name;

    public GenderType Gender { get; set; } = gender;

}

class HourlyEmployee(int ID, FullName name, GenderType gender,double rate) : Employee(ID, name, gender)
{
    public double Rate { get; set; } = rate;

    public double GetDayPay(Workday day) => Rate * day.RegularHours + Rate * 1.5 * day.OvertimeHours;

    public double GetWeekPay(WorkWeek week)
    {
        double res = 0;
        foreach (Workday day in week.Days)
            res += Rate * day.RegularHours + Rate * 1.5 * day.OvertimeHours;
        return res;
    }
}
}

namespace PayrollManager
{
    enum GenderType
{
    Male,
    Female,
    Other,
    Unspecified
}

enum PayPeriod 
{ 
    Weekly, 
    Biweekly, 
    Semimonthly, 
    Monthly 
}

enum DayType
{
    Monday = 0, Mon = Monday, M = Monday,
    Tuesday = 1, Tue = Tuesday, Tu = Tuesday,
    Wednesday = 2, Wed = Wednesday, W = Wednesday,
    Thursday = 3, Thu = Thursday, Th = Thursday,
    Friday = 4, Fri = Friday, F = Friday,
    Saturday = 5, Sat = Saturday, Sa = Saturday,
    Sunday = 6, Sun = Sunday, Su = Sunday,
    Any = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
    None
}
}
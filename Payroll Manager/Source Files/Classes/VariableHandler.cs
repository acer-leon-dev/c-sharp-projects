namespace PayrollManager
{
class VariableHandler : Dictionary<string, object>
{
    public void Delete(string name) => Remove(name);

    public void ShowVariables()
    {
        Console.WriteLine("----------");
        
        Console.WriteLine("Variables:");
        foreach (var variable in this)
            Console.WriteLine($"    {variable.Key} = {variable.Value}");

        Console.WriteLine("----------");
    }
}
}
using PayrollManager;

namespace PayrollManager
{
class FullName
{
    public FullName(string first, string last)
    {
        FirstName = first;
        MiddleNames = [];
        LastName = last;
    }

    public FullName(string first, string middle, string last)
    {
        FirstName = first;
        MiddleNames = [middle];
        LastName = last;
    }

    public FullName(params string[] names)
    {
        FirstName = names[0];
        MiddleNames = names[1..(names.Length-1)];
        LastName = names.Last();
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string[] MiddleNames { get; set; }

    private readonly Exception NonexistentNameException = new($"Exception: number of name must be greater than 0.");

    private readonly IndexOutOfRangeException NameOutOfRangeException = new($"Exception: number of name must be greater than 0.");

    public string GetMiddleName(int number)
    {
        if (number < 1)
            throw NonexistentNameException;
        
        if (number > MiddleNames.Length)
            throw NameOutOfRangeException;

        return MiddleNames[number - 1];
    }

    public void ChangeMiddleName(string name, int number)
    {   
        if (number < 1)
            throw NonexistentNameException;
        
        if (number > MiddleNames.Length)
            throw NameOutOfRangeException;
            
        MiddleNames[number - 1] = name;
    }

    public void InsertMiddleName(string name, int number)
    {   
        if (number < 1)
            throw NonexistentNameException;
        
        List<string> middlenamesList = new(MiddleNames); 
        
        if (number > MiddleNames.Length)
            middlenamesList.Add(name);
        else
            middlenamesList.Insert(number - 1, name);
        MiddleNames = [.. middlenamesList];
    }

    public void RemoveMiddleName(int number)
    {   
    if (number < 1)
        throw NonexistentNameException;

    if (number > MiddleNames.Length)
            throw NameOutOfRangeException;

    List<string> middlenamesList = new(MiddleNames);
    middlenamesList.RemoveAt(number - 1); 
    MiddleNames = [.. middlenamesList];
    }
}
}
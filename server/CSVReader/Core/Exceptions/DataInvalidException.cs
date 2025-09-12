namespace Core.Exceptions;

public class DataInvalidException : Exception
{
    public DataInvalidException() : base("Inpud data is not valid.") { }
    public DataInvalidException(string message) : base("Inpud data is not valid. " + message) { }
}

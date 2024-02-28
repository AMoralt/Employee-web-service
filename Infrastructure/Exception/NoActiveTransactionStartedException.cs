
public class NoActiveTransactionStartedException : System.Exception
{
    public NoActiveTransactionStartedException() : base("No active transaction started")
    {
    }
}
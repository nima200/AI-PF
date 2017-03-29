/// <summary>
/// Custom class to create a tuple.
/// </summary>
/// <typeparam name="TA">Type of first part of tuple.</typeparam>
/// <typeparam name="TB">Type of second part of tuple.</typeparam>
public class Pair<TA, TB>
{
    public TA AValue { get; set; }
    public TB BValue { get; set; }

    public Pair(TA aValue, TB bValue)
    {
        AValue = aValue;
        BValue = bValue;
    }
}
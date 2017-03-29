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
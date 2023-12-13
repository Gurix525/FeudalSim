public class EventParameter
{
    public string Name { get; set; }
    public object Value { get; set; }

    public EventParameter(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public static implicit operator EventParameter((string, object) tuple)
    {
        return new(tuple.Item1, tuple.Item2);
    }
}
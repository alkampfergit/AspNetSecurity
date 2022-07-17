namespace AspNetSecurity.Core.SqlHelpers
{
    internal class OutputParameter
    {
        public object? Value { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public OutputParameter(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}

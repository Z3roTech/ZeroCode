namespace ZeroCode.Tests.Moq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method |
                AttributeTargets.Property | AttributeTargets.Struct)]
public class TestingSingleAttribute(string? value) : Attribute
{
    public string? Value { get; } = value;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method |
                AttributeTargets.Property | AttributeTargets.Struct, AllowMultiple = true)]
public class TestingMultipleAttribute(int id) : Attribute
{
    public int Id { get; } = id;
}
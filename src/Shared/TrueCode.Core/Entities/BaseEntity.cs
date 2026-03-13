namespace TrueCode.Core.Entities;

public abstract class BaseEntity : Entity
{
    public Guid Id { get; set; }
}

public abstract class Entity { }
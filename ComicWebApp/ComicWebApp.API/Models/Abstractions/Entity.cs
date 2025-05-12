namespace ComicWebApp.API.Models.Abstractions;

public abstract class Entity
{
    public Guid Id { get; init; }
    protected Entity() { } // ctor for EF Core 
    protected Entity(Guid id)
    {
        Id = id;
    }
}

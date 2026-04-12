namespace HPT.SharedKernel.Abstractions;

public interface IEntity { 
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; } 
    public void AddDomainEvent(IDomainEvent @event); 
    void ClearDomainEvents(); 
}

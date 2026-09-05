namespace Garden
{
    public interface IDependentEntity
    {
        IEntityData HostEntity { get; }
    }
}
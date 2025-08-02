namespace VideoService.Core.Abstractions
{
    public abstract class Entity<T> : IEntity<T>
    {
        public required T Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }   
        public DateTime? DeletedAt { get; set; }
    }
}

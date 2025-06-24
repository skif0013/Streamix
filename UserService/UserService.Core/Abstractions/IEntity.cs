namespace sugar.Core.Abstractions
{
    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }

    public interface IEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
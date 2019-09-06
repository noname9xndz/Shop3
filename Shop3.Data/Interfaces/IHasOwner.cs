namespace Shop3.Data.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { set; get; }
    }
}

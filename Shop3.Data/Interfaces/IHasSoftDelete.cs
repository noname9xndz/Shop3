namespace Shop3.Data.Interfaces
{
    public interface IHasSoftDelete
    {
        bool IsDeleteted { set; get; } // được xóa hay chưa
    }
}






namespace Shop3.Infrastructure.SharedKernel
{
    // class base để các Entity khác kế thừa viết dưới dạng generic
    public abstract class DomainEntity<T> // những đối tượng dùng chung cho toàn project thì nên để abstract class
    {// id là 1 trường bắt buộc entity nào cũng phải có
        public T Id { get; set; }

        public bool IsTransient()
        { // check xem id có bằng với giá trị mặc định của T hay ko, true nếu domain entity được xét tự động tăng r
            return Id.Equals(default(T));
        }
    }
}
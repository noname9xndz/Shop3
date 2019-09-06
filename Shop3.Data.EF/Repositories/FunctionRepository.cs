using Shop3.Data.Entities;
using Shop3.Data.IRepositories;

namespace Shop3.Data.EF.Repositories
{
    // triển khai repository nếu dùng
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {

        }
    }
}

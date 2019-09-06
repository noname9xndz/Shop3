using Shop3.Infrastructure.Interfaces;

namespace Shop3.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public EFUnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        // save
        public void Commit()
        {
            _context.SaveChanges();
        }
        // giải phóng bộ nhớ
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
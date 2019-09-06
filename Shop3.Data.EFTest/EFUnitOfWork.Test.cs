using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Shop3.Data.EFTest
{

    public class EFUnitOfWorkTest
    {
        private readonly AppDbContext _context;

        public EFUnitOfWorkTest()
        {
            _context = ContextFactory.Create();
        }

        [Fact]
        public void Commit_Should_Success_When_Save_Data()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);
            EFUnitOfWork unitOfWork = new EFUnitOfWork(_context);
            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            unitOfWork.Commit();

            List<Function> functions = EFRepository.FindAll().ToList();
            Assert.Single(functions);
        }
    }
}


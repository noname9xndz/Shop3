using Shop3.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using Shop3.Infrastructure.Interfaces;
using Xunit;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using System.Linq;

namespace Shop3.Data.EFTest
{
    public class EFRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EFRepositoryTest()
        {
            _context = ContextFactory.Create();
            _context.Database.EnsureCreated();
            _unitOfWork = new EFUnitOfWork(_context);
            //db ảo nên có thể new thẳng UnitOfWork inject db ảo vào
        }

        [Fact]
        public void Constructor_Should_Success_When_Create_EFRepository()
        {
            EFRepository<Function, string> repository = new EFRepository<Function, string>(_context);
            Assert.NotNull(repository);
        }

        [Fact]
        public void Add_Should_Have_Record_When_Insert()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);
            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            _unitOfWork.Commit();

            Function function = EFRepository.FindById("USER");
            Assert.NotNull(function);

        }

        [Fact]
        public void FindAll_Should_Return_All_Record_In_Table()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);

            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            EFRepository.Add(new Function()
            {
                Id = "ROLE",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 2
            });
            _unitOfWork.Commit();

            List<Function> functions = EFRepository.FindAll().ToList();
            Assert.Equal(2, functions.Count);

        }

        [Fact]
        public void FindById_Should_Return_True_Record_In_Table()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);

            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            _unitOfWork.Commit();

            Function function = EFRepository.FindById("USER");
            Assert.Equal("Test", function.Name);

        }


        [Fact]
        public void Update_Should_Have_Change_Record()
        {

            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);
            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            _unitOfWork.Commit();

            Function function1 = EFRepository.FindById("USER");

            function1.Name = "Test2";
            EFRepository.Update(function1.Id,function1);
            _unitOfWork.Commit();

            Function function = EFRepository.FindById("USER");
            Assert.Equal("Test2", function.Name);
        }

        [Fact]
        public void Remove_Should_Success_When_Pass_Valid_Id()
        {

            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);
            Function function = new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            };
            EFRepository.Add(function);
            _unitOfWork.Commit();

            EFRepository.Remove(function);
            _unitOfWork.Commit();

            Function dbFunction = EFRepository.FindById("USER");
            Assert.Null(dbFunction);
        }



        [Fact]
        public void FindSingle_Should_Return_One_Record_If_Condition_Is_Match()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);

            Function function = new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            };
            EFRepository.Add(function);
            _unitOfWork.Commit();

            Function result = EFRepository.FindSingle(x => x.Name == "Test");
            Assert.NotNull(result);
        }


    }
}



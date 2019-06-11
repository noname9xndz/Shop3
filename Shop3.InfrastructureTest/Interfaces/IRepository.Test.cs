using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Xunit;

namespace Shop3.InfrastructureTest.Interfaces
{
    public class IRepositoryTest
    {
        private readonly Mock<IRepository<Blog, int>> _mockIRepository;
        private readonly Mock<Blog> _mockBlog;
        public IRepositoryTest()
        {
            _mockIRepository = new Mock<IRepository<Blog, int>>();
            _mockBlog = new Mock<Blog>();
        } 

        [Fact]
        public void Add_Entity_CreatEntity()
        {
          
            var entity = _mockIRepository.Setup(x => x.Add(_mockBlog.Object));

            Assert.NotNull(entity);
        }

        [Fact]
        public void Delete_Entity_DeleteEntitySuccsess()
        {
           
            var entity = _mockIRepository.Setup(x => x.Remove(_mockBlog.Object));

            Assert.NotNull(entity);
        }

        [Fact]
        public void Delete_EntityById_DeleteEntitySuccsess2()
        {

            var entity = _mockIRepository.Setup(x => x.Remove(_mockBlog.Object.Id));

            Assert.NotNull(entity);
        }

        // T FindById(K id, params Expression<Func<T, object>>[] includeProperties);  It.IsAny<Blog>().Id
        [Fact]
        public void Delete_EntityById_DeleteEntitySuccsess02()

        {
           

           _mockIRepository.Setup(x =>
                x.FindById(4, (It.IsAny<Expression<Func<Blog, object>>>())))
                .Returns(_mockBlog.Object);

          // Assert.Equal(_mockBlog.Object, _mockIRepository.Object.FindById(4));

        }

    }
}

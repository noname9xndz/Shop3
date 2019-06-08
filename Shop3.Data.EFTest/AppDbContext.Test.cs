using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shop3.Data.EF;
using Xunit;

namespace Shop3.Data.EFTest
{
    // sử dụng Db InMemory trong aspcore : nuget Microsoft.EntityFrameworkCore.InMemory
    public class AppDbContextTest
    {
        [Fact]
        public void Contructor_CreateInMemoryDb_Success()
        {
            //DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            //var context = new AppDbContext(options);

            var context = ContextFactory.Create();

            Assert.True(context.Database.EnsureCreated());
        }
    }
}

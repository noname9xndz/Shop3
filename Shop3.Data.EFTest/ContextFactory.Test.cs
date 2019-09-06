using Microsoft.EntityFrameworkCore;
using Shop3.Data.EF;
using System;

namespace Shop3.Data.EFTest
{
    // sử dụng Db InMemory trong aspcore : nuget Microsoft.EntityFrameworkCore.InMemory
    public class ContextFactory
    {
        // create db trên ram để test
        public static AppDbContext Create()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var context = new AppDbContext(options);

            return context;
        }

    }
}

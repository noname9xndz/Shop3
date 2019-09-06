using Moq;
using Shop3.Infrastructure.Interfaces;
using Xunit;

namespace Shop3.InfrastructureTest.Interfaces
{
    public class IUnitOfWorkTest
    {
        [Fact]
        public void Save_UnitOfWork_SaveEntity()
        {
            var entity = new Mock<IUnitOfWork>();

            var save = entity.Setup(x => x.Commit());

            Assert.NotNull(save);
        }
    }
}

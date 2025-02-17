using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AdminIvoire.Infrastructure.Tests.Persistence.Repository;

public class UnitOfWorkTests
{
    [Fact]
    public async Task GivenCommitAsync_WhenCall_ThenContextShouldSaveChangesAsync()
    {
        // Arrange
        var localiteContextMock = new Mock<LocaliteContext>(new DbContextOptionsBuilder<LocaliteContext>()
            .UseInMemoryDatabase(databaseName: nameof(GivenCommitAsync_WhenCall_ThenContextShouldSaveChangesAsync))
            .Options);
        var sut = new UnitOfWork(localiteContextMock.Object);

        // Act
        await sut.CommitAsync(CancellationToken.None);

        // Assert
        localiteContextMock.Verify(l => l.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}

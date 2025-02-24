using AdminIvoire.Application.Command;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;

namespace AdminIvoire.Architecture.Tests;

public class NamingConventionRules
{
    [Fact]
    public void ReadRepositories_ShouldEndWith_ReadRepository()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .ImplementInterface(typeof(ILocaliteReadRepository<>))
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.Persistence.Repository.Read")
            .And()
            .HaveNameEndingWith("ReadRepository")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void WriteRepositories_ShouldEndWith_WriteRepository()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .ImplementInterface(typeof(ILocaliteWriteRepository<>))
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.Persistence.Repository.Write")
            .And()
            .HaveNameEndingWith("WriteRepository")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void EntityTypeConfigurations_ShouldEndWith_EntityTypeConfiguration()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.Persistence.Configurations")
            .And()
            .HaveNameEndingWith("EntityTypeConfiguration")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DbContexts_ShouldEndWith_Context()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .AreClasses()
            .And()
            .Inherit(typeof(DbContext))
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.Persistence")
            .And()
            .HaveNameEndingWith("Context")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Factories_ShouldEndWith_Factory()
    {
        var result = Types.InAssembly(typeof(Domain.Entite.Commune).Assembly)
            .That()
            .AreClasses()
            .And()
            .ResideInNamespace("AdminIvoire.Domain.Factory")
            .Should()
            .HaveNameEndingWith("Factory")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Commands_ShouldEndWith_Command()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CommandHandlers_ShouldEndWith_CommandHandler()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CommandValidators_ShouldEndWith_Validator()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(IValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}

using NetArchTest.Rules;

namespace AdminIvoire.Architecture.Tests;

public class ArchitectureRulesTests
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(typeof(Domain.ValueObject.CoordonneesGeographiques).Assembly)
            .That()
            .ResideInNamespace("AdminIvoire.Domain")
            .ShouldNot()
            .HaveDependencyOn("AdminIvoire.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(typeof(Domain.ValueObject.CoordonneesGeographiques).Assembly)
            .That()
            .ResideInNamespace("AdminIvoire.Domain")
            .ShouldNot()
            .HaveDependencyOn("AdminIvoire.WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        var result = Types.InAssembly(typeof(Domain.ValueObject.CoordonneesGeographiques).Assembly)
            .That()
            .ResideInNamespace("AdminIvoire.Domain")
            .ShouldNot()
            .HaveDependencyOn("AdminIvoire.Application")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .ResideInNamespace("AdminIvoire.Application")
            .ShouldNot()
            .HaveDependencyOn("AdminIvoire.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .ResideInNamespace("AdminIvoire.Application")
            .ShouldNot()
            .HaveDependencyOn("AdminIvoire.WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ReadRepositories_ShouldHaveDependency_OnDomainLayer()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .HaveNameEndingWith("ReadRepository")
            .Should()
            .HaveDependencyOn("AdminIvoire.Domain")
            .And()
            .ImplementInterface(typeof(Domain.Repository.Read.ILocaliteReadRepository<>))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void WriteRepositories_ShouldHaveDependency_OnDomainLayer()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .HaveNameEndingWith("WriteRepository")
            .Should()
            .HaveDependencyOn("AdminIvoire.Domain")
            .And()
            .ImplementInterface(typeof(Domain.Repository.Write.ILocaliteWriteRepository<>))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Repository_ShouldBeInPersistenceNamespace()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.Persistence.Repository")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApiClient_ShouldBeInApiClientNamespaceAndDependOnApplicationLayer()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
            .That()
            .HaveNameEndingWith("ApiClient")
            .Should()
            .ResideInNamespace("AdminIvoire.Infrastructure.ApiClient")
            .And()
            .HaveDependencyOn("AdminIvoire.Application")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationServicesExtensions_ShouldBe_Static()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
             .That()
             .HaveNameEndingWith("ServicesExtensions")
             .Should()
             .BeStatic()
             .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void InfrastructureServicesExtensions_ShouldBe_Static()
    {
        var result = Types.InAssembly(typeof(Infrastructure.ServicesExtensions).Assembly)
             .That()
             .HaveNameEndingWith("ServicesExtensions")
             .Should()
             .BeStatic()
             .GetResult();

        Assert.True(result.IsSuccessful);
    }


    [Fact]
    public void ApplicationServices_ShouldBe_Sealed()
    {
        var result = Types.InAssembly(typeof(Application.ServicesExtensions).Assembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Service")
            .And()
            .ResideInNamespace("AdminIvoire.Application.Services")
            .Should()
            .BeSealed()
            .GetResult();
        Assert.True(result.IsSuccessful);
    }

}

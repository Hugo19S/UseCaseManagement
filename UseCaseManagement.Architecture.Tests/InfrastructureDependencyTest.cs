using NetArchTest.Rules;

namespace UseCaseManagement.Architecture.Tests;

public class InfrastructureDependencyTest
{
    private const string ApplicationNamespace = "UseCaseManagement.Application";
    //private const string InfrastructureNamespace = "UseCaseManagement.Infrastructure";
    private const string ServiceNamespace = "UseCaseManagement.Service";

    [Fact]
    public void Infrastructure_Should_Not_HaveDependecyOnOtherProjects()
    {
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        var otherProjects = new[]
        {
            ServiceNamespace
        };

        var testResult = Types.InAssembly(assembly)
            .ShouldNot().HaveDependencyOnAny(otherProjects)
            .GetResult();

        Assert.True(testResult.IsSuccessful);
    }

    [Fact]
    public void Repositories_Should_HaveDependecyOnOnAplication()
    {
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        var testResult = Types.InAssembly(assembly)
            .That().HaveNameEndingWith("Repository")
            .Should().HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        Assert.True(testResult.IsSuccessful);
    }
}

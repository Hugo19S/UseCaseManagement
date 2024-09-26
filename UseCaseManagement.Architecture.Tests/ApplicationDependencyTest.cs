using NetArchTest.Rules;

namespace UseCaseManagement.Architecture.Tests;

public class ApplicationDependencyTest
{
    //private const string ApplicationNamespace = "UseCaseManagement.Application";
    private const string DomainNamespace = "UseCaseManagement.Domain";
    private const string InfrastructureNamespace = "UseCaseManagement.Infrastructure";
    private const string ServiceNamespace = "UseCaseManagement.Service";

    [Fact]
    public void Application_Should_Not_HaveDependecyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Application.DependencyInjection).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            ServiceNamespace
        };

        //Act
        var testResult = Types.InAssembly(assembly)
            .ShouldNot().HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        Assert.True(testResult.IsSuccessful);
    }
    
    [Fact]
    public void Handler_Should_HaveDependecyOnOnDomain()
    {
        //Arrange
        var assembly = typeof(Application.DependencyInjection).Assembly;

        //Act
        var testResult = Types.InAssembly(assembly)
            .That().HaveNameEndingWith("Handler")
            .Should().HaveDependencyOn(DomainNamespace)
            .GetResult();

        //Assert
        Assert.True(testResult.IsSuccessful);
    }
}

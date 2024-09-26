using NetArchTest.Rules;

namespace UseCaseManagement.Architecture.Tests;

public class DomainDependencyTest
{
    private const string ApplicationNamespace = "UseCaseManagement.Application";
    //private const string DomainNamespace = "UseCaseManagement.Domain";
    private const string InfrastructureNamespace = "UseCaseManagement.Infrastructure";
    private const string ServiceNamespace = "UseCaseManagement.Service";

    [Fact]
    public void Domain_Should_Not_HaveDependecyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Domain.Entities.UseCase).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
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
}
using NetArchTest.Rules;

namespace UseCaseManagement.Architecture.Tests;

public class ServiceDependencyTest
{
    //private const string ApplicationNamespace = "UseCaseManagement.Application";
    //private const string DomainNamespace = "UseCaseManagement.Domain";
    private const string InfrastructureNamespace = "UseCaseManagement.Infrastructure";
    private const string ServiceNamespace = "UseCaseManagement.Service";

    [Fact]
    public void Service_Should_Not_HaveDependecyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Service.Controllers.VendorController).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace
        };

        //Act
        var testResult = Types.InAssembly(assembly)
            .That().ResideInNamespaceStartingWith(ServiceNamespace)
            .ShouldNot().HaveDependencyOnAny(otherProjects)
            .GetResult();

        //Assert
        Assert.True(testResult.IsSuccessful);
    }
}

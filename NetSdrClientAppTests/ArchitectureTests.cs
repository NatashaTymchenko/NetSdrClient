using NUnit.Framework;
using NetArchTest.Rules;
using NetSdrClientApp.Networking; 

namespace NetSdrClientAppTests
{
    public class ArchitectureTests
    {
        [Test]
        public void Networking_Classes_Should_Have_Service_Suffix()
        {
            var result = Types.InAssembly(typeof(TcpClientWrapper).Assembly)
                .That()
                .ResideInNamespace("NetSdrClientApp.Networking")
                .And()
                .AreClasses()
                .Should()
                .HaveNameEndingWith("Service") 
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Architecture violation: Networking classes must end with 'Service'");
        }
    }
}

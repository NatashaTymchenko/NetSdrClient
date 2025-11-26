using NUnit.Framework;
using EchoTcpServer;
using System.Threading.Tasks;

namespace EchoTcpServerTests
{
    [TestFixture]
    public class EchoServerTests
    {
        private EchoServer _server;

        [SetUp]
        public void Setup()
        {
            _server = new EchoServer();
        }

        [TearDown]
        public void Teardown()
        {
            _server.Stop();
        }

        [Test]
        public void Start_ShouldSetIsRunningToTrue()
        {            
            _server.Start(0); 

            Assert.IsTrue(_server.IsRunning, "Server should have IsRunning = true");
            Assert.AreNotEqual(0, _server.Port, "Port should be assigned automatically");
        }

        [Test]
        public void Stop_ShouldSetIsRunningToFalse()
        {
            _server.Start(0);

            _server.Stop();

            Assert.IsFalse(_server.IsRunning, "Server should have IsRunning = false");
        }
    }
}

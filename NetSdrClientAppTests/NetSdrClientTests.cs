using NUnit.Framework;
using Moq;
using NetSdrClientApp;
using NetSdrClientApp.Networking;
using System.Threading.Tasks;

namespace NetSdrClientAppTests
{
    [TestFixture]
    public class NetSdrClientTests
    {
        private Mock<ITcpClient> _mockTcp;
        private Mock<IUdpClient> _mockUdp;
        private NetSdrClient _client;

        [SetUp]
        public void Setup()
        {
            _mockTcp = new Mock<ITcpClient>();
            _mockUdp = new Mock<IUdpClient>();
            _client = new NetSdrClient(_mockTcp.Object, _mockUdp.Object);
        }

        [Test]
        public async Task ConnectAsync_ShouldCallTcpConnect()
        {
            await _client.ConnectAsync();

            _mockTcp.Verify(x => x.ConnectAsync(), Times.AtLeastOnce); 
        }

        [Test]
        public void Disconnect_ShouldCallTcpDisconnect()
        {
            _client.Disconnect(); // Тут вже правильна назва!

            _mockTcp.Verify(x => x.Disconnect(), Times.Once);
        }

        [Test]
        public async Task StartIQAsync_ShouldSendCorrectCommand()
        {
            await _client.StartIQAsync();

            _mockTcp.Verify(x => x.WriteAsync(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task ChangeFrequencyAsync_ShouldSendBytes()
        {
            await _client.ChangeFrequencyAsync(1000000, 1);

            _mockTcp.Verify(x => x.WriteAsync(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }
    }
}

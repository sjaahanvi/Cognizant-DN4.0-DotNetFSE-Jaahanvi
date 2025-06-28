using NUnit.Framework;
using Moq;
using CustomerCommLib;

namespace CustomerComm.Tests
{
    [TestFixture]
    public class CustomerCommTests
    {
        private Mock<IMailSender> _mockMailSender;
        private CustomerCommLib.CustomerComm _customerComm;

        [SetUp]
        public void SetUp()
        {
            // Initialize mock object for each test
            _mockMailSender = new Mock<IMailSender>();
            
            // Configure mock to return true for any string arguments
            _mockMailSender.Setup(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(true);
            
            // Create instance of CustomerComm with mocked dependency
            _customerComm = new CustomerCommLib.CustomerComm(_mockMailSender.Object);
        }

        [TestCase]
        public void SendMailToCustomer_ShouldReturnTrue_WhenMailSenderReturnsTrue()
        {
            // Act
            bool result = _customerComm.SendMailToCustomer();

            // Assert - Updated for NUnit 4
            Assert.That(result, Is.True);
            
            // Verify that SendMail was called exactly once
            _mockMailSender.Verify(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void SendMailToCustomer_ShouldCallSendMail_WithCorrectParameters()
        {
            // Act
            _customerComm.SendMailToCustomer();

            // Assert - Verify SendMail was called with expected parameters
            _mockMailSender.Verify(x => x.SendMail("cust123@abc.com", "Some Message"), Times.Once);
        }
    }
}

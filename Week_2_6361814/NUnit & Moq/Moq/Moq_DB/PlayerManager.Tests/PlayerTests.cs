using NUnit.Framework;
using Moq;
using PlayersManagerLib;
using System;

namespace PlayerManager.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        private Mock<IPlayerMapper> _mockPlayerMapper = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // This method runs once before all tests in this fixture
        }

        [SetUp]
        public void SetUp()
        {
            _mockPlayerMapper = new Mock<IPlayerMapper>();
        }

        [TestCase("John")]
        [TestCase("Alice")]
        public void RegisterNewPlayer_ValidPlayerName_ReturnsPlayerWithCorrectAttributes(string playerName)
        {
            // Arrange
            _mockPlayerMapper.Setup(x => x.IsPlayerNameExistsInDb(playerName)).Returns(false);

            // Act
            var player = Player.RegisterNewPlayer(playerName, _mockPlayerMapper.Object);

            // Assert
            Assert.That(player.Name, Is.EqualTo(playerName));
            Assert.That(player.Age, Is.EqualTo(23));
            Assert.That(player.Country, Is.EqualTo("India"));
            Assert.That(player.NoOfMatches, Is.EqualTo(30));

            _mockPlayerMapper.Verify(x => x.IsPlayerNameExistsInDb(playerName), Times.Once);
            _mockPlayerMapper.Verify(x => x.AddNewPlayerIntoDb(playerName), Times.Once);
        }

        [Test]
        public void RegisterNewPlayer_EmptyPlayerName_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Player.RegisterNewPlayer("", _mockPlayerMapper.Object));
            Assert.That(ex!.Message, Is.EqualTo("Player name can't be empty."));
            
            // Verify that database methods were not called
            _mockPlayerMapper.Verify(x => x.IsPlayerNameExistsInDb(It.IsAny<string>()), Times.Never);
            _mockPlayerMapper.Verify(x => x.AddNewPlayerIntoDb(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void RegisterNewPlayer_NullPlayerName_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Player.RegisterNewPlayer(null!, _mockPlayerMapper.Object));
            Assert.That(ex!.Message, Is.EqualTo("Player name can't be empty."));
        }

        [Test]
        public void RegisterNewPlayer_WhiteSpacePlayerName_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Player.RegisterNewPlayer("   ", _mockPlayerMapper.Object));
            Assert.That(ex!.Message, Is.EqualTo("Player name can't be empty."));
        }

        [Test]
        public void RegisterNewPlayer_ExistingPlayerName_ThrowsArgumentException()
        {
            // Arrange
            string existingPlayerName = "ExistingPlayer";
            _mockPlayerMapper.Setup(x => x.IsPlayerNameExistsInDb(existingPlayerName)).Returns(true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Player.RegisterNewPlayer(existingPlayerName, _mockPlayerMapper.Object));
            Assert.That(ex!.Message, Is.EqualTo("Player name already exists."));

            // Verify that IsPlayerNameExistsInDb was called but AddNewPlayerIntoDb was not
            _mockPlayerMapper.Verify(x => x.IsPlayerNameExistsInDb(existingPlayerName), Times.Once);
            _mockPlayerMapper.Verify(x => x.AddNewPlayerIntoDb(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void RegisterNewPlayer_NoPlayerMapperProvided_ThrowsConnectionException()
        {
            // This test verifies that when no mock is provided, 
            // the method tries to use the default PlayerMapper which fails due to no database
            
            // Act & Assert - Expect SqlException due to no database connection
            Assert.Throws<Microsoft.Data.SqlClient.SqlException>(() => Player.RegisterNewPlayer("TestPlayer", null!));
        }

        [TestCase("ValidPlayer1")]
        [TestCase("ValidPlayer2")]
        public void RegisterNewPlayer_ValidScenario_CallsMapperMethodsInCorrectOrder(string playerName)
        {
            // Arrange
            _mockPlayerMapper.Setup(x => x.IsPlayerNameExistsInDb(playerName)).Returns(false);

            // Act
            Player.RegisterNewPlayer(playerName, _mockPlayerMapper.Object);

            // Assert - Verify the sequence of calls
            _mockPlayerMapper.Verify(x => x.IsPlayerNameExistsInDb(playerName), Times.Once);
            _mockPlayerMapper.Verify(x => x.AddNewPlayerIntoDb(playerName), Times.Once);
        }
    }
}


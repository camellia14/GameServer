using GameServer.DB;
using GameServer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class CharacterExtendedTest : IDisposable
    {
        private readonly AppDbContext _context;

        public CharacterExtendedTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
        }

        [Fact]
        public void CharacterEntity_ShouldInitializeWithExtendedProperties()
        {
            // Arrange & Act
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                Level = 1
            };

            // Assert
            Assert.Equal(100, character.CurrentHp);
            Assert.Equal(100, character.MaxHp);
            Assert.Equal(50, character.CurrentMp);
            Assert.Equal(50, character.MaxMp);
            Assert.Equal(10, character.AttackPower);
            Assert.Equal(5, character.DefensePower);
            Assert.Equal(5.0f, character.MovementSpeed);
            Assert.Equal(0.0f, character.Rotation);
            Assert.True(character.IsAlive);
            Assert.Equal(0, character.Experience);
        }

        [Fact]
        public void UpdatePosition_ShouldUpdateCoordinatesAndRotation()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                PositionX = 0.0f,
                PositionY = 0.0f,
                Rotation = 0.0f
            };

            // Act
            character.UpdatePosition(10.5f, 20.3f, 45.0f);

            // Assert
            Assert.Equal(10.5f, character.PositionX);
            Assert.Equal(20.3f, character.PositionY);
            Assert.Equal(45.0f, character.Rotation);
        }

        [Fact]
        public void UpdatePosition_ShouldUpdateOnlyCoordinates_WhenRotationIsNull()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                PositionX = 0.0f,
                PositionY = 0.0f,
                Rotation = 90.0f
            };

            // Act
            character.UpdatePosition(5.0f, 10.0f);

            // Assert
            Assert.Equal(5.0f, character.PositionX);
            Assert.Equal(10.0f, character.PositionY);
            Assert.Equal(90.0f, character.Rotation); // Should remain unchanged
        }

        [Fact]
        public void TakeDamage_ShouldReduceHp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 100,
                IsAlive = true
            };

            // Act
            int damage = character.TakeDamage(30);

            // Assert
            Assert.Equal(30, damage);
            Assert.Equal(70, character.CurrentHp);
            Assert.True(character.IsAlive);
        }

        [Fact]
        public void TakeDamage_ShouldKillCharacter_WhenHpReachesZero()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 50,
                IsAlive = true
            };

            // Act
            int damage = character.TakeDamage(60);

            // Assert
            Assert.Equal(50, damage); // Should only take remaining HP
            Assert.Equal(0, character.CurrentHp);
            Assert.False(character.IsAlive);
        }

        [Fact]
        public void TakeDamage_ShouldReturnZero_WhenCharacterIsDead()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 0,
                IsAlive = false
            };

            // Act
            int damage = character.TakeDamage(50);

            // Assert
            Assert.Equal(0, damage);
            Assert.Equal(0, character.CurrentHp);
            Assert.False(character.IsAlive);
        }

        [Fact]
        public void Heal_ShouldRestoreHp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 50,
                MaxHp = 100
            };

            // Act
            int healed = character.Heal(30);

            // Assert
            Assert.Equal(30, healed);
            Assert.Equal(80, character.CurrentHp);
        }

        [Fact]
        public void Heal_ShouldNotExceedMaxHp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 90,
                MaxHp = 100
            };

            // Act
            int healed = character.Heal(20);

            // Assert
            Assert.Equal(10, healed); // Should only heal to max
            Assert.Equal(100, character.CurrentHp);
        }

        [Fact]
        public void ConsumeMp_ShouldReduceMp_WhenSufficientMp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentMp = 50
            };

            // Act
            bool result = character.ConsumeMp(20);

            // Assert
            Assert.True(result);
            Assert.Equal(30, character.CurrentMp);
        }

        [Fact]
        public void ConsumeMp_ShouldFail_WhenInsufficientMp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentMp = 10
            };

            // Act
            bool result = character.ConsumeMp(20);

            // Assert
            Assert.False(result);
            Assert.Equal(10, character.CurrentMp);
        }

        [Fact]
        public void RestoreMp_ShouldIncreaseMp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentMp = 20,
                MaxMp = 50
            };

            // Act
            int restored = character.RestoreMp(15);

            // Assert
            Assert.Equal(15, restored);
            Assert.Equal(35, character.CurrentMp);
        }

        [Fact]
        public void RestoreMp_ShouldNotExceedMaxMp()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentMp = 45,
                MaxMp = 50
            };

            // Act
            int restored = character.RestoreMp(10);

            // Assert
            Assert.Equal(5, restored); // Should only restore to max
            Assert.Equal(50, character.CurrentMp);
        }

        [Fact]
        public void GainExperience_ShouldIncreaseExperience()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                Experience = 0,
                Level = 1
            };

            // Act
            int newLevel = character.GainExperience(50);

            // Assert
            Assert.Equal(50, character.Experience);
            Assert.Equal(0, newLevel); // Should not level up yet
            Assert.Equal(1, character.Level);
        }

        [Fact]
        public void GainExperience_ShouldLevelUp_WhenEnoughExperience()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                Experience = 0,
                Level = 1,
                CurrentHp = 50,
                CurrentMp = 25,
                MaxHp = 100,
                MaxMp = 50,
                AttackPower = 10,
                DefensePower = 5
            };

            // Act
            int newLevel = character.GainExperience(150); // Should trigger level up

            // Assert
            Assert.Equal(150, character.Experience);
            Assert.Equal(2, newLevel);
            Assert.Equal(2, character.Level);
            // Should have stat increases and full healing
            Assert.Equal(110, character.MaxHp);
            Assert.Equal(55, character.MaxMp);
            Assert.Equal(12, character.AttackPower);
            Assert.Equal(6, character.DefensePower);
            Assert.Equal(110, character.CurrentHp); // Full heal on level up
            Assert.Equal(55, character.CurrentMp); // Full restore on level up
        }

        [Fact]
        public void Revive_ShouldRestoreLife()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 0,
                MaxHp = 100,
                IsAlive = false
            };

            // Act
            character.Revive(0.75f);

            // Assert
            Assert.True(character.IsAlive);
            Assert.Equal(75, character.CurrentHp);
        }

        [Fact]
        public void Revive_ShouldNotRevive_WhenAlreadyAlive()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 100,
                MaxHp = 100,
                IsAlive = true
            };

            // Act
            character.Revive(0.5f);

            // Assert
            Assert.True(character.IsAlive);
            Assert.Equal(100, character.CurrentHp); // Should remain unchanged
        }

        [Fact]
        public void Revive_ShouldUpdatePosition_WhenCoordinatesProvided()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                CurrentHp = 0,
                MaxHp = 100,
                IsAlive = false,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            // Act
            character.Revive(0.5f, 10.0f, 20.0f);

            // Assert
            Assert.True(character.IsAlive);
            Assert.Equal(50, character.CurrentHp);
            Assert.Equal(10.0f, character.PositionX);
            Assert.Equal(20.0f, character.PositionY);
        }

        [Fact]
        public void UpdateStats_ShouldUpdateSpecifiedStats()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                MaxHp = 100,
                MaxMp = 50,
                AttackPower = 10,
                DefensePower = 5,
                CurrentHp = 80,
                CurrentMp = 30
            };

            // Act
            character.UpdateStats(hp: 120, attackPower: 15);

            // Assert
            Assert.Equal(120, character.MaxHp);
            Assert.Equal(80, character.CurrentHp); // Current HP should be capped at new max
            Assert.Equal(50, character.MaxMp); // Should remain unchanged
            Assert.Equal(15, character.AttackPower);
            Assert.Equal(5, character.DefensePower); // Should remain unchanged
        }

        [Fact]
        public void UpdateStats_ShouldCapCurrentHp_WhenMaxHpDecreases()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                MaxHp = 100,
                CurrentHp = 90
            };

            // Act
            character.UpdateStats(hp: 80);

            // Assert
            Assert.Equal(80, character.MaxHp);
            Assert.Equal(80, character.CurrentHp); // Should be capped at new max
        }

        [Fact]
        public void IsInAttackRange_ShouldReturnTrue_WhenInRange()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            // Act
            bool inRange = character.IsInAttackRange(3.0f, 4.0f, 6.0f);

            // Assert
            Assert.True(inRange); // Distance is 5, range is 6
        }

        [Fact]
        public void IsInAttackRange_ShouldReturnFalse_WhenOutOfRange()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            // Act
            bool inRange = character.IsInAttackRange(6.0f, 8.0f, 5.0f);

            // Assert
            Assert.False(inRange); // Distance is 10, range is 5
        }

        [Fact]
        public void GetDistanceTo_ShouldCalculateCorrectDistance()
        {
            // Arrange
            var character1 = new CharacterEntity
            {
                Name = "Hero1",
                PlayerUserId = 1,
                PositionX = 0.0f,
                PositionY = 0.0f
            };

            var character2 = new CharacterEntity
            {
                Name = "Hero2",
                PlayerUserId = 2,
                PositionX = 3.0f,
                PositionY = 4.0f
            };

            // Act
            float distance = character1.GetDistanceTo(character2);

            // Assert
            Assert.Equal(5.0f, distance, 1); // 3-4-5 triangle
        }

        [Fact]
        public async Task CharacterEntity_ShouldSaveExtendedPropertiesToDatabase()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "ExtendedHero",
                PlayerUserId = 1,
                Level = 2,
                CurrentHp = 80,
                MaxHp = 120,
                CurrentMp = 40,
                MaxMp = 60,
                AttackPower = 15,
                DefensePower = 8,
                MovementSpeed = 6.5f,
                Rotation = 45.0f,
                IsAlive = true,
                Experience = 250
            };

            // Act
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            // Assert
            var savedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Name == "ExtendedHero");
            Assert.NotNull(savedCharacter);
            Assert.Equal(2, savedCharacter.Level);
            Assert.Equal(80, savedCharacter.CurrentHp);
            Assert.Equal(120, savedCharacter.MaxHp);
            Assert.Equal(40, savedCharacter.CurrentMp);
            Assert.Equal(60, savedCharacter.MaxMp);
            Assert.Equal(15, savedCharacter.AttackPower);
            Assert.Equal(8, savedCharacter.DefensePower);
            Assert.Equal(6.5f, savedCharacter.MovementSpeed);
            Assert.Equal(45.0f, savedCharacter.Rotation);
            Assert.True(savedCharacter.IsAlive);
            Assert.Equal(250, savedCharacter.Experience);
        }

        [Fact]
        public async Task CharacterEntity_ShouldUpdateExtendedPropertiesInDatabase()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "UpdatableHero",
                PlayerUserId = 1,
                CurrentHp = 100,
                CurrentMp = 50,
                Experience = 0
            };
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            // Act
            character.TakeDamage(30);
            character.ConsumeMp(20);
            character.GainExperience(150);
            await _context.SaveChangesAsync();

            // Assert
            var updatedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterId == character.CharacterId);
            Assert.NotNull(updatedCharacter);
            Assert.Equal(70, updatedCharacter.CurrentHp);
            Assert.Equal(30, updatedCharacter.CurrentMp);
            Assert.Equal(150, updatedCharacter.Experience);
        }

        [Fact]
        public void CalculateLevelFromExperience_ShouldReturnCorrectLevel()
        {
            // Arrange
            var character = new CharacterEntity
            {
                Name = "TestHero",
                PlayerUserId = 1
            };

            // Act & Assert - Using reflection to test private method
            var method = typeof(CharacterEntity).GetMethod("CalculateLevelFromExperience", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
            Assert.Equal(1, method.Invoke(character, new object[] { 0 }));    // Level 1 at 0 exp
            Assert.Equal(1, method.Invoke(character, new object[] { 99 }));   // Level 1 at 99 exp
            Assert.Equal(2, method.Invoke(character, new object[] { 100 }));  // Level 2 at 100 exp
            Assert.Equal(3, method.Invoke(character, new object[] { 400 }));  // Level 3 at 400 exp
            Assert.Equal(4, method.Invoke(character, new object[] { 900 }));  // Level 4 at 900 exp
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
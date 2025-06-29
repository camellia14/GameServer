using GameServer.DB;
using GameServer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class ChatTest : IDisposable
    {
        private readonly AppDbContext _context;

        public ChatTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
        }

        [Fact]
        public void ChatEntity_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Hello, world!",
                ReceiverUserId = 2
            };

            // Assert
            Assert.Equal(1, chat.SenderUserId);
            Assert.Equal(ChatType.Private, chat.ChatType);
            Assert.Equal("Hello, world!", chat.Message);
            Assert.Equal(2, chat.ReceiverUserId);
            Assert.False(chat.IsRead);
            Assert.False(chat.IsEdited);
            Assert.False(chat.IsDeleted);
            Assert.False(chat.IsSystemMessage);
            Assert.Null(chat.ReadAt);
            Assert.Null(chat.EditedAt);
            Assert.Null(chat.DeletedAt);
        }

        [Fact]
        public void MarkAsRead_ShouldSetReadStatusAndTime()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsRead = false
            };
            var beforeRead = DateTime.UtcNow;

            // Act
            chat.MarkAsRead();

            // Assert
            var afterRead = DateTime.UtcNow;
            Assert.True(chat.IsRead);
            Assert.NotNull(chat.ReadAt);
            Assert.True(chat.ReadAt >= beforeRead);
            Assert.True(chat.ReadAt <= afterRead);
        }

        [Fact]
        public void MarkAsRead_ShouldNotChange_WhenAlreadyRead()
        {
            // Arrange
            var originalReadAt = DateTime.UtcNow.AddMinutes(-5);
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsRead = true,
                ReadAt = originalReadAt
            };

            // Act
            chat.MarkAsRead();

            // Assert
            Assert.True(chat.IsRead);
            Assert.Equal(originalReadAt, chat.ReadAt);
        }

        [Fact]
        public void Edit_ShouldUpdateMessageAndSetEditStatus()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Original message",
                IsEdited = false
            };
            var beforeEdit = DateTime.UtcNow;

            // Act
            chat.Edit("Edited message");

            // Assert
            var afterEdit = DateTime.UtcNow;
            Assert.Equal("Edited message", chat.Message);
            Assert.True(chat.IsEdited);
            Assert.NotNull(chat.EditedAt);
            Assert.True(chat.EditedAt >= beforeEdit);
            Assert.True(chat.EditedAt <= afterEdit);
        }

        [Fact]
        public void Edit_ShouldNotEdit_WhenDeleted()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Original message",
                IsDeleted = true
            };

            // Act
            chat.Edit("Edited message");

            // Assert
            Assert.Equal("Original message", chat.Message);
            Assert.False(chat.IsEdited);
            Assert.Null(chat.EditedAt);
        }

        [Fact]
        public void Edit_ShouldNotEdit_WhenNewMessageIsEmpty()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Original message",
                IsEdited = false
            };

            // Act
            chat.Edit("");

            // Assert
            Assert.Equal("Original message", chat.Message);
            Assert.False(chat.IsEdited);
            Assert.Null(chat.EditedAt);
        }

        [Fact]
        public void Delete_ShouldSetDeleteStatus()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsDeleted = false
            };
            var beforeDelete = DateTime.UtcNow;

            // Act
            chat.Delete();

            // Assert
            var afterDelete = DateTime.UtcNow;
            Assert.True(chat.IsDeleted);
            Assert.NotNull(chat.DeletedAt);
            Assert.True(chat.DeletedAt >= beforeDelete);
            Assert.True(chat.DeletedAt <= afterDelete);
        }

        [Fact]
        public void Delete_ShouldNotChange_WhenAlreadyDeleted()
        {
            // Arrange
            var originalDeletedAt = DateTime.UtcNow.AddMinutes(-5);
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsDeleted = true,
                DeletedAt = originalDeletedAt
            };

            // Act
            chat.Delete();

            // Assert
            Assert.True(chat.IsDeleted);
            Assert.Equal(originalDeletedAt, chat.DeletedAt);
        }

        [Fact]
        public void IsRelevantToUser_ShouldReturnTrue_ForPrivateChatSender()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                ReceiverUserId = 2
            };

            // Act & Assert
            Assert.True(chat.IsRelevantToUser(1));
        }

        [Fact]
        public void IsRelevantToUser_ShouldReturnTrue_ForPrivateChatReceiver()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                ReceiverUserId = 2
            };

            // Act & Assert
            Assert.True(chat.IsRelevantToUser(2));
        }

        [Fact]
        public void IsRelevantToUser_ShouldReturnFalse_ForPrivateChatOtherUser()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                ReceiverUserId = 2
            };

            // Act & Assert
            Assert.False(chat.IsRelevantToUser(3));
        }

        [Fact]
        public void IsRelevantToUser_ShouldReturnTrue_ForGlobalChat()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Global,
                Message = "Global message"
            };

            // Act & Assert
            Assert.True(chat.IsRelevantToUser(1));
            Assert.True(chat.IsRelevantToUser(2));
            Assert.True(chat.IsRelevantToUser(999));
        }

        [Fact]
        public void IsDisplayable_ShouldReturnTrue_WhenNotDeleted()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsDeleted = false
            };

            // Act & Assert
            Assert.True(chat.IsDisplayable());
        }

        [Fact]
        public void IsDisplayable_ShouldReturnFalse_WhenDeleted()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Test message",
                IsDeleted = true
            };

            // Act & Assert
            Assert.False(chat.IsDisplayable());
        }

        [Fact]
        public async Task ChatEntity_ShouldSaveToDatabase()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Hello, database!",
                ReceiverUserId = 2
            };

            // Act
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            // Assert
            var savedChat = await _context.Chats.FirstOrDefaultAsync(c => c.SenderUserId == 1 && c.ReceiverUserId == 2);
            Assert.NotNull(savedChat);
            Assert.Equal("Hello, database!", savedChat.Message);
            Assert.Equal(ChatType.Private, savedChat.ChatType);
        }

        [Fact]
        public async Task ChatEntity_ShouldUpdateInDatabase()
        {
            // Arrange
            var chat = new ChatEntity
            {
                SenderUserId = 1,
                ChatType = ChatType.Private,
                Message = "Original message",
                ReceiverUserId = 2
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            // Act
            chat.Edit("Updated message");
            chat.MarkAsRead();
            await _context.SaveChangesAsync();

            // Assert
            var updatedChat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == chat.ChatId);
            Assert.NotNull(updatedChat);
            Assert.Equal("Updated message", updatedChat.Message);
            Assert.True(updatedChat.IsEdited);
            Assert.True(updatedChat.IsRead);
            Assert.NotNull(updatedChat.EditedAt);
            Assert.NotNull(updatedChat.ReadAt);
        }

        [Fact]
        public void ChatType_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)ChatType.Private);
            Assert.Equal(1, (int)ChatType.Group);
            Assert.Equal(2, (int)ChatType.Room);
            Assert.Equal(3, (int)ChatType.Global);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
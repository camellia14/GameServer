using GameServer.Services;
using Xunit;

namespace UnitTests
{
    public class ScriptExecutorTest
    {
        private readonly ScriptExecutor _scriptExecutor;

        public ScriptExecutorTest()
        {
            _scriptExecutor = new ScriptExecutor();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_ForValidScript()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("heal_hp_50", 1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("HPを50回復しました", result.Message);
            Assert.Null(result.ErrorDetails);
            Assert.True(result.ExecutionTimeMs >= 0);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_ForManaHealScript()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("heal_mp_30", 1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("MPを30回復しました", result.Message);
            Assert.Null(result.ErrorDetails);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_ForFullHealScript()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("heal_full", 1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("HPとMPを全回復しました", result.Message);
            Assert.Null(result.ErrorDetails);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_ForInvalidScript()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("invalid_script", 1, 1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("不明なスクリプトです", result.Message);
            Assert.Equal("Script not found: invalid_script", result.ErrorDetails);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_ForTestErrorScript()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("error_test", 1, 1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("テスト用エラーが発生しました", result.Message);
            Assert.Equal("This is a test error", result.ErrorDetails);
        }

        [Fact]
        public void IsScriptAvailable_ShouldReturnTrue_ForValidScripts()
        {
            // Act & Assert
            Assert.True(_scriptExecutor.IsScriptAvailable("heal_hp_50"));
            Assert.True(_scriptExecutor.IsScriptAvailable("heal_mp_30"));
            Assert.True(_scriptExecutor.IsScriptAvailable("heal_full"));
            Assert.True(_scriptExecutor.IsScriptAvailable("error_test"));
        }

        [Fact]
        public void IsScriptAvailable_ShouldReturnFalse_ForInvalidScripts()
        {
            // Act & Assert
            Assert.False(_scriptExecutor.IsScriptAvailable("invalid_script"));
            Assert.False(_scriptExecutor.IsScriptAvailable(""));
            Assert.False(_scriptExecutor.IsScriptAvailable(null));
            Assert.False(_scriptExecutor.IsScriptAvailable("   "));
        }

        [Fact]
        public void IsScriptAvailable_ShouldBeCaseInsensitive()
        {
            // Act & Assert
            Assert.True(_scriptExecutor.IsScriptAvailable("HEAL_HP_50"));
            Assert.True(_scriptExecutor.IsScriptAvailable("Heal_Mp_30"));
            Assert.True(_scriptExecutor.IsScriptAvailable("HEAL_FULL"));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldBeCaseInsensitive()
        {
            // Act
            var result1 = await _scriptExecutor.ExecuteAsync("HEAL_HP_50", 1, 1);
            var result2 = await _scriptExecutor.ExecuteAsync("Heal_Mp_30", 1, 1);

            // Assert
            Assert.True(result1.Success);
            Assert.Equal("HPを50回復しました", result1.Message);
            
            Assert.True(result2.Success);
            Assert.Equal("MPを30回復しました", result2.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleExecutionTime()
        {
            // Act
            var result = await _scriptExecutor.ExecuteAsync("heal_hp_50", 1, 1);

            // Assert
            Assert.True(result.ExecutionTimeMs >= 0);
            Assert.True(result.ExecutionTimeMs < 1000); // 処理は短時間で完了するはず
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleMultipleExecutions()
        {
            // Act
            var task1 = _scriptExecutor.ExecuteAsync("heal_hp_50", 1, 1);
            var task2 = _scriptExecutor.ExecuteAsync("heal_mp_30", 2, 2);
            var task3 = _scriptExecutor.ExecuteAsync("heal_full", 3, 3);

            var results = await Task.WhenAll(task1, task2, task3);

            // Assert
            Assert.All(results, result => Assert.True(result.Success));
            Assert.Equal("HPを50回復しました", results[0].Message);
            Assert.Equal("MPを30回復しました", results[1].Message);
            Assert.Equal("HPとMPを全回復しました", results[2].Message);
        }
    }
}
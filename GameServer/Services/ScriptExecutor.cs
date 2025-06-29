namespace GameServer.Services
{
    /// <summary>
    /// 外部スクリプト実行結果を表すクラス
    /// </summary>
    public class ScriptExecutionResult
    {
        /// <summary>
        /// スクリプトの実行が成功したかどうか
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// スクリプトの実行結果メッセージ
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// スクリプトで発生したエラー情報（ある場合）
        /// </summary>
        public string? ErrorDetails { get; set; }

        /// <summary>
        /// スクリプトの実行にかかった時間（ミリ秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// 外部スクリプト実行サービス
    /// スタックアイテムの効果を外部スクリプトで実行する機能を提供する
    /// エラーが発生しても呼び出し元がクラッシュしないようにする
    /// </summary>
    public class ScriptExecutor
    {
        private readonly ILogger<ScriptExecutor>? _logger;

        /// <summary>
        /// ScriptExecutorのコンストラクタ
        /// </summary>
        /// <param name="logger">ロガー（オプション）</param>
        public ScriptExecutor(ILogger<ScriptExecutor>? logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 指定されたスクリプト名でスクリプトを実行する
        /// </summary>
        /// <param name="scriptName">実行するスクリプト名</param>
        /// <param name="playerUserId">スクリプトを実行するプレイヤーのユーザーID</param>
        /// <param name="itemMasterId">使用されたアイテムのマスターID</param>
        /// <returns>スクリプト実行結果</returns>
        public async Task<ScriptExecutionResult> ExecuteAsync(string scriptName, int playerUserId, int itemMasterId)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                _logger?.LogInformation($"Executing script: {scriptName} for player {playerUserId}, item {itemMasterId}");

                // 実際の実装では、ここで外部スクリプトを実行する
                // 現在は模擬実装として、スクリプト名に基づいて固定の処理を行う
                var result = await ExecuteScriptLogic(scriptName, playerUserId, itemMasterId);
                
                stopwatch.Stop();
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                
                _logger?.LogInformation($"Script execution completed: {scriptName}, Success: {result.Success}, Time: {result.ExecutionTimeMs}ms");
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                _logger?.LogError(ex, $"Script execution failed: {scriptName} for player {playerUserId}");
                
                return new ScriptExecutionResult
                {
                    Success = false,
                    Message = "スクリプトの実行中にエラーが発生しました",
                    ErrorDetails = ex.Message,
                    ExecutionTimeMs = stopwatch.ElapsedMilliseconds
                };
            }
        }

        /// <summary>
        /// スクリプトロジックの実装（模擬実装）
        /// 実際の実装では、外部スクリプトエンジンやプロセス実行を行う
        /// </summary>
        /// <param name="scriptName">スクリプト名</param>
        /// <param name="playerUserId">プレイヤーのユーザーID</param>
        /// <param name="itemMasterId">アイテムマスターID</param>
        /// <returns>スクリプト実行結果</returns>
        private async Task<ScriptExecutionResult> ExecuteScriptLogic(string scriptName, int playerUserId, int itemMasterId)
        {
            // 非同期処理をシミュレート
            await Task.Delay(10);

            return scriptName.ToLower() switch
            {
                "heal_hp_50" => new ScriptExecutionResult
                {
                    Success = true,
                    Message = "HPを50回復しました"
                },
                "heal_mp_30" => new ScriptExecutionResult
                {
                    Success = true,
                    Message = "MPを30回復しました"
                },
                "heal_full" => new ScriptExecutionResult
                {
                    Success = true,
                    Message = "HPとMPを全回復しました"
                },
                "error_test" => new ScriptExecutionResult
                {
                    Success = false,
                    Message = "テスト用エラーが発生しました",
                    ErrorDetails = "This is a test error"
                },
                _ => new ScriptExecutionResult
                {
                    Success = false,
                    Message = "不明なスクリプトです",
                    ErrorDetails = $"Script not found: {scriptName}"
                }
            };
        }

        /// <summary>
        /// スクリプトが実行可能かを確認する
        /// </summary>
        /// <param name="scriptName">確認するスクリプト名</param>
        /// <returns>実行可能な場合はtrue、そうでない場合はfalse</returns>
        public bool IsScriptAvailable(string scriptName)
        {
            if (string.IsNullOrWhiteSpace(scriptName))
                return false;

            // 実際の実装では、スクリプトファイルの存在確認などを行う
            // 現在は模擬実装として、既知のスクリプト名のみ有効とみなす
            var availableScripts = new[]
            {
                "heal_hp_50",
                "heal_mp_30", 
                "heal_full",
                "error_test"
            };

            return availableScripts.Contains(scriptName.ToLower());
        }
    }
}
using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Client;
using Shared.Services;
using Shared.Data;
using Grpc.Core;

class Program
{
    static async Task Main()
    {
        // gRPC チャンネル作成
        var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Insecure
        });
        // MagicOnion クライアント生成
        IPlayerService client = MagicOnionClient.Create<IPlayerService>(channel);
        foreach (var i in Enumerable.Range(1, 5))
        {
            await GetPlayer(client, i);
        }

        ICharacterService characterClient = MagicOnionClient.Create<ICharacterService>(channel);
        // 現在のキャラクターをすべて取得
        var characters = await characterClient.GetPlayerCharacters(1);
        // キャラクター作成



    }
    static async Task GetPlayer(IPlayerService client, int playerId)
    {
        // サーバーのメソッド呼び出し
        PlayerData? result = await client.GetPlayer(playerId);
        if (result == null)
        {
            Console.WriteLine("Player not found or an error occurred.");
        }
        else
        {
            Console.WriteLine($"Result Name: {result.UserName}, Money: {result.Money}");
        }
    }
}
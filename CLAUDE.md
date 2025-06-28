# 概要
これはオンラインゲームのサーバープログラムです。ユニットテストと通信テスト用のCLIクライアントも含まれています。

#技術スタック:
##言語・フレームワーク:
- C# / .NET 8.0
- ASP.NET Core
- Entity Framework Core
##通信・RPC:
- MagicOnion (gRPC based)
- gRPC
- Protocol Buffers (.proto)
##データベース:
- MySQL 8.0
- Entity Framework Core with MySQL provider
##コンテナ化:
- Docker
- Docker Compose
##サーバー:
- Kestrel (HTTP/2)
##アーキテクチャパターン:
- Repository Pattern
- Use Case Pattern
- Clean Architecture

#主要なディレクトリ構成
  GameServer/
  ├── GameClient/          # CLIクライアントアプリケーション
  │   ├── Protos/          # Protocol Buffers定義
  │   └── Services/        # gRPCサービス
  ├── GameServer/          # サーバーアプリケーション
  │   ├── DB/              # データベースコンテキスト
  │   ├── Entities/        # エンティティ
  │   ├── Repositories/    # リポジトリパターン
  │   │   └── Interfaces/
  │   ├── Services/        # ビジネスロジック
  │   ├── UseCases/        # ユースケース
  │   ├── bat/             # バッチスクリプト
  │   └── sql/             # SQLスクリプト
  ├── Shared/              # 共通ライブラリ
  │   ├── Data/            # データクラス
  │   └── Services/        # サービスインターフェース
  └── UnitTests/           # テストコード
      └── Repositories/


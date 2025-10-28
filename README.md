# UploadFiles

Repositório: UploadFiles  
Alvo do projeto: .NET 9

Este README descreve de forma detalhada como configurar, compilar e publicar a solução para Linux e Windows, incluindo orientações sobre migrações de banco de dados com Entity Framework, publicação como aplicativo independente (self-contained) e execução local durante o desenvolvimento.

Índice
- Visão geral
- Requisitos
- Estrutura mínima do repositório
- Configuração (variáveis, connection string)
- Compilar e executar (CLI)
  - Linux
  - Windows (CLI)
  - Visual Studio 2022
- Publicar (produzir binário para distribuição)
  - Publicação para Linux
  - Publicação para Windows
  - Opções avançadas (single-file, trimming)
- Migrações e banco de dados (Entity Framework)
- Executando em Docker (opcional)
- Dicas e solução de problemas
- Contribuição

Visão geral
Este projeto contém múltiplos projetos (ex.: `UploadFiles.Api`, `UploadFiles.Infra`, `UploadFiles.App`, `UploadFiles.Domain`). O ponto de entrada da aplicação geralmente é o projeto API (`UploadFiles.Api`). Ajuste caminhos de projeto conforme a estrutura real do seu repositório.

Requisitos
- .NET 9 SDK instalado (`dotnet --list-sdks` deve listar `9.x.x`).
- (Se usar EF migrations) Ferramenta `dotnet-ef` instalada globalmente: `dotnet tool install --global dotnet-ef` — ou use `dotnet tool restore` se estiver no `toolmanifest`.
- Banco de dados (ex.: SQL Server, PostgreSQL) configurado e acessível.
- Visual Studio 2022 (para quem preferir IDE, com suporte ao .NET 9 workloads).
- Permissões de escrita nas pastas de publicação e nas pastas de armazenamento de arquivos configuradas pela aplicação.

Estrutura mínima do repositório (exemplo)
- `UploadFiles.sln` (opcional)
- `UploadFiles.Api/` (projeto API — startup)
- `UploadFiles.App/` (aplicação / use cases)
- `UploadFiles.Infra/` (infraestrutura, repositórios, EF DbContext)
- `UploadFiles.Domain/` (entidades, abstrações)

Configuração
1. Connection strings e outras configurações normalmente ficam em `appsettings.json` do projeto `UploadFiles.Api`. Exemplo (ajuste conforme seu projeto):
"ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=UploadFilesDb;User Id=sa;Password=Your_password123;" }
2. Você também pode definir via variável de ambiente (útil para containers/produção):
- Windows PowerShell:
  ```
  $env:ConnectionStrings__DefaultConnection = "Server=...;Database=...;User Id=...;Password=...;"
  ```
- Linux / macOS:
  ```
  export ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=...;"
  ```

Compilar e executar (CLI)
Abra um terminal na raiz do repositório.

Geral (compilar solução/projetos): dotnet build

Executar o projeto API diretamente (modo desenvolvimento): dotnet run --project UploadFiles.Api

ou, especificando o arquivo csproj: dotnet run --project ./UploadFiles.Api/UploadFiles.Api.csproj

Linux
1. Confirme que o SDK .NET 9 está instalado:
    dotnet --info
2. Build e run:
    dotnet build dotnet run --project ./UploadFiles.Api/UploadFiles.Api.csproj
3. Caso precise setar variáveis de ambiente antes de rodar:
    export ASPNETCORE_ENVIRONMENT=Development export ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=..."

Windows (CLI)
1. No PowerShell (ou CMD):
    dotnet build dotnet run --project .\UploadFiles.Api\UploadFiles.Api.csproj
2. Definir variáveis (PowerShell):
    $env:ASPNETCORE_ENVIRONMENT = "Development" $env:ConnectionStrings__DefaultConnection = "Server=...;Database=...;User Id=...;Password=..."

Visual Studio 2022
1. Abra a solução `UploadFiles.sln` (ou a pasta se não houver Sln).
2. Defina o projeto `UploadFiles.Api` como projeto de inicialização (Right-click -> Set as Startup Project).
3. Configure `appsettings.Development.json` e variáveis de ambiente em `Debug > Profiles`.
4. Pressione `F5` (debug) ou `Ctrl+F5` (run sem debug).

Publicar (gerar artefato para distribuição)
Geral: usamos `dotnet publish` com `-c Release`. Para builds self-contained (inclui runtime) use `-r <RID>` e `--self-contained true`.

Publicação para Linux (ex.: linux-x64)
    dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/linux-x64

Publicação para Windows x64

dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64


Publicação framework-dependent (menor tamanho, requer runtime no host):
    dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -o ./publish/fd

Opções avançadas
- Ativar trimming (reduz tamanho, testar bem):
    -p:PublishTrimmed=true
- Incluir ícone, certs, runtimeconfig adjustments podem ser configurados via `csproj` ou parâmetros.

Migrações e banco de dados (Entity Framework)
1. Instale a ferramenta `dotnet-ef`:
    dotnet tool install --global dotnet-ef

2. Adicionar uma migration (exemplo do seu repositório — adapta conforme nomes):
- Windows PowerShell:
  ```
  dotnet ef migrations add V1 --context UploadFilesDbContext --project UploadFiles.Infra --startup-project UploadFiles.Api -o Migrations
  ```
- Linux / macOS (mesma sintaxe):
  ```
  dotnet ef migrations add V1 --context UploadFilesDbContext --project UploadFiles.Infra --startup-project UploadFiles.Api -o Migrations
  ```
3. Aplicar as migrations na base:
    dotnet ef database update --context UploadFilesDbContext --project UploadFiles.Infra --startup-project UploadFiles.Api
Observação: Há um arquivo no repositório com exemplo (`UploadFiles.Infra/Utility/ExempleMigration.txt`) com comandos PowerShell equivalentes.

Executando em Docker (opcional)
1. Crie um `Dockerfile` para a API (exemplo simples multi-stage):
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build WORKDIR /src COPY . . RUN dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -o /app/publish
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 WORKDIR /app COPY --from=build /app/publish . ENTRYPOINT ["dotnet", "UploadFiles.Api.dll"]
2. Build e run:
    docker build -t uploadfiles:latest . docker run -e ConnectionStrings__DefaultConnection="Server=...;" -p 5000:80 uploadfiles:latest


Dicas e solução de problemas
- Erro: "SDK 9.x not found" — verifique `dotnet --list-sdks` e instale o SDK .NET 9.
- Erro EF: "No migrations" — confirme `--startup-project` e `--project` corretos; verifique se o DbContext está no projeto `Infra`.
- Permissão de escrita ao salvar arquivos — verifique o caminho configurado no serviço de armazenamento (`IUploadFileStorageService`) e garanta permissões.
- Porta em uso — mude a porta via `ASPNETCORE_URLS` ou `launchSettings.json`.
- Para builds `PublishSingleFile` com trimming, teste em ambiente semelhante ao de produção — trimming pode remover algo necessário se reflexion estiver em uso.

Contribuição
- Abra issues para bugs ou sugestões.
- Faça PRs dirigidos à branch principal; siga o padrão de commits e inclua descrições de testes.

Licença
Este projeto é licenciado sob a licença MIT. Veja `LICENSE.txt`.

---

Se precisar, posso gerar comandos/publish específicos para outros RIDs (ex.: `linux-arm64`, `win-arm64`), gerar um `Dockerfile` pronto para o seu repo, ou descrever exatamente como ajustar `appsettings` e `User Secrets` para o ambiente de desenvolvimento.


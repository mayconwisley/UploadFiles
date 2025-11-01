# 📂 UploadFiles

**Repositório:** `UploadFiles`  
**Alvo:** .NET 9  
**Linguagem:** C#  

> Este README fornece instruções completas em **português (Brasil)** para configurar, compilar, executar, publicar e manter o projeto, incluindo **migrações com Entity Framework** e **execução via Docker**.

---

## 🧭 Índice
- [Visão Geral](#visão-geral)
- [Requisitos](#requisitos)
- [Estrutura do Repositório](#estrutura-do-repositório)
- [Configuração](#configuração)
- [Executando em Desenvolvimento](#executando-em-desenvolvimento)
  - [CLI (Linux / macOS / Windows)](#cli-linux--macos--windows)
  - [Visual Studio 2022](#visual-studio-2022)
- [Publicação](#publicação)
  - [Publicar para Linux](#publicar-para-linux)
  - [Publicar para Windows](#publicar-para-windows)
  - [Opções Avançadas](#opções-avançadas)
- [Migrações e Banco de Dados (EF)](#migrações-e-banco-de-dados-entity-framework)
- [Executando em Docker](#executando-em-docker)
- [Dicas e Solução de Problemas](#dicas-e-solução-de-problemas)
- [Endpoints e Versionamento](#endpoints-e-versionamento)
- [Testes](#testes)
- [Contribuição](#contribuição)
- [Licença](#licença)
- [Contato / Suporte](#contato--suporte)
- [Observações Finais](#observações-finais)

---

## 🌐 Visão Geral
Este projeto implementa uma **API** para gerenciamento e armazenamento de arquivos.  
A solução é dividida em quatro camadas principais:

- `UploadFiles.Api` — API (ponto de entrada)
- `UploadFiles.App` — lógica de aplicação / *use cases*
- `UploadFiles.Infra` — infraestrutura (repositórios, `DbContext`, etc.)
- `UploadFiles.Domain` — entidades e abstrações

---

## ⚙️ Requisitos
- .NET 9 SDK instalado  
  ```bash
  dotnet --list-sdks
  ```
- (Opcional) `dotnet-ef` instalado para migrações:
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- Banco de dados configurado (SQL Server, PostgreSQL, etc.)
- Visual Studio 2022
- Permissões de escrita nas pastas de armazenamento de arquivos

---

## 🗂️ Estrutura do Repositório (exemplo)
```
UploadFiles.sln
UploadFiles.Api/
UploadFiles.App/
UploadFiles.Infra/
UploadFiles.Domain/
```

---

## 🔧 Configuração

1. Defina a **connection string** e outras configurações em  
   `UploadFiles.Api/appsettings.json` ou `appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=UploadFilesDb;User Id=sa;Password=Your_password123;"
     },
     "Storage": {
       "Path": "C:\\UploadFiles\\Storage"
     }
   }
   ```

2. (Recomendado) Use **variáveis de ambiente** para segredos:

   **PowerShell:**
   ```powershell
   $env:ConnectionStrings__DefaultConnection = "Server=...;Database=...;User Id=...;Password=..."
   $env:ASPNETCORE_ENVIRONMENT = "Development"
   ```

   **Linux / macOS:**
   ```bash
   export ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=..."
   export ASPNETCORE_ENVIRONMENT=Development
   ```

---

## ▶️ Executando em Desenvolvimento

### CLI (Linux / macOS / Windows)
```bash
dotnet build
dotnet run --project ./UploadFiles.Api/UploadFiles.Api.csproj
```

Com variáveis de ambiente:
```bash
export ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=..."
export ASPNETCORE_ENVIRONMENT=Development
dotnet run --project ./UploadFiles.Api/UploadFiles.Api.csproj
```

---

### Visual Studio 2022
1. Abra `UploadFiles.sln`
2. Clique com o botão direito em `UploadFiles.Api` → **Set as Startup Project**
3. Configure `appsettings.Development.json` e variáveis via `Debug > Profiles`
4. Pressione `F5` para depurar ou `Ctrl + F5` para executar sem depuração

---

## 🚀 Publicação

### Publicar para Linux (self-contained)
```bash
dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj \
  -c Release -r linux-x64 --self-contained true \
  -p:PublishSingleFile=true -o ./publish/linux-x64
```

### Publicar para Windows (self-contained)
```bash
dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj \
  -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true -o ./publish/win-x64
```

### Publicação framework-dependent
```bash
dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -o ./publish/fd
```

### Opções Avançadas
Ativar *trimming* (reduz o tamanho do binário — testar antes):
```bash
-p:PublishTrimmed=true
```

---

## 🧱 Migrações e Banco de Dados (Entity Framework)

### Instalar EF CLI
```bash
dotnet tool install --global dotnet-ef
```

### Criar uma migration
```bash
dotnet ef migrations add InitialCreate \
  --context UploadFilesDbContext \
  --project UploadFiles.Infra \
  --startup-project UploadFiles.Api \
  -o Migrations
```

### Aplicar migrations
```bash
dotnet ef database update \
  --context UploadFilesDbContext \
  --project UploadFiles.Infra \
  --startup-project UploadFiles.Api
```

---

## 🐳 Executando em Docker

### Exemplo de `Dockerfile`
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish ./UploadFiles.Api/UploadFiles.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UploadFiles.Api.dll"]
```

### Build e execução
```bash
docker build -t uploadfiles:latest .
docker run -e ConnectionStrings__DefaultConnection="Server=...;" -p 5000:80 uploadfiles:latest
```

---

## 🧩 Dicas e Solução de Problemas
- **Erro "SDK 9.x not found"**  
  → Verifique instalação com `dotnet --list-sdks`
- **Erro EF “No migrations”**  
  → Confirme `--startup-project` e `--project`
- **Permissões de escrita**  
  → Confirme o caminho de armazenamento
- **Porta em uso**  
  → Altere via `ASPNETCORE_URLS` ou `launchSettings.json`
- **Problemas com trimming**  
  → Teste em ambiente real (pode afetar reflexão e libs nativas)

---

## 🔗 Endpoints e Versionamento
- A API usa versionamento com atributo:
  ```csharp
  [ApiVersion("1.0")]
  ```
- Rotas seguem o formato:
  ```
  /api/v{version}/[controller]
  ```
  Exemplo:
  ```
  GET /api/v1.0/GenerateKey
  ```

---

## 🧪 Testes
Se existirem projetos de teste:
```bash
dotnet test
```

---

## 🤝 Contribuição
- Abra *issues* para bugs ou sugestões  
- Envie *Pull Requests* para a branch principal  
- Descreva as alterações e testes realizados  
- Siga o padrão de commits do repositório (se existir)

---

## ⚖️ Licença
Este projeto utiliza a **licença MIT**.  
Consulte o arquivo `LICENSE.txt` para mais detalhes.

---

## 📞 Contato / Suporte
- Abra uma *issue* no repositório para dúvidas ou suporte  
- Inclua informações relevantes (versão .NET, passos para reproduzir, logs)

---

## 📝 Observações Finais
- Ajuste nomes de contexto/projetos conforme seu ambiente  
- Mantenha `appsettings.*.json` e segredos **fora do controle de versão**  
- Utilize variáveis de ambiente ou *secrets manager* para CI/CD  

---

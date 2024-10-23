FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Instalar as dependências e o wkhtmltopdf
RUN apt-get update && apt-get install -y \
    libgdiplus \
    fontconfig \
    xfonts-75dpi \
    xfonts-base \
    wkhtmltopdf \
    && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WslPdfApi.csproj", "./"]
RUN dotnet restore "./WslPdfApi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WslPdfApi.dll"]


#comandos para buidar
#docker build -t wslpdfapi .
#docker run -d -p 5000:8080 --name wslpdfapi_container wslpdfapi


#--docker build -t (t=interativo) (nome da imagem) (onde)
#-- docker run -d (-d libera o terminal) -p (porta) (5000 mapeada para 8080 do container) (nome do container) (nome da imagem usada)

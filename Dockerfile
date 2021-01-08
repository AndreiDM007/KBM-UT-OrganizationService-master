FROM microsoft/dotnet:2.2.0-aspnetcore-runtime-alpine3.8

# required package to actually be a gRPC client|server
RUN apk update && apk add libc6-compat

WORKDIR /app
COPY OrganizationService.Web/out .

ENTRYPOINT ["dotnet", "/app/Kebormed.Core.OrganizationService.Web.dll"]



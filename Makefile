run:
	dotnet build ; \
	cd OrganizationService.Web ; \
    ASPNETCORE_ENVIRONMENT="Development" \
    CONNECTIONSTRINGS__ORGANIZATIONSERVICEDATACONTEXT='Server=tcp:127.0.0.1,1433;Initial Catalog=core_organizationservice;Persist Security Info=False;User ID=sa;Password=123.mssql.321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=Yes;Connection Timeout=30;' \
    HAZELCASTCONFIG__ADDRESS='127.0.0.1:5701' \
    dotnet ./bin/Debug/netcoreapp2.2/Kebormed.Core.OrganizationService.Web.dll
    
publish:    
	dotnet publish -c Release -o out

docker-build: publish
	docker build --rm -t=core-organizationservice .

docker-run: docker-build
	docker run \
		--network corelocalsetup_default \
		-e ASPNETCORE_ENVIRONMENT='Development' \
		-e CONNECTIONSTRINGS__ORGANIZATIONSERVICEDATACONTEXT='Server=tcp:core-mssql,1433;Initial Catalog=core_OrganizationService;Persist Security Info=False;User ID=sa;Password=123.mssql.321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=Yes;Connection Timeout=30;' \
		-e HAZELCASTCONFIG__ADDRESS='TBD TBD TBD TBD TBD TBD TBD TBD TBD TBD TBD TBD TBD' \
		-p 5008:5000 \
		--name core-organizationservice \
		-d core-organizationservice && \
	docker ps

docker-clean:
		docker kill core-organizationservice; docker rm -v core-organizationservice

using System;
using System.Threading.Tasks;
using Grpc.Core;
using Jaeger.Util;
using Kebormed.Core.OrganizationService.Grpc.Generated;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class HealthController : HealthService.HealthServiceBase
    {
        public override Task<Ping.Types.Response> Ping(Ping.Types.Request request, ServerCallContext context)
        {
            var res = new Ping.Types.Response
            {
                Timestamp = DateTime.UtcNow.ToUnixTimeMicroseconds()
            };
            return Task.FromResult(res);
        }
    }
}
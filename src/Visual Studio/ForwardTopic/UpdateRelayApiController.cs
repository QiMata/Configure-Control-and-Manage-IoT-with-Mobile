using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Azure.Devices;

namespace ForwardTopic
{
    public class UpdateRelayApiController : ApiController
    {
        private readonly ServiceClient _serviceClient;

        public UpdateRelayApiController()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString("{Enter azure iot connection string here}");
        }

        public async Task<IHttpActionResult> Post()
        {
            await _serviceClient.SendAsync("{Enter device name here}", new Message(await Request.Content.ReadAsByteArrayAsync()));
            return Ok();
        }
    }
}
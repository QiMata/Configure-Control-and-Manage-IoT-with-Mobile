using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;

namespace ForwardTopic
{
    public class TopicHub : Hub
    {
        private const string TopicConnectionString =
                "{Enter your topic conneciton string}"
            ;

        private SubscriptionClient _subscriptionClient;

        public TopicHub()
        {
            _subscriptionClient = SubscriptionClient.CreateFromConnectionString(TopicConnectionString, "{Enter the topic name}", "{Enter the subscription name}");
            _subscriptionClient.OnMessage(DataRecieved);
        }

        private void DataRecieved(BrokeredMessage obj)
        {
            using (var stream = obj.GetBody<Stream>())
            {
                var bytes = ReadFully(stream);
                var str = Encoding.UTF8.GetString(bytes);
                Clients.All.NewData(str);
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
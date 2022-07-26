using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Intacct.SDK;
using Intacct.SDK.Logging;

namespace IntacctSDKCustomDateTimeExample
{
    public static class Bootstrap
    {
        public static OnlineClient Client(ILogger logger, IConfiguration config)
        {
            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = config.GetValue<string>("sender_id"),
                SenderPassword = config.GetValue<string>("sender_password"),
                CompanyId = config.GetValue<string>("company_id"),
                UserId = config.GetValue<string>("user_id"),
                UserPassword = config.GetValue<string>("user_password"),
                Logger = logger,
                LogLevel = LogLevel.Information,
                LogMessageFormatter = new MessageFormatter("\"{method} {target} HTTP/{version}\" {code}"),
            };
            OnlineClient client = new OnlineClient(clientConfig);

            return client;
        }
    }
}

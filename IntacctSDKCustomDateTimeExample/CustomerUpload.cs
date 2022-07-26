using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FayezCustomerUpload.Models;
using Intacct.SDK;
using Intacct.SDK.Functions.AccountsReceivable;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;

namespace IntacctSDKCustomDateTimeExample
{
    internal class CustomerUpload
    {
        public string Errors { get; set; } = string.Empty;

        private readonly ILogger<CustomerUpload> _log;
        private readonly IConfiguration _config;
        public CustomerUpload(ILogger<CustomerUpload> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }
        public void Run()
        {
            UploadCustomers();
        }
        public string UploadCustomers()
        {
            OnlineClient client = Bootstrap.Client(_log, _config);
            _log.LogInformation("Beginning upload via API");
            CustomerModel customer = new CustomerModel();
            customer.CustomerNo = "000001";
            customer.CustomerName = "Test Customer";
            customer.DateEstablished = DateTime.Today;

            Dictionary<string, dynamic> custFields = new Dictionary<string, dynamic>();

            custFields.Add("DATE_ESTABLISHED", customer.DateEstablished);

            CustomerCreate create = new CustomerCreate()
            {
                CustomerId = "00000001",
                CustomerName = "Some Test Customer",
                StateProvince = "TX",
                Country = "US",
                RestrictionType = "Restricted",
                RestrictedLocations = new List<string> { "010" },
                CustomFields = custFields,
                //TODO - Figure out what we're doing with the other UDF's and fields in the .csv file
            };

            try
            {
                Task<OnlineResponse> createTask = client.Execute(create);
                createTask.Wait();
                OnlineResponse createResponse = createTask.Result;
                Result createResult = createResponse.Results[0];

                return "Created Customer";
            }
            catch (Exception excep)
            {
                if (excep.InnerException != null)
                {
                    Errors = excep.Message + ": " + excep.InnerException.Message;
                }
                else
                {
                    Errors = excep.Message;
                }
                _log.LogError("Customer {CustomeNo} - Exception: {Error}{Newline} Stack: {Stack}", customer.CustomerNo, Errors, Environment.NewLine, excep.StackTrace);
                return null;
            }
        }
    }
}

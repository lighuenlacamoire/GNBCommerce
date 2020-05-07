using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Domain.External
{
    public class RestService : ExternalService<IRestResponse>
    {
        IRestClient _client;

        #region Constructor
        public RestService(string source)
        {
            _client = new RestClient(source);
        }
        #endregion

        #region Methods
        public override IRestResponse Invoke(string parameters = null)
        {
            var request = new RestRequest(parameters, Method.GET);
            return _client.Execute(request);
        }
        #endregion
    }
}

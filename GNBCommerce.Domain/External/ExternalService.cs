using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Domain.External
{
    public abstract class ExternalService<TResponse> where TResponse : class
    {
        public abstract TResponse Invoke(string parameters = null);
    }
}

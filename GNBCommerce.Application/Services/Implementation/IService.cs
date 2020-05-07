﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Application.Services.Implementation
{
    public interface IService<TObject> where TObject : class
    {
        IEnumerable<TObject> GetAll();
    }
}

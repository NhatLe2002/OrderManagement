﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<bool> CommitAsync();
    }
}

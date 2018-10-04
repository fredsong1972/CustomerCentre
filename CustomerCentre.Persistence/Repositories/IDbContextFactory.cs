using System;
using System.Collections.Generic;
using System.Text;
using CustomerCentre.Persistence.Models;

namespace CustomerCentre.Persistence.Repositories
{
    public interface IDbContextFactory
    {
        CustomerCentreDbContext DbContext { get; }
    }
}

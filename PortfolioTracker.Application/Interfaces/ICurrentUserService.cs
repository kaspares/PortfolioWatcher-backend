using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string userId { get; }
    }
}

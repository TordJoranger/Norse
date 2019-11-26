using System.Collections.Generic;
using DevTest.Models;

namespace DevTest.Services
{
    public interface IRouteService
    {
        Route GetRoute(string start, string destination);
    }
}

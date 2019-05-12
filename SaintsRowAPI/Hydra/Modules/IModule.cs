using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Hydra.Modules
{
    interface IModule
    {
        void HandleRequest(HydraRequest request);
    }
}

using MXtend.Contracts.Services;
using MXtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXtend;

public class MXApp : Application
{
    public MXApp(IServiceProvider serviceProvider)
    {
        MXBuilder.RegisterProvider(serviceProvider);
    }
}

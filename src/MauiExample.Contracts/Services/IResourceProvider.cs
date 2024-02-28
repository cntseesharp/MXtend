using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiExample.Contracts.Services
{
    public interface IResourceProvider
    {
        Task<Stream> GetStream(string resourceName);
    }
}

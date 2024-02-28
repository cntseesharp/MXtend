using MauiExample.Contracts.Services;
using MXtend.Attributes;

namespace MauiExample.Platforms.Windows.Dependencies
{
    [RegisterDependency(Type = DependencyType.Singleton)]
    public class ResourceProvider : IResourceProvider
    {
        public async Task<Stream> GetStream(string resourceName)
        {
            if (!await FileSystem.AppPackageFileExistsAsync(resourceName))
                throw new FileNotFoundException(resourceName);

            Stream stream = await FileSystem.OpenAppPackageFileAsync(resourceName);
            return stream;
        }
    }
}

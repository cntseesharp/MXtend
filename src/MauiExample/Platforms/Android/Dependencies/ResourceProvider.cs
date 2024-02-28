using MauiExample.Contracts.Services;
using MXtend.Attributes;

namespace MauiExample.Platforms.Android.Dependencies
{
    [RegisterDependency(Type = DependencyType.Singleton)]
    public class ResourceProvider : IResourceProvider
    {
        public async Task<Stream> GetStream(string resourceName)
        {
            if (!await FileSystem.AppPackageFileExistsAsync(resourceName))
                throw new FileNotFoundException(resourceName);

            Stream stream = await FileSystem.OpenAppPackageFileAsync(resourceName);

            // Android implementation returns stream abstraction that is unsupported
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

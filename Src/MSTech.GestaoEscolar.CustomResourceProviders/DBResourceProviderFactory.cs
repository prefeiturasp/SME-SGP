using System.Web.Compilation;

namespace MSTech.GestaoEscolar.CustomResourceProviders
{

    public class DBResourceProviderFactory : ResourceProviderFactory
    {

        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new DBResourceProvider(classKey);
        }
        
        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            // we should always get a path from the runtime
            string classKey = virtualPath;
            if (!string.IsNullOrEmpty(virtualPath))
            {
                virtualPath = virtualPath.Remove(0, 1);
                classKey = virtualPath.Remove(0, virtualPath.IndexOf('/') + 1);
            }

            return new DBResourceProvider(classKey);
        }
    }
}
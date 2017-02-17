using System.Globalization;

namespace MSTech.GestaoEscolar.CustomResourceProviders
{
    public class CustomResource
    {
        public static string GetGlobalResourceObject(string classKey, string resourceKey)
        {
            DBResourceProvider provider = new DBResourceProvider(classKey);
            return provider.GetObject(resourceKey, CultureInfo.CurrentUICulture) as string;
        }
    }
}

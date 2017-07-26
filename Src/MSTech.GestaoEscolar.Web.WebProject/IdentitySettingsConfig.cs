using System.Web.Configuration;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class IdentitySettingsConfig
    {
        private static IdentitySettings idsSettings = null;

        public static string Msg { get; private set; } = string.Empty;
        public static IdentitySettings IDSSettings
        {
            get
            {
                if (idsSettings == null)
                {
                    idsSettings = ConfigureIdentitySettingsWebConfig();
                    return idsSettings;
                }
                return idsSettings;
            }
        }

        private static IdentitySettings ConfigureIdentitySettingsWebConfig()
        {
            try
            {
                IdentitySettings settings = new IdentitySettings
                {
                    Cookies_AuthenticationType = WebConfigurationManager.AppSettings["Cookies_AuthenticationType"],
                    Cookies_CookieName = WebConfigurationManager.AppSettings["Cookies_CookieName"],
                    Cookies_LoginPath = WebConfigurationManager.AppSettings["Cookies_LoginPath"],
                    Cookies_CookieDomain = WebConfigurationManager.AppSettings["Cookies_CookieDomain"],
                    AuthenticationType = WebConfigurationManager.AppSettings["AuthenticationType"],
                    Authority = WebConfigurationManager.AppSettings["Authority"],
                    ClientId = WebConfigurationManager.AppSettings["ClientId"],
                    ClientSecret = WebConfigurationManager.AppSettings["ClientSecret"],
                    EndpointUserInfo = WebConfigurationManager.AppSettings["EndpointUserInfo"],
                    RedirectUri = WebConfigurationManager.AppSettings["RedirectUri"],
                    ResponseType = WebConfigurationManager.AppSettings["ResponseType"],
                    Scope = WebConfigurationManager.AppSettings["Scope"],
                    SignInAsAuthenticationType = WebConfigurationManager.AppSettings["SignInAsAuthenticationType"]
                };

                if (!settings.IsValid)
                {
                    Msg = "Verifique as configurações no Web.config referente ao autenticador. Ex: ClientId e Authority.";
                }
                else
                {
                    return settings;
                }
            }
            catch (System.Exception)
            {
                Msg = "Ocorreu um erro ao ler as configurações do autenticador no Web.config. Veja o log de erros do sistema.";
            }

            return null;
        }

    }

    public class IdentitySettings
    {
        public IdentitySettings() { }

        public string Cookies_AuthenticationType { get; set; }
        public string Cookies_CookieName { get; set; }
        public string Cookies_LoginPath { get; set; }
        public string Cookies_CookieDomain { get; set; }
        public string AuthenticationType { get; set; }
        public string SignInAsAuthenticationType { get; set; }
        public string Authority { get; set; }
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string ResponseType { get; set; }
        public string EndpointUserInfo { get; set; }

        public bool IsValid
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ClientId)
                    && !string.IsNullOrWhiteSpace(this.Authority))
                {
                    return true;
                }
                return false;
            }
        }
    }
}

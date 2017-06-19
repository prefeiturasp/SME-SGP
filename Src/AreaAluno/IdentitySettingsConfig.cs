using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace AreaAluno
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
                    idsSettings = ConfigureIdentitySettings();
                    return idsSettings;
                }
                return idsSettings;
            }
        }


        private static IdentitySettings ConfigureIdentitySettings()
        {
            var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonConvert.DefaultSettings = () => { return defaultSettings; };

            try
            {
                //throw new System.Exception("Forcando um erro.");
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/identitysettings.json");
                if (!File.Exists(mappedPath))
                {
                    Msg = "Arquivo de configuração identitysettings.json não foi localizado.";
                }
                else
                {
                    var jsonFile = File.ReadAllText(mappedPath);
                    if (string.IsNullOrEmpty(jsonFile))
                    { 
                        Msg = "O arquivo de configuração identitysettings.json está vazio.";
                    }
                    else
                    {
                        var settings = JsonConvert.DeserializeObject<IdentitySettings>(jsonFile);
                        if (settings == null)
                        {
                            Msg = "Não foi possível ler o arquivo de configuração identitysettings.json.";
                        }
                        else
                        {
                            if (!settings.IsValid)
                            {
                                Msg = "O arquivo de configuração identitysettings.json possui informações incorretas. Verifique o ClientId e Authority";
                            }
                            else
                            {
                                return settings;
                            }
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                //TODO: Gravar log de Erro
                Msg = "Ocorreu um erro ao ler o arquivo de configuração identitysettings.json. Veja o log de erros do sistema.";
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
                if(!string.IsNullOrWhiteSpace(this.ClientId) 
                    && !string.IsNullOrWhiteSpace(this.Authority))
                {
                    return true;
                }
                return false;
            }
        } 
    }

}

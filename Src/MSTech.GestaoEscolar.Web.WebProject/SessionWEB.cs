using System;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject.WebArea;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    [Serializable]
    public class SessionWEB : MSTech.Web.WebProject.SessionWEB
    {
        #region Atributos

        private string postMessages = string.Empty;

        private string usuarioLogado = string.Empty;
        private string tituloSistema = string.Empty;

        private readonly string tituloGeral = string.Empty;
        private readonly string mensagemCopyright = string.Empty;
        private readonly string urlCoreSSO = string.Empty;

        private string logoImagemCaminho = string.Empty;
        private string urlLogoCabecalho;
        private string urlInstituicao = string.Empty;
        private string logoCliente = string.Empty;
        private string temaPadrao = string.Empty;

        private readonly string helpDeskContato = string.Empty;

        private int bib_id = -1;

        #endregion Atributos

        #region Construtores

        public SessionWEB()
        {
            __UsuarioWEB = new UsuarioWEB();

            //Armazena titulo geral do sistema definido nos parâmetros do CoreSSO

            tituloGeral = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TITULO_GERAL);

            //Armazena URL do sistema administrativo definido nos parâmetros do CoreSSO
            urlCoreSSO = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.URL_ADMINISTRATIVO);

            //Armazena URL do cliente definido nos parâmetros do CoreSSO
            urlInstituicao = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.URL_CLIENTE);

            //Armazena mensagem de copyright definido nos parâmetros do CoreSSO
            mensagemCopyright = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.MENSAGEM_COPYRIGHT);

            //Armazena contato do help desk definido nos parâmetros do CoreSSO
            helpDeskContato = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.HELP_DESK_CONTATO);
        }

        #endregion Construtores

        #region Propriedades

        /// <summary>
        /// Guarda em sessão a busca feita pelo usuário nas telas de cadastro.
        /// </summary>
        public BuscaGestao BuscaRealizada
        {
            get;
            set;
        }
               
        /// <summary>
        /// Retorna valor do ID da cidade referente a Entidade do usuário.
        /// </summary>
        public Guid _cid_id { get; set; }

        public new UsuarioWEB __UsuarioWEB { get; set; }

        /// <summary>
        /// Recebe e atribui mensagens a serem transferidas em um post ou redirect.
        /// Ao ler a mensagens enviada a propriedade é automaticamente limpada.
        /// </summary>
        public string PostMessages
        {
            get
            {
                try
                {
                    return postMessages;
                }
                finally
                {
                    postMessages = string.Empty;
                }
            }
            set
            {
                postMessages = value;
            }
        }

        /// <summary>
        /// Armazena o nome da pessoa ou o login do
        /// usuário logado no sistema
        /// </summary>
        public string UsuarioLogado
        {
            get
            {
                return usuarioLogado;
            }
            set
            {
                usuarioLogado = value;
            }
        }

        /// <summary>
        /// Armazena o tema do sistema atual
        /// Disponível no web.config do sistema atual
        /// </summary>
        public string TemaPadrao
        {
            get
            {
                return temaPadrao;
            }
            set
            {
                temaPadrao = value;
            }
        }

        /// <summary>
        /// Armazena o título geral do sistema
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string TituloGeral
        {
            get
            {
                return tituloGeral;
            }
        }

        /// <summary>
        /// Armazena o título do sistema atual
        /// Disponível na tabela SYS_Sitema do CoreSSO
        /// Depois de logado
        /// </summary>
        public string TituloSistema
        {
            get
            {
                return tituloSistema;
            }
            set
            {
                tituloSistema = value;
            }
        }

        /// <summary>
        /// Armazena a mensagem de copyright
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string MensagemCopyright
        {
            get
            {
                return mensagemCopyright;
            }
        }

        /// <summary>
        /// Armazena a URL do sistema administrativo (CoreSSO)
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string UrlCoreSSO
        {
            get
            {
                return urlCoreSSO;
            }
        }

        /// <summary>
        /// Armazena a URL do logo do cabeçalho.
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string UrlLogoCabecalho
        {
            get
            {
                return urlLogoCabecalho;
            }
            set
            {
                urlLogoCabecalho = value;
            }
        }

        /// <summary>
        /// Armazena a URL da Instituição
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string UrlInstituicao
        {
            get
            {
                return urlInstituicao;
            }
            set
            {
                urlInstituicao = value;
            }
        }

        /// <summary>
        /// Armazena o contato do help desk do cliente
        /// Disponível na tabela de parâmetros do CoreSSO.
        /// </summary>
        public string HelpDeskContato
        {
            get
            {
                return HttpUtility.HtmlDecode(helpDeskContato);
            }
        }

        /// <summary>
        /// Armazena o caminho da pasta de logos
        /// </summary>
        public string LogoImagemCaminho
        {
            get
            {
                if (String.IsNullOrEmpty(logoImagemCaminho))
                {
                    // Pega o tema da página que chamou.
                    temaPadrao = HttpContext.Current.Handler is Page ? ((Page)HttpContext.Current.Handler).Theme ?? string.Empty : string.Empty;

                    if (string.IsNullOrEmpty(temaPadrao))
                    {
                        return "/Default/images/logos/";
                    }

                    logoImagemCaminho = "/" + temaPadrao + "/images/logos/";
                }

                return logoImagemCaminho;
            }
        }

        /// <summary>
        /// Armazena a url do logo geral no Sistema Administrativo
        /// </summary>
        public string UrlLogoGeral
        {
            get
            {
                return LogoImagemCaminho + "LOGO_GERAL_SISTEMA.png";
            }
        }

        /// <summary>
        /// Armazena a url do logo do sistema no Sistema Administrativo
        /// </summary>
        public string UrlLogoSistema
        {
            get
            {
                return LogoImagemCaminho + urlLogoCabecalho;
            }
        }

        public string LogoCliente
        {
            get
            {
                return logoCliente;
            }
            set
            {
                logoCliente = value;
            }
        }

        /// <summary>
        /// Armazena a url do logo da instituição no Sistema Administrativo
        /// </summary>
        public string UrlLogoInstituicao
        {
            get
            {
                return LogoImagemCaminho + LogoCliente;
            }
        }

        /// <summary>
        /// Retorna informações da área onde o usuário está logado como
        /// diretório virtual, de imagens e includes, titulo da página entre outros.
        /// </summary>
        public Area _AreaAtual { get; set; }

        /// <summary>
        /// Retorna o id da biblioteca do usuário logado.
        /// </summary>
        public int _Bib_id
        {
            get
            {
                return bib_id;
            }
            set
            {
                bib_id = value;
            }
        }

        #endregion Propriedades
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno
{
    public partial class Manutencao : MotherPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblDataErro.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                string caminhoErro = __SessionWEB._AreaAtual._Diretorio + "App_Themes/" + NomeTemaAtual + "/images/" + "error.png";
                string caminhoAlerta = __SessionWEB._AreaAtual._Diretorio + "App_Themes/" + NomeTemaAtual + "/images/" + "warning.png";

                imgErro.ImageUrl = caminhoErro;

                string erro = Convert.ToString(Request.QueryString["erro"]);
                erro = string.IsNullOrEmpty(erro) ? "0" : erro;

                switch (Convert.ToInt32(erro))
                {
                    case 403:
                        lblMensagem.Text = "Permissão de acesso negada.";
                        imgErro.ImageUrl = caminhoAlerta;
                        break;
                    case 404:
                        lblMensagem.Text = "A página solicitada não existe.";
                        imgErro.ImageUrl = caminhoAlerta;
                        break;
                    case 408:
                        lblMensagem.Text = "O servidor atingiu o tempo limite ao aguardar a solicitação.";
                        imgErro.ImageUrl = caminhoErro;
                        break;
                    case 500:
                        lblMensagem.Text = "Ocorreu um erro inesperado. Por favor, tente novamente. Se o problema persistir, avise a equipe de suporte e apoio da ferramenta.";
                        imgErro.ImageUrl = caminhoErro;
                        break;
                    case 503:
                        lblMensagem.Text = "O servidor está temporariamente indisponível.";
                        imgErro.ImageUrl = caminhoErro;
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
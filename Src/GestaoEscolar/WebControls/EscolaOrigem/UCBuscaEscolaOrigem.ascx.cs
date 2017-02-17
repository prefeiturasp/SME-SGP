using System.Web.UI;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.EscolaOrigem
{
    public partial class UCBuscaEscolaOrigem : MotherUserControl
    {
        #region DELEGATES

        public delegate void onNovo();
        public event onNovo NovaEscola;

        public void ChamarEventoNovaEscola()
        {
            if (NovaEscola != null)
                NovaEscola();
        }

        public delegate void onLimpar();
        public event onLimpar LimparEscola;

        public void ChamarEventoLimparEscola()
        {
            if (LimparEscola != null)
                LimparEscola();
        }

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Seta um titulo diferente do padrão para a escola de origem/destino
        /// </summary>
        public string Titulo
        {
            set
            {
                lblEscolaOrigemDestino.Text = value;
            }
        }

        /// <summary>
        /// Seta um texto para a escola de origem/destino
        /// </summary>
        public string Texto
        {
            set
            {
                txtEscolaOrigemDestino.Text = value;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool ExibirBotaoLimpar
        {
            set
            {
                btnLimpar.Visible = value;
            }
        }

        public bool PermiteEditar
        {
            set
            {
                txtEscolaOrigemDestino.Enabled = value;
            }
        }

        #endregion

        #region EVENTOS

        protected void btnEscolaOrigemDestino_Click(object sender, ImageClickEventArgs e)
        {
            // Evento para adicionar uma nova escola
            ChamarEventoNovaEscola();

            // Abre a busca de escolas de origem/destino cadastradas
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigemDestinoAbrir", "$('#divBuscaEscolaOrigemDestino').dialog('open');", true);
        }

        protected void btnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            // Evento para limpar os dados da escola            
            ChamarEventoLimparEscola();

            txtEscolaOrigemDestino.Text = string.Empty;
            btnLimpar.Visible = false;
        }

        #endregion
    }
}
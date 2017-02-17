using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno.WebControls.Mensagens
{
    public partial class UCConfirmacaoOperacao : MotherUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCConfirmaOperacao.js"));
            }
        }

        #region DELEGATES

        public delegate void onConfimaOperacao();
        public event onConfimaOperacao ConfimaOperacao;

        public void ChamarConfimaOperacao()
        {
            if (ConfimaOperacao != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmacaoPadrão", "$('#divConfirmacao').dialog('close');", true);
                ConfimaOperacao();
            }
        }

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Seta o evento ClientClick do botão "Não", e depois fecha o dialog.
        /// </summary>
        public string btnNao_ClientClick_AntesFecharJanela
        {
            set
            {
                btnNao.OnClientClick = value + btnNao.OnClientClick;
            }
        }

        /// <summary>
        /// Seta uma mensagem para o usuário
        /// </summary>
        public string Mensagem
        {
            set
            {
                lblConfirmacao.Text = value;
            }
        }

        /// <summary>
        /// Seta um texto para o campo de observação
        /// </summary>
        public bool ObservacaoVisivel
        {
            set
            {
                divObservacao.Visible = value;
                UCCamposObrigatorios1.Visible = value;
            }
        }

        /// <summary>
        /// Seta um texto para o campo de observação
        /// </summary>
        public string ObservacaoTexto
        {
            set
            {
                lblObservacao.Text = value;
                rfvObservacao.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Retorna a observação digitada
        /// </summary>
        public string ObservacaoCampo
        {
            get
            {
                return txtObservacao.Text;
            }

            set
            {
                txtObservacao.Text = value;
            }
        }

        /// <summary>
        /// Propriedade que seta a label e a validação do combo
        /// </summary>
        public bool ObservacaoObrigatorio
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblObservacao);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblObservacao);
                }

                rfvObservacao.Visible = value;
                UCCamposObrigatorios1.Visible = value;
            }
        }

        #endregion

        #region EVENTOS

        protected void btnSim_Click(object sender, EventArgs e)
        {
            ChamarConfimaOperacao();
        }

        #endregion
    }
}
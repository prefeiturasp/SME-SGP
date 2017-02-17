using System;
using System.Web.UI;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Mensagens
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

        public void Update()
        {
            updConfirmacaoOperacao.Update();
        }

        /// <summary>
        /// Seta uma PostbackTrigger no updatepanel para que o botão "Sim" faça um postback na tela.
        /// </summary>
        public void SetaTriggerPostbackSim()
        { 
            PostBackTrigger trigger = new PostBackTrigger();
            trigger.ControlID = "btnSim";

            this.updConfirmacaoOperacao.Triggers.Add(trigger);
        }

        #region DELEGATES

        public delegate void onConfimaOperacao();
        public event onConfimaOperacao ConfimaOperacao;

        public delegate void onCancelaOperacao();
        public event onCancelaOperacao CancelaOperacao;

        public void ChamarConfimaOperacao()
        {
            if (ConfimaOperacao != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmacaoPadrão", "$('#divConfirmacao').dialog('close');", true);
                ConfimaOperacao();
            }
        }

        public void ChamarCancelaOperacao()
        {
            if (CancelaOperacao != null)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharConfirmacaoPadrão", "$('#divConfirmacao').dialog('close');", true);
                CancelaOperacao();
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

        public bool EventBtnNao
        {
            set
            {
                btnNao.Visible = !value;
                btnNaoClick.Visible = value;
            }
        }

        #endregion

        #region EVENTOS

        protected void btnSim_Click(object sender, EventArgs e)
        {
            ChamarConfimaOperacao();
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            ChamarCancelaOperacao();
        }

        #endregion
    }
}
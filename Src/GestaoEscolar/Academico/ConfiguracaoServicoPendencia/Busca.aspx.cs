using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ConfiguracaoServicoPendencia
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades
        /// <summary>
        /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
        /// </summary>
        private bool ParametroPermanecerTela
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tne_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tne_id
        {
            get
            {
                if (ViewState["VS_tne_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tne_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tne_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tme_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_tme_id
        {
            get
            {
                if (ViewState["VS_tme_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_tme_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_tme_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de tur_tipo
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private byte VS_tur_tipo
        {
            get
            {
                if (ViewState["VS_tur_tipo"] != null)
                {
                    return Convert.ToByte(ViewState["VS_tur_tipo"]);
                }
                return 0;
            }
            set
            {
                ViewState["VS_tur_tipo"] = value;
            }
        }

        #endregion       

        #region Métodos
        private void Inicializa()
        {
            divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            btnCancelar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

            UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
            UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();

            setValorCombos(-1, -1, 0);
        }

        private void setValorCombos(int tne_id, int tme_id, byte tur_tipo)
        {
            UCComboTipoNivelEnsino.Valor = tne_id <= 0 ? -1 : tne_id;
            UCComboTipoModalidadeEnsino.Valor = tme_id <= 0 ? -1 : tme_id;
            UCComboTipoTurma.Valor = tur_tipo <= 0 ? Convert.ToByte(0) : tur_tipo;
        }

        private void VerificaCadastro()
        {
            fdsConfiguracao.Visible = VS_tne_id > 0 && VS_tme_id > 0 && VS_tur_tipo > 0;
            UCComboTipoNivelEnsino.PermiteEditar = UCComboTipoModalidadeEnsino.PermiteEditar = UCComboTipoTurma.PermiteEditar = !fdsConfiguracao.Visible;
            if (fdsConfiguracao.Visible)
            {
                LoadByEntity(VS_tne_id, VS_tme_id, VS_tur_tipo);
            }
        }

        private void LoadByEntity(int tne_id, int tme_id, byte tur_tipo)
        {
            ACA_ConfiguracaoServicoPendencia entity = ACA_ConfiguracaoServicoPendenciaBO.SelectBy_tne_id_tme_id_tur_tipo(tne_id, tme_id, tur_tipo);
            if (entity.csp_id > 0)
            {
                chkDisciplinaSemAula.Checked = entity.csp_disciplinaSemAula;
                chkSemNota.Checked = entity.csp_semNota;
                chkSemParecer.Checked = entity.csp_semParecer;
                chkSemPlanejamento.Checked = entity.csp_semPlanejamento;
                chkSemResultadoFinal.Checked = entity.csp_semResultadoFinal;
                chkSemSintese.Checked = entity.csp_semSintese;
                chkSemPlanoAula.Checked = entity.csp_semPlanoAula;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            UCComboTipoNivelEnsino.IndexChanged += UCComboTipoNivelEnsino_IndexChanged;
            UCComboTipoModalidadeEnsino.IndexChanged += UCComboTipoModalidadeEnsino_IndexChanged;
            UCComboTipoTurma.IndexChanged += UCComboTipoTurma_IndexChanged;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                try
                {
                    Inicializa();
                    Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
            Inicializa();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            ACA_ConfiguracaoServicoPendencia entity = ACA_ConfiguracaoServicoPendenciaBO.SelectBy_tne_id_tme_id_tur_tipo(UCComboTipoNivelEnsino.Valor, UCComboTipoModalidadeEnsino.Valor, UCComboTipoTurma.Valor);

            entity.tne_id = UCComboTipoNivelEnsino.Valor;
            entity.tme_id = UCComboTipoModalidadeEnsino.Valor;
            entity.tur_tipo = UCComboTipoTurma.Valor;
            entity.csp_disciplinaSemAula = chkDisciplinaSemAula.Checked;
            entity.csp_semNota = chkSemNota.Checked;
            entity.csp_semParecer = chkSemParecer.Checked;
            entity.csp_semPlanejamento = chkSemPlanejamento.Checked;
            entity.csp_semResultadoFinal = chkSemResultadoFinal.Checked;
            entity.csp_semSintese = chkSemSintese.Checked;
            entity.csp_semPlanoAula = chkSemPlanoAula.Checked;
            entity.IsNew = entity.csp_id <= 0;

            if (ACA_ConfiguracaoServicoPendenciaBO.Save(entity))
            {
                ApplicationWEB._GravaLogSistema(entity.IsNew ? LOG_SistemaTipo.Insert : LOG_SistemaTipo.Update, "Cadastro de configuração de serviço de pendência. csp_id" + entity.csp_id);
                string message = UtilBO.GetErroMessage("Configuração de servico de pendência gravada com sucesso.", UtilBO.TipoMensagem.Sucesso);

                if (ParametroPermanecerTela)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = message;
                    VerificaCadastro();
                    setValorCombos(VS_tne_id, VS_tme_id, VS_tur_tipo);
                }
                else
                {
                    __SessionWEB.PostMessages = message;
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Inicializa();
                }
            }
        }

        protected void UCComboTipoNivelEnsino_IndexChanged()
        {
            VS_tne_id = UCComboTipoNivelEnsino.Valor;
            VerificaCadastro();
        }

        protected void UCComboTipoModalidadeEnsino_IndexChanged()
        {
            VS_tme_id = UCComboTipoModalidadeEnsino.Valor;
            VerificaCadastro();
        }

        protected void UCComboTipoTurma_IndexChanged()
        {
            VS_tur_tipo = UCComboTipoTurma.Valor;
            VerificaCadastro();
        }

        #endregion
    }
}
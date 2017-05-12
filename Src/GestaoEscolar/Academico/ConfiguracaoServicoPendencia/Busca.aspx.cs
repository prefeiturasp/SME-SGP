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
                    //VerificaBusca();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                
                
                Page.Form.DefaultFocus = UCComboTipoNivelEnsino.ClientID;

                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnCancelar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ConfiguracaoServicoPendencia)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tne_id", out valor);
                UCComboTipoNivelEnsino.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tme_id", out valor);
                UCComboTipoModalidadeEnsino.Valor = Convert.ToInt32(valor);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_tipo", out valor);
                UCComboTipoTurma.Valor = Convert.ToByte(valor);                
            }
            else
            {
                fdsConfiguracao.Visible = false;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/ConfiguracaoServicoPendencia/Busca.aspx", false);
        }        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //TODO:[ANA] 
            ACA_ConfiguracaoServicoPendencia entity = ACA_ConfiguracaoServicoPendenciaBO.SelectBy_tne_id_tme_id_tur_tipo(UCComboTipoNivelEnsino.Valor, UCComboTipoModalidadeEnsino.Valor,UCComboTipoTurma.Valor);
            
            entity.tne_id = UCComboTipoNivelEnsino.Valor;
            entity.tme_id = UCComboTipoModalidadeEnsino.Valor;
            entity.tur_tipo = UCComboTipoTurma.Valor;
            entity.csp_disciplinaSemAula = chkDisciplinaSemAula.Checked;
            entity.csp_semNota = chkSemNota.Checked;
            entity.csp_semParecer = chkSemParecer.Checked;
            entity.csp_semPlanejamento = chkSemPlanejamento.Checked;
            entity.csp_semResultadoFinal = chkSemResultadoFinal.Checked;
            entity.IsNew = entity.csp_id < 0;

            if (ACA_ConfiguracaoServicoPendenciaBO.Save(entity))
            {
                ApplicationWEB._GravaLogSistema(entity.IsNew ? LOG_SistemaTipo.Insert : LOG_SistemaTipo.Update, "Cadastro de configuração de serviço de pendência. csp_id" + entity.csp_id);
                lblMessage.Text = UtilBO.GetErroMessage("Configuração de servico de pendência gravada com sucesso.", UtilBO.TipoMensagem.Sucesso);
            }


        }

        void VerificaAbrirCadastro()
        {
            fdsConfiguracao.Visible = UCComboTipoNivelEnsino.Valor > 0 && UCComboTipoModalidadeEnsino.Valor > 0 && UCComboTipoTurma.Valor > 0;
            UCComboTipoNivelEnsino.PermiteEditar = UCComboTipoModalidadeEnsino.PermiteEditar = UCComboTipoTurma.PermiteEditar = !fdsConfiguracao.Visible;            
        }

        protected void UCComboTipoNivelEnsino_IndexChanged()
        {
            VerificaAbrirCadastro();
        }

        protected void UCComboTipoModalidadeEnsino_IndexChanged()
        {
            VerificaAbrirCadastro();
        }

        protected void UCComboTipoTurma_IndexChanged()
        {
            VerificaAbrirCadastro();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.Declaracoes
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade que retorna o valor de rda_id.
        /// </summary>
        /// <value>
        /// edit_atg_id.
        /// </value>
        public int Edit_rda_id
        {
            get
            {
                return Convert.ToInt32(grvDeclaracoes.DataKeys[grvDeclaracoes.EditIndex].Values[0] ?? 0);
            }
        }

        /// <summary>
        /// Propriedade que retorna o valor de rlt_id.
        /// </summary>
        public int Edit_rlt_id
        {
            get
            {
                return Convert.ToInt32(grvDeclaracoes.DataKeys[grvDeclaracoes.EditIndex].Values[1] ?? 0);
            }
        }

        /// <summary>
        /// Propriedade que retorna o valor de pda_id.
        /// </summary>
        public int Edit_pda_id
        {
            get
            {
                return Convert.ToInt32(grvDeclaracoes.DataKeys[grvDeclaracoes.EditIndex].Values[2] ?? 0);
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Trata o numero de linhas por pagina da grid.
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvDeclaracoes.PageSize = UCComboQtdePaginacao1.Valor;
            grvDeclaracoes.PageIndex = 0;
            // atualiza o grid
            grvDeclaracoes.DataBind();
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocumentosAluno.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        _lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        _updDeclaracoes.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion

        #region Eventos
      
        protected void grvDeclaracoes_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CFG_RelatorioDocumentoAlunoBO.GetTotalRecords();
        }

        #endregion
    }
}
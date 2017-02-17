using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Linq;

namespace GestaoEscolar.Configuracao.CabecalhoAvisosTextosGerais
{
    public partial class BuscaCabecalhoAvisosTextos : MotherPageLogado
    {

        #region Propriedades
        /// <summary>
        /// Propriedade que retorna o valor de Edit_Cabecalho.
        /// </summary>
        /// <value>
        /// Edit_Cabecalho.
        /// </value>
        public int Edit_Cabecalho
        {
            get
            {
                return Convert.ToInt32(grvCabecalhos.DataKeys[grvCabecalhos.EditIndex].Values[0] ?? 0);
            }
        }
        #endregion

        #region Enumerador
        /// <summary>
        /// Enumerador com os tipos de avisos/textos gerais
        /// </summary>
        public enum eTiposAvisosTextosGerais : byte
        {
            [Description("Declaração")]
            Cabecalho = 4
            ,
            [Description("Relatório")]
            CabecalhoRelatorio = 6
        }
        #endregion Enumerador

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
                        _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
                CarregarEnumeradores();
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para carregar os valores do enumerador para o gridview.
        /// </summary>
        private void CarregarEnumeradores()
        {
            var x = (from eTiposAvisosTextosGerais item in Enum.GetValues(typeof(eTiposAvisosTextosGerais))
                     select new
                     {
                         id = (byte)item,
                         Cabecalhos = GetDescription(item)
                     });
            grvCabecalhos.DataSource = x;
            grvCabecalhos.DataBind();
        }

        /// <summary>
        /// Método para pegar a descrição do enumerable.
        /// Recebe um attributo enum e retorna sua descrição.
        /// </summary>
        /// <param name="en">attributo enum</param>
        public string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
        #endregion

        #region Eventos
        protected void grvCabecalhos_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            grvCabecalhos.EditIndex = e.NewEditIndex;
        }
        #endregion
    }

}
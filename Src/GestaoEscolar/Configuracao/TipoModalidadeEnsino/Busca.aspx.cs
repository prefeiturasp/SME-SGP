using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Web;

public partial class Configuracao_TipoModalidadeEnsino_Busca : MotherPageLogado
{
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTipoModalidadeEnsino;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTipoModalidadeEnsino.PageSize = itensPagina;
            // atualiza o grid
            _dgvTipoModalidadeEnsino.DataBind();
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTipoModalidadeEnsino.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTipoModalidadeEnsino.PageIndex = 0;
        // atualiza o grid
        _dgvTipoModalidadeEnsino.DataBind();
    }

    #endregion
    
    #region Eventos

    protected void _odsTipoModalidadeEnsino_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }
    
    #endregion

}

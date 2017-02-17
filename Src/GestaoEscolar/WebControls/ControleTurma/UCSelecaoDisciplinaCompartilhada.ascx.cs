using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.ControleTurma
{
    public partial class UCSelecaoDisciplinaCompartilhada : MotherUserControl
    {
        #region Delegates

        public delegate void commandSelecionarDisciplina(long tud_id);
        public commandSelecionarDisciplina SelecionarDisciplina;

        #endregion Delegates

        #region Metodos

        /// <summary>
        /// Abre o dialog para selecao da disciplina compartilhada.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="titulo"></param>
        public void AbrirDialog(long tud_id, long doc_id, string titulo)
        {
            UCComboTurmaDisciplinaRelacionada.CarregarDisciplinasCompartilhadas(tud_id, doc_id);
            updDisciplinasCompartilhadas.Update();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisciplinasCompartilhadas"
                , "$(document).ready(function() { $('#divDisciplinasCompartilhadas').unbind('dialog');" +
                " $('#divDisciplinasCompartilhadas').dialog({" +
                " title: '" + titulo +
                "', closeOnEscape: false " +
                " , open: function (event, ui) { "+
                    "$('#divDisciplinasCompartilhadas').parent().prependTo($('#aspnetForm'));$('#divDisciplinasCompartilhadas').parent().find('.ui-dialog-titlebar-close').hide(); }"
                + "}); $('#divDisciplinasCompartilhadas').dialog('open'); });"
                , true);
        }

        #endregion Metodos

        #region Eventos

        protected void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (this.SelecionarDisciplina != null)
                this.SelecionarDisciplina(UCComboTurmaDisciplinaRelacionada.Valor);
        }

        #endregion Eventos
    }
}
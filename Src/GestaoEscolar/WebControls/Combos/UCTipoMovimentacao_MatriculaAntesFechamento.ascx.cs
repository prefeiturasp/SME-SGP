using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCTipoMovimentacao_MatriculaAntesFechamento : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged OnSelectedIndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                if (!string.IsNullOrEmpty(rdbTipoMovimentacao.SelectedValue))
                    return rdbTipoMovimentacao.SelectedItem.ToString();

                return null;
            }
        }

        /// <summary>
        /// Tmo_Id selecionado no combo.
        /// </summary>
        public int Tmo_id
        {
            get
            {
                return Convert.ToInt32(rdbTipoMovimentacao.SelectedValue.Split(';')[0]);
            }
        }

        /// <summary>
        /// Tmo_Id selecionado no combo.
        /// </summary>
        public MTR_TipoMovimentacaoTipoMovimento Tmo_tipoMovimento
        {
            get
            {
                return (MTR_TipoMovimentacaoTipoMovimento)Convert.ToByte
                        (rdbTipoMovimentacao.SelectedValue.Split(';')[1]);
            }
        }

        /// <summary>
        /// ValidationGroup para validar o tipo de movimentação.
        /// </summary>
        public string ValidationGroup
        {
            set { rfvTipoMovimentacao.ValidationGroup = value; }
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Carrega tipos de movimentação possíveis para incluir alunos antes do fechamento de matrícula.
        /// </summary>
        /// <param name="novoAluno">Indica se é inclusão de novo aluno</param>
        public void CarregarTipoMovimentacao_AlunosIncluidosAntesFechamentoMatricula(bool novoAluno)
        {
            rdbTipoMovimentacao.Items.Clear();

            //if (novoAluno)
            //{
            //    // Se for inclusão, só carrega a matrícula inicial.
            //    rdbTipoMovimentacao.DataValueField = "";
            //    rdbTipoMovimentacao.DataTextField = "";

            //    MTR_TipoMovimentacaoTipoMovimento tipo = MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial;
            //    int tmo_id = MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId((byte)tipo, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            //    string idsReconducao = tmo_id + ";" + ((byte)tipo);

            //    ListItem item =
            //        new ListItem(MTR_TipoMovimentacaoBO.GetEntity(new MTR_TipoMovimentacao { tmo_id = tmo_id }).tmo_nome,
            //                     idsReconducao);
            //    rdbTipoMovimentacao.Items.Add(item);
            //}
            //else
            //{
                // Se for alteração, carrega todos os tipos de inclusão normais.
                rdbTipoMovimentacao.DataValueField = "tmo_id_tmo_tipoMovimento";
                rdbTipoMovimentacao.DataTextField = "tmo_nome";

                rdbTipoMovimentacao.DataSource = MTR_TipoMovimentacaoBO.SelecionaTipoMovimentacaoPorCategoria(true, false, false, false, false,
                                                                                                              __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                rdbTipoMovimentacao.DataBind();

                if (!novoAluno)
                {
                    int tmo_id =
                        MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId(
                            (byte) MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial,
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    string idsMatriculaInicial = tmo_id + ";" +
                                                 ((byte) MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial);
                    //Remove o item referente a matricula inicial no dropdown
                    ListItem itemMatriculaInicial = rdbTipoMovimentacao.Items.FindByValue(idsMatriculaInicial);
                    rdbTipoMovimentacao.Items.Remove(itemMatriculaInicial);

                    int tmo_idR =
                        MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId(
                            (byte)MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial,
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    string idsRenovacaoInicial = tmo_idR + ";" +
                                                 ((byte)MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial);
                    //Remove o item referente a renovação inicial no dropdown
                    ListItem itemRenovacaoInicial = rdbTipoMovimentacao.Items.FindByValue(idsRenovacaoInicial);
                    rdbTipoMovimentacao.Items.Remove(itemRenovacaoInicial);
                }
            //}
        }

        #endregion

        #region Eventos

        protected void rdbTipoMovimentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
            {
                OnSelectedIndexChanged();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rdbTipoMovimentacao.AutoPostBack = OnSelectedIndexChanged != null;
        }

        #endregion
    }
}
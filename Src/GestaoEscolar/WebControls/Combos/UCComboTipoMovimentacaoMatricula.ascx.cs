using System;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboTipoMovimentacaoMatricula : MotherUserControl
    {
        #region DELEGATES

        public delegate void SelectedIndexChanged();
        public event SelectedIndexChanged IndexChanged;

        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// valor[0] = tmo_id
        /// valor[1] = tmno_tipoMovimento
        /// </summary>
        public Int32[] Valor
        {
            get
            {
                string[] s = null;

                if (!String.IsNullOrEmpty(rblTipoInclusao.SelectedValue))
                    s = rblTipoInclusao.SelectedValue.Split(';');
                else if (!String.IsNullOrEmpty(rblTipoRealocacao.SelectedValue))
                    s = rblTipoRealocacao.SelectedValue.Split(';');
                else if (!String.IsNullOrEmpty(rblTipoExclusao.SelectedValue))
                    s = rblTipoExclusao.SelectedValue.Split(';');
                else if (!String.IsNullOrEmpty(rblTipoOutros.SelectedValue))
                    s = rblTipoOutros.SelectedValue.Split(';');
                else if (!String.IsNullOrEmpty(rblTipoRenovacao.SelectedValue))
                    s = rblTipoRenovacao.SelectedValue.Split(';');

                if (s != null && s.Length == 2)
                    return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]) };

                return new[] { -1, -1 };
            }
            set
            {                
                if (value.Length == 2)
                {
                    rblTipoInclusao.ClearSelection();
                    rblTipoRealocacao.ClearSelection();
                    rblTipoExclusao.ClearSelection();
                    rblTipoOutros.ClearSelection();
                    rblTipoRenovacao.ClearSelection();

                    string s = value[0] + ";" + value[1];
                    if (rblTipoInclusao.Items.FindByValue(s) != null)
                        rblTipoInclusao.SelectedValue = s;

                    if (rblTipoRealocacao.Items.FindByValue(s) != null)
                        rblTipoRealocacao.SelectedValue = s;

                    if (rblTipoExclusao.Items.FindByValue(s) != null)
                        rblTipoExclusao.SelectedValue = s;

                    if (rblTipoOutros.Items.FindByValue(s) != null)
                        rblTipoOutros.SelectedValue = s;

                    if (rblTipoRenovacao.Items.FindByValue(s) != null)
                        rblTipoRenovacao.SelectedValue = s;
                }
                else if (value.Length == 1)
                {
                    string valor = value[0].ToString();

                    // Busca o valor pelo primeiro valor nos itens.
                    SelecionaItemPrimeiroValor(rblTipoInclusao, valor);
                    SelecionaItemPrimeiroValor(rblTipoRealocacao, valor);
                    SelecionaItemPrimeiroValor(rblTipoExclusao, valor);
                    SelecionaItemPrimeiroValor(rblTipoOutros, valor);
                    SelecionaItemPrimeiroValor(rblTipoRenovacao, valor);
                }
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                if (!string.IsNullOrEmpty(rblTipoInclusao.SelectedValue))
                    return rblTipoInclusao.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(rblTipoRealocacao.SelectedValue))
                    return rblTipoRealocacao.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(rblTipoExclusao.SelectedValue))
                    return rblTipoExclusao.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(rblTipoOutros.SelectedValue))
                    return rblTipoOutros.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(rblTipoRenovacao.SelectedValue))
                    return rblTipoRenovacao.SelectedItem.ToString();
                
                return null;
            }
        }

        /// <summary>
        /// Propriedade que seta a label e a validação do combo
        /// </summary>
        public bool Obrigatorio
        {
            set
            {
                if (value)
                {
                    AdicionaAsteriscoObrigatorio(lblTitulo);
                }
                else
                {
                    RemoveAsteriscoObrigatorio(lblTitulo);

                }

                SetaObrigatorio(value);
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                pTipoMovimentacao.Enabled = value;
            }
        }

        /// <summary>
        /// Exibe ou não o combo, o titulo e o validador de acordo com o valor passado
        /// </summary>
        public bool ExibeCombo
        {
            set
            {
                pTipoMovimentacao.Visible = value;
            }
        }

        public bool IsValid
        {
            get
            {
                if (chkTipoMov.Checked)
                    return (!(string.IsNullOrEmpty(rblTipoInclusao.SelectedValue) &&
                              string.IsNullOrEmpty(rblTipoRealocacao.SelectedValue) &&
                              string.IsNullOrEmpty(rblTipoExclusao.SelectedValue) &&
                              string.IsNullOrEmpty(rblTipoOutros.SelectedValue) &&
                              string.IsNullOrEmpty(rblTipoRenovacao.SelectedValue)));
                
                return true;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Busca o valor pelo primeiro valor nos itens.
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="valor"></param>
        private void SelecionaItemPrimeiroValor(RadioButtonList rbl, string valor)
        {
            var x = from ListItem item in rbl.Items
                    where item.Value.Split(';')[0] == valor
                    select item.Value;

            if (x.Count() > 0)
            {
                rbl.SelectedValue = x.First();
            }
        }

        /// <summary>
        /// Seta os campos na tela de acordo com a obrigatoreidade.
        /// </summary>
        /// <param name="valor"></param>
        private void SetaObrigatorio(bool valor)
        {
            chkTipoMov.Checked = valor;
            chkTipoMov.Visible = !valor;
            pTipoMovimentacao.Visible = valor;

            if (!valor)
            {
                rblTipoOutros.ClearSelection();
                rblTipoRealocacao.ClearSelection();
                rblTipoExclusao.ClearSelection();
                rblTipoInclusao.ClearSelection();
                rblTipoRenovacao.ClearSelection();
                if (IndexChanged != null)
                    IndexChanged();
            }
        }

        /// <summary>
        /// Carrega os parâmetros de movimentação não excluídos logicamente no combo por categoria        
        /// </summary>
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param>
        /// <param name="novo_aluno"></param>
        /// <param name="alu_id"></param>
        public void CarregarTipoMovimentacaoPorCategoria
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , bool renovacao
            , bool novo_aluno
            , long alu_id
            , byte alu_situacao
        )
        {
            fdsTipoInclusao.Visible = fdsTipoRealocacao.Visible = fdsTipoExclusao.Visible =
            fdsTipoOutros.Visible = fdsTipoRenovacao.Visible = false;

            if (inclusao)
            {
                odsTipoInclusao.SelectParameters.Clear();
                rblTipoInclusao.Items.Clear();

                odsTipoInclusao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoInclusao.SelectParameters.Add("inclusao", "true");
                odsTipoInclusao.SelectParameters.Add("realocacao", "false");
                odsTipoInclusao.SelectParameters.Add("exclusao", "false");
                odsTipoInclusao.SelectParameters.Add("outros", "false");
                odsTipoInclusao.SelectParameters.Add("renovacao", "false");
                odsTipoInclusao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoInclusao.DataBind();

                if ((ACA_AlunoSituacao)alu_situacao == ACA_AlunoSituacao.Inativo)
                {
                    int tmo_id = MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId((byte)MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    string idsMatriculaInicial = tmo_id + ";" + ((byte)MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial);
                    //Remove o item referente a matricula inicial no dropdown
                    ListItem itemMatriculaInicial = rblTipoInclusao.Items.FindByValue(idsMatriculaInicial);
                    rblTipoInclusao.Items.Remove(itemMatriculaInicial);

                    int tmo_idRenovacaoInicial = MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId((byte)MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    string idsRenovacaoInicial = tmo_idRenovacaoInicial + ";" + ((byte)MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial);
                    //Remove o item referente a renovação inicial no dropdown
                    ListItem itemRenovacaoInicial = rblTipoInclusao.Items.FindByValue(idsRenovacaoInicial);
                    rblTipoInclusao.Items.Remove(itemRenovacaoInicial);
                }

                fdsTipoInclusao.Visible = rblTipoInclusao.Items.Count > 0;
            }
            if (realocacao)
            {
                odsTipoRealocacao.SelectParameters.Clear();
                rblTipoRealocacao.Items.Clear();

                odsTipoRealocacao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoRealocacao.SelectParameters.Add("inclusao", "false");
                odsTipoRealocacao.SelectParameters.Add("realocacao", "true");
                odsTipoRealocacao.SelectParameters.Add("exclusao", "false");
                odsTipoRealocacao.SelectParameters.Add("outros", "false");
                odsTipoRealocacao.SelectParameters.Add("renovacao", "false");
                odsTipoRealocacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoRealocacao.DataBind();

                fdsTipoRealocacao.Visible = rblTipoRealocacao.Items.Count > 0;
            }
            if (exclusao)
            {
                odsTipoExclusao.SelectParameters.Clear();
                rblTipoExclusao.Items.Clear();

                odsTipoExclusao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoExclusao.SelectParameters.Add("inclusao", "false");
                odsTipoExclusao.SelectParameters.Add("realocacao", "false");
                odsTipoExclusao.SelectParameters.Add("exclusao", "true");
                odsTipoExclusao.SelectParameters.Add("outros", "false");
                odsTipoExclusao.SelectParameters.Add("renovacao", "false");
                odsTipoExclusao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoExclusao.DataBind();

                fdsTipoExclusao.Visible = rblTipoExclusao.Items.Count > 0;
            }
            if (outros)
            {
                odsTipoOutros.SelectParameters.Clear();
                rblTipoOutros.Items.Clear();

                odsTipoOutros.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoOutros.SelectParameters.Add("inclusao", "false");
                odsTipoOutros.SelectParameters.Add("realocacao", "false");
                odsTipoOutros.SelectParameters.Add("exclusao", "false");
                odsTipoOutros.SelectParameters.Add("outros", "true");
                odsTipoOutros.SelectParameters.Add("renovacao", "false");
                odsTipoOutros.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoOutros.DataBind();

                fdsTipoOutros.Visible = rblTipoOutros.Items.Count > 0;
            }
            if (renovacao)
            {
                odsTipoRenovacao.SelectParameters.Clear();
                rblTipoRenovacao.Items.Clear();

                odsTipoRenovacao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoRenovacao.SelectParameters.Add("inclusao", "false");
                odsTipoRenovacao.SelectParameters.Add("realocacao", "false");
                odsTipoRenovacao.SelectParameters.Add("exclusao", "false");
                odsTipoRenovacao.SelectParameters.Add("outros", "false");
                odsTipoRenovacao.SelectParameters.Add("renovacao", "true");
                odsTipoRenovacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoRenovacao.DataBind();

                fdsTipoRenovacao.Visible = rblTipoRenovacao.Items.Count > 0;
            }

            //Esconde o tipo de movimentaçao de recondução se estiver ocorrendo a inclusao de um aluno
            if (rblTipoInclusao.Items.Count > 1)
            {
                if (inclusao)
                {
                    if ((!novo_aluno) && ((ACA_AlunoSituacao)alu_situacao == ACA_AlunoSituacao.Ativo))
                    {
                        int tmo_id = MTR_TipoMovimentacaoBO.Retorna_TipoMovimentacaoId((byte)MTR_TipoMovimentacaoTipoMovimento.Reconducao, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        string idsReconducao = tmo_id + ";" + ((byte)MTR_TipoMovimentacaoTipoMovimento.Reconducao);
                        //Remove o item referente a reconducao no dropdown
                        ListItem itemReconducao = rblTipoInclusao.Items.FindByValue(idsReconducao);
                        rblTipoInclusao.Items.Remove(itemReconducao);

                        fdsTipoInclusao.Visible = rblTipoInclusao.Items.Count > 0;
                    }
                }
            }

            if ((inclusao || realocacao || exclusao || outros || renovacao || novo_aluno) &&
                 !fdsTipoInclusao.Visible && !fdsTipoRealocacao.Visible &&
                 !fdsTipoExclusao.Visible && !fdsTipoOutros.Visible && !fdsTipoRenovacao.Visible)
                lblMessage.Text = "Nenhum tipo de movimentação disponível.";
        }

        /// <summary>
        /// Carrega os parâmetros de movimentação não excluídos logicamente no combo por categoria     
        /// Utilizado para alteração da movimentação
        /// </summary>
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param>
        /// <param name="novo_aluno"></param>
        /// <param name="alu_id"></param>
        public void CarregarTipoMovimentacaoPorCategoriaAlteracaoMovimentacao
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , bool renovacao
            , bool novo_aluno
            , long alu_id
            , byte alu_situacao
        )
        {
            fdsTipoInclusao.Visible = fdsTipoRealocacao.Visible = fdsTipoExclusao.Visible =
            fdsTipoOutros.Visible = fdsTipoRenovacao.Visible = false;

            if (inclusao)
            {
                odsTipoInclusao.SelectParameters.Clear();
                rblTipoInclusao.Items.Clear();

                odsTipoInclusao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoInclusao.SelectParameters.Add("inclusao", "true");
                odsTipoInclusao.SelectParameters.Add("realocacao", "false");
                odsTipoInclusao.SelectParameters.Add("exclusao", "false");
                odsTipoInclusao.SelectParameters.Add("outros", "false");
                odsTipoInclusao.SelectParameters.Add("renovacao", "false");
                odsTipoInclusao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoInclusao.DataBind();

                fdsTipoInclusao.Visible = rblTipoInclusao.Items.Count > 0;

            }
            if (realocacao)
            {
                odsTipoRealocacao.SelectParameters.Clear();
                rblTipoRealocacao.Items.Clear();

                odsTipoRealocacao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoRealocacao.SelectParameters.Add("inclusao", "false");
                odsTipoRealocacao.SelectParameters.Add("realocacao", "true");
                odsTipoRealocacao.SelectParameters.Add("exclusao", "false");
                odsTipoRealocacao.SelectParameters.Add("outros", "false");
                odsTipoRealocacao.SelectParameters.Add("renovacao", "false");
                odsTipoRealocacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoRealocacao.DataBind();

                fdsTipoRealocacao.Visible = rblTipoRealocacao.Items.Count > 0;
            }
            if (exclusao)
            {
                odsTipoExclusao.SelectParameters.Clear();
                rblTipoExclusao.Items.Clear();

                odsTipoExclusao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoExclusao.SelectParameters.Add("inclusao", "false");
                odsTipoExclusao.SelectParameters.Add("realocacao", "false");
                odsTipoExclusao.SelectParameters.Add("exclusao", "true");
                odsTipoExclusao.SelectParameters.Add("outros", "false");
                odsTipoExclusao.SelectParameters.Add("renovacao", "false");
                odsTipoExclusao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoExclusao.DataBind();

                fdsTipoExclusao.Visible = rblTipoExclusao.Items.Count > 0;
            }
            if (outros)
            {
                odsTipoOutros.SelectParameters.Clear();
                rblTipoOutros.Items.Clear();

                odsTipoOutros.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoOutros.SelectParameters.Add("inclusao", "false");
                odsTipoOutros.SelectParameters.Add("realocacao", "false");
                odsTipoOutros.SelectParameters.Add("exclusao", "false");
                odsTipoOutros.SelectParameters.Add("outros", "true");
                odsTipoOutros.SelectParameters.Add("renovacao", "false");
                odsTipoOutros.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoOutros.DataBind();

                fdsTipoOutros.Visible = rblTipoOutros.Items.Count > 0;
            }
            if (renovacao)
            {
                odsTipoRenovacao.SelectParameters.Clear();
                rblTipoRenovacao.Items.Clear();

                odsTipoRenovacao.SelectMethod = "SelecionaTipoMovimentacaoPorCategoria";
                odsTipoRenovacao.SelectParameters.Add("inclusao", "false");
                odsTipoRenovacao.SelectParameters.Add("realocacao", "false");
                odsTipoRenovacao.SelectParameters.Add("exclusao", "false");
                odsTipoRenovacao.SelectParameters.Add("outros", "false");
                odsTipoRenovacao.SelectParameters.Add("renovacao", "true");
                odsTipoRenovacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

                rblTipoRenovacao.DataBind();

                fdsTipoRenovacao.Visible = rblTipoRenovacao.Items.Count > 0;
            }

            if ((inclusao || realocacao || exclusao || outros || renovacao || novo_aluno) &&
                 !fdsTipoInclusao.Visible && !fdsTipoRealocacao.Visible &&
                 !fdsTipoExclusao.Visible && !fdsTipoOutros.Visible && !fdsTipoRenovacao.Visible)
                lblMessage.Text = "Nenhum tipo de movimentação disponível.";
        }

        #endregion

        #region EVENTOS

        protected void chkTipoMov_CheckedChanged(object sender, EventArgs e)
        {
            pTipoMovimentacao.Visible = chkTipoMov.Checked;
            
            if (!chkTipoMov.Checked)
            {
                rblTipoOutros.ClearSelection();
                rblTipoRealocacao.ClearSelection();
                rblTipoExclusao.ClearSelection();
                rblTipoInclusao.ClearSelection();
                rblTipoRenovacao.ClearSelection();
                if (IndexChanged != null)
                    IndexChanged();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rblTipoInclusao.AutoPostBack = (IndexChanged != null);
            rblTipoRealocacao.AutoPostBack = (IndexChanged != null);
            rblTipoExclusao.AutoPostBack = (IndexChanged != null);
            rblTipoOutros.AutoPostBack = (IndexChanged != null);
            rblTipoRenovacao.AutoPostBack = (IndexChanged != null);
        }

        protected void rblTipoInclusao_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoRealocacao.ClearSelection();
            rblTipoExclusao.ClearSelection();
            rblTipoOutros.ClearSelection();
            rblTipoRenovacao.ClearSelection();
            if (IndexChanged != null)
                IndexChanged();
        }
        
        protected void rblTipoRealocacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoInclusao.ClearSelection();
            rblTipoExclusao.ClearSelection();
            rblTipoOutros.ClearSelection();
            rblTipoRenovacao.ClearSelection();
            if (IndexChanged != null)
                IndexChanged();
        }
        
        protected void rblTipoExclusao_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoRealocacao.ClearSelection();
            rblTipoInclusao.ClearSelection();
            rblTipoOutros.ClearSelection();
            rblTipoRenovacao.ClearSelection();
            if (IndexChanged != null)
                IndexChanged();
        }
        
        protected void rblTipoOutros_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoRealocacao.ClearSelection();
            rblTipoExclusao.ClearSelection();
            rblTipoInclusao.ClearSelection();
            rblTipoRenovacao.ClearSelection();
            if (IndexChanged != null)
                IndexChanged();
        }

        protected void rblTipoRenovacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoRealocacao.ClearSelection();
            rblTipoExclusao.ClearSelection();
            rblTipoInclusao.ClearSelection();
            rblTipoOutros.ClearSelection();
            if (IndexChanged != null)
                IndexChanged();
        }

        protected void odsMovimentacoes_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                // Grava o erro e mostr pro usuário.
                ApplicationWEB._GravaErro(e.Exception.InnerException);

                e.ExceptionHandled = true;
                lblMessage.Text = "Erro ao tentar carregar tipo de movimentação.";
                lblMessage.Visible = true;
            }
        }

        #endregion
    }
}

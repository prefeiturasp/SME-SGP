using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.ControleVigenciaDocentes
{
    public partial class ControleVigenciaDocentes : MotherUserControl
    {
        #region Constantes

        private const int grvControleVigenciaIndiceColunaAlterar = 3;

        private const int grvControleVigenciaIndiceColunaDeletar = 4;

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// Tabela que vai guardar todos os docentes da escola, pra depois filtrar e jogar no combo de docentes.
        /// </summary>
        private DataTable dtDocentesEscola;

        /// <summary>
        /// ValidationGroup usado na tela.
        /// </summary>
        protected string ConfiguraValidationGroup
        {
            get
            {
                return this.ClientID;
            }
        }

        /// <summary>
        /// Permite editar o controle de vigência de docentes.
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                if (!value)
                {
                    btnControleVigencia.Enabled = true;
                    divVigencia.Visible = false;
                    btnAdicionar.Enabled = value;
                    grvControleVigencia.Columns[grvControleVigenciaIndiceColunaAlterar].Visible = value;
                    grvControleVigencia.Columns[grvControleVigenciaIndiceColunaDeletar].Visible = value;
                }
                else
                    btnControleVigencia.Enabled = value;
            }
        }

        #endregion Propriedades

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            btnControleVigencia.OnClientClick = "$('#" + divVigenciaDocentesDis.ClientID + "').dialog('open');return false;";
            btnFechar.OnClientClick = "$('#" + divVigenciaDocentesDis.ClientID + "').dialog('close');return false;";
        }

        #endregion Eventos Page Life Cycle

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    List<TUR_Turma_Docentes_Disciplina> list = RetornaDadosGrid();

                    DateTime vigenciaInicio = Convert.ToDateTime(txtVigenciaIni.Text);
                    DateTime vigenciaFim = string.IsNullOrEmpty(txtVigenciaFim.Text) ? new DateTime() : Convert.ToDateTime(txtVigenciaFim.Text);
                    int tdt_id = string.IsNullOrEmpty(lbltdt_id.Text) ? 0 : Convert.ToInt32(lbltdt_id.Text);

                    // Se for uma alteração deleta do list e adiciona novamente
                    if (!string.IsNullOrEmpty(lblIndice.Text))
                    {
                        list.Remove(list.Find(p => p.indice == Convert.ToInt32(lblIndice.Text)));
                    }

                    DateTime dtFim2 = vigenciaFim == new DateTime() ? DateTime.MaxValue : vigenciaFim;

                    // Verifica se não existe conflito de datas
                    foreach (TUR_Turma_Docentes_Disciplina item in list)
                    {
                        DateTime dtFim = item.entDocente.tdt_vigenciaFim == new DateTime() ? DateTime.MaxValue : item.entDocente.tdt_vigenciaFim;

                        if (GestaoEscolarUtilBO.ExisteConflitoDatas(item.entDocente.tdt_vigenciaInicio, dtFim, vigenciaInicio, dtFim2))
                        {
                            throw new ValidationException("Existe conflito entre as vigências.");
                        }
                    }

                    // RF_076
                    string periodoVigencia;
                    if (!RHU_ColaboradorCargoBO.ValidarVigenciaPorData(uccDocente.Valor, vigenciaInicio, vigenciaFim, out periodoVigencia))
                    {
                        throw new ValidationException
                            ("A vigência do docente na turma deve estar dentro da vigência do seu cargo (" +
                            periodoVigencia + ").");
                    }

                    TUR_Turma_Docentes_Disciplina entDoc = new TUR_Turma_Docentes_Disciplina
                    {
                        doc_nome = uccDocente.TextoSelecionado,
                        entDocente = new TUR_TurmaDocente
                        {
                            doc_id = uccDocente.Doc_id,
                            col_id = uccDocente.Valor[1],
                            crg_id = (int)uccDocente.Valor[2],
                            coc_id = (int)uccDocente.Valor[3],
                            tdt_vigenciaInicio = vigenciaInicio,
                            tdt_vigenciaFim = vigenciaFim,
                            tdt_id = tdt_id
                        }
                    };

                    list.Add(entDoc);
                    AtualizaGrid(list);
                    LimparCampos();

                    CarregarDocenteVigente(list);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o docente.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        protected void btnAdicionarRemoverDocente_Click(object sender, EventArgs e)
        {
            ImageButton btnSender = (ImageButton)sender;

            if (btnSender.ID.Equals(btnAdicionar.ID))
            {
                btnAdicionar.Visible = false;
                btnRemover.Visible = true;
                txtNomeDocente.Visible = true;
                lblNomeDocente.Visible = true;
                btnControleVigencia.Visible = true;
            }
            else
            {
                btnAdicionar.Visible = true;
                btnRemover.Visible = false;

                grvControleVigencia.DataSource = null;
                grvControleVigencia.DataBind();
                btnControleVigencia.Visible = false;
                txtNomeDocente.Visible = false;
                txtNomeDocente.Text = string.Empty;
                lblNomeDocente.Visible = false;
            }
        }

        protected void grvControleVigencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string dtFim = ((DataBinder.Eval(e.Row.DataItem, "tdt_vigenciaFim") == DBNull.Value
                                || Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "tdt_vigenciaFim")) == new DateTime()) ? string.Empty
                                : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "tdt_vigenciaFim")).ToString("dd/MM/yyyy"));

                Label lblVigenciaInicio = (Label)e.Row.FindControl("lblVigenciaInicio");
                if (lblVigenciaInicio != null)
                {
                    lblVigenciaInicio.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "tdt_vigenciaInicio")).ToString("dd/MM/yyyy");
                }

                Label lblVigenciaFim = (Label)e.Row.FindControl("lblVigenciaFim");
                if (lblVigenciaFim != null)
                {
                    lblVigenciaFim.Text = dtFim;
                }

                ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnDelete != null)
                {
                    btnDelete.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnAlterar = (ImageButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvControleVigencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    List<TUR_Turma_Docentes_Disciplina> list = RetornaDadosGrid();

                    foreach (TUR_Turma_Docentes_Disciplina Item in list)
                    {
                        // Remove do cache as turmas do docente.
                        TUR_TurmaBO.RemoveCacheDocenteControleTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, Item.entDocente.doc_id);
                    }

                    list.Remove(list.Find(p => p.indice == Convert.ToInt32(e.CommandArgument)));
                    AtualizaGrid(list);
                    CarregarDocenteVigente(list);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a vigência do docente.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Editar")
            {
                try
                {
                    List<TUR_Turma_Docentes_Disciplina> list = RetornaDadosGrid();
                    TUR_Turma_Docentes_Disciplina entity = list.Find(p => p.indice == Convert.ToInt32(e.CommandArgument));

                    uccDocente.Valor = new long[] { -1, -1, -1, -1 };
                    uccDocente.Valor = new[] { entity.entDocente.doc_id, entity.entDocente.col_id, entity.entDocente.crg_id, entity.entDocente.coc_id };
                    txtVigenciaIni.Text = entity.entDocente.tdt_vigenciaInicio.ToString("dd/MM/yyyy");
                    txtVigenciaFim.Text = entity.entDocente.tdt_vigenciaFim != new DateTime() ? entity.entDocente.tdt_vigenciaFim.ToString("dd/MM/yyyy") : string.Empty;
                    lblIndice.Text = entity.indice.ToString();
                    lbltdt_id.Text = entity.entDocente.tdt_id.ToString();

                    if (uccDocente.Valor[0] == -1)
                    {
                        uccDocente._Combo.Items.Add(new ListItem(entity.doc_nome, entity.entDocente.doc_id.ToString() + ";" +
                                                                                  entity.entDocente.col_id.ToString() + ";" +
                                                                                  entity.entDocente.crg_id.ToString() + ";" +
                                                                                  entity.entDocente.coc_id.ToString()));
                        uccDocente.Valor = new[] { entity.entDocente.doc_id, entity.entDocente.col_id, entity.entDocente.crg_id, entity.entDocente.coc_id };
                        uccDocente.PermiteEditar = false;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar editar a vigência do docente.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            grvControleVigencia.EmptyDataText = "Não existem docentes associados para o(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + ".";
        }

        #endregion Eventos

        #region Métodos

        public void updateTela()
        {
            updVigenciaDocente.Update();
            updBotao.Update();
        }

        /// <summary>
        /// Carrega as diversas posições de docentes.
        /// </summary>
        /// <param name="doc_nome">Nome do docente.</param>
        /// <param name="posicao">Posicao.</param>
        /// <param name="qtdeDocentes">Quantidade de docentes.</param>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <param name="dtDocentes">DataTable com os docentes.</param>
        /// <param name="tds_id">Id das disciplinas.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="tur_docenteEspecialista">Turma de docente especialista.</param>
        /// <param name="bloqueioAtribuicaoDocente">Flag que indica se é pra bloquear a atribuição de docente para a escola</param>
        public void CarregarDocente
        (
            string doc_nome
            , byte posicao
            , int qtdeDocentes
            , long tud_id
            , ref DataTable dtDocentes
            , int tds_id
            , int esc_id
            , int uni_id
            , bool tur_docenteEspecialista
            , bool bloqueioAtribuicaoDocente
            , ref DataTable DtVigenciasDocentes
        )
        {
            try
            {
                if (qtdeDocentes > 1)
                {
                    lblNomeDocente.Text = posicao.ToString();
                    lblNomeDocente.Visible = true;
                }
                else
                {
                    lblNomeDocente.Visible = false;
                }

                txtNomeDocente.Text = doc_nome;

                // Guarda as informações no UserControl
                lblposicao.Text = Convert.ToString(posicao);
                lblTud_id.Text = Convert.ToString(tud_id);

                grvControleVigencia.Columns[grvControleVigenciaIndiceColunaAlterar].Visible =
                grvControleVigencia.Columns[grvControleVigenciaIndiceColunaDeletar].Visible =
                fdsDadosDocentes.Visible = !bloqueioAtribuicaoDocente;

                if (posicao > 1)
                {
                    bool existeDocente = !string.IsNullOrEmpty(doc_nome);
                    btnAdicionar.ToolTip = "Associar " + CardinalToOrdinal(posicao) + " docente";
                    btnAdicionar.Visible = !existeDocente && !bloqueioAtribuicaoDocente;
                    btnRemover.ToolTip = "Remover " + CardinalToOrdinal(posicao) + " docente";
                    btnRemover.Visible = existeDocente && !bloqueioAtribuicaoDocente;
                    txtNomeDocente.Visible = existeDocente;
                    lblNomeDocente.Visible = existeDocente;
                    btnControleVigencia.Visible = existeDocente;
                }
                else
                {
                    btnAdicionar.Visible = false;
                    btnRemover.Visible = false;
                    lblNomeDocente.Visible = true;
                    txtNomeDocente.Visible = true;
                }

                dtDocentesEscola = dtDocentes;
                CarregarComboProfessor(tds_id, esc_id, uni_id, tur_docenteEspecialista);
                dtDocentes = dtDocentesEscola;
                CarregarGridVigenciaDocentes(tud_id, posicao, ref DtVigenciasDocentes);

                updVigenciaDocente.Update();
                updBotao.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Retorna as vigências da posição e por disciplina.
        /// </summary>
        /// <param name="listDocentesPosicoes">Lista com as posições do docente.</param>
        /// <param name="posicao">Posição.</param>
        /// <param name="tud_id">Id das disciplinas.</param>
        public void RetornaDocentesPosicao(ref List<TUR_Turma_Docentes_Disciplina> listDocentesPosicoes, byte posicao, long tud_id)
        {
            foreach (GridViewRow row in grvControleVigencia.Rows)
            {
                TUR_Turma_Docentes_Disciplina entityDoc = new TUR_Turma_Docentes_Disciplina
                {
                    indice = row.RowIndex,
                    doc_nome = grvControleVigencia.DataKeys[row.RowIndex].Values["doc_nome"].ToString(),
                    entDocente = new TUR_TurmaDocente
                    {
                        doc_id = (long)grvControleVigencia.DataKeys[row.RowIndex].Values["doc_id"],
                        col_id = (long)grvControleVigencia.DataKeys[row.RowIndex].Values["col_id"],
                        crg_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["crg_id"],
                        coc_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["coc_id"],
                        tdt_posicao = posicao,
                        tdt_tipo = 1, // Regular
                        tud_id = tud_id,
                        tdt_vigenciaInicio = Convert.ToDateTime(grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_vigenciaInicio"]),
                        tdt_vigenciaFim = string.IsNullOrEmpty(grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_vigenciaFim"].ToString()) ? new DateTime() : Convert.ToDateTime(grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_vigenciaFim"]),
                        tdt_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_id"],
                        IsNew = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_id"] > 0 ? false : true
                    }
                };

                listDocentesPosicoes.Add(entityDoc);
            }
        }

        /// <summary>
        /// Limpar campos da tela.
        /// </summary>
        private void LimparCampos()
        {
            txtVigenciaIni.Text = string.Empty;
            txtVigenciaFim.Text = string.Empty;
            lblIndice.Text = string.Empty;
            if (!uccDocente.PermiteEditar)
            {
                uccDocente._Combo.Items.Remove(uccDocente._Combo.SelectedItem);
                uccDocente.PermiteEditar = true;
            }
            uccDocente.Valor = new long[] { -1, -1, -1, -1 };
            lbltdt_id.Text = string.Empty; 
        }

        /// <summary>
        /// Carrega docente vigente.
        /// </summary>
        private void CarregarDocenteVigente(List<TUR_Turma_Docentes_Disciplina> list)
        {
            TUR_Turma_Docentes_Disciplina entityDocenteAtual = list.Find(p => (p.entDocente.tdt_vigenciaInicio <= DateTime.Now.Date &&
                                                                                         (p.entDocente.tdt_vigenciaFim == new DateTime() || p.entDocente.tdt_vigenciaFim >= DateTime.Now.Date)));
            if (entityDocenteAtual.entDocente != null)
            {
                txtNomeDocente.Text = entityDocenteAtual.doc_nome;
                lblposicao.Text = entityDocenteAtual.indice.ToString();
                lblTud_id.Text = entityDocenteAtual.entDocente.tud_id.ToString();
            }
            else
            {
                txtNomeDocente.Text = string.Empty;
                lblposicao.Text = string.Empty;
                lblTud_id.Text = string.Empty;
            }

            updBotao.Update();
        }

        /// <summary>
        /// Cria uma lista de dados da grid.
        /// </summary>
        /// <returns>Lista dos docentes por disciplina.</returns>
        public List<TUR_Turma_Docentes_Disciplina> RetornaDadosGrid()
        {
            List<TUR_Turma_Docentes_Disciplina> list = new List<TUR_Turma_Docentes_Disciplina>();

            try
            {
                foreach (GridViewRow row in grvControleVigencia.Rows)
                {
                    string dataInicio = grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_vigenciaInicio"].ToString();
                    string dataFim = grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_vigenciaFim"].ToString();

                    TUR_Turma_Docentes_Disciplina entity = new TUR_Turma_Docentes_Disciplina
                    {
                        indice = row.RowIndex,
                        doc_nome = grvControleVigencia.DataKeys[row.RowIndex].Values["doc_nome"].ToString(),

                        entDocente = new TUR_TurmaDocente
                        {
                            doc_id = (long)grvControleVigencia.DataKeys[row.RowIndex].Values["doc_id"],
                            col_id = (long)grvControleVigencia.DataKeys[row.RowIndex].Values["col_id"],
                            crg_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["crg_id"],
                            coc_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["coc_id"],
                            tdt_vigenciaInicio = string.IsNullOrEmpty(dataInicio) ? new DateTime() : Convert.ToDateTime(dataInicio),
                            tdt_vigenciaFim = string.IsNullOrEmpty(dataFim) ? new DateTime() : Convert.ToDateTime(dataFim),
                            tdt_id = (int)grvControleVigencia.DataKeys[row.RowIndex].Values["tdt_id"],
                            tdt_tipo = 1 // Regular
                        }
                    };

                    list.Add(entity);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ler os dados na tabela.", UtilBO.TipoMensagem.Erro);
            }

            return list;
        }

        /// <summary>
        /// Atualiza o grid.
        /// </summary>
        /// <param name="list">Lista dos docentes por disciplina.</param>
        private void AtualizaGrid(List<TUR_Turma_Docentes_Disciplina> list)
        {
            var x = from TUR_Turma_Docentes_Disciplina item in list
                    orderby item.entDocente.tdt_vigenciaInicio descending
                    select
                    new
                    {
                        item.entDocente.doc_id,
                        item.entDocente.coc_id,
                        item.entDocente.col_id,
                        item.entDocente.crg_id,
                        item.doc_nome,
                        item.entDocente.tdt_vigenciaInicio,
                        item.entDocente.tdt_vigenciaFim,
                        item.entDocente.tdt_id
                    };

            grvControleVigencia.DataSource = x.ToList();
            grvControleVigencia.DataBind();
        }

        /// <summary>
        /// Carrega o combo de docente por disciplina.
        /// </summary>
        /// <param name="tds_id">Ids da disciplina.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="tur_docenteEspecialista">Turma de docente especialista.</param>
        private void CarregarComboProfessor(int tds_id, int esc_id, int uni_id, bool tur_docenteEspecialista)
        {
            uccDocente._CancelaSelect = true;
            uccDocente.VS_vinculoExtra = true;
            uccDocente._LoadBy_Especialidade(
                esc_id,
                uni_id,
                tur_docenteEspecialista,
                tds_id,
                true,
                ref dtDocentesEscola);
        }

        /// <summary>
        /// Carrega a grid de vigências dos docentes.
        /// </summary>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <param name="posicao">Posição</param>
        protected void CarregarGridVigenciaDocentes(long tud_id, byte posicao, ref DataTable DtVigenciasDocentes)
        {
            IEnumerable<DataRow> query =
                from order in DtVigenciasDocentes.AsEnumerable()
                where order.Field<long>("tud_id") == tud_id
                        && order.Field<byte>("tdt_posicao") == posicao
                select order;

            DataTable dt;

            if (query.Any())
                dt = query.CopyToDataTable<DataRow>();
            else
                dt = DtVigenciasDocentes.Clone();

            grvControleVigencia.DataSource = dt;
            grvControleVigencia.DataBind();
        }

        /// <summary>
        /// Converte número cardinal para ordinal (1 -> primeiro).
        /// Obs.: somente até o 100º
        /// </summary>
        /// <param name="cardinal">Cardinal a ser convertido.</param>
        /// <returns>Número ordinal.</returns>
        private string CardinalToOrdinal(int cardinal)
        {
            string sCardinal = cardinal.ToString();

            if (sCardinal.Length >= 3)
            {
                return "centésimo";
            }

            string retorno = "";

            if (sCardinal.Length == 2)
            {
                switch (sCardinal[1])
                {
                    case '1': retorno = "décimo "; break;
                    case '2': retorno = "vigésimo "; break;
                    case '3': retorno = "trigésimo "; break;
                    case '4': retorno = "quadragésimo "; break;
                    case '5': retorno = "quinquagésimo "; break;
                    case '6': retorno = "sexagésimo "; break;
                    case '7': retorno = "septuagésimo "; break;
                    case '8': retorno = "octogésimo "; break;
                    case '9': retorno = "nonagésimo "; break;
                }
            }

            switch (sCardinal[0])
            {
                case '0': retorno.Trim(); break;
                case '1': retorno += "primeiro"; break;
                case '2': retorno += "segundo"; break;
                case '3': retorno += "terceiro"; break;
                case '4': retorno += "quarto"; break;
                case '5': retorno += "quinto"; break;
                case '6': retorno += "sexto"; break;
                case '7': retorno += "sétimo"; break;
                case '8': retorno += "oitavo"; break;
                case '9': retorno += "nono"; break;
            }

            return retorno;
        }

        #endregion Métodos
    }
}
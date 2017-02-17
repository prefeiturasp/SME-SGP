using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.WebControls.Combos
{
    public partial class UCComboAvaliacao : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();

        public event SelectedIndexChanged IndexChanged;

        #endregion Delegates

        #region Propriedades

        /// <summary>
        /// Retorna e seta o valor selecionado no combo.
        /// </summary>
        public int Valor
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlAvaliacao.SelectedValue))
                {
                    return Convert.ToInt32(ddlAvaliacao.SelectedValue);
                }

                return -1;
            }
            set
            {
                if (ddlAvaliacao.Items.FindByValue(value.ToString()) != null)
                    ddlAvaliacao.SelectedValue = value.ToString();
                else if (ddlAvaliacao.Items.Count > 0)
                    ddlAvaliacao.SelectedValue = ddlAvaliacao.Items[0].Value;
            }
        }

        /// <summary>
        /// Retorna o texto selecionado no combo
        /// </summary>
        public string Texto
        {
            get
            {
                return ddlAvaliacao.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Retorna ou seta o index selecionado.
        /// </summary>
        public int Index
        {
            set
            {
                ddlAvaliacao.SelectedIndex = value;
            }
            get
            {
                return ddlAvaliacao.SelectedIndex;
            }
        }

        /// <summary>
        /// Propriedade que seta a label e a validação do combo.
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

                cpvAvaliacao.Visible = value;
            }
        }

        /// <summary>
        /// Seta o validationGroup do combo.
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cpvAvaliacao.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Deixa o combo habilitado de acordo com o valor passado.
        /// </summary>
        public bool PermiteEditar
        {
            set
            {
                ddlAvaliacao.Enabled = value;
            }
        }

        ///<summary>
        ///Seta a Label lblTitulo
        ///</summary>
        public string Titulo
        {
            set
            {
                lblTitulo.Text = value;
                cpvAvaliacao.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
            }
        }

        /// <summary>
        /// Mostra ou não o label do combo
        /// </summary>
        public bool ExibeTitulo
        {
            set
            {
                lblTitulo.Visible = value;
            }
        }

        /// <summary>
        /// Seta um valor diferente do padrão para o SkinID do combo.
        /// </summary>
        public string Combo_CssClass
        {
            set
            {
                ddlAvaliacao.CssClass = value;
            }
        }

        /// <summary>
        /// Coloca na primeira linha a mensagem de selecione um item.
        /// </summary>
        public bool MostrarMensagemSelecione
        {
            set
            {
                if (value)
                    ddlAvaliacao.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "-1", true));
            }
        }

        /// <summary>
        /// Retorna a quantidade de itens do combo
        /// </summary>
        public int Count
        {
            get
            {
                return ddlAvaliacao.Items.Count;
            }
        }

        /// <summary>
        /// Armazena o ID do tipo do período do calendário em viewstate.
        /// </summary>
        public int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Retorna se existe o valor informado em um dos itens do combo.
        /// </summary>
        /// <param name="valor">Valor a ser buscado</param>
        /// <returns></returns>
        public bool ExisteValor(int valor)
        {
            return (ddlAvaliacao.Items.FindByValue(valor.ToString()) != null);
        }

        /// <summary>
        /// Seleciona o primeiro item no combo de UA, caso possua somente um.
        /// </summary>
        public void SelecionaPrimeiroItem()
        {
            if (Count == 2)
            {
                ddlAvaliacao.SelectedValue = ddlAvaliacao.Items[1].Value;
            }

            // Dispara os eventos do indexChanged configurados.
            ddlAvaliacao_SelectedIndexChanged(this, null);
        }

        /// <summary>
        /// Grava erro e mostra mensagem no label.
        /// </summary>
        /// <param name="ex">Excessão</param>
        private void TrataErro(Exception ex)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(ex);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }

        /// <summary>
        /// Seta o foco no combo.
        /// </summary>
        public void SetarFoco()
        {
            ddlAvaliacao.Focus();
        }

        /// <summary>
        /// Carrega as avaliações no combo que estão liberadas de acordo com os eventos do calendário,
        /// e com os períodos ligados à disciplina.
        /// </summary>
        /// <param name="entityTurma">Entidade da turma</param>
        /// <param name="tud_id">Id da disciplina</param>
        public void CarregarAvaliacao(TUR_Turma entityTurma, long tud_id, int tpc_idFiltrar = -1, bool incluirPeriodoFinal = false, long doc_id = -1)
        {
            try
            {
                // Busca o evento ligado ao calendário, que seja do tipo definido
                // no parâmetro como de efetivação.
                List<ACA_Evento> listEvento = ACA_EventoBO.GetEntity_Efetivacao_List(entityTurma.cal_id, entityTurma.tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, tpc_idFiltrar, ApplicationWEB.AppMinutosCacheLongo, true, doc_id);

                if (entityTurma.fav_id <= 0)
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("É necessário selecionar uma turma que possua um formato de avaliação.", UtilBO.TipoMensagem.Alerta);

                    Response.Redirect("~/Classe/Efetivacao/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    int valor = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorRecuperacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorRecuperacaoFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    // verifica se existe evento do tipo Efetivação Nota
                    string listaTpcIdPeriodicaPeriodicaFinal = string.Empty;
                    IEnumerable<ACA_Evento> dadoNota =
                        (from ACA_Evento item in listEvento
                         where item.tev_id == valor
                         select item);
                    // se existir, pega os tpc_id's
                    List<ACA_Evento> lt = dadoNota.ToList();
                    if (lt.Count > 0)
                    {
                        var x = from ACA_Evento evt in listEvento
                                where evt.tev_id == valor
                                select evt.tpc_id;

                        foreach (int tpc_id in x.ToList())
                        {
                            if (string.IsNullOrEmpty(listaTpcIdPeriodicaPeriodicaFinal))
                                listaTpcIdPeriodicaPeriodicaFinal += Convert.ToString(tpc_id);
                            else
                                listaTpcIdPeriodicaPeriodicaFinal += "," + Convert.ToString(tpc_id);
                        }
                    }

                    // verifica se existe evento do tipo efetivação recuperacao
                    string listaTpcIdRecuperacao = string.Empty;
                    IEnumerable<ACA_Evento> dadoRecuperacao =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorRecuperacao
                         select item);
                    List<ACA_Evento> ltRe = dadoRecuperacao.ToList();
                    // se existir, pega os tpc_id's
                    if (ltRe.Count > 0)
                    {
                        var x = from ACA_Evento evt in listEvento
                                where
                                    evt.tev_id == valorRecuperacao
                                select evt.tpc_id;

                        foreach (int tpc_id in x.ToList())
                        {
                            if (string.IsNullOrEmpty(listaTpcIdRecuperacao))
                                listaTpcIdRecuperacao += Convert.ToString(tpc_id);
                            else
                                listaTpcIdRecuperacao += "," + Convert.ToString(tpc_id);
                        }
                    }

                    // verifica se existe evento do tipo efetivação final
                    bool existeFinal = false;
                    IEnumerable<ACA_Evento> dadoFinal =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorFinal
                         select item);
                    List<ACA_Evento> ltFinal = dadoFinal.ToList();
                    // se existir, marca para trazer as avaliações do tipo final
                    if (ltFinal.Count > 0)
                    {
                        existeFinal = true;
                    }

                    // verifica se existe evento do tipo recuperação final
                    bool existeRecuperacaoFinal = false;
                    IEnumerable<ACA_Evento> dadoRecuperacaoFinal =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorRecuperacaoFinal
                         select item);
                    List<ACA_Evento> ltRecuperacaoFinal = dadoRecuperacaoFinal.ToList();
                    // se existir, marca para trazer as avaliações do tipo recuperação final
                    if (ltRecuperacaoFinal.Count > 0)
                    {
                        existeRecuperacaoFinal = true;
                    }

                    DataTable dtAvaliacoes;

                    // Se for turma eletiva do aluno, carrega apenas os períodos do calendário em que
                    // a turma é oferecida
                    if ((TUR_TurmaTipo)entityTurma.tur_tipo == TUR_TurmaTipo.EletivaAluno)
                    {
                        List<CadastroTurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectCadastradosBy_Turma(entityTurma.tur_id);
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao_TurmaDisciplinaCalendario(entityTurma.tur_id, listaDisciplinas[0].entTurmaDisciplina.tud_id, entityTurma.fav_id, listaTpcIdPeriodicaPeriodicaFinal, listaTpcIdRecuperacao, existeFinal, true, true);

                        if (tpc_idFiltrar > 0)
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where (tpc_idFiltrar == Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"]))
                                                   select dr);
                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                    }
                    else
                    {
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao(entityTurma.tur_id, entityTurma.fav_id, tud_id, listaTpcIdPeriodicaPeriodicaFinal, listaTpcIdRecuperacao, existeFinal, existeRecuperacaoFinal, true, true, tpc_idFiltrar, ApplicationWEB.AppMinutosCacheLongo);
                    }

                    if (incluirPeriodoFinal)
                    {
                        if (tpc_idFiltrar > 0)
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where (tpc_idFiltrar == Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"]))
                                                   select dr);

                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                        else
                        {
                            var eletivaFiltrada = (from DataRow dr in dtAvaliacoes.Rows
                                                   where ((byte)dr["ava_tipo"] == (byte)AvaliacaoTipo.Final)
                                                   select dr);

                            dtAvaliacoes = eletivaFiltrada.Any() ? eletivaFiltrada.CopyToDataTable() : new DataTable();
                        }
                    }

                    var avaliacoes = (from DataRow dr in dtAvaliacoes.Rows
                                      let fechado = Convert.ToBoolean(dr["ava_tpc_fechado"])
                                      let cap_dataInicio = Convert.ToDateTime(string.IsNullOrEmpty(dr["cap_dataInicio"].ToString()) ? new DateTime().ToString() : dr["cap_dataInicio"])
                                      let tpc_id = Convert.ToInt32(string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? 0 : dr["tpc_id"])
                                      where (!(fechado && cap_dataInicio > DateTime.Now)) && ((VS_tpc_id == -1) || (tpc_id == VS_tpc_id))
                                      select dr);

                    dtAvaliacoes = avaliacoes.Any() ? avaliacoes.CopyToDataTable() : new DataTable();

                    ddlAvaliacao.Items.Clear();
                    ddlAvaliacao.DataTextField = "ava_tpc_nome";
                    ddlAvaliacao.DataSource = dtAvaliacoes;
                    ddlAvaliacao.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "-1"));
                    ddlAvaliacao.AppendDataBoundItems = true;
                    ddlAvaliacao.DataBind();

                    //if (tpc_idSelecionar > 0)
                    //{
                    //    // Seleciona o tpc_id da avaliação relacionada.
                    //}
                }
            }
            catch (ValidationException ex)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Classe/Efetivacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                TrataErro(ex);
            }
        }

        public void CarregarAvaliacaoPorTurmaTipo(long tur_id, string ava_tipos)
        {
            try
            {
                ddlAvaliacao.Items.Clear();
                DataTable dt = new DataTable();

                List<ACA_AvaliacaoBO.sTipoAvaliacao> lstTipoAvaliacao = ACA_AvaliacaoBO.SelecionaPorTipoAvaliacao(ava_tipos, tur_id);

                dt.Columns.Add("ava_id");
                dt.Columns.Add("ava_nome");

                foreach (ACA_AvaliacaoBO.sTipoAvaliacao lstItem in lstTipoAvaliacao)
                {
                    DataRow r = dt.NewRow();

                    r["ava_id"] = lstItem.fav_ava_id.ToString().Split(';')[1];
                    r["ava_nome"] = lstItem.ava_nome;

                    dt.Rows.Add(r);
                }

                ddlAvaliacao.DataSource = dt;
                ddlAvaliacao.AppendDataBoundItems = true;
                ddlAvaliacao.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar avaliações.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlAvaliacao.AutoPostBack = IndexChanged != null;
        }

        protected void ddlAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        #endregion Eventos
    }
}
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MSTech.GestaoEscolar.BLL.ACA_AlunoHistoricoBO;

namespace GestaoEscolar.WebControls.HistoricoEscolar
{
    public partial class UCAddResultadoFinal : MotherUserControl
    {
        #region CONTRANTES

        private const int grvResFinalCompCurricularColumnNota = 1;

        #endregion
        #region PROPRIEDADES

        // private int esa_id = 0;
        // private byte tipo = 0;
        DataTable disciplinas;
        DataTable dtDados;
        private List<ACA_EscalaAvaliacaoParecer> _ltPareceresNota;
        private DataTable _dtPareceresFinal;
        private DataTable _dtResFinalConclusivo;

        public bool VS_permiteEditar
        {
            get
            {
                return ViewState["VS_permiteEditar"] == null ? false : Convert.ToBoolean(ViewState["VS_permiteEditar"]);
            }
            set
            {
                ViewState["VS_permiteEditar"] = value;
            }
        }

        public Int64 VS_alu_id
        {
            get
            {
                return ViewState["VS_alu_id"] == null ? 0 : Convert.ToInt64(ViewState["VS_alu_id"]);
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        public int esa_id
        {
            get
            {
                return ViewState["esa_id"] == null ? 0 : Convert.ToInt16(ViewState["esa_id"]);
            }
            set
            {
                ViewState["esa_id"] = value;
            }
        }

        public short VS_fav_tipo
        {
            get
            {
                return ViewState["VS_fav_tipo"] == null ? (short)0 : Convert.ToInt16(ViewState["VS_fav_tipo"]);
            }
            set
            {
                ViewState["VS_fav_tipo"] = value;
            }
        }

        public byte tipo
        {
            get
            {
                return ViewState["tipo"] == null ? (byte)0 : Convert.ToByte(ViewState["tipo"]);
            }
            set
            {
                ViewState["tipo"] = value;
            }
        }

        public int auxContador
        {
            get
            {
                return Convert.ToInt16(ViewState["auxContador"]);
            }
            set
            {
                ViewState["auxContador"] = value;
            }
        }

        private DataTable dtResFinalConclusivo
        {
            get
            {
                if (_dtResFinalConclusivo != null)
                    return _dtResFinalConclusivo;
                _dtResFinalConclusivo = new DataTable();
                _dtResFinalConclusivo.Columns.Add("tds_nome");

                return _dtResFinalConclusivo;
            }
            set
            {
                _dtResFinalConclusivo = value;
            }
        }

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceresNota
        {
            get
            {
                return _ltPareceresNota ??
                       (_ltPareceresNota = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(esa_id));
            }
        }

        /// <summary>
        /// DataTable de pareceres final.
        /// </summary>
        private DataTable DtPareceresFinal
        {
            get
            {
                return _dtPareceresFinal ??
                       (_dtPareceresFinal = ACA_TipoResultadoBO.SELECT_By_TipoLancamento((byte)EnumTipoLancamento.HistoricoEscolar));
            }
        }

        public string message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        #endregion

        #region MÉTODOS

        public void Carregar(int anoLetivo, int tcp_id, int tne_id)
        {
            try
            {
                dtDados = ACA_AlunoHistoricoBO.Seleciona_TipoCurriculoPeriodoAnoLetivo(tcp_id, anoLetivo, tne_id);
                dtDados.Columns.Add("ahd_id");
                dtDados.Columns.Add("grade", typeof(bool));
                dtDados.Columns.Add("alu_id", typeof(int));
                dtDados.Columns.Add("ahd_avaliacao", typeof(string));
                dtDados.Columns.Add("ahd_frequencia", typeof(int));

                foreach (DataRow row in dtDados.Rows)
                {
                    if (Convert.ToInt32(row["tds_id"]) > 0)
                    {
                        row["grade"] = true;
                    }
                    else
                        row["grade"] = false;
                }

                if (dtDados.Rows.Count == 0)
                {
                    lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.NenhumDadoParaInserir"),
                                                            UtilBO.TipoMensagem.Informacao);
                }
                else
                {
                    ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao { fav_id = Convert.ToInt32(dtDados.Rows[0]["fav_id"]) };
                    ACA_FormatoAvaliacaoBO.GetEntity(fav);

                    VS_fav_tipo = fav.fav_tipo;

                    esa_id = fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal;

                    ACA_EscalaAvaliacao esa = new ACA_EscalaAvaliacao { esa_id = esa_id };
                    ACA_EscalaAvaliacaoBO.GetEntity(esa);
                    tipo = esa.esa_tipo;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ErroCarregar"),
                                                        UtilBO.TipoMensagem.Erro);
            }
        }

        public void CarregarNovo(bool permiteEditar, List<StructHistoricoDisciplina> ltDisciplina)
        {
            VS_permiteEditar = permiteEditar;
            if (dtDados.Rows.Count > 0)
            {
                #region COMPONENTE CURRICULAR
                if (tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    grvResFinalCompCurricular.Columns[grvResFinalCompCurricularColumnNota].HeaderText = (string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.grvResFinalCompCurricular.Header.Conceito");
                else
                    grvResFinalCompCurricular.Columns[grvResFinalCompCurricularColumnNota].HeaderText = (string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.grvResFinalCompCurricular.Header.Nota");

                StructCompCurricular disc = new StructCompCurricular();
                List<StructCompCurricular> lista = (from dados in dtDados.AsEnumerable()
                                                    where Convert.ToByte(dados.Field<object>("tds_tipo")) == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.Disciplina
                                                    select (StructCompCurricular)GestaoEscolarUtilBO.DataRowToEntity(dados, disc)).ToList();

                // Adicionar itens que não fazem parte da grade
                foreach (StructHistoricoDisciplina discHist in ltDisciplina.FindAll(p => Convert.ToInt32(p.tds_id) <= 0))
                {
                    StructCompCurricular item = new StructCompCurricular
                    {
                        ahd_id = discHist.entDisciplina.ahd_id,
                        tds_nome = discHist.entDisciplina.ahd_disciplina,
                        tds_id = discHist.entDisciplina.tds_id,
                        grade = false
                    };

                    lista.Add(item);
                }

                // Adiciona uma linha em branco no final
                StructCompCurricular novoItem = new StructCompCurricular
                {
                    ahd_id = -1,
                    tds_id = -1,
                    grade = false
                };

                lista.Add(novoItem);
                auxContador = lista.Count;
                grvResFinalCompCurricular.DataSource = lista;

                grvResFinalCompCurricular.DataBind();
                #endregion

                #region ENRIQUECIMENTO CURRICULAR
                grvResFinalEnrCurricular.DataSource = (from dados in dtDados.AsEnumerable()
                                                       where Convert.ToByte(dados.Field<object>("tds_tipo")) == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.EnriquecimentoCurricular
                                                       select dados).CopyToDataTable();
                grvResFinalEnrCurricular.DataBind();
                #endregion
            }
            #region PORJ/ATIV COMPLEMENTAR
            grvResFinalProjAtivCompl.DataSource = ACA_AlunoHistoricoProjetoBO.SelectBy_Aluno(VS_alu_id);
            grvResFinalProjAtivCompl.DataBind();

            #endregion

            #region PARECER CONCLUSIVO
            DataRow rowAdd = dtResFinalConclusivo.NewRow();
            rowAdd["tds_nome"] = (string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.grvResFinalConclusivo.ItemParecerConclusivo");
            dtResFinalConclusivo.Rows.Add(rowAdd);

            grvResFinalConclusivo.DataSource = dtResFinalConclusivo;
            grvResFinalConclusivo.DataBind();
            #endregion

        }

        public void CarregarDados(SortedList<int, ACA_AlunoHistoricoBO.StructHistorico> alh, bool permiteEditar)
        {
            if (dtDados.Rows.Count > 0)
                CarregarNovo(permiteEditar, alh.FirstOrDefault().Value.ltDisciplina);
            {
                #region COMPONENTE CURRICULAR

                foreach (GridViewRow row in grvResFinalCompCurricular.Rows)
                {
                    TextBox txtNota = (TextBox)row.FindControl("txtNota");
                    DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                    HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");

                    int ahdId;
                    int.TryParse(hdnAhdId.Value, out ahdId);

                    if (ahdId == 0)
                    {
                        hdnAhdId.Value = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                          where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"])
                                          select dados.entDisciplina.ahd_id).FirstOrDefault().ToString();
                    }

                    if (ahdId < 0)
                    {
                        hdnAhdId.Value = "-1";
                    }

                    if (ahdId > 0 || Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"]) > 0)
                    {

                        if (txtNota.Visible)
                        {
                            if (ahdId <= 0)
                            {
                                txtNota.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                                where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"])
                                                select dados.entDisciplina.ahd_avaliacao).FirstOrDefault();
                            }
                            else
                            {
                                txtNota.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                                where dados.entDisciplina.ahd_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["ahd_id"])
                                                select dados.entDisciplina.ahd_avaliacao).FirstOrDefault();
                            }
                        }
                        else if (ddlPareceres.Visible)
                        {
                            string valor;
                            if (ahdId <= 0)
                            {
                                valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                         where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"])
                                         select dados.entDisciplina.ahd_avaliacao).FirstOrDefault();
                            }
                            else
                            {
                                valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                         where dados.entDisciplina.ahd_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["ahd_id"])
                                         select dados.entDisciplina.ahd_avaliacao).FirstOrDefault();
                            }
                            valor = (from par in LtPareceresNota
                                     where par.eap_valor.Equals(valor)
                                     select par.eap_valor + ";" + par.eap_ordem.ToString()).FirstOrDefault();
                            if (ddlPareceres.Items.FindByValue(valor) != null)
                                ddlPareceres.SelectedValue = valor;
                        }

                        TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                        DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");

                        if (txtFrequencia.Visible)
                        {
                            if (ahdId <= 0)
                            {
                                txtFrequencia.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                                      where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"])
                                                      select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                            }
                            else
                            {
                                txtFrequencia.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                                      where dados.entDisciplina.ahd_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["ahd_id"])
                                                      select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                            }
                        }
                        else if (ddlFrequencia.Visible)
                        {
                            string valor;
                            if (ahdId <= 0)
                            {
                                valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                         where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"])
                                         select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                            }
                            else
                            {
                                valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                         where dados.entDisciplina.ahd_id == Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["ahd_id"])
                                         select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                            }

                            if (ddlFrequencia.Items.FindByValue(valor) != null)
                                ddlFrequencia.SelectedValue = valor;
                        }
                    }
                }

                #endregion

                #region ENRIQUECIMENTO CURRICULAR
                foreach (GridViewRow row in grvResFinalEnrCurricular.Rows)
                {
                    TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                    DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                    HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");

                    hdnAhdId.Value = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                      where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalEnrCurricular.DataKeys[row.RowIndex]["tds_id"])
                                      select dados.entDisciplina.ahd_id).FirstOrDefault().ToString();

                    if (txtFrequencia.Visible)
                        txtFrequencia.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                              where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalEnrCurricular.DataKeys[row.RowIndex]["tds_id"])
                                              select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                    else if (ddlFrequencia.Visible)
                    {
                        string valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                        where dados.entDisciplina.tds_id == Convert.ToInt32(grvResFinalEnrCurricular.DataKeys[row.RowIndex]["tds_id"])
                                        select dados.entDisciplina.ahd_resultado.ToString()).FirstOrDefault();
                        if (ddlFrequencia.Items.FindByValue(valor) != null)
                            ddlFrequencia.SelectedValue = valor;
                    }
                }
                #endregion
            }
            #region PORJ/ATIV COMPLEMENTAR
            foreach (GridViewRow row in grvResFinalProjAtivCompl.Rows)
            {
                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");

                hdnAhdId.Value = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                  where dados.entDisciplina.ahp_id == Convert.ToInt32(grvResFinalProjAtivCompl.DataKeys[row.RowIndex]["ahp_id"])
                                  select dados.entDisciplina.ahd_id).FirstOrDefault().ToString();

                if (txtFrequencia.Visible)
                    txtFrequencia.Text = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                          where dados.entDisciplina.ahp_id == Convert.ToInt32(grvResFinalProjAtivCompl.DataKeys[row.RowIndex]["ahp_id"])
                                          select dados.entDisciplina.ahd_frequencia).FirstOrDefault();
                else if (ddlFrequencia.Visible)
                {
                    string valor = (from dados in alh.FirstOrDefault().Value.ltDisciplina
                                    where dados.entDisciplina.ahp_id == Convert.ToInt32(grvResFinalProjAtivCompl.DataKeys[row.RowIndex]["ahp_id"])
                                    select dados.entDisciplina.ahd_resultado.ToString()).FirstOrDefault();
                    if (ddlFrequencia.Items.FindByValue(valor) != null)
                        ddlFrequencia.SelectedValue = valor;
                }
            }
            #endregion

            #region PARECER CONCLUSIVO
            foreach (GridViewRow row in grvResFinalConclusivo.Rows)
            {
                DropDownList ddlParecerConclusivo = (DropDownList)row.FindControl("ddlParecerConclusivo");

                if (ddlParecerConclusivo.Items.FindByValue(ACA_AlunoHistoricoBO.HistoricoRes(alh.FirstOrDefault().Value.entHistorico.alh_resultado).ToString()) != null)
                    ddlParecerConclusivo.SelectedValue = ACA_AlunoHistoricoBO.HistoricoRes(alh.FirstOrDefault().Value.entHistorico.alh_resultado).ToString();
            }
            #endregion

        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        /// <param name="Final">Indica se serão carregados os pareceres da avaliação final</param>
        private void CarregarPareceres(DropDownList ddlPareceres, bool Final)
        {
            try
            {
                if (Final)
                {
                    ListItem li = new ListItem((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ddlPareceresFinal.Selecione"), "-1", true);
                    ddlPareceres.Items.Add(li);

                    foreach (DataRow dr in DtPareceresFinal.Rows)
                    {
                        li = new ListItem(dr["tpr_nomenclatura"].ToString(), dr["tpr_resultado"].ToString());
                        ddlPareceres.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ddlPareceres.Selecione"), "-1;-1", true);
                    ddlPareceres.Items.Add(li);

                    foreach (ACA_EscalaAvaliacaoParecer eap in LtPareceresNota)
                    {
                        li = new ListItem(eap.descricao, eap.eap_valor + ";" + eap.eap_ordem);
                        ddlPareceres.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ErroCarregarPareceres"),
                                                        UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        private void CarregarPareceresFreq(DropDownList ddlFrequencia)
        {
            ddlFrequencia.Items.Add(new ListItem((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ddlFrequencia.Selecione"), "-1", true));
            ddlFrequencia.Items.Add(new ListItem((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ddlFrequencia.Frequente"), ((short)ResultadoHistoricoDisciplina.Aprovado).ToString()));
            ddlFrequencia.Items.Add(new ListItem((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.ddlFrequencia.Ausente"), ((short)ResultadoHistoricoDisciplina.ReprovadoFrequencia).ToString()));
        }

        public List<ACA_AlunoHistoricoBO.StructCompCurricular> RetornaResultadoFinal()
        {
            List<ACA_AlunoHistoricoBO.StructCompCurricular> retorno = new List<ACA_AlunoHistoricoBO.StructCompCurricular>();
            foreach (GridViewRow row in grvResFinalCompCurricular.Rows)
            {
                TextBox txtNota = (TextBox)row.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");
                TextBox txtNome = (TextBox)row.FindControl("txtNome");
                Label lblNome = (Label)row.FindControl("lblNome");

                if (!String.IsNullOrEmpty(txtNome.Text.Trim()) || !String.IsNullOrEmpty(lblNome.Text.Trim()))
                {
                    ACA_AlunoHistoricoBO.StructCompCurricular add = new ACA_AlunoHistoricoBO.StructCompCurricular();
                    add.ahd_id = string.IsNullOrEmpty(hdnAhdId.Value) ? -1 : Convert.ToInt32(hdnAhdId.Value);
                    add.nota = txtNota.Visible ? txtNota.Text : ddlPareceres.SelectedValue.Split(';')[0].Equals("-1") ? "" : ddlPareceres.SelectedValue.Split(';')[0];
                    add.frequencia = txtFrequencia.Visible ? txtFrequencia.Text : ddlFrequencia.SelectedValue.Equals("-1") ? "" : ddlFrequencia.SelectedValue;

                    if (Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"]) <= 0)
                    {
                        add.tds_id = 0;
                        add.tds_nome = txtNome.Text;
                    }
                    else
                    {
                        add.tds_nome = grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_nome"].ToString();
                        add.tds_id = Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"]);
                    }

                    retorno.Add(add);

                    if (txtFrequencia.Visible && !string.IsNullOrEmpty(txtFrequencia.Text) &&
                        (Convert.ToDecimal(txtFrequencia.Text) > 100 || Convert.ToDecimal(txtFrequencia.Text) < 0))
                        throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.FrequenciaInvalida"));
                }
                else if (txtNota.Visible && (!String.IsNullOrEmpty(txtNota.Text) || !String.IsNullOrEmpty(txtFrequencia.Text) || !String.IsNullOrEmpty(ddlPareceres.SelectedValue)
                        || !String.IsNullOrEmpty(ddlFrequencia.SelectedValue)))
                {
                    throw new ValidationException("Componente curricular é obrigatório.");
                }
                else if (!txtNota.Visible && ((!String.IsNullOrEmpty(txtNota.Text) || !String.IsNullOrEmpty(txtFrequencia.Text) || ddlPareceres.SelectedValue != "-1;-1")
                        || !String.IsNullOrEmpty(ddlFrequencia.SelectedValue)))
                {
                    throw new ValidationException("Componente curricular é obrigatório.");
                }

            }
            return retorno;
        }

        public List<ACA_AlunoHistoricoBO.StructEnrCurricular> RetornaEnriquecimentoCurricular()
        {
            List<ACA_AlunoHistoricoBO.StructEnrCurricular> retorno = new List<ACA_AlunoHistoricoBO.StructEnrCurricular>();
            foreach (GridViewRow row in grvResFinalEnrCurricular.Rows)
            {
                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");

                ACA_AlunoHistoricoBO.StructEnrCurricular add = new ACA_AlunoHistoricoBO.StructEnrCurricular();
                add.tds_id = Convert.ToInt32(grvResFinalEnrCurricular.DataKeys[row.RowIndex]["tds_id"]);
                add.ahd_id = string.IsNullOrEmpty(hdnAhdId.Value) ? -1 : Convert.ToInt32(hdnAhdId.Value);
                add.tds_nome = grvResFinalEnrCurricular.DataKeys[row.RowIndex]["tds_nome"].ToString();
                add.frequencia = txtFrequencia.Visible ? txtFrequencia.Text : ddlFrequencia.SelectedValue.Equals("-1") ? "" : ddlFrequencia.SelectedValue;
                retorno.Add(add);

                if (txtFrequencia.Visible && !string.IsNullOrEmpty(txtFrequencia.Text) &&
                    (Convert.ToDecimal(txtFrequencia.Text) > 100 || Convert.ToDecimal(txtFrequencia.Text) < 0))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.FrequenciaInvalida"));
            }
            return retorno;
        }

        public List<ACA_AlunoHistoricoBO.StructProjAtivComplementar> RetornaProjAtivComplementares()
        {
            List<ACA_AlunoHistoricoBO.StructProjAtivComplementar> retorno = new List<ACA_AlunoHistoricoBO.StructProjAtivComplementar>();
            foreach (GridViewRow row in grvResFinalProjAtivCompl.Rows)
            {
                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");

                ACA_AlunoHistoricoBO.StructProjAtivComplementar add = new ACA_AlunoHistoricoBO.StructProjAtivComplementar();
                add.ahp_id = Convert.ToInt32(grvResFinalProjAtivCompl.DataKeys[row.RowIndex]["ahp_id"]);
                add.ahd_id = string.IsNullOrEmpty(hdnAhdId.Value) ? -1 : Convert.ToInt32(hdnAhdId.Value);
                add.ahp_nome = grvResFinalProjAtivCompl.DataKeys[row.RowIndex]["ahp_nome"].ToString();
                add.frequencia = txtFrequencia.Visible ? txtFrequencia.Text : ddlFrequencia.SelectedValue.Equals("-1") ? "" : ddlFrequencia.SelectedValue;
                retorno.Add(add);

                if (txtFrequencia.Visible && !string.IsNullOrEmpty(txtFrequencia.Text) &&
                    (Convert.ToDecimal(txtFrequencia.Text) > 100 || Convert.ToDecimal(txtFrequencia.Text) < 0))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.FrequenciaInvalida"));
            }
            return retorno;
        }

        public List<ACA_AlunoHistoricoBO.StructCompCurricular> RetornaListaDisciplinas(bool exclusao)
        {
            List<ACA_AlunoHistoricoBO.StructCompCurricular> retorno = new List<ACA_AlunoHistoricoBO.StructCompCurricular>();

            foreach (GridViewRow row in grvResFinalCompCurricular.Rows)
            {
                TextBox txtNota = (TextBox)row.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)row.FindControl("ddlPareceres");
                TextBox txtFrequencia = (TextBox)row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)row.FindControl("ddlFrequencia");
                HiddenField hdnAhdId = (HiddenField)row.FindControl("hdnAhdId");
                TextBox txtNome = (TextBox)row.FindControl("txtNome");
                Label lblNome = (Label)row.FindControl("lblNome");

                ACA_AlunoHistoricoBO.StructCompCurricular add = new ACA_AlunoHistoricoBO.StructCompCurricular();

                add.ahd_id = string.IsNullOrEmpty(hdnAhdId.Value) ? -1 : Convert.ToInt32(hdnAhdId.Value);
                add.nota = txtNota.Visible ? txtNota.Text : ddlPareceres.SelectedValue.Split(';')[0].Equals("-1") ? "" : ddlPareceres.SelectedValue;
                add.frequencia = txtFrequencia.Visible ? txtFrequencia.Text : ddlFrequencia.SelectedValue.Equals("-1") ? "" : ddlFrequencia.SelectedValue;

                if (Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"]) <= 0)
                {
                    add.tds_id = 0;
                    add.grade = false;
                    if (!String.IsNullOrEmpty(txtNome.Text.Trim()))
                        add.tds_nome = txtNome.Text;
                    else if (!exclusao)
                    {
                        throw new ValidationException("Componente curricular é obrigatório.");
                    }
                }
                else
                {
                    add.tds_id = Convert.ToInt32(grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_id"]);
                    add.grade = true;
                    add.tds_nome = grvResFinalCompCurricular.DataKeys[row.RowIndex]["tds_nome"].ToString();
                }

                retorno.Add(add);

                if (txtFrequencia.Visible && !string.IsNullOrEmpty(txtFrequencia.Text) &&
                    (Convert.ToDecimal(txtFrequencia.Text) > 100 || Convert.ToDecimal(txtFrequencia.Text) < 0))
                    throw new ValidationException((string)GetGlobalResourceObject("UserControl", "UCAddResultadoFinal.FrequenciaInvalida"));
            }
            return retorno;
        }

        public short RetornaParecerConclusivo()
        {
            if (grvResFinalConclusivo.Rows.Count > 0)
            {
                DropDownList ddlParecerConclusivo = (DropDownList)grvResFinalConclusivo.Rows[0].FindControl("ddlParecerConclusivo");
                return Convert.ToInt16(ddlParecerConclusivo.SelectedValue);
            }
            return -1;
        }

        public string RetornaParecerConclusivoText()
        {
            if (grvResFinalConclusivo.Rows.Count > 0)
            {
                DropDownList ddlParecerConclusivo = (DropDownList)grvResFinalConclusivo.Rows[0].FindControl("ddlParecerConclusivo");
                if (Convert.ToInt16(ddlParecerConclusivo.SelectedValue) > 0)
                    return ddlParecerConclusivo.SelectedItem.Text;
            }
            return "";
        }

        #endregion

        #region EVENTOS

        protected void grvResFinalCompCurricular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                StructCompCurricular disc = (StructCompCurricular)e.Row.DataItem;
                TextBox txtNota = (TextBox)e.Row.FindControl("txtNota");
                DropDownList ddlPareceres = (DropDownList)e.Row.FindControl("ddlPareceres");

                txtNota.Visible = tipo == (byte)EscalaAvaliacaoTipo.Numerica;
                ddlPareceres.Visible = tipo == (byte)EscalaAvaliacaoTipo.Pareceres;



                if (ddlPareceres.Visible)
                {
                    CarregarPareceres(ddlPareceres, false);
                    ddlPareceres.SelectedValue = disc.nota;
                }

                if (txtNota.Visible)
                {
                    txtNota.Text = disc.nota;
                }


                TextBox txtFrequencia = (TextBox)e.Row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)e.Row.FindControl("ddlFrequencia");

                TextBox txtNome = (TextBox)e.Row.FindControl("txtNome");
                Label lblNome = (Label)e.Row.FindControl("lblNome");
                ImageButton adicionar = (ImageButton)e.Row.FindControl("btnAdicionar");
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");


                txtFrequencia.Text = disc.frequencia;
                txtFrequencia.Visible = true;
                ddlFrequencia.Visible = false;

                txtNota.Enabled = ddlPareceres.Enabled = txtFrequencia.Enabled = ddlFrequencia.Enabled = VS_permiteEditar;

                if (disc.grade)
                {
                    lblNome.Visible = true;
                    txtNome.Visible = false;
                    btnExcluir.Visible = false;
                }
                else
                {
                    lblNome.Visible = false;
                    txtNome.Visible = true;
                    txtNome.Text = disc.tds_nome;
                    btnExcluir.Visible = true;
                }

                if (e.Row.RowIndex == auxContador - 1)
                {
                    adicionar.Visible = true;
                    btnExcluir.Visible = false;
                }
                else
                {
                    adicionar.Visible = false;
                }
            }

        }

        protected void grvResFinalEnrCurricular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtFrequencia = (TextBox)e.Row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)e.Row.FindControl("ddlFrequencia");

                txtFrequencia.Visible = false;
                ddlFrequencia.Visible = true;

                CarregarPareceresFreq(ddlFrequencia);

                txtFrequencia.Enabled = ddlFrequencia.Enabled = VS_permiteEditar;
            }

        }

        protected void grvResFinalProjAtivCompl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtFrequencia = (TextBox)e.Row.FindControl("txtFrequencia");
                DropDownList ddlFrequencia = (DropDownList)e.Row.FindControl("ddlFrequencia");

                txtFrequencia.Visible = false;
                ddlFrequencia.Visible = true;

                CarregarPareceresFreq(ddlFrequencia);

                //txtFrequencia.Enabled = ddlFrequencia.Enabled = VS_permiteEditar;
            }
        }

        protected void grvResFinalConclusivo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlParecerConclusivo = (DropDownList)e.Row.FindControl("ddlParecerConclusivo");
                CarregarPareceres(ddlParecerConclusivo, true);

                ddlParecerConclusivo.Enabled = VS_permiteEditar;
            }
        }

        protected void grvResFinalCompCurricular_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Adicionar")
                {
                    List<ACA_AlunoHistoricoBO.StructCompCurricular> retorno = new List<ACA_AlunoHistoricoBO.StructCompCurricular>();
                    retorno = RetornaListaDisciplinas(false);

                    //Adiciona linha           
                    StructCompCurricular novoItem = new StructCompCurricular
                    {
                        ahd_id = -1,
                        tds_id = -1,
                        grade = false
                    };
                    retorno.Add(novoItem);


                    auxContador = retorno.Count;
                    grvResFinalCompCurricular.DataSource = retorno;
                    grvResFinalCompCurricular.DataBind();
                }

                if (e.CommandName == "Excluir")
                {
                    List<ACA_AlunoHistoricoBO.StructCompCurricular> retorno = new List<ACA_AlunoHistoricoBO.StructCompCurricular>();
                    retorno = RetornaListaDisciplinas(true);

                    Int32 id = Convert.ToInt32(e.CommandArgument);
                    retorno.RemoveAt(id);
                    auxContador = retorno.Count;
                    grvResFinalCompCurricular.DataSource = retorno;
                    grvResFinalCompCurricular.DataBind();
                }
            }
            catch (ValidationException ex)
            {
                __SessionWEB.PostMessages = ex.Message;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro alterar histórico.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}
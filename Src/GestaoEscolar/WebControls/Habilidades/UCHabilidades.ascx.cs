using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Habilidades
{
    public partial class UCHabilidades : MotherUserControl
    {
        #region Structs

        [Serializable]
        private struct sOrientacaoAlcancada
        {
            public long ocr_id { get; set; }
            public int id { get; set; }
            public bool alcancado { get; set; }
        }

        #endregion Structs

        #region Propriedades

        public string TituloFildSet
        {
            get { return lblTituloFildSet.Text; }
            set { lblTituloFildSet.Text = value; }
        }

        public string LegendaCheck
        {
            get { return lblLegendaCheck.Text; }
            set { lblLegendaCheck.Text = value; }
        }

        public string sTitulo
        {
            get { return lblTituloFildSet.Text; }
            set { lblTituloFildSet.Text = value; }
        }

        public bool bHabilidaEdicao
        {
            get
            {
                if (ViewState["VS_bHabilidaEdicao"] == null)
                    return false;
                return (bool)ViewState["VS_bHabilidaEdicao"];
            }
            set { ViewState["VS_bHabilidaEdicao"] = value; }
        }

        private List<sOrientacaoNivelAprendizado> dtOrientacaoNiveisAprendizado
        {
            get
            {
                if (ViewState["VS_dtOrientacaoNiveisAprendizado"] == null)
                    return new List<sOrientacaoNivelAprendizado>();
                return (List<sOrientacaoNivelAprendizado>)ViewState["VS_dtOrientacaoNiveisAprendizado"];
            }
            set { ViewState["VS_dtOrientacaoNiveisAprendizado"] = value; }
        }

        private int? nivel;

        /// <summary>
        /// Nível da orientação curricular anterior usado no databound desses.
        /// </summary>
        private int Nivel
        {
            get
            {
                return Convert.ToInt32(nivel ?? 0);
            }

            set
            {
                nivel = value;
            }
        }

        private byte VS_PosicaoDocente
        {
            get
            {
                if (ViewState["VS_PosicaoDocente"] == null)
                    return 0;
                return (byte)ViewState["VS_PosicaoDocente"];
            }
            set { ViewState["VS_PosicaoDocente"] = value; }
        }

        public int VS_tnt_id
        {
            get
            {
                if (ViewState["VS_tnt_id"] == null)
                    return 0;
                return (int)ViewState["VS_tnt_id"];
            }
            set { ViewState["VS_tnt_id"] = value; }
        }

        public string VS_idAluno
        {
            get
            {
                if (ViewState["VS_idAluno"] == null)
                    return string.Empty;
                return ViewState["VS_idAluno"].ToString();
            }
            set { ViewState["VS_idAluno"] = value; }
        }

        private List<sOrientacaoAlcancada> listHabilidades;

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// O método carrega as orientações curriculares da turma.
        /// </summary>
        private void CarregarHabilidades(
            DataTable dtDataSource,
            byte PosicaoDocente,
             List<sOrientacaoNivelAprendizado> dtNiveisAprendizado
        )
        {
            rptHabilidades.DataSource = dtDataSource;
            rptHabilidades.DataBind();

            Nivel = 0;
            VS_PosicaoDocente = PosicaoDocente;

            if (!listHabilidades.Any())
            {
                dtOrientacaoNiveisAprendizado = dtNiveisAprendizado == null ? new List<sOrientacaoNivelAprendizado>() : dtNiveisAprendizado;
            }
        }

        /// <summary>
        /// O método carrega as orientações curriculares da turma.
        /// </summary>
        public void CarregarHabilidades(
            int cur_id,
            int crr_id,
            int crp_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            int tnt_id,
            int tpc_id
        )
        {
            DataTable dtOrientacaoCurricular;

            // se a avaliacao tive uma avaliacao pai relacionada, mostrar apenas as habilidades selecionadas na avaliacao pai
            CLS_TurmaNota avaRelacionada = CLS_TurmaNotaBO.GetSelectRelacionadaPai(tud_id, tnt_id);
            if (avaRelacionada.tnt_id > 0)
            {
                dtOrientacaoCurricular = CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplinaAvaliacao
                                            (cur_id, crr_id, crp_id, -1, tpc_id, tur_id, tud_id, cal_id, tdt_posicao, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id, avaRelacionada.tnt_id);

            }
            else
            {
                dtOrientacaoCurricular = CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplina
                                            (cur_id, crr_id, crp_id, -1, tpc_id, tur_id, tud_id, cal_id, tdt_posicao, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }

            VS_tnt_id = tnt_id;
            VS_idAluno = string.Empty;

            //Carrega as habilidades cadastradas para a avaliacao e atualiza o datatable
            listHabilidades = new List<sOrientacaoAlcancada>();
            if (VS_tnt_id > 0)
            {
                CLS_TurmaNotaOrientacaoCurricularBO.SelecionaPorAvaliacao(tud_id, VS_tnt_id).ForEach(
                    p => listHabilidades.Add(new sOrientacaoAlcancada { ocr_id = p.ocr_id, id = p.toc_id, alcancado = p.toc_alcancado })
                );
            }

            List<sOrientacaoNivelAprendizado> dtOrientacaoNiveisAprendizadoAux = new List<sOrientacaoNivelAprendizado>();
            if (dtOrientacaoCurricular.Rows.Count > 0)
            {
                string ocr_ids = string.Join(";", (from DataRow dr in dtOrientacaoCurricular.Rows
                                                   let chave = dr["Chave"].ToString()
                                                   let idsChave = chave.Split(';')
                                                   let ocr_id = idsChave.Length > 1 ? idsChave[1] : ""
                                                   where !string.IsNullOrEmpty(ocr_id)
                                                   select ocr_id).Distinct().ToArray());

                dtOrientacaoNiveisAprendizadoAux = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, null, ApplicationWEB.AppMinutosCacheLongo);
            }

            CarregarHabilidades(dtOrientacaoCurricular, tdt_posicao, dtOrientacaoNiveisAprendizadoAux);
        }

        /// <summary>
        /// O método carrega as orientações curriculares da avaliação.
        /// </summary>
        public void CarregarHabilidadesAluno(
            int cur_id,
            int crr_id,
            int crp_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            int tnt_id,
            int tpc_id,
            long alu_id,
            int mtu_id,
            int mtd_id
        )
        {
            DataTable dtOrientacaoCurricular = CLS_PlanejamentoOrientacaoCurricularBO.SelecionaPorTurmaPeriodoDisciplinaAvaliacao
                                                    (cur_id, crr_id, crp_id, -1, tpc_id, tur_id, tud_id, cal_id, tdt_posicao, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id, tnt_id);

            VS_tnt_id = tnt_id;
            VS_idAluno = string.Format("{0};{1};{2}", alu_id, mtu_id, mtd_id);

            //Carrega as habilidades cadastradas para o aluno na avaliacao e atualiza o datatable
            listHabilidades = new List<sOrientacaoAlcancada>();
            if (VS_tnt_id > 0)
            {
                CLS_TurmaNotaAlunoOrientacaoCurricularBO.SelecionaPorAvaliacaoAluno(tud_id, VS_tnt_id, alu_id, mtu_id, mtd_id).ForEach(
                    p => listHabilidades.Add(new sOrientacaoAlcancada { ocr_id = p.ocr_id, id = p.aoc_id, alcancado = p.aoc_alcancado })
                );
            }

            List<sOrientacaoNivelAprendizado> dtOrientacaoNiveisAprendizadoAux = new List<sOrientacaoNivelAprendizado>();
            if (dtOrientacaoCurricular.Rows.Count > 0)
            {
                string ocr_ids = string.Join(";", (from DataRow dr in dtOrientacaoCurricular.Rows
                                                   let chave = dr["Chave"].ToString()
                                                   let idsChave = chave.Split(';')
                                                   let ocr_id = idsChave.Length > 1 ? idsChave[1] : ""
                                                   where !string.IsNullOrEmpty(ocr_id)
                                                   select ocr_id).Distinct().ToArray());

                dtOrientacaoNiveisAprendizadoAux = ORC_OrientacaoCurricularNivelAprendizadoBO.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, 0, null, ApplicationWEB.AppMinutosCacheLongo);
            }

            CarregarHabilidades(dtOrientacaoCurricular, tdt_posicao, dtOrientacaoNiveisAprendizadoAux);
        }

        public List<CLS_TurmaNotaOrientacaoCurricular> RetornaListaHabilidades()
        {
            List<CLS_TurmaNotaOrientacaoCurricular> lista = new List<CLS_TurmaNotaOrientacaoCurricular>();

            lista.AddRange(
                (
                    from RepeaterItem habilidade in rptHabilidades.Items
                    let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                    let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                    where permiteLancamento
                    let hdnPosicao = (HiddenField)habilidade.FindControl("hdnPosicao")
                    let posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicao.Value) ? VS_PosicaoDocente.ToString() : hdnPosicao.Value)
                    let chave = ((HiddenField)habilidade.FindControl("hdnChave")).Value
                    let chkAlcancado = (CheckBox)habilidade.FindControl("chkAlcancado")
                    select new CLS_TurmaNotaOrientacaoCurricular
                    {
                        tud_id = Convert.ToInt64(chave.Split(';')[0]),
                        tnt_id = VS_tnt_id,
                        ocr_id = Convert.ToInt64(chave.Split(';')[1]),
                        toc_id = Convert.ToInt32(chave.Split(';')[3]),
                        toc_alcancado = chkAlcancado.Checked,
                        toc_situacao = (short)TurmaNotaOrientacaoCurricularSituacao.Ativo,
                        IsNew = !(Convert.ToInt64(chave.Split(';')[3]) > 0)
                    }
                ).ToList()
            );

            return lista;
        }

        public List<CLS_TurmaNotaAlunoOrientacaoCurricular> RetornaListaHabilidadesAluno()
        {
            List<CLS_TurmaNotaAlunoOrientacaoCurricular> lista = new List<CLS_TurmaNotaAlunoOrientacaoCurricular>();

            lista.AddRange(
                (
                    from RepeaterItem habilidade in rptHabilidades.Items
                    let hdnPermiteLancamento = (HiddenField)habilidade.FindControl("hdnPermiteLancamento")
                    let permiteLancamento = Convert.ToBoolean(hdnPermiteLancamento.Value)
                    where permiteLancamento
                    let hdnPosicao = (HiddenField)habilidade.FindControl("hdnPosicao")
                    let posicao = Convert.ToByte(string.IsNullOrEmpty(hdnPosicao.Value) ? VS_PosicaoDocente.ToString() : hdnPosicao.Value)
                    let chave = ((HiddenField)habilidade.FindControl("hdnChave")).Value
                    let chkAlcancado = (CheckBox)habilidade.FindControl("chkAlcancado")
                    let idAluno = VS_idAluno.Split(';')
                    select new CLS_TurmaNotaAlunoOrientacaoCurricular
                    {
                        tud_id = Convert.ToInt64(chave.Split(';')[0]),
                        tnt_id = VS_tnt_id,
                        alu_id = Convert.ToInt64(idAluno[0]),
                        mtu_id = Convert.ToInt32(idAluno[1]),
                        mtd_id = Convert.ToInt32(idAluno[2]),
                        ocr_id = Convert.ToInt64(chave.Split(';')[1]),
                        aoc_id = Convert.ToInt32(chave.Split(';')[3]),
                        aoc_alcancado = chkAlcancado.Checked,
                        aoc_situacao = (short)TurmaNotaAlunoOrientacaoCurricularSituacao.Ativo,
                        IsNew = !(Convert.ToInt64(chave.Split(';')[3]) > 0)
                    }
                ).ToList()
            );

            return lista;
        }

        /// <summary>
        /// O método copia n vezes uma string e a concatena para si mesma.
        /// </summary>
        /// <param name="valor">String a ser copiado.</param>
        /// <param name="multiplicacao">Quantidade de vezes que o valor será replicado.</param>
        /// <returns></returns>
        private string MultiplicaString(string valor, int multiplicacao)
        {
            StringBuilder sb = new StringBuilder(multiplicacao * valor.Length);
            for (int i = 0; i < multiplicacao; i++)
            {
                sb.Append(valor);
            }

            return sb.ToString();
        }

        #endregion Métodos

        #region Eventos

        protected void rptHabilidades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chkAlcancado = (CheckBox)e.Item.FindControl("chkAlcancado");

                #region Seta a visibilidade das colunas

                HtmlControl spanAlcancadoColuna = (HtmlControl)e.Item.FindControl("spanAlcancadoColuna");

                if (chkAlcancado != null)
                    chkAlcancado.Enabled = bHabilidaEdicao;

                #endregion Seta a visibilidade das colunas

                #region Monta a arvore

                int nivelLinha = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Nivel"));
                bool permiteLancamento = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PermiteLancamento"));
                string ocr_codigo = DataBinder.Eval(e.Item.DataItem, "Codigo").ToString();
                string ocr_descricao = DataBinder.Eval(e.Item.DataItem, "Descricao").ToString();

                bool AlcanceEfetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "AlcanceEfetivado"));

                bool HouveModificacao = !Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "NI"));
                if (AlcanceEfetivado)
                    HouveModificacao = false;

                string chave = DataBinder.Eval(e.Item.DataItem, "Chave").ToString();
                Literal litCabecalho = (Literal)e.Item.FindControl("litCabecalho");
                Literal litConteudo = (Literal)e.Item.FindControl("litConteudo");
                HtmlControl divHabilidade = (HtmlControl)e.Item.FindControl("divHabilidade");
                Literal lblHabilidade = (Literal)e.Item.FindControl("lblHabilidade");

                Literal litRodape = (Literal)e.Item.FindControl("litRodape");

                string ul = String.Format("<ul class='treeview' style='display: {0};'>", nivelLinha == 1 ? "block" : "none");
                string li = nivelLinha == Nivel ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") :
                                                  (nivelLinha > Nivel || !string.IsNullOrEmpty(chave) ? (permiteLancamento ? "<li style='display: table; width: 100%;'>" : "<li class='expandable'>") : "<li class='expandable'>");

                string cabecalho = nivelLinha == Nivel ? li :
                    (nivelLinha > Nivel ? ul + li : MultiplicaString("</li></ul>", Nivel - nivelLinha) + li);
                cabecalho += permiteLancamento ? string.Empty : "<div class='hitarea expandable-hitarea'></div>";
                litCabecalho.Text = cabecalho;

                lblHabilidade.Text = litConteudo.Text = (string.IsNullOrEmpty(ocr_codigo) ? string.Empty : ocr_codigo + " - ") + ocr_descricao;

                divHabilidade.Visible = permiteLancamento;
                litConteudo.Visible = !permiteLancamento;

                litRodape.Visible = permiteLancamento || (nivelLinha == Nivel);
                litRodape.Text = permiteLancamento ? "</li>" : string.Empty;
                litRodape.Visible = false;

                bool mostraLegenda = false;
                int toc_id = -1;

                HiddenField hdnChave = (HiddenField)e.Item.FindControl("hdnChave");

                #region Atualiza as chaves

                if (!string.IsNullOrEmpty(chave) && listHabilidades.Any(p => p.ocr_id == Convert.ToInt64(chave.Split(';')[1])))
                {
                    sOrientacaoAlcancada orientacaoAvaliacao = listHabilidades.First(p => p.ocr_id == Convert.ToInt64(chave.Split(';')[1]));
                    toc_id = orientacaoAvaliacao.id;

                    if (permiteLancamento && chkAlcancado != null)
                        chkAlcancado.Checked = orientacaoAvaliacao.alcancado;
                }

                chave = string.IsNullOrEmpty(chave) ? string.Empty : chave + ";" + toc_id.ToString();

                #endregion Atualiza as chaves

                if (permiteLancamento)
                {
                    if (chkAlcancado != null)
                        chkAlcancado.CssClass += " OrientacaoAlcancada ";

                    if (hdnChave != null)
                        hdnChave.Value = chave + ";" + toc_id.ToString();
                }

                Nivel = nivelLinha;

                #endregion Monta a arvore

                #region Busca níveis de aprendizado da orientação curricular

                string[] idsChave = chave.Split(';');
                long ocrId = idsChave.Length > 1 ? Convert.ToInt64(idsChave[1]) : -1;
                if (ocrId > 0 && dtOrientacaoNiveisAprendizado.Count() > 0)
                {
                    var nivelAprendizado = from dr in dtOrientacaoNiveisAprendizado
                                           where dr.ocr_id == ocrId
                                           select new
                                           {
                                               nap_id = dr.nap_id,
                                               nap_sigla = dr.nap_sigla.ToString(),
                                               nap_descricao = dr.nap_descricao.ToString()
                                           };

                    string niveisSiglas = string.Empty;
                    string niveisLegenda = string.Empty;

                    foreach (var item in nivelAprendizado)
                    {
                        niveisSiglas += item.nap_sigla + " / ";
                        niveisLegenda += item.nap_sigla + " - " + item.nap_descricao + "<br>";
                    }

                    if (!string.IsNullOrEmpty(niveisSiglas))
                    {
                        niveisSiglas = niveisSiglas.Substring(0, niveisSiglas.Length - 3);

                        Label lblLegPlanej = (Label)e.Item.FindControl("lblLegendaPlanejado");
                        Label lblLegTrab = (Label)e.Item.FindControl("lblLegendaTrabalhado");
                        Label lblLegAlcan = (Label)e.Item.FindControl("lblLegendaAlcancado");

                        if (lblLegPlanej != null && lblLegTrab != null && lblLegAlcan != null)
                        {
                            lblLegPlanej.Visible = lblLegTrab.Visible = lblLegAlcan.Visible = true;
                            lblLegPlanej.Text = lblLegTrab.Text = lblLegAlcan.Text = niveisSiglas;
                        }

                        // Repeater Diagnostico inicial
                        Label lblLegDiagIni = (Label)e.Item.FindControl("lblLegendaDiagInicialOrientacao");
                        if (lblLegDiagIni != null)
                            lblLegDiagIni.Text = niveisSiglas;
                    }
                }

                #endregion Busca níveis de aprendizado da orientação curricular
            }
        }

        #endregion Eventos

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion Page Life Cycle
    }
}
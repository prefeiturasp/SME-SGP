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

namespace GestaoEscolar.WebControls.TurmaDisciplina
{
    public partial class UCGridDisciplinaRegencia : MotherUserControl
    {
        #region Constantes

        private const int indiceColunaQtAulasSemanais = 1;
        private const int indiceColunaAvaliacoesPeriodicas = 2;

        #endregion Constantes

        #region Propriedades

        private DataTable dtDocentesEscola;
        private DataTable DtDisciplinaNaoAvaliado;
        private DataTable DtAvaliacoesFormato;
        private DataTable DtVigenciasDocentes;
        private List<TUR_Turma_Docentes_Disciplina> listTurmaDocentes;

        /// <summary>
        /// Propriedade que recebe o nome do grid.
        /// </summary>
        public string nomeTipoDisciplina
        {
            get
            {
                return lblTipoDisciplina.Text;
            }

            set
            {
                lblTipoDisciplina.Text = value;
            }
        }

        // Propriedades recebidas por parametros
        private int escola_esc_id;

        private int escola_uni_id;
        private bool escola_bloqueioAtribuicaoDocente;
        private bool professorEspecialista;
        private bool aplicarNovaRegraDocenciaCompartilhada = false;

        /// <summary>
        /// Retorna o texto para o label de mensagem sobre o campo Controle Semestral
        /// </summary>
        protected string TextoControleSemestral
        {
            get
            {
                return UtilBO.GetErroMessage(
                    "Em \"Controle semestral\", marque quando os(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " não serão avaliadas.",
                    UtilBO.TipoMensagem.Informacao);
            }
        }

        public bool PermiteEditar
        {
            set
            {
                if (!value)
                {
                    foreach (GridViewRow row in gvRegencia.Rows)
                    {
                        Repeater rptDocentes = (Repeater)row.FindControl("rptDocentes");
                        if (rptDocentes != null)
                        {
                            foreach (RepeaterItem item in rptDocentes.Items)
                            {
                                ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes = (ControleVigenciaDocentes.ControleVigenciaDocentes)item.FindControl("UCControleVigenciaDocentes");
                                if (UCControleVigenciaDocentes != null)
                                {
                                    UCControleVigenciaDocentes.PermiteEditar = value;
                                }
                            }
                        }
                        CheckBoxList chkAvaliacoesPeriodicas = (CheckBoxList)row.FindControl("chkAvaliacoesPeriodicas");
                        if (chkAvaliacoesPeriodicas != null)
                            chkAvaliacoesPeriodicas.Enabled = value;
                    }
                    HabilitaControles(gvCompRegencia.Controls, value);
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Carrega os grids de disciplinas da turma para a nova matriz curricular.
        /// </summary>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id">id do currículo</param>
        /// <param name="crp_id">id do currículo período</param>
        /// <param name="tipo">tipo de disciplina (ex: 1–Obrigatória,3–Optativa...)</param>
        /// <param name="tur_id">id da turma</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="ProfessorEspecialista">Professor especialista</param>
        /// <param name="dtDocentes">Tabela de docentes da escola</param>
        /// <param name="dtAvaliacoesFormato">Tabela com avaliações periódicas do formato - será mostrada na coluna controle semestral</param>
        /// <param name="dtDisciplinaNaoAvaliado">Tabela de disciplinas não avaliadas - todas as disciplinas da turma</param>
        /// <param name="bloqueioAtribuicaoDocente">Flag que indica se é pra bloquear a atribuição de docente para a escola</param>
        /// <param name="cur_idDestino">Id do curso da nova matriz curricular.</param>
        /// <param name="crr_idDestino">Id do currículo da nova matriz curricular.</param>
        public void CarregaGridDisciplinasNovaMatrizCurricular
        (
            int cur_id
            , int crr_id
            , int crp_id
            , ACA_CurriculoDisciplinaTipo tipo
            , long tur_id
            , int esc_id
            , int uni_id
            , bool ProfessorEspecialista
            , ref DataTable dtDocentes
            , DataTable dtAvaliacoesFormato
            , ref DataTable dtDisciplinaNaoAvaliado
            , bool bloqueioAtribuicaoDocente
            , int cur_idDestino
            , int crr_idDestino
            , ref DataTable dtVigenciasDocentes
            , bool aplicarNovaRegraDocenciaCompartilhada
        )
        {
            dtDocentesEscola = dtDocentes;
            DtAvaliacoesFormato = dtAvaliacoesFormato;
            DtDisciplinaNaoAvaliado = dtDisciplinaNaoAvaliado;
            DtVigenciasDocentes = dtVigenciasDocentes;

            if ((DtDisciplinaNaoAvaliado == null) && (tur_id > 0))
            {
                // Carregar avaliações que devem ser desconsideradas para a disciplina.
                DtDisciplinaNaoAvaliado = TUR_TurmaDisciplinaNaoAvaliadoBO.GetSelectBy_Turma(tur_id);
            }

            // Variáveis que carregam o combo de professor.
            escola_esc_id = esc_id;
            escola_uni_id = uni_id;
            escola_bloqueioAtribuicaoDocente = bloqueioAtribuicaoDocente;
            professorEspecialista = ProfessorEspecialista;

            gvRegencia.Columns[indiceColunaQtAulasSemanais].Visible =
                tipo != ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal;

            // Carregando as disciplinas de acordo como o tipo dado.
            DataTable dtDisciplina = ACA_CurriculoDisciplinaBO.SelecionaCursosPorNovaMatrizCurricularTipo(cur_id, crr_id, crp_id, tipo, tur_id, cur_idDestino, crr_idDestino);

            bool mostraAvaliacoes = (DtAvaliacoesFormato != null) && (DtAvaliacoesFormato.Rows.Count > 0) && TUR_TurmaBO.VerificaAcessoControleSemestral(tur_id);

            gvRegencia.Columns[indiceColunaAvaliacoesPeriodicas].Visible = mostraAvaliacoes;
            lblMensagemControleSemestral.Visible = mostraAvaliacoes && dtDisciplina.Rows.Count > 0;
            lblMensagemControleSemestral.Text = TextoControleSemestral;

            // Guarda todas as disciplinas da turma
            string tud_ids = string.Join(",", (from DataRow dr in dtDisciplina.Rows
                                               select dr["tud_id"].ToString()).ToArray());

            listTurmaDocentes = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(tud_ids);

            // Caso o não tenho o tipo de disciplina aparece seguinte mensagem.
            gvRegencia.EmptyDataText = (dtDisciplina == null)
                                            ? string.Format("É necessário selecionar o(a) {0} e o(a) {1}.",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower())
                                            : string.Format("Não foram encontrados(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " para o(a) {0} e o(a) {1} selecionado(a).",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower());

            this.aplicarNovaRegraDocenciaCompartilhada = aplicarNovaRegraDocenciaCompartilhada;
            gvRegencia.DataSource = dtDisciplina;
            gvRegencia.DataBind();

            // Volta o valor das tabelas (caso tenham sido carregados no DataBind do grid).
            dtDocentes = dtDocentesEscola;
            dtDisciplinaNaoAvaliado = DtDisciplinaNaoAvaliado;
        }

        /// <summary>
        /// Carrega os grids de disciplinas da turma
        /// </summary>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id">id do currículo</param>
        /// <param name="crp_id">id do currículo período</param>
        /// <param name="tipo">tipo de disciplina (ex: 1–Obrigatória,3–Optativa...)</param>
        /// <param name="tur_id">id da turma</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="ProfessorEspecialista">Professor especialista</param>
        /// <param name="dtDocentes">Tabela de docentes da escola</param>
        /// <param name="dtAvaliacoesFormato">Tabela com avaliações periódicas do formato - será mostrada na coluna controle semestral</param>
        /// <param name="dtDisciplinaNaoAvaliado">Tabela de disciplinas não avaliadas - todas as disciplinas da turma</param>
        /// <param name="bloqueioAtribuicaoDocente">Flag que indica se é pra bloquear a atribuição de docente para a escola</param>
        public void CarregaGridDisciplinas
        (
            int cur_id
            , int crr_id
            , int crp_id
            , ACA_CurriculoDisciplinaTipo tipo
            , long tur_id
            , int esc_id
            , int uni_id
            , bool ProfessorEspecialista
            , ref DataTable dtDocentes
            , DataTable dtAvaliacoesFormato
            , ref DataTable dtDisciplinaNaoAvaliado
            , bool bloqueioAtribuicaoDocente
            , ref DataTable dtVigenciasDocentes
            , bool aplicarNovaRegraDocenciaCompartilhada
        )
        {
            dtDocentesEscola = dtDocentes;
            DtAvaliacoesFormato = dtAvaliacoesFormato;
            DtDisciplinaNaoAvaliado = dtDisciplinaNaoAvaliado;
            DtVigenciasDocentes = dtVigenciasDocentes;

            if ((DtDisciplinaNaoAvaliado == null) && (tur_id > 0))
            {
                // Carregar avaliações que devem ser desconsideradas para a disciplina.
                DtDisciplinaNaoAvaliado = TUR_TurmaDisciplinaNaoAvaliadoBO.GetSelectBy_Turma(tur_id);
            }

            // Variáveis que carregam o combo de professor.
            escola_esc_id = esc_id;
            escola_uni_id = uni_id;
            escola_bloqueioAtribuicaoDocente = bloqueioAtribuicaoDocente;
            professorEspecialista = ProfessorEspecialista;

            gvRegencia.Columns[indiceColunaQtAulasSemanais].Visible =
                tipo != ACA_CurriculoDisciplinaTipo.DisciplinaPrincipal;

            // Carregando as disciplinas de acordo como o tipo dado.
            DataTable dtDisciplina = ACA_CurriculoDisciplinaBO.GetSelectBy_Curso_Tipo(cur_id, crr_id, crp_id, tipo, tur_id);

            DataTable dtCompRegencia = ACA_CurriculoDisciplinaBO.GetSelectBy_Curso_Tipo(cur_id, crr_id, crp_id, ACA_CurriculoDisciplinaTipo.ComponenteRegencia, tur_id);

            bool mostraAvaliacoes = (DtAvaliacoesFormato != null) && (DtAvaliacoesFormato.Rows.Count > 0) && TUR_TurmaBO.VerificaAcessoControleSemestral(tur_id);

            gvRegencia.Columns[indiceColunaAvaliacoesPeriodicas].Visible = mostraAvaliacoes;
            lblMensagemControleSemestral.Visible = mostraAvaliacoes && dtDisciplina.Rows.Count > 0;
            lblMensagemControleSemestral.Text = TextoControleSemestral;

            // Guarda todas as disciplinas da turma
            string tud_ids = string.Join(",", (from DataRow dr in dtDisciplina.Rows
                                               select dr["tud_id"].ToString()).ToArray());

            listTurmaDocentes = TUR_TurmaDocenteBO.SelecionaDocentesDisciplina(tud_ids);

            // Caso o não tenho o tipo de disciplina aparece seguinte mensagem.
            gvRegencia.EmptyDataText = (dtDisciplina == null)
                                            ? string.Format("É necessário selecionar o(a) {0} e o(a) {1}.",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower())
                                            : string.Format("Não foram encontrados(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " para o(a) {0} e o(a) {1} selecionado(a).",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower());

            this.aplicarNovaRegraDocenciaCompartilhada = aplicarNovaRegraDocenciaCompartilhada;
            gvRegencia.DataSource = dtDisciplina;
            gvRegencia.DataBind();

            gvCompRegencia.EmptyDataText = (dtCompRegencia == null)
                                            ? string.Format("É necessário selecionar o(a) {0} e o(a) {1}.",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower())
                                            : string.Format("Não foram encontrados(as) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL_MIN") + " para o(a) {0} e o(a) {1} selecionado(a).",
                                            GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower(),
                                            GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower());

            gvCompRegencia.DataSource = dtCompRegencia;
            gvCompRegencia.DataBind();

            // Volta o valor das tabelas (caso tenham sido carregados no DataBind do grid).
            dtDocentes = dtDocentesEscola;
            dtDisciplinaNaoAvaliado = DtDisciplinaNaoAvaliado;
        }

        /// <summary>
        /// Carrega o repeater com a quantidade de docentes definida no parâmetro acadêmico,
        /// com o controle de vigência de cada um deles.
        /// </summary>
        /// <param name="Row">Linha do grid de disciplinas</param>
        /// <param name="tud_id">Id da disciplina</param>
        /// <param name="tds_id">Id do tipo de disciplina para carregar o docente por especialidade</param>
        private void CarregarControleDocentes(GridViewRow Row, long tud_id, int tds_id)
        {
            int qtdeDocentes = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_DOCENTES_VIGENTES_DISCIPLINA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            DataTable dtDocentes = new DataTable();
            dtDocentes.Columns.Add("posicao");
            dtDocentes.Columns.Add("tud_id");
            dtDocentes.Columns.Add("qtdedocentes");
            dtDocentes.Columns.Add("tds_id");

            for (int i = 1; i <= qtdeDocentes; i++)
            {
                EnumTipoDocente tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao((byte)i, ApplicationWEB.AppMinutosCacheLongo);
                if (!aplicarNovaRegraDocenciaCompartilhada
                    || (tipoDocente != EnumTipoDocente.Compartilhado && tipoDocente != EnumTipoDocente.Projeto))
                {
                    DataRow dr = dtDocentes.NewRow();
                    dr["posicao"] = i;
                    dr["tud_id"] = tud_id;
                    dr["qtdedocentes"] = qtdeDocentes;
                    dr["tds_id"] = tds_id;
                    dtDocentes.Rows.Add(dr);
                }
            }

            Repeater rptDocentes = (Repeater)Row.FindControl("rptDocentes");
            if (rptDocentes != null)
            {
                rptDocentes.DataSource = dtDocentes;
                rptDocentes.DataBind();
            }
        }

        /// <summary>
        /// Retorna os lists necessários para salvar a turma.
        /// </summary>
        /// <param name="tud_codigo">código da turma disciplina</param>
        /// <param name="tud_vagas">quantidade de vagas turma disciplina</param>
        /// <param name="tud_minimoMatriculados">quantidade mínima de vagas turma disciplina</param>
        /// <param name="tud_duracao">Disciplina duração</param>
        /// <returns>As listas de entidades com as discplinas a serem salvas</returns>
        public List<CadastroTurmaDisciplina> RetornaDisciplinas(string tud_codigo
                                                                , int tud_vagas
                                                                , int tud_minimoMatriculados
                                                                , byte tud_duracao)
        {
            List<CadastroTurmaDisciplina> listTurmaDisciplina = new List<CadastroTurmaDisciplina>();
            CadastroTurmaDisciplina entRegencia = new CadastroTurmaDisciplina();

            // Disciplinas obrigatórias.
            foreach (GridViewRow row in gvRegencia.Rows)
            {
                CadastroTurmaDisciplina ent = AdicionaDisciplina(row, tud_codigo, tud_vagas, tud_minimoMatriculados, tud_duracao);

                entRegencia = ent;

                listTurmaDisciplina.Add(ent);
            }

            int totalCargaHorariaSemanal = 0;

            foreach (GridViewRow row in gvCompRegencia.Rows)
            {
                CadastroTurmaDisciplina ent = AdicionaCompRegencia(row, tud_codigo, tud_vagas, tud_minimoMatriculados, tud_duracao);

                ent.listaTurmaDocente = entRegencia.listaTurmaDocente;
                ent.entTurmaCalendario = entRegencia.entTurmaCalendario;
                ent.listaAvaliacoesNaoAvaliar = entRegencia.listaAvaliacoesNaoAvaliar;
                totalCargaHorariaSemanal += ent.entTurmaDisciplina.tud_cargaHorariaSemanal;

                listTurmaDisciplina.Add(ent);
            }

            if (listTurmaDisciplina.Any())
                listTurmaDisciplina[listTurmaDisciplina.IndexOf(entRegencia)].entTurmaDisciplina.tud_cargaHorariaSemanal = totalCargaHorariaSemanal;

            return listTurmaDisciplina;
        }

        /// <summary>
        /// Adiciona nas listas as entidades da disciplina da linha atual.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tud_codigo">código da turma disciplina</param>
        /// <param name="tud_vagas">quantidade de vagas turma disciplina</param>
        /// <param name="tud_minimoMatriculados">quantidade mínima de vagas turma disciplina</param>
        /// <param name="tud_duracao">Disciplina duração</param>
        /// <returns>As listas de entidades com as discplinas a serem salvas</returns>
        private CadastroTurmaDisciplina AdicionaDisciplina
        (
            GridViewRow item
            , string tud_codigo
            , int tud_vagas
            , int tud_minimoMatriculados
            , byte tud_duracao
        )
        {
            // Adicionando na entidades os valores a ser salvo.
            TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina
            {
                tud_tipo = String.IsNullOrEmpty(gvRegencia.DataKeys[item.RowIndex].Values["crd_tipo"].ToString()) ? new byte() : Convert.ToByte(gvRegencia.DataKeys[item.RowIndex].Values["crd_tipo"].ToString()),
                tud_global = false,
                tud_nome = gvRegencia.DataKeys[item.RowIndex].Values["dis_nome"].ToString(),
                tud_codigo = tud_codigo,
                tud_id = Convert.ToInt32(gvRegencia.DataKeys[item.RowIndex].Values["tud_id"]),
                tud_situacao = (byte)TurmaDisciplinaSituacao.Ativo,
                tud_multiseriado = false,
                tud_vagas = tud_vagas,
                tud_minimoMatriculados = tud_minimoMatriculados,
                tud_duracao = tud_duracao,
                tud_modo = (byte)TurmaDisciplinaModo.Normal,
                tud_aulaForaPeriodoNormal = false,
                tud_semProfessor = ((CheckBox)item.FindControl("chkSemDocente")).Checked,
                IsNew = Convert.ToInt32(gvRegencia.DataKeys[item.RowIndex].Values["tud_id"]) <= 0
            };

            // Adicionando valores na entidade de relacionemento.
            TUR_TurmaDisciplinaRelDisciplina relDis = new TUR_TurmaDisciplinaRelDisciplina
            {
                dis_id = Convert.ToInt32(gvRegencia.DataKeys[item.RowIndex].Values["dis_id"]),
                tud_id = ent.tud_id,
                IsNew = Convert.ToInt32(gvRegencia.DataKeys[item.RowIndex].Values["tud_id"]) <= 0
            };

            List<TUR_TurmaDisciplinaCalendario> turCal = new List<TUR_TurmaDisciplinaCalendario>();

            // Avaliações que não serão avaliadas.
            CheckBoxList chkList = (CheckBoxList)item.FindControl("chkAvaliacoesPeriodicas");
            List<TUR_TurmaDisciplinaNaoAvaliado> lista =
                (from ListItem it in chkList.Items
                 where it.Selected
                 select new TUR_TurmaDisciplinaNaoAvaliado
                 {
                     tud_id = ent.tud_id,
                     fav_id = Convert.ToInt32(it.Value.Split(';')[0]),
                     ava_id = Convert.ToInt32(it.Value.Split(';')[1])
                 }
                ).ToList();

            Repeater rptDocentes = (Repeater)item.FindControl("rptDocentes");
            List<TUR_Turma_Docentes_Disciplina> listDocentesPosicoes = new List<TUR_Turma_Docentes_Disciplina>();

            foreach (RepeaterItem itemD in rptDocentes.Items)
            {
                GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes = (GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes)itemD.FindControl("UCControleVigenciaDocentes");
                byte posicao = Convert.ToByte(((Label)itemD.FindControl("lblposicao")).Text);

                UCControleVigenciaDocentes.RetornaDocentesPosicao(ref listDocentesPosicoes, posicao, ent.tud_id);
            }

            return new CadastroTurmaDisciplina
            {
                entTurmaDisciplina = ent,
                entTurmaDiscRelDisciplina = relDis,
                listaTurmaDocente = listDocentesPosicoes,
                entTurmaCalendario = turCal,
                listaAvaliacoesNaoAvaliar = lista
            };
        }

        /// <summary>
        /// Adiciona nas listas as entidades da disciplina da linha atual.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tud_codigo">código da turma disciplina</param>
        /// <param name="tud_vagas">quantidade de vagas turma disciplina</param>
        /// <param name="tud_minimoMatriculados">quantidade mínima de vagas turma disciplina</param>
        /// <param name="tud_duracao">Disciplina duração</param>
        /// <returns>As listas de entidades com as discplinas a serem salvas</returns>
        private CadastroTurmaDisciplina AdicionaCompRegencia
        (
            GridViewRow item
            , string tud_codigo
            , int tud_vagas
            , int tud_minimoMatriculados
            , byte tud_duracao
        )
        {
            // Adicionando na entidades os valores a ser salvo.
            TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina
            {
                tud_tipo = String.IsNullOrEmpty(gvCompRegencia.DataKeys[item.RowIndex].Values["crd_tipo"].ToString()) ? new byte() : Convert.ToByte(gvCompRegencia.DataKeys[item.RowIndex].Values["crd_tipo"].ToString()),
                tud_global = false,
                tud_nome = gvCompRegencia.DataKeys[item.RowIndex].Values["dis_nome"].ToString(),
                tud_codigo = tud_codigo,
                tud_id = Convert.ToInt32(gvCompRegencia.DataKeys[item.RowIndex].Values["tud_id"]),
                tud_situacao = (byte)TurmaDisciplinaSituacao.Ativo,
                tud_cargaHorariaSemanal = String.IsNullOrEmpty(((TextBox)item.FindControl("txtAulaSemanal")).Text) ? 0 : Convert.ToInt32(((TextBox)item.FindControl("txtAulaSemanal")).Text),
                tud_multiseriado = false,
                tud_vagas = tud_vagas,
                tud_minimoMatriculados = tud_minimoMatriculados,
                tud_duracao = tud_duracao,
                tud_modo = (byte)TurmaDisciplinaModo.Normal,
                tud_aulaForaPeriodoNormal = false,
                IsNew = Convert.ToInt32(gvCompRegencia.DataKeys[item.RowIndex].Values["tud_id"]) <= 0
            };

            // Adicionando valores na entidade de relacionemento.
            TUR_TurmaDisciplinaRelDisciplina relDis = new TUR_TurmaDisciplinaRelDisciplina
            {
                dis_id = Convert.ToInt32(gvCompRegencia.DataKeys[item.RowIndex].Values["dis_id"]),
                tud_id = ent.tud_id,
                IsNew = Convert.ToInt32(gvCompRegencia.DataKeys[item.RowIndex].Values["tud_id"]) <= 0
            };

            List<TUR_TurmaDisciplinaCalendario> turCal = new List<TUR_TurmaDisciplinaCalendario>();

            // Avaliações que não serão avaliadas.
            List<TUR_TurmaDisciplinaNaoAvaliado> lista = new List<TUR_TurmaDisciplinaNaoAvaliado>();

            List<TUR_Turma_Docentes_Disciplina> listDocentesPosicoes = new List<TUR_Turma_Docentes_Disciplina>();

            return new CadastroTurmaDisciplina
            {
                entTurmaDisciplina = ent,
                entTurmaDiscRelDisciplina = relDis,
                listaTurmaDocente = listDocentesPosicoes,
                entTurmaCalendario = turCal,
                listaAvaliacoesNaoAvaliar = lista
            };
        }

        #endregion Métodos

        #region Eventos

        protected void rptDocentes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            long tud_id = Convert.ToInt64(((Label)e.Item.FindControl("lbltud_id")).Text);
            byte posicao = Convert.ToByte(((Label)e.Item.FindControl("lblposicao")).Text);
            int qtdedocentes = Convert.ToInt32(((Label)e.Item.FindControl("lblqtdedocentes")).Text);
            int tds_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tds_id"));

            GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes UCControleVigenciaDocentes = (GestaoEscolar.WebControls.ControleVigenciaDocentes.ControleVigenciaDocentes)e.Item.FindControl("UCControleVigenciaDocentes");

            TUR_Turma_Docentes_Disciplina entity = listTurmaDocentes.Find(p => (p.entDocente.tud_id == tud_id && p.entDocente.tdt_posicao == posicao && p.entDocente.tdt_situacao == 1));

            UCControleVigenciaDocentes.CarregarDocente
                (entity.doc_nome, posicao, qtdedocentes, tud_id, ref dtDocentesEscola, tds_id
                , escola_esc_id, escola_uni_id, professorEspecialista, escola_bloqueioAtribuicaoDocente, ref DtVigenciasDocentes);
        }

        protected void gvRegencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tds_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "tds_id"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));

                // Carrega o user control de controle de docentes.
                CarregarControleDocentes(e.Row, tud_id, tds_id);

                CheckBoxList list = (CheckBoxList)e.Row.FindControl("chkAvaliacoesPeriodicas");
                if (list != null)
                {
                    list.DataSource = DtAvaliacoesFormato;
                    list.DataBind();
                }

                if ((tud_id > 0) && (DtDisciplinaNaoAvaliado != null))
                {
                    // Selecionar os itens que já estão marcados para não avaliar.
                    foreach (ListItem item in list.Items)
                    {
                        item.Selected =
                            (from DataRow dr in DtDisciplinaNaoAvaliado.Rows
                             where dr["fav_id"] + ";" + dr["ava_id"] == item.Value
                                   && Convert.ToInt64(dr["tud_id"]) == tud_id
                             select new { value = item.Value }
                                ).Any();
                    }
                }
            }
        }

        protected void gvCompRegencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tds_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "tds_id"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));

                // Carrega o user control de controle de docentes.
                CarregarControleDocentes(e.Row, tud_id, tds_id);
            }
        }

        #endregion Eventos
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;

namespace GestaoEscolar.WebControls.ControleTurma
{
    public partial class UCControleTurma : MotherUserControl
    {
        #region Delegates

        public delegate void SelectedIndexChanged();
        public SelectedIndexChanged IndexChanged;

        public delegate void DisciplinaCompartilhadaSelectedIndexChanged();
        public DisciplinaCompartilhadaSelectedIndexChanged DisciplinaCompartilhadaIndexChanged;

        public delegate void chkTurmasNormaisMultisseriadasSelectedIndexChanged();
        public chkTurmasNormaisMultisseriadasSelectedIndexChanged chkTurmasNormaisMultisseriadasIndexChanged;

        #endregion

        #region Propriedades

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        /// <summary>
        /// Armazena o ID da escola em viewstate.
        /// </summary>
        public int VS_esc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da unidade em viewstate.
        /// </summary>
        public int VS_uni_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_uni_id"] ?? -1);
            }

            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma em viewstate.
        /// </summary>
        public long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma normal (multisseriada) em viewstate.
        /// </summary>
        public long VS_tur_idNormal
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_idNormal"] ?? -1);
            }

            set
            {
                ViewState["VS_tur_idNormal"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma disciplina em viewstate.
        /// </summary>
        public long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da turma disciplina do aluno em viewstate.
        /// </summary>
        public long VS_tud_idAluno
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_idAluno"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_idAluno"] = value;
            }
        }

        /// <summary>
        /// Armazena se a disciplina pode lançar nota em viewstate.
        /// </summary>
        public bool VS_tud_naoLancarNota
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_tud_naoLancarNota"] ?? false);
            }

            set
            {
                ViewState["VS_tud_naoLancarNota"] = value;
            }
        }

        /// <summary>
        /// Armazena se a disciplina pode lançar frequencia em viewstate.
        /// </summary>
        public bool VS_tud_naoLancarFrequencia
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_tud_naoLancarFrequencia"] ?? false);
            }

            set
            {
                ViewState["VS_tud_naoLancarFrequencia"] = value;
            }
        }

        /// <summary>
        /// Armazena a posição do docente em viewstate.
        /// </summary>
        public byte VS_tdt_posicao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tdt_posicao"] ?? 0);
            }

            set
            {
                ViewState["VS_tdt_posicao"] = value;
            }
        }

        /// <summary>
        /// Armazena a data de encerramento da turma em viewstate.
        /// </summary>
        public DateTime VS_tur_dataEncerramento
        {
            get
            {
                if (ViewState["VS_tur_dataEncerramento"] == null)
                    return new DateTime();
                else
                    return Convert.ToDateTime(ViewState["VS_tur_dataEncerramento"]);
            }
            set
            {
                ViewState["VS_tur_dataEncerramento"] = value;
            }
        }

        public byte VS_tur_situacao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tur_situacao"] ?? 0);
            }

            set
            {
                ViewState["VS_tur_situacao"] = value;
            }
        }

        /// <summary>
        /// Retorna o texto da label da turma selecionada.
        /// </summary>
        public string LabelTurmas
        {
            get
            {
                return lblTurma.Text;
            }

            set
            {
                lblTurma.Text = value;
            }
        }


        /// <summary>
        /// Retorna o valor do combo de turma.
        /// </summary>
        public string ValorTurmas
        {
            get
            {
                return uccTurmaDisciplina.Valor;
            }
        }

        /// <summary>
        /// Retorna o texto selecionado do combo de turma.
        /// </summary>
        public string TextoSelecionadoTurmas
        {
            get
            {
                return uccTurmaDisciplina.TextoSelecionado;
            }

        }

        /// <summary>
        /// Armazena os IDs do tipo de ciclo em viewstate.
        /// </summary>
        public string VS_tciIds
        {
            get
            {
                return (ViewState["VS_tciIds"] ?? "").ToString();
            }

            set
            {
                ViewState["VS_tciIds"] = value;
            }
        }

        /// <summary>
        /// Tipo de turma.
        /// </summary>
        public byte VS_tur_tipo
        {
            get
            {
                if (ViewState["VS_tur_tipo"] == null)
                {
                    TUR_Turma entityTurma = new TUR_Turma { tur_id = VS_tur_id };
                    TUR_TurmaBO.GetEntity(entityTurma);
                    ViewState["VS_tur_tipo"] = entityTurma.tur_tipo;
                    return Convert.ToByte(ViewState["VS_tur_tipo"]);
                }
                else
                {
                    return Convert.ToByte(ViewState["VS_tur_tipo"]);
                }
            }

            set
            {
                ViewState["VS_tur_tipo"] = value;
            }
        }

        public System.Drawing.Color CorTituloTurma
        {
            set
            {
                lblTurma.ForeColor = lbkAlterarTurma.ForeColor = lblDisciplinaCompartilhada.ForeColor =
                    lbkAlterarDisciplinaCompartilhada.ForeColor = lblTituloDisciplinaCompartilhada.ForeColor = value;
            }
        }

        /// <summary>
        /// Retorna o valor do combo de disciplina compartilhada.
        /// </summary>
        public string ValorDisciplinaCompartilhada
        {
            get
            {
                return uccDisciplinaCompartilhada.Valor;
            }
        }

        /// <summary>
        /// Retorna as turmas normais selecionadas que possuem alunos matriculados na turma multisseriada.
        /// </summary>
        public List<sTurmaDisciplina> TurmasNormaisMultisseriadas
        {
            get
            {
                return (from ListItem item in chkTurmasNormaisMultisseriadas.Items
                        where item.Selected
                        let arrayValue = item.Value.Split(';')
                        select new sTurmaDisciplina
                        {
                            tur_id = Convert.ToInt64(arrayValue[0])
                            ,
                            tud_id = Convert.ToInt64(arrayValue[1])
                        }).ToList();
            }
        }

        /// <summary>
        /// Armazena os IDs das turmas e das turmas disciplinas
        /// </summary>
        public List<string> VS_tur_tud_ids
        {
            get
            {
                return (List<String>)(ViewState["VS_tur_tud_ids"] ?? new List<string>());
            }

            set
            {
                ViewState["VS_tur_tud_ids"] = value;
            }
        }

        /// <summary>
        /// Guarda atribuições do docente que devem verificar a vigência para criação de aulas.
        /// </summary>
        public List<VigenciaCriacaoAulas> AtribuicoesVerificarVigencia
        {
            get
            {
                if (ViewState["AtribuicoesVerificarVigencia"] == null)
                    return new List<VigenciaCriacaoAulas>();

                return (List<VigenciaCriacaoAulas>)ViewState["AtribuicoesVerificarVigencia"];
            }
            set
            {
                ViewState["AtribuicoesVerificarVigencia"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega no combo as disciplinas disponíveis para o docente de acordo com o objeto
        /// passado.
        /// </summary>
        /// <param name="dadosTurmas">Objeto com as turmas do docente</param>
        /// <param name="cal_id">ID do calendário</param>
        public void CarregaTurmas(List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmas, int cal_id, byte tud_tipo, bool fav_fechamentoAutomatico, bool removerCompartilhada = false)
        {
            DateTime dataLimiteLancamento = new DateTime();
            string dataBloqueio = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.DATA_VALIDADE_BLOQUEIO_ACESSO_MINHAS_TURMAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (visaoDocente
                && !string.IsNullOrEmpty(dataBloqueio) && DateTime.TryParse(dataBloqueio, out dataLimiteLancamento)
                && DateTime.Today >= dataLimiteLancamento)
            {
                // Se passou a data limite para lançamento das aulas previstas, só carrega disciplinas com
                // as aulas previstas lançadas.
                if (removerCompartilhada)
                {
                    dadosTurmas = dadosTurmas.Where(p =>
                       (
                        (p.aulasPrevistasPreenchida == true)
                        ||
                           (
                               p.tdt_posicao != (byte)EnumTipoDocente.Titular
                               && p.tdt_posicao != (byte)EnumTipoDocente.SegundoTitular
                               && p.tdt_posicao != (byte)EnumTipoDocente.Especial
                           )
                        )
                        &&
                        (p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                    ).ToList();
                }
                else
                {
                    dadosTurmas = dadosTurmas.Where(p =>
                        (p.aulasPrevistasPreenchida == true || p.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada)
                        ||
                        (
                            p.tdt_posicao != (byte)EnumTipoDocente.Titular
                            && p.tdt_posicao != (byte)EnumTipoDocente.SegundoTitular
                            && p.tdt_posicao != (byte)EnumTipoDocente.Especial
                        )).ToList();
                }
            }

            //Remove as disciplinas do tipo territorio do saber
            dadosTurmas = dadosTurmas.Where(p => p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.TerritorioSaber).ToList();

            uccTurmaDisciplina.CarregarCombo(dadosTurmas, "TurmaDisciplinaEscola", "DataValueFieldCombo");
            uccTurmaDisciplina.Valor = VS_tur_id + ";" + VS_tud_id + ";" + cal_id + ";" + VS_tdt_posicao + ";" + tud_tipo + ";" + VS_tur_tipo + ";" + VS_tur_idNormal + ";" + VS_tud_idAluno + ";" + (fav_fechamentoAutomatico ? "true" : "false");

            // Seta as atribuições que devem ter a vigência verificada para a criação de aulas.
            AtribuicoesVerificarVigencia =
                (from Struct_MinhasTurmas.Struct_Turmas item in dadosTurmas
                 where item.crg_tipo == (byte)eTipoCargo.AtribuicaoEsporadica
                 select new VigenciaCriacaoAulas
                 {
                     tud_id = item.tud_id
                    ,
                     tdt_id = item.tdt_id
                    ,
                     crg_tipo = item.crg_tipo
                    ,
                     tdt_vigenciaInicio = item.tdt_vigenciaInicio
                    ,
                     tdt_vigenciaFim = item.tdt_vigenciaFim
                 }).ToList();


            if (VS_tur_tipo == (byte)TUR_TurmaTipo.MultisseriadaDocente)
            {
                chkTurmasNormaisMultisseriadas.DataSource = TUR_TurmaBO.SelecionaTurmasNormaisMatriculaMutisseriada(VS_tur_id);
                chkTurmasNormaisMultisseriadas.DataBind();
                if (!VS_tur_tud_ids.Any())
                {
                    chkTurmasNormaisMultisseriadas.Items.Cast<ListItem>()
                                                        .ToList()
                                                        .ForEach
                                                        (
                                                            item =>
                                                            {
                                                                item.Selected = true;
                                                            }
                                                        );
                }
                else
                {
                    (from ListItem item in chkTurmasNormaisMultisseriadas.Items.Cast<ListItem>()
                     join string tur_tud_id in VS_tur_tud_ids on item.Value equals tur_tud_id
                     select item).ToList().ForEach
                                           (
                                               item =>
                                                   {
                                                       item.Selected = true;
                                                   }
                                           );
                }
                pnlTurmasMultisseriada.Visible = chkTurmasNormaisMultisseriadas.Items.Count > 0;
            }
            else
            {
                chkTurmasNormaisMultisseriadas.Items.Add(new ListItem("", String.Format("{0};{1}", VS_tur_idNormal, VS_tud_idAluno)));
                chkTurmasNormaisMultisseriadas.Items.Cast<ListItem>()
                                                    .ToList()
                                                    .ForEach
                                                    (
                                                        item =>
                                                        {
                                                            item.Selected = true;
                                                        }
                                                    );
                pnlTurmasMultisseriada.Visible = false;
            }
        }

        public void CarregarDisciplinaCompartilhada(List<sTurmaDisciplinaRelacionada> dadosDisciplinas, long tud_id, long tdr_id)
        {
            pnlDisciplinaCompartilhada.Visible = true;
            uccDisciplinaCompartilhada.CarregarCombo(dadosDisciplinas, "tud_nome", "DataValueFieldCombo");
            uccDisciplinaCompartilhada.Valor = string.Format("{0};{1}", tud_id, tdr_id);
            lblDisciplinaCompartilhada.Text = uccDisciplinaCompartilhada.TextoSelecionado;
        }

        #endregion

        #region Page Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            uccTurmaDisciplina.IndexChanged = uccTurmaDisciplina_IndexChanged;
            uccDisciplinaCompartilhada.IndexChanged = uccDisciplinaCompartilhada_IndexChanged;
        }

        #endregion

        #region Eventos

        protected void uccTurmaDisciplina_IndexChanged()
        {
            if (IndexChanged != null)
                IndexChanged();
        }

        protected void uccDisciplinaCompartilhada_IndexChanged()
        {
            lblDisciplinaCompartilhada.Text = uccDisciplinaCompartilhada.TextoSelecionado;
            if (DisciplinaCompartilhadaIndexChanged != null)
                DisciplinaCompartilhadaIndexChanged();
        }

        protected void chkTurmasNormaisMultisseriadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkTurmasNormaisMultisseriadasIndexChanged != null)
                chkTurmasNormaisMultisseriadasIndexChanged();
        }

        //protected void lbkAlterarTurma_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(hdnTurmaSelecionada.Value))
        //        uccTurmaDisciplina.Valor = hdnTurmaSelecionada.Value;

        //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlterarTurma", "$('#spTrocarTurma').css('display','inline-block');$('#spTrocarTurma').find('select').focus();$('#spTituloTurma').css('display','none');", true);
        //}

        #endregion
    }
}
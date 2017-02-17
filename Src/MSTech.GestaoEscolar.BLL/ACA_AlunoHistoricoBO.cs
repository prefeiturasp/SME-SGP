using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.CoreSSO.DAL;
using System.Text;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    /// <summary>
    /// Classe de excessão referente à entidade ACA_AlunoHistorico.
    /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
    /// na entidade do ACA_AlunoHistorico.
    /// </summary>
    public class ACA_AlunoHistorico_ValidationException : ValidationException
    {
        public ACA_AlunoHistorico_ValidationException(string message) : base(message) { }
    }

    #endregion Excessões

    #region Enumerador

    public enum TipoControleNotas
    {
        Global = 1
        ,

        PorDisciplina = 2
        ,

        GlobalPorDisciplina = 3
    }

    public enum ResultadoHistorico
    {
        Aprovado = 1
        ,
        Reprovado = 2
        ,
        ReprovadoFrequencia = 3
    }

    #endregion Enumerador

    #region HistoricoPedagogico

    public enum textalign
    {
        [StringValue("text-align:left;")]
        left = 1,
        [StringValue("text-align:center;")]
        center = 2,
        [StringValue("text-align:right;")]
        right = 3
    }

    public enum verticalalign
    {
        [StringValue("vertical-align:middle;")]
        middle = 1
    }

    public enum backgroundcolor
    {
        [StringValue("background-color:#D3D3D3 !important;")]
        header = 1,
        [StringValue("background-color:transparent !important;")]
        cell = 2
    }

    public enum color
    {
        [StringValue("color:#000000 !important;")]
        padrao = 1
    }

    public enum border
    {
        [StringValue("border-left:#000000 1px solid !important;")]
        left = 1,
        [StringValue("border-top:#000000 1px solid !important;")]
        top = 2,
        [StringValue("border-right:#000000 1px solid !important;")]
        right = 3,
        [StringValue("border-bottom:#000000 1px solid !important;")]
        bottom = 4
    }

    public enum font
    {
        [StringValue("font-family:Arial !important; font-size:11px !important;")]
        arial11 = 1,
        [StringValue("font-family:Arial !important; font-size:10px !important;")]
        arial10 = 2,
        [StringValue("font-family:Arial !important; font-size:9px !important;")]
        arial9 = 3,
        [StringValue("font-family:Arial !important; font-size:8px !important;")]
        arial8 = 4,
        [StringValue("font-family:Arial !important; font-size:7px !important;")]
        arial7 = 5,
        [StringValue("font-family:Arial !important; font-size:5px !important;")]
        arial5 = 6
    }

    public enum fontweight
    {
        [StringValue("font-weight:normal !important;")]
        normal = 1,
        [StringValue("font-weight:bold !important;")]
        bold = 2
    }

    public enum padding
    {
        [StringValue("padding-left:2px !important;")]
        left = 1,
        [StringValue("padding-right:2px !important;")]
        right = 2
    }

    public static class HistoricoPedagogico
    {
        public const string style = " style=\"";
        public const string styleTable = " cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-width:0px;empty-cells:show;background-color:#FFFFFF;\"";

        public static string valorEnum(Enum enu)
        {
            return StringValueAttribute.GetStringValue(enu) + " ";
        }

        public static string tdCell(string text, string styleAux, int rowspan, int colspan, int width, int height)
        {
            return "<td" + style + valorEnum(color.padrao) + valorEnum(backgroundcolor.cell) +
                                   valorEnum(font.arial8) + valorEnum(fontweight.normal) +
                                   valorEnum(verticalalign.middle) + valorEnum(padding.left) +
                                   valorEnum(padding.right) + styleAux +
                                   (width == 0 ? "" : " width:" + width + "px;") +
                                   (height == 0 ? "" : " height:" + height + "px;") +
                                   "\" " +
                           "rowspan=\"" + rowspan.ToString() + "\"" +
                           "colspan=\"" + colspan.ToString() + "\"" +
                           ">" +
                   text +
                   "</td>";
        }

        public static string tdCellSemFonte(string text, string styleAux, int rowspan, int colspan, int width, int height)
        {
            return "<td" + style + valorEnum(color.padrao) + valorEnum(backgroundcolor.cell) +
                                   valorEnum(fontweight.normal) + valorEnum(verticalalign.middle) +
                                   valorEnum(padding.left) + valorEnum(padding.right) + styleAux +
                                   (width == 0 ? "" : " width:" + width + "px;") +
                                   (height == 0 ? "" : " height:" + height + "px;") +
                                   "\" " +
                           "rowspan=\"" + rowspan.ToString() + "\"" +
                           "colspan=\"" + colspan.ToString() + "\"" +
                           ">" +
                   text +
                   "</td>";
        }
    }

    #endregion

    public class ACA_AlunoHistoricoBO : BusinessBase<ACA_AlunoHistoricoDAO, ACA_AlunoHistorico>
    {
        #region Constantes

        public const string style_table = " cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width=713px;border-width:0px;empty-cells:show;background-color:#FFFFFF;\"";
        public const string style_textVertical = "-transform:rotate(270deg); -webkit-transform:rotate(270deg); -moz-transform:rotate(270deg); -o-transform:rotate(270deg); -ms-transform:rotate(270deg); writing-mode: tb-rl\\9; filter: flipv fliph;";

        #endregion

        #region Estruturas

        /// <summary>
        ///  Estrutura para Histórico do aluno.
        /// </summary>
        [Serializable]
        public struct StructHistorico
        {
            public ACA_AlunoHistorico entHistorico { get; set; }

            public ACA_AlunoEscolaOrigem entEscolaOrigem { get; set; }

            // public END_Endereco entEscolaOrigem { get; set; }
            public string alh_resultadoDescricao { get; set; }

            public string alh_resultadoDescricaoText { get; set; }

            public string eco_nome { get; set; }

            public string cur_crp_nome { get; set; }

            public string crp_descricao { get; set; }

            public int tcp_id { get; set; }

            public Guid uad_id { get; set; }

            public List<StructHistoricoDisciplina> ltDisciplina { get; set; }

            public DataState State { get; set; }
        }

        /// <summary>
        ///  Estrutura para disciplina do Histórico do aluno.
        /// </summary>
        [Serializable]
        public struct StructHistoricoDisciplina
        {
            public ACA_AlunoHistoricoDisciplina entDisciplina { get; set; }

            public string tds_id { get; set; }

            public string tds_nome { get; set; }

            public string ahd_resultadoDescricao { get; set; }

            public DataState State { get; set; }

            public int indice { get; set; }
            public int ahd_id { get; set; }

        }


        /// <summary>
        /// Estrutura para observação do Histórico do aluno.
        /// </summary>
        [Serializable]
        public struct StructHistoricoObservacao
        {
            public ACA_AlunoHistoricoObservacao entObservacao { get; set; }

            public DataState State { get; set; }
        }

        /// <summary>
        ///  Estrutura para projeto/atividade complementar
        /// </summary>
        [Serializable]
        public struct StructProjAtivComplementar
        {
            public int ahd_id { get; set; }

            public int ahp_id { get; set; }

            public string ahp_nome { get; set; }

            public string frequencia { get; set; }
        }

        /// <summary>
        ///  Estrutura para enriquecimento curricular
        /// </summary>
        [Serializable]
        public struct StructEnrCurricular
        {
            public int ahd_id { get; set; }

            public int tds_id { get; set; }

            public string tds_nome { get; set; }

            public string frequencia { get; set; }
        }

        /// <summary>
        ///  Estrutura para componente curricular
        /// </summary>
        [Serializable]
        public struct StructCompCurricular
        {
            public int alh_id { get; set; }
            public int ahd_id { get; set; }

            public int tds_id { get; set; }

            public string tds_nome { get; set; }

            public string nota { get; set; }

            public string frequencia { get; set; }

            public bool grade { get; set; }
        }

        /// <summary>
        ///  Estrutura para estudos
        /// </summary>
        [Serializable]
        public struct StructEstudos
        {
            public int tcp_id { get; set; }

            public int tcp_ordem { get; set; }

            public string anoLetivo { get; set; }

            public string cargaHoraria { get; set; }

            public string tci_nome { get; set; }

            public string tcp_descricao { get; set; }

            public string eta_nome { get; set; }

            public string estabelecimento { get; set; }

            public string municipio { get; set; }

            public string uf { get; set; }
        }

        /// <summary>
        ///  Estrutura para grade de históricos
        /// </summary>
        [Serializable]
        public struct StructGrade
        {
            public int tcp_id { get; set; }

            public int fav_id { get; set; }

            public int tci_id { get; set; }

            public Int64 alu_id { get; set; }

            public int alh_id { get; set; }

            public int ahd_id { get; set; }

            public int aco_id { get; set; }

            public int aco_ordem { get; set; }

            public byte aco_tipoBase { get; set; }

            public byte aco_tipoBaseGeral { get; set; }

            public byte ahd_resultado { get; set; }

            public int tds_id { get; set; }

            public int tds_ordem { get; set; }

            public byte tds_tipo { get; set; }

            public int ahp_id { get; set; }

            public string tcp_descricao { get; set; }

            public string tci_nome { get; set; }

            public string alh_resultado { get; set; }

            public string aco_nome { get; set; }

            public string tds_nome { get; set; }

            public string ahp_nome { get; set; }

            public string ahd_avaliacao { get; set; }

            public string ahd_frequencia { get; set; }

        }

        /// <summary>
        ///  Estrutura para transferência no histórico
        /// </summary>
        [Serializable]
        public struct StructTransferencia
        {
            public int tcp_id { get; set; }

            public int tpc_id { get; set; }

            public int fav_id { get; set; }

            public int tci_id { get; set; }

            public int ahp_id { get; set; }

            public Int64 alu_id { get; set; }

            public int aco_id { get; set; }

            public int aco_ordem { get; set; }

            public byte aco_tipoBase { get; set; }

            public byte aco_tipoBaseGeral { get; set; }

            public int tds_id { get; set; }

            public int tds_ordem { get; set; }

            public byte tds_tipo { get; set; }

            public string tcp_descricao { get; set; }

            public string tpc_nome { get; set; }

            public string tci_nome { get; set; }

            public string aco_nome { get; set; }

            public string Disciplina { get; set; }

            public string avaliacao { get; set; }

            public string frequencia { get; set; }

            public byte ResultadoProjeto { get; set; }

            public string dataTransferencia { get; set; }
        }

        /// <summary>
        ///  Estrutura para o resultado final Histórico do aluno.
        /// </summary>
        [Serializable]
        public struct StructResFinalHistorico
        {
            public int tcp_id { get; set; }

            public string tcp_descricao { get; set; }

            public Int64 alu_id { get; set; }

            public int alh_id { get; set; }

            public int fav_id { get; set; }

            public string alh_resultado { get; set; }

            public int ahd_id { get; set; }

            public int ahp_id { get; set; }

            public string ahp_nome { get; set; }

            public int tds_id { get; set; }

            public byte tds_ordem { get; set; }

            public string tds_nome { get; set; }

            public byte tds_tipo { get; set; }

            public string ahd_avaliacao { get; set; }

            public string ahd_frequencia { get; set; }

            public string ahd_resultado { get; set; }
        }
        #endregion Estruturas

        #region Métodos para estruturas

        /// <summary>
        /// Retorna List de StructHistorico
        /// contendo todos Historicos do Aluno
        /// apartir do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns></returns>
        public static SortedList<int, StructHistorico> CarregarPorAluno(long alu_id)
        {
            DataTable dtHistorico = GetSelectBy_alu_id(alu_id, false, 1, 1);
            DataTable dtHistoricoDisciplina = ACA_AlunoHistoricoDisciplinaBO.GetSelectBy_alu_id(alu_id);

            return CarregaHistAluno(dtHistorico, dtHistoricoDisciplina);
        }

        /// <summary>
        /// Retorna List de StructHistorico
        /// contendo todos Historicos do Aluno
        /// apartir do aluno e ano letivo.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="alh_anoLetivo">Ano letivo</param>
        /// <returns></returns>
        public static SortedList<int, StructHistorico> CarregarPorAlunoAnoLetivo(long alu_id, int alh_anoLetivo)
        {
            DataTable dtHistorico = GetSelectBy_alu_id_alh_anoLetivo(alu_id, alh_anoLetivo, false, 1, 1);
            DataTable dtHistoricoDisciplina = ACA_AlunoHistoricoDisciplinaBO.GetSelectBy_alu_id_alh_anoLetivo(alu_id, alh_anoLetivo);

            return CarregaHistAluno(dtHistorico, dtHistoricoDisciplina);
        }

        /// <summary>
        /// Retorna List de StructHistorico
        /// contendo todos Historicos do Aluno
        /// apartir do aluno e tipo curriculo periodo (serie).
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="tcp_id">Id da serie</param>
        /// <returns></returns>
        public static SortedList<int, StructHistorico> CarregarPorAlunoSerie(long alu_id, int tcp_id)
        {
            DataTable dtHistorico = GetSelectBy_alu_id_tcp_id(alu_id, tcp_id, false, 1, 1);
            DataTable dtHistoricoDisciplina = ACA_AlunoHistoricoDisciplinaBO.GetSelectBy_alu_id_tcp_id(alu_id, tcp_id);

            return CarregaHistAluno(dtHistorico, dtHistoricoDisciplina);
        }

        /// <summary>
        /// Retorna List de StructHistorico
        /// contendo todos Historicos do Aluno
        /// apartir do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="alh_id">ID do histórico do aluno</param>
        /// <returns></returns>
        public static SortedList<int, StructHistorico> CarregarPorAlunoHistorico(long alu_id, int alh_id)
        {
            DataTable dtHistorico = GetSelectBy_alu_id_alh_id(alu_id, alh_id, false, 1, 1);
            DataTable dtHistoricoDisciplina = ACA_AlunoHistoricoDisciplinaBO.GetSelectBy_alu_id_alh_id(alu_id, alh_id);

            return CarregaHistAluno(dtHistorico, dtHistoricoDisciplina);
        }

        private static SortedList<int, StructHistorico> CarregaHistAluno(DataTable dtHistorico, DataTable dtHistoricoDisciplina)
        {
            SortedList<int, StructHistorico> ltStructHistorico = new SortedList<int, StructHistorico>();

            foreach (DataRow drHistorico in dtHistorico.Rows)
            {
                StructHistorico historico = new StructHistorico();

                // Configura entidade ACA_AlunoHistorico
                ACA_AlunoHistorico entityAlunoHistorico = new ACA_AlunoHistorico();
                entityAlunoHistorico = new ACA_AlunoHistoricoDAO().DataRowToEntity(drHistorico, entityAlunoHistorico);
                historico.entHistorico = entityAlunoHistorico;
                historico.alh_resultadoDescricao = drHistorico["alh_resultadoDescricao"].ToString();
                historico.alh_resultadoDescricaoText = drHistorico["alh_resultadoDescricaoText"].ToString();
                historico.eco_nome = drHistorico["eco_nome"].ToString();
                historico.cur_crp_nome = drHistorico["cur_crp_nome"].ToString();
                historico.crp_descricao = drHistorico["crp_descricao"].ToString();
                historico.tcp_id = string.IsNullOrEmpty(drHistorico["tcp_id"].ToString()) ? -1 : Convert.ToInt32(drHistorico["tcp_id"].ToString());
                historico.uad_id = string.IsNullOrEmpty(drHistorico["uad_id"].ToString()) ? new Guid() : new Guid(drHistorico["uad_id"].ToString());
                historico.entHistorico.alh_tipoControleNotas = Convert.ToByte(drHistorico["alh_tipoControleNotas"]);
                historico.entHistorico.alh_cargaHorariaBaseNacional = String.IsNullOrEmpty(drHistorico["alh_cargaHorariaBaseNacional"].ToString()) ? -1 :
                    Convert.ToInt32(drHistorico["alh_cargaHorariaBaseNacional"]);
                historico.entHistorico.alh_cargaHorariaBaseDiversificada = String.IsNullOrEmpty(drHistorico["alh_cargaHorariaBaseDiversificada"].ToString()) ? -1 :
                    Convert.ToInt32(drHistorico["alh_cargaHorariaBaseDiversificada"]);

                // Configura entidade ACA_AlunoEscolaOrigem
                ACA_AlunoEscolaOrigem entityAlunoEscolaOrigem = new ACA_AlunoEscolaOrigem();
                entityAlunoEscolaOrigem = new ACA_AlunoEscolaOrigemDAO().DataRowToEntity(drHistorico, entityAlunoEscolaOrigem);
                historico.entEscolaOrigem = entityAlunoEscolaOrigem;

                // Configura List de entidade ACA_AlunoHistoricoDisciplina
                IEnumerable<DataRow> IEnumAlunoHistoricoDisciplina = (from DataRow dr in dtHistoricoDisciplina.Rows
                                                                      where Convert.ToInt32(dr["alh_id"]) == entityAlunoHistorico.alh_id
                                                                      select dr);
                List<StructHistoricoDisciplina> ltAlunoHistoricoDisciplina = new List<StructHistoricoDisciplina>();
                ACA_AlunoHistoricoDisciplinaDAO daoAlunoHistoricoDisciplina = new ACA_AlunoHistoricoDisciplinaDAO();
                foreach (DataRow dr in IEnumAlunoHistoricoDisciplina)
                {
                    ACA_AlunoHistoricoDisciplina entityAlunoHistoricoDisciplina = new ACA_AlunoHistoricoDisciplina();

                    ltAlunoHistoricoDisciplina.Add(new StructHistoricoDisciplina
                    {
                        entDisciplina = daoAlunoHistoricoDisciplina.DataRowToEntity(dr, entityAlunoHistoricoDisciplina)
                        ,
                        tds_id = dr["tds_id"].ToString()
                        ,
                        tds_nome = dr["tds_nome"].ToString()
                        ,
                        ahd_resultadoDescricao = dr["ahd_resultadoDescricao"].ToString()
                    });
                }
                historico.ltDisciplina = ltAlunoHistoricoDisciplina;

                ltStructHistorico.Add(historico.entHistorico.alh_id, historico);
            }

            return ltStructHistorico;
        }

        public static List<StructResFinalHistorico> CarregarResFinal(Int64 alu_id, int chp_anoLetivo, int tne_id)
        {
            DataTable dtHistorico = SelecionaHistoricoResFinal(alu_id, chp_anoLetivo, tne_id);

            List<StructResFinalHistorico> retorno = new List<StructResFinalHistorico>();

            foreach (DataRow drHistorico in dtHistorico.Rows)
            {
                StructResFinalHistorico historico = new StructResFinalHistorico();

                historico.tcp_id = string.IsNullOrEmpty(drHistorico["tcp_id"].ToString()) ? -1 : Convert.ToInt32(drHistorico["tcp_id"]);
                historico.tcp_descricao = drHistorico["tcp_descricao"].ToString();
                historico.alu_id = Convert.ToInt64(drHistorico["alu_id"]);
                historico.alh_id = Convert.ToInt32(drHistorico["alh_id"]);
                historico.fav_id = Convert.ToInt32(drHistorico["fav_id"]);
                historico.alh_resultado = drHistorico["alh_resultado"].ToString();
                historico.ahd_id = Convert.ToInt32(drHistorico["ahd_id"]);
                historico.ahp_id = Convert.ToInt32(drHistorico["ahp_id"]);
                historico.ahp_nome = drHistorico["ahp_nome"].ToString();
                historico.tds_id = Convert.ToInt32(drHistorico["tds_id"]);
                historico.tds_ordem = Convert.ToByte(drHistorico["tds_ordem"]);
                historico.tds_nome = drHistorico["tds_nome"].ToString();
                historico.tds_tipo = Convert.ToByte(drHistorico["tds_tipo"]);
                historico.ahd_avaliacao = drHistorico["ahd_avaliacao"].ToString();
                historico.ahd_frequencia = drHistorico["ahd_frequencia"].ToString();
                historico.ahd_resultado = drHistorico["ahd_resultado"].ToString();

                retorno.Add(historico);
            }

            return retorno;
        }

        /// <summary>
        /// Retorna os estudos do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>DataTable contendo os estudos</returns>
        public static List<StructEstudos> Seleciona_Estudos(long alu_id, Guid ent_id)
        {
            ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
            DataTable dtEstudos = dal.Seleciona_Estudos(alu_id, DateTime.Today.Year, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, ent_id));

            List<StructEstudos> retorno = new List<StructEstudos>();

            foreach (DataRow drEstudo in dtEstudos.Rows)
            {
                StructEstudos estudo = new StructEstudos();

                estudo.tcp_id = string.IsNullOrEmpty(drEstudo["tcp_id"].ToString()) ? -1 : Convert.ToInt32(drEstudo["tcp_id"]);
                estudo.tcp_ordem = Convert.ToInt32(drEstudo["tcp_ordem"]);
                estudo.tci_nome = drEstudo["tci_nome"].ToString();
                estudo.tcp_descricao = drEstudo["tcp_descricao"].ToString();
                estudo.eta_nome = drEstudo["eta_nome"].ToString();
                estudo.anoLetivo = drEstudo["anoLetivo"].ToString();
                estudo.cargaHoraria = drEstudo["cargaHoraria"].ToString();
                estudo.estabelecimento = drEstudo["estabelecimento"].ToString();
                estudo.municipio = drEstudo["municipio"].ToString();
                estudo.uf = drEstudo["uf"].ToString();

                retorno.Add(estudo);
            }

            return retorno;
        }

        /// <summary>
        /// Retorna a grade de históricos do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>DataTable contendo as grades de históricos</returns>
        public static List<StructGrade> Seleciona_Grade(long alu_id, Guid ent_id)
        {
            ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
            DataTable dtGrades = dal.Seleciona_Grade(alu_id, DateTime.Today.Year, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_FUNDAMENTAL, ent_id),
                                ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO, ent_id));

            List<StructGrade> retorno = new List<StructGrade>();

            foreach (DataRow drGrade in dtGrades.Rows)
            {
                if (Convert.ToByte(drGrade["tds_tipo"]) == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.RegenciaClasse)
                    continue;

                StructGrade grade = new StructGrade();

                grade.tcp_id = string.IsNullOrEmpty(drGrade["tcp_id"].ToString()) ? -1 : Convert.ToInt32(drGrade["tcp_id"]);
                grade.fav_id = Convert.ToInt32(drGrade["fav_id"]);
                grade.tci_id = Convert.ToInt32(drGrade["tci_id"]);
                grade.alu_id = Convert.ToInt64(drGrade["alu_id"]);
                grade.alh_id = Convert.ToInt32(drGrade["alh_id"]);
                grade.ahd_id = Convert.ToInt32(drGrade["ahd_id"]);
                grade.aco_id = Convert.ToInt32(drGrade["aco_id"]);
                grade.aco_ordem = Convert.ToInt32(drGrade["aco_ordem"]);
                grade.aco_tipoBase = Convert.ToByte(drGrade["aco_tipoBase"]);
                grade.aco_tipoBaseGeral = Convert.ToByte(drGrade["aco_tipoBaseGeral"]);
                grade.ahd_resultado = Convert.ToByte(drGrade["ahd_resultado"].ToString());
                grade.tds_id = Convert.ToInt32(drGrade["tds_id"]);
                grade.tds_ordem = Convert.ToInt32(drGrade["tds_ordem"]);
                grade.tds_tipo = Convert.ToByte(drGrade["tds_tipo"]);
                grade.ahp_id = Convert.ToInt32(drGrade["ahp_id"]);
                grade.tcp_descricao = drGrade["tcp_descricao"].ToString();
                grade.tci_nome = drGrade["tci_nome"].ToString();
                grade.alh_resultado = drGrade["alh_resultado"].ToString();
                grade.aco_nome = drGrade["aco_nome"].ToString();
                grade.tds_nome = drGrade["tds_nome"].ToString();
                grade.ahp_nome = drGrade["ahp_nome"].ToString();
                grade.ahd_avaliacao = drGrade["ahd_avaliacao"].ToString().Replace(",0", "").Replace(",00", "").Replace(".0", "").Replace(".00", "");
                grade.ahd_frequencia = drGrade["ahd_frequencia"].ToString();

                retorno.Add(grade);
            }

            return retorno;
        }

        /// <summary>
        /// Retorna a transferencia no histórico do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns>DataTable contendo a transferencia no históricos</returns>
        public static List<StructTransferencia> Seleciona_Transferencia(long alu_id, int mtu_id)
        {
            ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
            DataTable dtTransferencias = dal.Seleciona_Transferencia(alu_id, mtu_id);

            List<StructTransferencia> retorno = new List<StructTransferencia>();

            foreach (DataRow drTransferencia in dtTransferencias.Rows)
            {
                if (Convert.ToByte(drTransferencia["tds_tipo"]) == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.RegenciaClasse)
                    continue;

                StructTransferencia transferencia = new StructTransferencia();

                transferencia.fav_id = Convert.ToInt32(drTransferencia["fav_id"]);
                transferencia.tci_id = Convert.ToInt32(drTransferencia["tci_id"]);
                transferencia.tpc_id = Convert.ToInt32(drTransferencia["tpc_id"]);
                transferencia.tcp_id = string.IsNullOrEmpty(drTransferencia["tcp_id"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["tcp_id"]);
                transferencia.alu_id = Convert.ToInt64(drTransferencia["alu_id"]);
                transferencia.aco_id = string.IsNullOrEmpty(drTransferencia["aco_id"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["aco_id"]);
                transferencia.aco_ordem = string.IsNullOrEmpty(drTransferencia["aco_ordem"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["aco_ordem"]);
                transferencia.aco_tipoBase = string.IsNullOrEmpty(drTransferencia["aco_tipoBase"].ToString()) ? (byte)0 : Convert.ToByte(drTransferencia["aco_tipoBase"]);
                transferencia.aco_tipoBaseGeral = string.IsNullOrEmpty(drTransferencia["aco_tipoBaseGeral"].ToString()) ? (byte)0 : Convert.ToByte(drTransferencia["aco_tipoBaseGeral"]);
                transferencia.tds_id = string.IsNullOrEmpty(drTransferencia["tds_id"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["tds_id"]);
                transferencia.tds_ordem = string.IsNullOrEmpty(drTransferencia["tds_ordem"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["tds_ordem"]);
                transferencia.tds_tipo = string.IsNullOrEmpty(drTransferencia["tds_tipo"].ToString()) ? (byte)0 : Convert.ToByte(drTransferencia["tds_tipo"]);
                transferencia.tpc_nome = drTransferencia["tpc_nome"].ToString();
                transferencia.tcp_descricao = drTransferencia["tcp_descricao"].ToString();
                transferencia.tci_nome = drTransferencia["tci_nome"].ToString();
                transferencia.aco_nome = drTransferencia["aco_nome"].ToString();
                transferencia.Disciplina = drTransferencia["Disciplina"].ToString();
                transferencia.dataTransferencia = drTransferencia["alc_dataSaida"].ToString();
                transferencia.avaliacao = drTransferencia["avaliacao"].ToString().Replace(",0", "").Replace(",00", "").Replace(".0", "").Replace(".00", "");
                transferencia.frequencia = drTransferencia["frequencia"].ToString();
                transferencia.ResultadoProjeto = string.IsNullOrEmpty(drTransferencia["ResultadoProjeto"].ToString()) ? (byte)0 : Convert.ToByte(drTransferencia["ResultadoProjeto"]);
                transferencia.ahp_id = string.IsNullOrEmpty(drTransferencia["ProjetoId"].ToString()) ? -1 : Convert.ToInt32(drTransferencia["ProjetoId"]);

                retorno.Add(transferencia);
            }

            return retorno;
        }

        /// <summary>
        /// Gera um html table para os históricos do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static string GerarEsturosHistPedagogico(Int64 alu_id, Guid ent_id)
        {
            List<StructEstudos> lstEstudos = Seleciona_Estudos(alu_id, ent_id);

            if (!lstEstudos.Any())
                return "";

            StringBuilder retorno = new StringBuilder();

            retorno.AppendLine("<table " + style_table + ">");

            #region Header
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("CICLO/ANO", HistoricoPedagogico.valorEnum(border.left) +
                                                                       HistoricoPedagogico.valorEnum(border.top) +
                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                       HistoricoPedagogico.valorEnum(textalign.center), 1, 2, 66, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("ETAPA/ EJA", HistoricoPedagogico.valorEnum(border.left) +
                                                                       HistoricoPedagogico.valorEnum(border.top) +
                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                       HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 32, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("ANO DE CONCLUSÃO", HistoricoPedagogico.valorEnum(border.left) +
                                                                              HistoricoPedagogico.valorEnum(border.top) +
                                                                              HistoricoPedagogico.valorEnum(border.bottom) +
                                                                              HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 50, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("CARGA HORÁRIA", HistoricoPedagogico.valorEnum(border.left) +
                                                                           HistoricoPedagogico.valorEnum(border.top) +
                                                                           HistoricoPedagogico.valorEnum(border.bottom) +
                                                                           HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 50, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("ESTABELECIMENTO", HistoricoPedagogico.valorEnum(border.left) +
                                                                             HistoricoPedagogico.valorEnum(border.top) +
                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                       HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 360, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("MUNICÍPIO", HistoricoPedagogico.valorEnum(border.left) +
                                                                       HistoricoPedagogico.valorEnum(border.top) +
                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                       HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 140, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("UF", HistoricoPedagogico.valorEnum(border.left) +
                                                                HistoricoPedagogico.valorEnum(border.top) +
                                                                HistoricoPedagogico.valorEnum(border.right) +
                                                                HistoricoPedagogico.valorEnum(border.bottom) +
                                                                HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 20, 0));
            retorno.AppendLine("</tr>");
            #endregion
            #region Historicos
            int rowspanCiclo = 0;
            string ciclo = "";
            int rowspanEtapa = 0;
            string etapa = "";

            foreach (StructEstudos estudo in lstEstudos)
            {
                retorno.AppendLine("<tr style=\"height:14px;\">");
                if (estudo.tci_nome != ciclo)
                {
                    ciclo = estudo.tci_nome;
                    rowspanCiclo = lstEstudos.Where(e => e.tci_nome == estudo.tci_nome).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(estudo.tci_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                           HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                           HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                           HistoricoPedagogico.valorEnum(font.arial5) +
                                                                                           style_textVertical,
                                                                          rowspanCiclo, 1, 20, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.tcp_descricao, HistoricoPedagogico.valorEnum(border.left) +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 46, 0));
                if (etapa != estudo.eta_nome)
                {
                    etapa = estudo.eta_nome;
                    rowspanEtapa = lstEstudos.Where(e => e.eta_nome == estudo.eta_nome).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.eta_nome,
                                                                  HistoricoPedagogico.valorEnum(border.left) +
                                                                  HistoricoPedagogico.valorEnum(border.bottom) +
                                                                  HistoricoPedagogico.valorEnum(textalign.center),
                                                                  rowspanEtapa, 1, 32, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.anoLetivo, HistoricoPedagogico.valorEnum(border.left) +
                                                                                HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 0, 14));
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.cargaHoraria, HistoricoPedagogico.valorEnum(border.left) +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 0, 14));
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.estabelecimento, HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                      HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 0, 14));
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.municipio, HistoricoPedagogico.valorEnum(border.left) +
                                                                                HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 0, 14));
                retorno.AppendLine(HistoricoPedagogico.tdCell(estudo.uf, HistoricoPedagogico.valorEnum(border.left) +
                                                                         HistoricoPedagogico.valorEnum(border.right) +
                                                                         HistoricoPedagogico.valorEnum(border.bottom) +
                                                                         HistoricoPedagogico.valorEnum(textalign.center), 1, 1, 0, 14));
                retorno.AppendLine("</tr>");
            }
            #endregion
            retorno.AppendLine("</table>");

            return retorno.ToString();
        }

        public static short HistoricoRes(short resultado)
        {
            if (resultado == (short)TipoResultado.Aprovado || resultado == (short)TipoResultado.AprovadoConselho)
                return (short)ResultadoHistorico.Aprovado;
            else if (resultado == (short)TipoResultado.Reprovado)
                return (short)ResultadoHistorico.Reprovado;
            else if (resultado == (short)TipoResultado.ReprovadoFrequencia)
                return (short)ResultadoHistorico.ReprovadoFrequencia;
            else
                return 0;
        }

        /// <summary>
        /// Gera um html table para a grade de históricos do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static string GerarGradeHistPedagogico(Int64 alu_id, Guid ent_id)
        {
            List<StructGrade> lstGrade = Seleciona_Grade(alu_id, ent_id);

            if (!lstGrade.Any())
                return "";

            StringBuilder retorno = new StringBuilder();

            retorno.AppendLine("<table " + style_table + ">");

            #region Header
            int colspanGeral = 4;
            int rowspanTitulo = 4;
            int colspanSeries = lstGrade.GroupBy(g => g.tcp_descricao).Count();
            int colspanCiclos = lstGrade.GroupBy(g => g.tci_nome).Count();
            int rowspanProjetos = lstGrade.GroupBy(g => g.ahp_id > 0).Count() < 2 ? 2 : lstGrade.GroupBy(g => g.ahp_id > 0).Count();
            int rowspanResolucao = lstGrade.Where(g => g.aco_tipoBaseGeral == (byte)TipoBaseGeral.Resolucao).GroupBy(g => g.tds_nome).Count();
            int rowspanDecreto = lstGrade.Where(g => g.aco_tipoBaseGeral == (byte)TipoBaseGeral.Decreto).GroupBy(g => g.tds_nome).Count();
            int rowspanNacional = lstGrade.Where(g => g.aco_tipoBase == (byte)TipoBase.Nacional).GroupBy(g => g.tds_nome).Count();
            int rowspanDiversificada = lstGrade.Where(g => g.aco_tipoBase == (byte)TipoBase.Diversificada).GroupBy(g => g.tds_nome).Count();

            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("RES. CNE/ CEB Nº 4/10 e 07/10", HistoricoPedagogico.valorEnum(border.left) +
                                                                                          HistoricoPedagogico.valorEnum(border.top) +
                                                                                          HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                          HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                          style_textVertical,
                                                          rowspanTitulo + rowspanResolucao, 1, 20, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("BASE NA CIO NAL CO MUM", HistoricoPedagogico.valorEnum(border.left) +
                                                                                 HistoricoPedagogico.valorEnum(border.top) +
                                                                                 HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                 HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                 style_textVertical,
                                                          rowspanTitulo + rowspanNacional, 1, 20, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("Áreas de conhecimento", HistoricoPedagogico.valorEnum(border.left) +
                                                                                   HistoricoPedagogico.valorEnum(border.top) +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center),
                                                          rowspanTitulo, 1, 109, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("Componentes curriculares", HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.top) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                      HistoricoPedagogico.valorEnum(textalign.center),
                                                          rowspanTitulo, 1, 110, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("Resultado Final", HistoricoPedagogico.valorEnum(border.left) +
                                                                             HistoricoPedagogico.valorEnum(border.top) +
                                                                             HistoricoPedagogico.valorEnum(border.bottom) +
                                                                             HistoricoPedagogico.valorEnum(border.right) +
                                                                             HistoricoPedagogico.valorEnum(textalign.center) +
                                                                             HistoricoPedagogico.valorEnum(fontweight.bold),
                                                          1, colspanSeries * 2, 0, 0));
            retorno.AppendLine("</tr>");
            #endregion
            #region Ciclo
            int qtdCiclo = 0;
            string ciclo = "";
            bool borderLeft = false;
            retorno.AppendLine("<tr>");
            foreach (var grade in lstGrade.GroupBy(g => new { g.tci_nome }))
                if (grade.Key.tci_nome != ciclo)
                {
                    ciclo = grade.Key.tci_nome;
                    qtdCiclo = lstGrade.Where(g => g.tci_nome == grade.Key.tci_nome).GroupBy(g => new { g.tcp_descricao }).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.tci_nome, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                      HistoricoPedagogico.valorEnum(border.right) +
                                                                                      HistoricoPedagogico.valorEnum(textalign.center),
                                                                  1, qtdCiclo * 2, (int)(407 / colspanCiclos), 0));
                    borderLeft = true;
                }
            retorno.AppendLine("</tr>");
            #endregion
            #region Series
            string item = "";
            ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao();
            ACA_EscalaAvaliacao esa = new ACA_EscalaAvaliacao();
            borderLeft = false;
            retorno.AppendLine("<tr>");
            foreach (var grade in lstGrade.GroupBy(g => new { g.tcp_descricao }))
            {
                retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.tcp_descricao, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                       HistoricoPedagogico.valorEnum(border.right) +
                                                                                       HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 2, (int)(407 / colspanSeries), 0));
                borderLeft = true;
            }
            retorno.AppendLine("</tr>");
            //coluna C/N e F
            borderLeft = false;
            retorno.AppendLine("<tr>");
            foreach (var grade in lstGrade.GroupBy(g => new { g.tcp_descricao, g.fav_id }))
            {
                if (fav.fav_id != grade.Key.fav_id)
                {
                    fav = new ACA_FormatoAvaliacao { fav_id = grade.Key.fav_id };
                    ACA_FormatoAvaliacaoBO.GetEntity(fav);

                    if (esa.esa_id != (fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal))
                    {
                        esa = new ACA_EscalaAvaliacao { esa_id = fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal };
                        ACA_EscalaAvaliacaoBO.GetEntity(esa);

                        if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                            item = "N";
                        else if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                            item = "C";
                        else
                            item = "";
                    }
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(item, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(border.right) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 1, (int)((407 / colspanSeries) / 2), 0));
                retorno.AppendLine(HistoricoPedagogico.tdCell("F", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                  HistoricoPedagogico.valorEnum(border.right) +
                                                                                  HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 1, (int)((407 / colspanSeries) / 2), 0));
            }
            retorno.AppendLine("</tr>");
            #endregion
            #region Disciplinas - Resolucao
            int qtdareaC = 0;
            string areaC = "";
            bool addBaseD = false;
            foreach (var grade in lstGrade.Where(g => g.aco_id > 0 && g.aco_tipoBaseGeral == (byte)TipoBaseGeral.Resolucao)
                                          .GroupBy(g => new { g.aco_id, g.aco_nome, g.tds_nome, g.aco_tipoBase }))
            {
                borderLeft = false;
                retorno.AppendLine("<tr>");
                if (!addBaseD && grade.Key.aco_tipoBase == (byte)TipoBase.Diversificada)
                {
                    addBaseD = true;
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte("PARTE DIVER SIFICADA", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                 HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                                 HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                                 HistoricoPedagogico.valorEnum(font.arial5),
                                                                          rowspanDiversificada, 1, 20, 0));
                }
                if (areaC != grade.Key.aco_nome)
                {
                    areaC = grade.Key.aco_nome;
                    qtdareaC = lstGrade.Where(g => g.aco_nome == grade.Key.aco_nome).GroupBy(g => g.tds_nome).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.aco_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom),
                                                                  qtdareaC, 1, 0, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.tds_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                foreach (StructGrade disc in lstGrade.Where(g => g.aco_id == grade.Key.aco_id && g.tds_nome == grade.Key.tds_nome))
                {
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(disc.ahd_avaliacao, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                              HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                              HistoricoPedagogico.valorEnum(border.right) +
                                                                                              HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                              HistoricoPedagogico.valorEnum(font.arial7), 1, 1, 0, 0));
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(disc.ahd_frequencia, HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                               HistoricoPedagogico.valorEnum(border.right) +
                                                                                               HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                               HistoricoPedagogico.valorEnum(font.arial7), 1, 1, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            #endregion
            #region Ensino religioso
            borderLeft = false;
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("Ensino religioso", HistoricoPedagogico.valorEnum(border.left) +
                                                                              HistoricoPedagogico.valorEnum(border.bottom) +
                                                                              HistoricoPedagogico.valorEnum(textalign.center),
                                                          1, colspanGeral, 0, 0));
            for (int i = 0; i < colspanSeries; i++)
            {
                retorno.AppendLine(HistoricoPedagogico.tdCell("", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                  HistoricoPedagogico.valorEnum(border.bottom) +
                                                                  HistoricoPedagogico.valorEnum(border.right) +
                                                                  HistoricoPedagogico.valorEnum(textalign.center) +
                                                                  HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                borderLeft = true;
            }
            retorno.AppendLine("</tr>");
            #endregion
            #region Disciplinas - Decreto
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte("Decreto Muni cipal nº 54.452 /13", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                    HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                                    HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                                    HistoricoPedagogico.valorEnum(font.arial7) +
                                                                                                    style_textVertical,
                                                                  rowspanDecreto + rowspanProjetos, 1, 0, 0));
            bool addTr = false;
            foreach (var grade in lstGrade.Where(g => g.aco_tipoBaseGeral == (byte)TipoBaseGeral.Decreto)
                                          .GroupBy(g => new { g.aco_id, g.aco_nome, g.tds_nome }))
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                if (areaC != grade.Key.aco_nome)
                {
                    areaC = grade.Key.aco_nome;
                    qtdareaC = lstGrade.Where(g => g.aco_nome == grade.Key.aco_nome).GroupBy(g => g.tds_nome).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.aco_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom),
                                                                  qtdareaC, 2, 0, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.tds_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom),
                                                              1, 1, 0, 0));
                foreach (StructGrade disc in lstGrade.Where(g => g.aco_id == grade.Key.aco_id && g.tds_nome == grade.Key.tds_nome))
                {
                    string resFreq = "";
                    if (disc.ahd_resultado > 0 && disc.ahd_resultado > 0)
                    {
                        if (disc.ahd_resultado == (byte)ResultadoHistoricoDisciplina.ReprovadoFrequencia)
                            resFreq = "NF";
                        else if (disc.ahd_resultado == (byte)ResultadoHistoricoDisciplina.Aprovado)
                            resFreq = "F";
                    }
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(resFreq,
                                       (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                       HistoricoPedagogico.valorEnum(border.bottom) + HistoricoPedagogico.valorEnum(border.right) +
                                       HistoricoPedagogico.valorEnum(textalign.center) + HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            #region Projetos/Atividades complementares
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("Projetos/Atividades complementares", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                HistoricoPedagogico.valorEnum(border.bottom),
                                                          rowspanProjetos, 2, 0, 0));
            addTr = false;
            foreach (var grade in lstGrade.Where(g => g.ahp_id > 0)
                                          .GroupBy(g => new { g.ahp_id, g.ahp_nome }))
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                retorno.AppendLine(HistoricoPedagogico.tdCell(grade.Key.ahp_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                foreach (StructGrade ahp in lstGrade.Where(g => g.ahp_id == grade.Key.ahp_id && g.ahp_nome == grade.Key.ahp_nome))
                {
                    string resFreq = "";
                    if (ahp.ahd_resultado > 0 && ahp.ahd_resultado > 0)
                    {
                        resFreq = ahp.ahd_frequencia;
                    }
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(resFreq,
                                                                          (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                          HistoricoPedagogico.valorEnum(border.bottom) + HistoricoPedagogico.valorEnum(border.right) +
                                                                          HistoricoPedagogico.valorEnum(textalign.center) + HistoricoPedagogico.valorEnum(font.arial7),
                                                                          1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            //se não há 2 projetos cadastrados para o aluno então gera linhas em branco
            for (int i = lstGrade.Where(g => g.ahp_id > 0).GroupBy(g => g.ahp_id).Count(); i < 2; i++)
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                retorno.AppendLine(HistoricoPedagogico.tdCell("", HistoricoPedagogico.valorEnum(border.left) +
                                                                  HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                for (int p = 0; p < colspanSeries; p++)
                {
                    retorno.AppendLine(HistoricoPedagogico.tdCell("", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                      HistoricoPedagogico.valorEnum(border.right) +
                                                                      HistoricoPedagogico.valorEnum(textalign.center) +
                                                                      HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            #endregion
            #endregion
            #region Parecer conclusivo
            borderLeft = false;
            DataTable dtPareceresFinal = ACA_TipoResultadoBO.SELECT_By_TipoLancamento((byte)EnumTipoLancamento.HistoricoEscolar);
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("Parecer conclusivo", HistoricoPedagogico.valorEnum(border.left) +
                                                                                HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                HistoricoPedagogico.valorEnum(textalign.center),
                                                          1, colspanGeral, 0, 0));
            foreach (var grade in lstGrade.GroupBy(g => new { g.tcp_descricao, g.alh_resultado }))
            {
                if (!string.IsNullOrEmpty(grade.Key.alh_resultado) && Convert.ToByte(grade.Key.alh_resultado) > 0)
                {
                    string resultado = dtPareceresFinal.AsEnumerable().Where(p => Convert.ToByte(p.Field<object>("tpr_resultado")) == HistoricoRes(Convert.ToByte(grade.Key.alh_resultado)))
                                                                      .FirstOrDefault().Field<object>("tpr_nomenclatura").ToString();
                    resultado = resultado.Substring(0, resultado.IndexOf("(")).Trim();
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(resultado, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                     HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                     HistoricoPedagogico.valorEnum(border.right) +
                                                                                     HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                     HistoricoPedagogico.valorEnum(font.arial7),
                                                                          1, 2, 0, 0));
                }
                else
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte("", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                          HistoricoPedagogico.valorEnum(border.bottom) +
                                                                          HistoricoPedagogico.valorEnum(border.right) +
                                                                          HistoricoPedagogico.valorEnum(textalign.center) +
                                                                          HistoricoPedagogico.valorEnum(font.arial7),
                                                                          1, 2, 0, 0));
                borderLeft = true;
            }
            retorno.AppendLine("</tr>");
            #endregion
            retorno.AppendLine("</table>");

            return retorno.ToString();
        }

        /// <summary>
        /// Gera um html table para a transferência no histórico do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="dataTransferencia">data de transferência</param>
        /// <returns></returns>
        public static string GerarTransferenciaHistPedagogico(Int64 alu_id, int mtu_id, out string dataTransferencia)
        {
            bool mostraNotas = MTR_MovimentacaoBO.VerificaAlunoPossuiMovimentacaoSaidaEscola(alu_id, mtu_id);

            dataTransferencia = "";

            List<StructTransferencia> lstTransferencia = Seleciona_Transferencia(alu_id, mtu_id);

            if (!lstTransferencia.Any())
                return "";

            dataTransferencia = "TRANSFERÊNCIA DURANTE O ANO LETIVO ATÉ {0}";
            dataTransferencia = string.Format(dataTransferencia,
                                              (string.IsNullOrEmpty(lstTransferencia.FirstOrDefault().dataTransferencia) ? "_____/_____/_____" :
                                               lstTransferencia.FirstOrDefault().dataTransferencia));

            StringBuilder retorno = new StringBuilder();

            retorno.AppendLine("<table " + style_table + ">");

            #region Header
            int colspanGeral = 4;
            int colspanBimestre = lstTransferencia.GroupBy(t => t.tpc_nome).Count();
            int rowspanTitulo = 3;
            int rowspanProjetos = lstTransferencia.Where(g => g.ahp_id > 0).Count() < 2 ? 2 : lstTransferencia.Where(g => g.ahp_id > 0).Count();
            int rowspanResolucao = lstTransferencia.Where(t => t.aco_tipoBaseGeral == (byte)TipoBaseGeral.Resolucao).GroupBy(t => t.Disciplina).Count();
            int rowspanDecreto = lstTransferencia.Where(t => t.aco_tipoBaseGeral == (byte)TipoBaseGeral.Decreto).GroupBy(t => t.Disciplina).Count();
            int rowspanNacional = lstTransferencia.Where(t => t.aco_tipoBase == (byte)TipoBase.Nacional).GroupBy(t => t.Disciplina).Count();
            int rowspanDiversificada = lstTransferencia.Where(t => t.aco_tipoBase == (byte)TipoBase.Diversificada).GroupBy(t => t.Disciplina).Count();

            string conceitoNota = "CONCEITOS/NOTAS";
            string anoSerie = string.IsNullOrEmpty(lstTransferencia.FirstOrDefault().tcp_descricao) ? "____ ANO" : lstTransferencia.FirstOrDefault().tcp_descricao.ToUpper();
            string cicloMsg = string.IsNullOrEmpty(lstTransferencia.FirstOrDefault().tci_nome) ? "CICLO _____________________" : lstTransferencia.FirstOrDefault().tci_nome.ToUpper();
            string anoSerieProx = string.IsNullOrEmpty(lstTransferencia.FirstOrDefault().tcp_descricao) ? "____ ano" : lstTransferencia.FirstOrDefault().tcp_descricao.ToLower();
            string cicloMsgProx = string.IsNullOrEmpty(lstTransferencia.FirstOrDefault().tci_nome) ? "ciclo _____________________" : lstTransferencia.FirstOrDefault().tci_nome.ToLower();
            ACA_EscalaAvaliacao esa = new ACA_EscalaAvaliacao();
            ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao { fav_id = lstTransferencia.FirstOrDefault().fav_id };
            ACA_FormatoAvaliacaoBO.GetEntity(fav);

            if (esa.esa_id != (fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal))
            {
                esa = new ACA_EscalaAvaliacao { esa_id = fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal };
                ACA_EscalaAvaliacaoBO.GetEntity(esa);

                if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                    conceitoNota = "NOTAS";
                else if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                    conceitoNota = "CONCEITOS";
                else
                    conceitoNota = "CONCEITOS/NOTAS";
            }
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("RES. CNE/ CEB Nº 4/10 e 07/10", HistoricoPedagogico.valorEnum(border.left) +
                                                                                          HistoricoPedagogico.valorEnum(border.top) +
                                                                                          HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                          HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                          style_textVertical,
                                                          rowspanTitulo + rowspanResolucao, 1, 20, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("BASE NA CIO NAL CO MUM", HistoricoPedagogico.valorEnum(border.left) +
                                                                                 HistoricoPedagogico.valorEnum(border.top) +
                                                                                 HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                 HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                 style_textVertical,
                                                          rowspanTitulo + rowspanNacional, 1, 20, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("Áreas de conhecimento", HistoricoPedagogico.valorEnum(border.left) +
                                                                                   HistoricoPedagogico.valorEnum(border.top) +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center),
                                                          rowspanTitulo, 1, 110, 0));
            retorno.AppendLine(HistoricoPedagogico.tdCell("Componentes curriculares", HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.top) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                      HistoricoPedagogico.valorEnum(textalign.center),
                                                          rowspanTitulo, 1, 120, 0));
            string msgAluno = "{0} E Nº DE FALTAS NO {1} DO {2} DO ENSINO FUNDAMENTAL";
            msgAluno = string.Format(msgAluno, conceitoNota, anoSerie, cicloMsg);
            retorno.AppendLine(HistoricoPedagogico.tdCell(msgAluno, HistoricoPedagogico.valorEnum(border.left) +
                                                          HistoricoPedagogico.valorEnum(border.top) + HistoricoPedagogico.valorEnum(border.bottom) +
                                                          HistoricoPedagogico.valorEnum(border.right) + HistoricoPedagogico.valorEnum(textalign.center) +
                                                          HistoricoPedagogico.valorEnum(fontweight.bold), 1, colspanBimestre * 2, 0, 0));
            retorno.AppendLine("</tr>");
            #endregion
            #region Bimestre
            string item = "";
            bool borderLeft = false;
            retorno.AppendLine("<tr>");
            foreach (var trans in lstTransferencia.GroupBy(t => new { t.tpc_nome }))
            {
                retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.tpc_nome, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                       HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                       HistoricoPedagogico.valorEnum(border.right) +
                                                                                       HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 2, (int)(455 / colspanBimestre), 0));
                borderLeft = true;
            }
            retorno.AppendLine("</tr>");
            //coluna C/N e F
            borderLeft = false;
            fav = new ACA_FormatoAvaliacao();
            esa = new ACA_EscalaAvaliacao();
            item = "";
            retorno.AppendLine("<tr>");
            foreach (var trans in lstTransferencia.GroupBy(t => new { t.tpc_nome, t.fav_id }))
            {
                if (fav.fav_id != trans.Key.fav_id)
                {
                    fav = new ACA_FormatoAvaliacao { fav_id = trans.Key.fav_id };
                    ACA_FormatoAvaliacaoBO.GetEntity(fav);

                    if (esa.esa_id != (fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal))
                    {
                        esa = new ACA_EscalaAvaliacao { esa_id = fav.esa_idPorDisciplina > 0 ? fav.esa_idPorDisciplina : fav.esa_idConceitoGlobal };
                        ACA_EscalaAvaliacaoBO.GetEntity(esa);

                        if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                            item = "N";
                        else if (esa.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                            item = "C";
                        else
                            item = "";
                    }
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(item, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                   HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                   HistoricoPedagogico.valorEnum(border.right) +
                                                                                   HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 1, (int)(455 / colspanBimestre / 2), 0));
                retorno.AppendLine(HistoricoPedagogico.tdCell("F", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                  HistoricoPedagogico.valorEnum(border.right) +
                                                                                  HistoricoPedagogico.valorEnum(textalign.center),
                                                              1, 1, (int)(455 / colspanBimestre / 2), 0));
            }
            retorno.AppendLine("</tr>");
            #endregion
            #region Disciplinas - Resolucao
            int qtdareaC = 0;
            string areaC = "";
            bool addBaseD = false;
            foreach (var trans in lstTransferencia.Where(t => t.aco_id > 0 && t.aco_tipoBaseGeral == (byte)TipoBaseGeral.Resolucao)
                                                  .GroupBy(t => new { t.aco_id, t.aco_nome, t.Disciplina, t.aco_tipoBase }))
            {
                borderLeft = false;
                retorno.AppendLine("<tr>");
                if (!addBaseD && trans.Key.aco_tipoBase == (byte)TipoBase.Diversificada)
                {
                    addBaseD = true;
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte("PARTE DIVER SIFICADA", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                 HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                                 HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                                 HistoricoPedagogico.valorEnum(font.arial5),
                                                                          rowspanDiversificada, 1, 20, 0));
                }
                if (areaC != trans.Key.aco_nome)
                {
                    areaC = trans.Key.aco_nome;
                    qtdareaC = lstTransferencia.Where(t => t.aco_nome == trans.Key.aco_nome).GroupBy(t => t.Disciplina).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.aco_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom),
                                                                  qtdareaC, 1, 0, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.Disciplina, HistoricoPedagogico.valorEnum(border.left) +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                foreach (StructTransferencia disc in lstTransferencia.Where(t => t.aco_id == trans.Key.aco_id && t.Disciplina == trans.Key.Disciplina))
                {
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(!mostraNotas ? "" : disc.avaliacao, (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                                           HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                           HistoricoPedagogico.valorEnum(border.right) +
                                                                                           HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                           HistoricoPedagogico.valorEnum(font.arial7), 1, 1, 0, 0));
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(!mostraNotas ? "" : disc.frequencia, HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                           HistoricoPedagogico.valorEnum(border.right) +
                                                                                           HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                           HistoricoPedagogico.valorEnum(font.arial7), 1, 1, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            #endregion
            #region Ensino religioso
            borderLeft = false;
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("Ensino religioso", HistoricoPedagogico.valorEnum(border.left) +
                                                                              HistoricoPedagogico.valorEnum(border.bottom) +
                                                                              HistoricoPedagogico.valorEnum(textalign.center),
                                                          1, colspanGeral, 0, 0));
            for (int i = 0; i < colspanBimestre; i++)
            {
                retorno.AppendLine(HistoricoPedagogico.tdCell("", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                  HistoricoPedagogico.valorEnum(border.bottom) +
                                                                  HistoricoPedagogico.valorEnum(border.right) +
                                                                  HistoricoPedagogico.valorEnum(textalign.center) +
                                                                  HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                borderLeft = true;
            }
            retorno.AppendLine("</tr>");
            #endregion
            #region Disciplinas - Decreto
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte("Decreto Muni cipal nº 54.452 /13", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                    HistoricoPedagogico.valorEnum(border.bottom) +
                                                                                                    HistoricoPedagogico.valorEnum(textalign.center) +
                                                                                                    HistoricoPedagogico.valorEnum(font.arial7) +
                                                                                                    style_textVertical,
                                                                  rowspanDecreto + rowspanProjetos, 1, 0, 0));
            bool addTr = false;
            foreach (var trans in lstTransferencia.Where(t => t.aco_tipoBaseGeral == (byte)TipoBaseGeral.Decreto)
                                                  .GroupBy(t => new { t.aco_id, t.aco_nome, t.Disciplina }))
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                if (areaC != trans.Key.aco_nome)
                {
                    areaC = trans.Key.aco_nome;
                    qtdareaC = lstTransferencia.Where(t => t.aco_nome == trans.Key.aco_nome).GroupBy(t => t.Disciplina).Count();
                    retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.aco_nome, HistoricoPedagogico.valorEnum(border.left) +
                                                                                      HistoricoPedagogico.valorEnum(border.bottom),
                                                                  qtdareaC, 2, 0, 0));
                }
                retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.Disciplina, HistoricoPedagogico.valorEnum(border.left) +
                                                                                  HistoricoPedagogico.valorEnum(border.bottom),
                                                              1, 1, 0, 0));
                foreach (StructTransferencia disc in lstTransferencia.Where(t => t.aco_id == trans.Key.aco_id && t.Disciplina == trans.Key.Disciplina))
                {
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(!mostraNotas ? "" : disc.frequencia,
                                       (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                       HistoricoPedagogico.valorEnum(border.bottom) + HistoricoPedagogico.valorEnum(border.right) +
                                       HistoricoPedagogico.valorEnum(textalign.center) + HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            DataTable dtProjetos = ACA_AlunoHistoricoProjetoBO.SelectBy_Aluno(alu_id);
            #region Projetos/Atividades complementares
            retorno.AppendLine("<tr>");
            retorno.AppendLine(HistoricoPedagogico.tdCell("Projetos/Atividades complementares", HistoricoPedagogico.valorEnum(border.left) +
                                                                                                HistoricoPedagogico.valorEnum(border.bottom),
                                                          rowspanProjetos, 2, 0, 0));
            addTr = false;
            foreach (var trans in lstTransferencia.Where(t => t.ahp_id > 0)
                                                  .GroupBy(t => new { t.ahp_id, t.Disciplina }))
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                retorno.AppendLine(HistoricoPedagogico.tdCell(trans.Key.Disciplina, HistoricoPedagogico.valorEnum(border.left) +
                                                                                    HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                foreach (StructTransferencia ahp in lstTransferencia.Where(t => t.ahp_id == trans.Key.ahp_id && t.Disciplina == trans.Key.Disciplina))
                {
                    //string resFreq = "";
                    //if (ahp.ResultadoProjeto > 0 && ahp.ResultadoProjeto > 0)
                    //{
                    //    if (ahp.ResultadoProjeto == (byte)TipoResultado.ReprovadoFrequencia)
                    //        resFreq = "NF";
                    //    else if (ahp.ResultadoProjeto == (byte)TipoResultado.Aprovado)
                    //        resFreq = "F";
                    //}
                    retorno.AppendLine(HistoricoPedagogico.tdCellSemFonte(!mostraNotas ? "" : ahp.frequencia,//resFreq,
                                                                          (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                          HistoricoPedagogico.valorEnum(border.bottom) + HistoricoPedagogico.valorEnum(border.right) +
                                                                          HistoricoPedagogico.valorEnum(textalign.center) + HistoricoPedagogico.valorEnum(font.arial7),
                                                                          1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            //se não há 2 projetos cadastrados para o aluno então gera linhas em branco
            for (int i = lstTransferencia.Where(t => t.ahp_id > 0).GroupBy(t => t.ahp_id).Count(); i < 2; i++)
            {
                borderLeft = false;
                if (addTr)
                    retorno.AppendLine("<tr>");
                addTr = true;
                retorno.AppendLine(HistoricoPedagogico.tdCell("", HistoricoPedagogico.valorEnum(border.left) +
                                                                  HistoricoPedagogico.valorEnum(border.bottom), 1, 1, 0, 0));
                for (int p = 0; p < colspanBimestre; p++)
                {
                    retorno.AppendLine(HistoricoPedagogico.tdCell("", (!borderLeft ? HistoricoPedagogico.valorEnum(border.left) : "") +
                                                                      HistoricoPedagogico.valorEnum(border.bottom) +
                                                                      HistoricoPedagogico.valorEnum(border.right) +
                                                                      HistoricoPedagogico.valorEnum(textalign.center) +
                                                                      HistoricoPedagogico.valorEnum(font.arial7), 1, 2, 0, 0));
                    borderLeft = true;
                }
                retorno.AppendLine("</tr>");
            }
            #endregion
            #endregion
            #region Mensagem final
            retorno.AppendLine("<tr>");
            string msgFinal = "O(a) aluno(a) tem direito à matrícula no {0} do {1} do Ensino Fundamental";
            msgFinal = string.Format(msgFinal, anoSerieProx, cicloMsgProx);
            retorno.AppendLine(HistoricoPedagogico.tdCell(msgFinal, HistoricoPedagogico.valorEnum(border.left) +
                                                                    HistoricoPedagogico.valorEnum(border.bottom) +
                                                                    HistoricoPedagogico.valorEnum(border.right) +
                                                                    HistoricoPedagogico.valorEnum(textalign.center),
                                                          1, colspanGeral + colspanBimestre, 0, 0));
            retorno.AppendLine("</tr>");
            #endregion
            retorno.AppendLine("</table>");

            return retorno.ToString();
        }

        #endregion Métodos para estruturas

        #region Métodos de consulta

        /// <summary>
        /// Retorna um DataTable retorna as disciplinas do currículo
        /// utilizado no cadastro de histórico. Não trás as eletivas
        /// do aluno.
        /// </summary>
        /// <param name="cur_id_crr_id_crp_id"></param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>        
        /// <returns>DataTable contendo as disciplinas do currículo</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCurriculoDisciplina
        (
            string cur_id_crr_id_crp_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            int cur_id = 0;
            int crr_id = 0;
            int crp_id = 0;

            if (!string.IsNullOrEmpty(cur_id_crr_id_crp_id))
            {
                cur_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[0]);
                crr_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[1]);
                crp_id = Convert.ToInt32(cur_id_crr_id_crp_id.Split(';')[2]);
            }

            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
            return dal.SelecionaCurriculoDisciplina(cur_id, crr_id, crp_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }
                
        /// <summary>
        /// Seleciona as disciplinas para o histórico configurado no ano letivo informado 
        /// pelo tipo de curriculo período e tipo nível de ensino.
        /// </summary>
        /// <param name="tcp_id"></param>
        /// <param name="chp_anoLetivo"></param>
        /// <param name="tne_id"></param>
        public static DataTable Seleciona_TipoCurriculoPeriodoAnoLetivo(int tcp_id, int chp_anoLetivo, int tne_id)
        {
            ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
            return dal.Seleciona_TipoCurriculoPeriodoAnoLetivo(tcp_id, chp_anoLetivo, tne_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelectBy_alu_id(alu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e ano letivo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_anoLetivo">Ano letivo do historico</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_alh_anoLetivo
        (
            long alu_id
            , int alh_anoLetivo
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelectBy_alu_id_alh_anoLetivo(alu_id, alh_anoLetivo, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e tipo curriculo periodo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="tcp_id">Id da tabela ACA_TipoCurriculoPeriodo</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_tcp_id
        (
            long alu_id
            , int tcp_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelectBy_alu_id_tcp_id(alu_id, tcp_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id e historico id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="alh_id">Id do historico do aluno</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_alu_id_alh_id
        (
            long alu_id
            , int alh_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelectBy_alu_id_alh_id(alu_id, alh_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna uma lista com os históricos do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<ACA_AlunoHistorico> RetornaListaHistoricosAluno
        (
            long alu_id
            , TalkDBTransaction banco
        )
        {
            List<ACA_AlunoHistorico> lista = new List<ACA_AlunoHistorico>();

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO
            {
                _Banco = banco
            };

            DataTable dt = GetSelectBy_alu_id(alu_id, false, 1, 1);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_AlunoHistorico ent = new ACA_AlunoHistorico();
                lista.Add(dao.DataRowToEntity(dr, ent));
            }

            return lista;
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id (obrigatoriamente > 0)
        /// Stored Procedure derivada da SelectBy_alu_id, porém otimizada para trazer apenas os campos necessários para carergar o grid
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <returns>DataTable com os Historicos do Aluno</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaHistoricoPorAluno
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelectHistorico_PorAluno(alu_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// para carregar a tela de historicos resultado final do aluno
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <param name="chp_anoLetivo">Ano letivo para pegar os tipos de curriculo periodo</param>
        /// <param name="tne_id">Id do tipo nivel ensino do fundamental</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public static DataTable SelecionaHistoricoResFinal(Int64 alu_id, int chp_anoLetivo, int tne_id)
        {
            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO();
            return dao.SelecionaHistoricoResFinal(alu_id, chp_anoLetivo, tne_id);
        }

        /// <summary>
        /// Seleciona histórico do aluno por matrícula turma.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma.</param>
        /// <returns></returns>
        public static ACA_AlunoHistorico SelecionaPorMatriculaTurma(long alu_id, int mtu_id)
        {
            return new ACA_AlunoHistoricoDAO().SelecionaPorMatriculaTurma(alu_id, mtu_id);
        }

        #endregion Métodos de consulta

        #region Métodos de validação

        /// <summary>
        /// Verifica se foi digitado apenas números ou caracter "+".
        /// </summary>
        /// <param name="frequencia">O valor da frequência.</param>
        /// <returns></returns>
        public static bool ValidaCaracteresFrequencia(string frequencia)
        {
            int iCont;

            for (iCont = 0; iCont < frequencia.Length; iCont++)
            {
                if (frequencia[iCont] != '+' && frequencia[iCont].ToString().Where(c => char.IsNumber(c)).Count() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Valida nota global (obrigatório e max 4 caract) e frequência (obrigatório e max. 3 caract.)
        /// </summary>
        /// <param name="entityAlunoHistorico">Entidade do histórico.</param>
        /// <param name="listaErros">Lista de erros.</param>
        /// <returns>Retorna true - encontrou problema no dados; false - nenhum problem encontrado</returns>
        public static void ValidaGlobal(ACA_AlunoHistorico entityAlunoHistorico, ref List<string> listaErros)
        {
            // Conceito Global
            if (entityAlunoHistorico.alh_tipoControleNotas == (byte)TipoControleNotas.Global || entityAlunoHistorico.alh_tipoControleNotas == (byte)TipoControleNotas.GlobalPorDisciplina)
            {
                //  Avaliação (Máximo de 4 caracteres).
                if (string.IsNullOrEmpty(entityAlunoHistorico.alh_avaliacao) || entityAlunoHistorico.alh_avaliacao.Count() > 4)
                {
                    listaErros.Add(String.Format("A avaliação do histórico de {0} é obrigatório e deve possuir no máximo 4 caracteres.", entityAlunoHistorico.alh_anoLetivo));
                }

                if (!string.IsNullOrEmpty(entityAlunoHistorico.alh_frequencia))
                {
                    decimal frequencia;

                    if (decimal.TryParse(entityAlunoHistorico.alh_frequencia, out frequencia))
                    {
                        if (frequencia < 0 || frequencia > 100)
                        {
                            listaErros.Add(String.Format("A frequência do histórico de {0} deve possuir um valor entre 0 e 100.", entityAlunoHistorico.alh_anoLetivo));
                        }
                    }
                    else
                    {
                        listaErros.Add(String.Format("A frequência do histórico de {0} deve possuir um valor decimal.", entityAlunoHistorico.alh_anoLetivo));
                    }
                }
                else
                {
                    listaErros.Add(String.Format("A frequência do histórico de {0} é obrigatório.", entityAlunoHistorico.alh_anoLetivo));
                }
            }
        }

        /// <summary>
        /// Valida nota do conceito global e resultado
        /// </summary>
        /// <param name="entityAlunoHistorico">Entidade do histórico.</param>
        /// <param name="listaErros">Lista de erros.</param>
        /// <param name="banco">Banco de dados.</param>
        /// <returns>Retorna true - encontrou problema no dados; false - nenhum problem encontrado</returns>
        public static void ValidaGlobalDentroRede(ACA_AlunoHistorico entityAlunoHistorico, ref List<string> listaErros, TalkDBTransaction banco)
        {
            string ConceitosPermitidos;
            // Conceito global
            if (ValidarAvaliacaoConceitoGlobal(entityAlunoHistorico, out ConceitosPermitidos, banco))
            {
                listaErros.Add(String.Format("O conceito da avaliação é inválido para ano de {0}. Os conceitos permitidos para este ano letivo são: {1}.",
                                            entityAlunoHistorico.alh_anoLetivo, ConceitosPermitidos));
            }

            // Resultado
            if (string.IsNullOrEmpty(entityAlunoHistorico.alh_resultado.ToString()) || entityAlunoHistorico.alh_resultado <= 0)
            {
                listaErros.Add(String.Format("O resultado do histórico para ano de {0} é obrigatório.", entityAlunoHistorico.alh_anoLetivo, ConceitosPermitidos));
            }
        }

        /// <summary>
        /// Valida as notas do conceito global
        /// </summary>
        /// <param name="entityAlunoHistorico">Entidade do histórico.</param>
        /// <param name="banco">Banco de dados.</param>
        /// <returns>Retorna true - encontrou problema no dados; false - nenhum problem encontrado</returns>
        public static bool ValidarAvaliacaoConceitoGlobal(ACA_AlunoHistorico entityAlunoHistorico, out string ConceitosPermitidos, TalkDBTransaction banco)
        {
            ConceitosPermitidos = string.Empty;

            // Retorna os conceitos do histórico (Validação apenas para escolas dentro da rede)
            List<CFG_HistoricoConceito> listConceitosHistorico = CFG_HistoricoConceitoBO.RetornaListaConceitosHistorico(banco);

            // Conceito Global
            if (entityAlunoHistorico.alh_tipoControleNotas == (byte)TipoControleNotas.Global || entityAlunoHistorico.alh_tipoControleNotas == (byte)TipoControleNotas.GlobalPorDisciplina)
            {
                List<CFG_HistoricoConceito> ltConceitosAno = listConceitosHistorico.FindAll(p => p.chc_ano == entityAlunoHistorico.alh_anoLetivo);

                if (ltConceitosAno != null && ltConceitosAno.Count > 0)
                {
                    List<string> conceitos = new List<string>();

                    foreach (CFG_HistoricoConceito entity in ltConceitosAno.ToList())
                    {
                        conceitos.AddRange(entity.chc_conceitos.Split('|').ToList());

                        if (string.IsNullOrEmpty(ConceitosPermitidos))
                            ConceitosPermitidos = entity.chc_conceitos.Replace('|', ',');
                        else
                            ConceitosPermitidos = ',' + entity.chc_conceitos.Replace('|', ',');
                    }

                    if (conceitos.Find(p => p == entityAlunoHistorico.alh_avaliacao.ToString().ToUpper()) == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Valida as notas das disciplinas
        /// </summary>
        /// <param name="ACA_AlunoHistoricoBO.StructHistoricoDisciplina">Lista de disciplina.</param>
        /// <param name="alh_anoLetivo">Ano letivo.</param>
        /// <param name="listaErros">Lista de erros.</param>
        public static void ValidarDisciplinas(List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina> lt, int alh_anoLetivo, ref List<string> listaErros)
        {
            List<string> listaDisciplinaFrequenciaMax = new List<string>();
            List<string> listaDisciplinaApenasNumericos = new List<string>();
            List<string> listaDisciplinaNotaMax = new List<string>();

            foreach (StructHistoricoDisciplina disciplina in lt)
            {
                if (!string.IsNullOrEmpty(disciplina.entDisciplina.ahd_frequencia))
                {
                    decimal frequencia;

                    if (decimal.TryParse(disciplina.entDisciplina.ahd_frequencia, out frequencia))
                    {
                        if (frequencia < 0 || frequencia > 100)
                        {
                            listaDisciplinaFrequenciaMax.Add(disciplina.entDisciplina.ahd_disciplina);
                        }
                    }
                    else
                    {
                        listaDisciplinaApenasNumericos.Add(disciplina.entDisciplina.ahd_disciplina);
                    }
                }
                else
                {
                    listaDisciplinaFrequenciaMax.Add(disciplina.entDisciplina.ahd_disciplina);
                }

                if (string.IsNullOrEmpty(disciplina.entDisciplina.ahd_avaliacao) || disciplina.entDisciplina.ahd_avaliacao.Count() > 4)
                {
                    listaDisciplinaNotaMax.Add(disciplina.entDisciplina.ahd_disciplina);
                }

            }

            if (listaDisciplinaFrequenciaMax.Count > 0)
            {
                listaErros.Add(string.Format("A(s) frequência(s) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaFrequenciaMax.ToArray()) + " do ano letivo de {0} é(são) obrigatório(s) e deve(m) possuir valor(es) entre 0 e 100.", alh_anoLetivo));
            }

            if (listaDisciplinaApenasNumericos.Count > 0)
            {
                listaErros.Add(string.Format("A(s) frequência(s) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaApenasNumericos.ToArray()) + " do ano letivo de {0} deve(m) possuir apenas números.", alh_anoLetivo));
            }

            if (listaDisciplinaNotaMax.Count > 0)
            {
                listaErros.Add(string.Format("A(s) avaliação(ões) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaNotaMax.ToArray()) + " do ano letivo de {0} é(são) obrigatório(s) e deve(m) possuir no  máximo 4 caracteres. ", alh_anoLetivo));
            }
        }

        /// <summary>
        /// Valida as notas das disciplinas fora da rede
        /// </summary>
        /// <param name="ACA_AlunoHistoricoBO.StructHistoricoDisciplina">Lista de disciplina.</param>
        /// <param name="alh_anoLetivo">Ano letivo.</param>
        /// <param name="listaErros">Lista de erros.</param>
        /// <param name="banco">Banco de dados.</param>
        public static void ValidarDisciplinasDentroRede(List<ACA_AlunoHistoricoBO.StructHistoricoDisciplina> lt, int alh_anoLetivo, ref List<string> listaErros, TalkDBTransaction banco)
        {
            List<string> listaDisciplinaResultado = new List<string>();
            List<string> listaDisciplinaNota = new List<string>();
            List<string> listaDisciplinaNotaMax = new List<string>();
            List<string> listaDisciplinaNotaNaoDecimal = new List<string>();
            List<string> listaDisciplinaConceito = new List<string>();
            Decimal notaDisciplina;
            string ConceitosPermitidos = string.Empty;

            // Retorna os conceitos do histórico (Validação apenas para escolas dentro da rede)
            List<CFG_HistoricoConceito> listConceitosHistorico = CFG_HistoricoConceitoBO.RetornaListaConceitosHistorico(banco);
            List<CFG_HistoricoConceito> ltConceitosAno = listConceitosHistorico.FindAll(p => p.chc_ano == alh_anoLetivo);
            List<string> conceitos = new List<string>();

            if (ltConceitosAno != null && ltConceitosAno.Count > 0)
            {
                foreach (CFG_HistoricoConceito entity in ltConceitosAno.ToList())
                {
                    conceitos.AddRange(entity.chc_conceitos.Split('|').ToList());

                    if (string.IsNullOrEmpty(ConceitosPermitidos))
                        ConceitosPermitidos = entity.chc_conceitos.Replace('|', ',');
                    else
                        ConceitosPermitidos = ',' + entity.chc_conceitos.Replace('|', ',');
                }
            }

            foreach (StructHistoricoDisciplina disciplina in lt)
            {
                // Nota disciplina (Números 0 a 10, com precisão de 1 casa decimal. De 0,0 a 10,0).

                if (string.IsNullOrEmpty(disciplina.entDisciplina.ahd_resultado.ToString()) || disciplina.entDisciplina.ahd_resultado <= 0)
                {
                    listaDisciplinaResultado.Add(disciplina.entDisciplina.ahd_disciplina);
                }

                if (string.IsNullOrEmpty(disciplina.entDisciplina.ahd_avaliacao) || disciplina.entDisciplina.ahd_avaliacao.Count() > 4)
                {
                    listaDisciplinaNotaMax.Add(disciplina.entDisciplina.ahd_disciplina);
                }
                else
                {
                    if (Convert.ToInt32(alh_anoLetivo) >= 2000 && Convert.ToInt32(alh_anoLetivo) <= 2009)
                    {
                        if (conceitos.Count > 0)
                        {
                            if (conceitos.Find(p => p == disciplina.entDisciplina.ahd_avaliacao.ToString().ToUpper()) == null)
                            {
                                listaDisciplinaConceito.Add(disciplina.entDisciplina.ahd_disciplina);
                            }
                        }

                    }
                    else if (Convert.ToInt32(alh_anoLetivo) >= 2010)
                    {
                        if (!Decimal.TryParse(disciplina.entDisciplina.ahd_avaliacao, out notaDisciplina))
                        {
                            listaDisciplinaNotaNaoDecimal.Add(disciplina.entDisciplina.ahd_disciplina);
                        }
                        else
                        {
                            // Verifica se informou apenas 1 casa decimal e se o intervalo da nota está entre 0 a 10).
                            if ((Convert.ToInt32(notaDisciplina) != notaDisciplina && notaDisciplina != Decimal.Round(notaDisciplina, 1)) || (notaDisciplina < 0 || notaDisciplina > 10))
                            {
                                listaDisciplinaNota.Add(disciplina.entDisciplina.ahd_disciplina);
                            }
                        }
                    }
                }
            }

            if (listaDisciplinaNotaNaoDecimal.Count > 0)
            {
                listaErros.Add(string.Format("A(s) avaliação(ões) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaNotaNaoDecimal.ToArray()) + " do ano letivo de {0} deve(m) possuir apenas números. ", alh_anoLetivo));
            }

            if (listaDisciplinaNota.Count > 0)
            {
                listaErros.Add(string.Format("A(s) avaliação(ões) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaNota.ToArray()) + " do ano letivo de {0} deve(m) possuir deve possuir 1 casa decimal e intervalo entre 0,0 a 10,0.", alh_anoLetivo));
            }

            if (listaDisciplinaResultado.Count > 0)
            {
                listaErros.Add(string.Format("O resultado é obrigatório no(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaResultado.ToArray()) + " do ano letivo {0}.", alh_anoLetivo));
            }

            if (listaDisciplinaConceito.Count > 0)
            {
                listaErros.Add(String.Format("A(s) avaliação(ões) do(a)(s) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " " + string.Join(",", listaDisciplinaConceito.ToArray()) + " é inválido para ano de {0}. Os conceitos permitidos para este ano letivo são: {1}.",
                                                   alh_anoLetivo, ConceitosPermitidos));
            }
        }


        /// <summary>
        /// Valida os históricos do aluno
        /// </summary>
        /// <param name="ltAlunoHistorico">Lista dos históricos.</param>
        /// <param name="banco">Banco de dados.</param>
        /// <returns></returns>
        public static void ValidarHistoricos(List<StructHistorico> ltAlunoHistorico, TalkDBTransaction banco)
        {
            List<string> listaErros = new List<string>();
            foreach (StructHistorico historico in ltAlunoHistorico)
            {
                ACA_AlunoHistorico entityAlunoHistorico = historico.entHistorico;

                if (entityAlunoHistorico.mtu_id <= 0)
                {
                    ValidaGlobal(entityAlunoHistorico, ref listaErros);

                    ValidarDisciplinas(historico.ltDisciplina, entityAlunoHistorico.alh_anoLetivo, ref listaErros);

                    // Escolas dentro da rede.
                    if (entityAlunoHistorico.esc_id > 0 && entityAlunoHistorico.uni_id > 0)
                    {
                        ValidaGlobalDentroRede(entityAlunoHistorico, ref listaErros, banco);
                        ValidarDisciplinasDentroRede(historico.ltDisciplina, entityAlunoHistorico.alh_anoLetivo, ref listaErros, banco);
                    }
                }

            }

            if (listaErros.Count > 0)
            {
                throw new ValidationException(string.Join("<BR/>", listaErros.ToArray()));
            }
        }


        #endregion

        #region Métodos de alteração e inclusão

        /// <summary>
        /// Salva os dados do histórico do aluno.
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoHistorico</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_AlunoHistorico entity
            , TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ACA_AlunoHistorico_ValidationException(UtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva históricos do aluno.
        /// </summary>
        /// <param name="entityAluno">Entidade ACA_Aluno</param>
        /// <param name="ltAlunoHistorico">List contendo estruturas dos históricos do aluno</param>
        /// <param name="salvar_sempre_maiusculo"></param>
        /// <param name="bancoCore">Transação com banco CoreSSO</param>
        /// <param name="bancoGestao">Transação com banco GestaoEscolar</param>
        /// <param name="deletarHistoricos">Flag que indica se o método irá excluir históricos que não estiveren na lista ltAlunoHistorico</param>
        /// <returns></returns>
        public static void SalvarHistoricosAluno
            (
                ACA_Aluno entityAluno
                , List<StructHistorico> ltAlunoHistorico
                , bool salvar_sempre_maiusculo
                , TalkDBTransaction bancoCore
                , TalkDBTransaction bancoGestao
                , bool deletarHistoricos = true
            )
        {
            SalvarHistoricosAluno(entityAluno.alu_id, ltAlunoHistorico, salvar_sempre_maiusculo, bancoCore, bancoGestao, deletarHistoricos);
        }

        /// <summary>
        /// Salva históricos do aluno.
        /// </summary>
        /// <param name="alu_id">Id do ACA_Aluno</param>
        /// <param name="ltAlunoHistorico">List contendo estruturas dos históricos do aluno</param>
        /// <param name="salvar_sempre_maiusculo"></param>
        /// <param name="bancoCore">Transação com banco CoreSSO</param>
        /// <param name="bancoGestao">Transação com banco GestaoEscolar</param>
        /// <param name="deletarHistoricos">Flag que indica se o método irá excluir históricos que não estiveren na lista ltAlunoHistorico</param>
        /// <returns></returns>
        public static void SalvarHistoricosAluno
            (
                Int64 alu_id
                , List<StructHistorico> ltAlunoHistorico
                , bool salvar_sempre_maiusculo
                , TalkDBTransaction bancoCore
                , TalkDBTransaction bancoGestao
                , bool deletarHistoricos
            )
        {

            // Retorna todas as disciplinas cadastradas
            List<ACA_AlunoHistoricoDisciplina> listAlunoHistoricosDisciplinasCadastrados = ACA_AlunoHistoricoDisciplinaBO.RetornaListaHistoricoDisciplinasAluno(alu_id, bancoGestao);

            // Salva históricos do aluno
            foreach (StructHistorico historico in ltAlunoHistorico)
            {
                ACA_AlunoHistorico entityAlunoHistorico = historico.entHistorico;
                entityAlunoHistorico.alu_id = alu_id;
                entityAlunoHistorico.alh_situacao = 1;

                if (salvar_sempre_maiusculo)
                {
                    entityAlunoHistorico.alh_avaliacao = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoHistorico.alh_avaliacao);
                }

                // Salva escola de origem, caso necessário
                ACA_AlunoEscolaOrigem entityAlunoEscolaOrigem = historico.entEscolaOrigem;
                if ((entityAlunoEscolaOrigem.eco_id <= 0) &&
                    (!string.IsNullOrEmpty(entityAlunoEscolaOrigem.eco_nome)) &&
                    (historico.entHistorico.esc_id <= 0))
                {
                    entityAlunoEscolaOrigem.eco_situacao = 1;
                    entityAlunoEscolaOrigem.IsNew = true;

                    if (salvar_sempre_maiusculo)
                    {
                        entityAlunoEscolaOrigem.eco_nome = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_nome);
                        entityAlunoEscolaOrigem.eco_codigoInep = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_codigoInep);
                        entityAlunoEscolaOrigem.eco_numero = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_numero);
                        entityAlunoEscolaOrigem.eco_complemento = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_complemento);
                    }

                    ACA_AlunoEscolaOrigemBO.Save(entityAlunoEscolaOrigem, bancoGestao);
                }
                entityAlunoHistorico.eco_id = entityAlunoEscolaOrigem.eco_id;


                // Retirar da frequência o caracter "%"
                if (!string.IsNullOrEmpty(entityAlunoHistorico.alh_frequencia))
                    entityAlunoHistorico.alh_frequencia = entityAlunoHistorico.alh_frequencia.Replace("%", "");

                Save(entityAlunoHistorico, bancoGestao);


                // Salva disciplinas do histórico do aluno
                foreach (StructHistoricoDisciplina disciplina in historico.ltDisciplina)
                {
                    if (disciplina.entDisciplina.ahd_disciplina == "GLB")
                    {
                        entityAlunoHistorico.alh_frequencia = disciplina.entDisciplina.ahd_frequencia;
                    }
                    else
                    {
                        ACA_AlunoHistoricoDisciplina entityAlunoHistoricoDisciplina = disciplina.entDisciplina;
                        entityAlunoHistoricoDisciplina.alu_id = alu_id;
                        entityAlunoHistoricoDisciplina.alh_id = entityAlunoHistorico.alh_id;
                        entityAlunoHistoricoDisciplina.tds_id = string.IsNullOrEmpty(disciplina.tds_id) ? -1 : Convert.ToInt32(disciplina.tds_id);
                        entityAlunoHistoricoDisciplina.ahd_situacao = 1;
                        entityAlunoHistoricoDisciplina.IsNew = entityAlunoHistoricoDisciplina.ahd_id > 0 ? false : true;

                        // Retirar da frequência o caracter "%"
                        if (!string.IsNullOrEmpty(entityAlunoHistoricoDisciplina.ahd_frequencia))
                            entityAlunoHistoricoDisciplina.ahd_frequencia = entityAlunoHistoricoDisciplina.ahd_frequencia.Replace("%", "");


                        if (salvar_sempre_maiusculo)
                        {
                            entityAlunoHistoricoDisciplina.ahd_avaliacao = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoHistoricoDisciplina.ahd_avaliacao);
                        }

                        ACA_AlunoHistoricoDisciplinaBO.Save(entityAlunoHistoricoDisciplina, bancoGestao);

                    }
                }

                if (deletarHistoricos)
                {
                    List<ACA_AlunoHistoricoDisciplina> listaDisciplinasCadastradas = listAlunoHistoricosDisciplinasCadastrados.FindAll(p => p.alh_id == historico.entHistorico.alh_id);

                    foreach (ACA_AlunoHistoricoDisciplina entCadastrada in listaDisciplinasCadastradas)
                    {
                        ACA_AlunoHistoricoDisciplina cadastrada = entCadastrada;
                        if (!historico.ltDisciplina.Exists
                            (p =>
                                (p.entDisciplina.alh_id == cadastrada.alh_id &&
                                p.entDisciplina.ahd_id == cadastrada.ahd_id)))
                        {
                            ACA_AlunoHistoricoDisciplinaBO.Delete(cadastrada, bancoGestao);
                        }
                    }
                }
            }

            if (deletarHistoricos)
            {
                List<ACA_AlunoHistorico> listAlunoHistoricosCadastrados = RetornaListaHistoricosAluno(alu_id, bancoGestao);

                // Exclui os históricos.
                foreach (ACA_AlunoHistorico entCadastrada in listAlunoHistoricosCadastrados)
                {
                    ACA_AlunoHistorico cadastrada = entCadastrada;
                    if (!ltAlunoHistorico.Exists
                        (p => p.entHistorico.alh_id == cadastrada.alh_id))
                    {
                        Delete(cadastrada, bancoGestao);
                    }
                }
            }
        }

        /// <summary>
        /// Salva históricos do aluno sem excluir os históricos que não estão na lista
        /// </summary>
        /// <param name="alu_id">Id do ACA_Aluno</param>
        /// <param name="ltAlunoHistorico">List contendo estruturas dos históricos do aluno</param>
        /// <param name="salvar_sempre_maiusculo"></param>
        /// <param name="bancoCore">Transação com banco CoreSSO</param>
        /// <param name="bancoGestao">Transação com banco GestaoEscolar</param>
        /// <returns></returns>
        public static void AdicionarSalvarHistoricosAluno
            (
                Int64 alu_id
                , List<StructHistorico> ltAlunoHistorico
                , bool salvar_sempre_maiusculo
                , TalkDBTransaction bancoCore
                , TalkDBTransaction bancoGestao
            )
        {
            // Salva históricos do aluno
            foreach (StructHistorico historico in ltAlunoHistorico)
            {
                ACA_AlunoHistorico entityAlunoHistorico = historico.entHistorico;
                entityAlunoHistorico.alu_id = alu_id;
                entityAlunoHistorico.alh_situacao = 1;

                if (salvar_sempre_maiusculo)
                {
                    entityAlunoHistorico.alh_avaliacao = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoHistorico.alh_avaliacao);
                }

                // Salva escola de origem, caso necessário
                ACA_AlunoEscolaOrigem entityAlunoEscolaOrigem = historico.entEscolaOrigem;
                if ((entityAlunoEscolaOrigem.eco_id <= 0) &&
                    (!string.IsNullOrEmpty(entityAlunoEscolaOrigem.eco_nome)) &&
                    (historico.entHistorico.esc_id <= 0))
                {
                    entityAlunoEscolaOrigem.eco_situacao = 1;
                    entityAlunoEscolaOrigem.IsNew = true;

                    if (salvar_sempre_maiusculo)
                    {
                        entityAlunoEscolaOrigem.eco_nome = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_nome);
                        entityAlunoEscolaOrigem.eco_codigoInep = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_codigoInep);
                        entityAlunoEscolaOrigem.eco_numero = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_numero);
                        entityAlunoEscolaOrigem.eco_complemento = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoEscolaOrigem.eco_complemento);
                    }

                    ACA_AlunoEscolaOrigemBO.Save(entityAlunoEscolaOrigem, bancoGestao);
                }
                entityAlunoHistorico.eco_id = entityAlunoEscolaOrigem.eco_id;


                // Retirar da frequência o caracter "%"
                if (!string.IsNullOrEmpty(entityAlunoHistorico.alh_frequencia))
                    entityAlunoHistorico.alh_frequencia = entityAlunoHistorico.alh_frequencia.Replace("%", "");

                Save(entityAlunoHistorico, bancoGestao);

                //Desabilita disciplinas excluidas
                DataTable disciplinasBanco = new DataTable();
                ACA_AlunoHistoricoDAO dal = new ACA_AlunoHistoricoDAO();
                disciplinasBanco = dal.ComparaDisciplinas(alu_id);

                //Exclui as disciplina que foram excluidas.

                // Salva disciplinas do histórico do aluno
                foreach (StructHistoricoDisciplina disciplina in historico.ltDisciplina)
                {
                    if (disciplina.entDisciplina.ahd_disciplina == "GLB")
                    {
                        entityAlunoHistorico.alh_frequencia = disciplina.entDisciplina.ahd_frequencia;
                    }
                    else
                    {
                        ACA_AlunoHistoricoDisciplina entityAlunoHistoricoDisciplina = disciplina.entDisciplina;
                        entityAlunoHistoricoDisciplina.alu_id = alu_id;
                        entityAlunoHistoricoDisciplina.alh_id = entityAlunoHistorico.alh_id;
                        entityAlunoHistoricoDisciplina.tds_id = string.IsNullOrEmpty(disciplina.tds_id) ? -1 : Convert.ToInt32(disciplina.tds_id);
                        entityAlunoHistoricoDisciplina.ahd_situacao = 1;
                        entityAlunoHistoricoDisciplina.IsNew = entityAlunoHistoricoDisciplina.ahd_id > 0 ? false : true;

                        // Retirar da frequência o caracter "%"
                        if (!string.IsNullOrEmpty(entityAlunoHistoricoDisciplina.ahd_frequencia))
                            entityAlunoHistoricoDisciplina.ahd_frequencia = entityAlunoHistoricoDisciplina.ahd_frequencia.Replace("%", "");


                        if (salvar_sempre_maiusculo)
                        {
                            entityAlunoHistoricoDisciplina.ahd_avaliacao = GestaoEscolarUtilBO.ConverterParaMaiusculo(entityAlunoHistoricoDisciplina.ahd_avaliacao);
                        }

                        ACA_AlunoHistoricoDisciplinaBO.Save(entityAlunoHistoricoDisciplina, bancoGestao);
                    }
                }

                List<DataRow> disciplinasDeletadas = new List<DataRow>();

                //Exclui as disciplina que foram excluidas.
                foreach (DataRow row in disciplinasBanco.Rows)
                {
                    if (Convert.ToInt32(row["alh_id"]) == entityAlunoHistorico.alh_id)
                    {
                        bool exclui = true;
                        // Salva disciplinas do histórico do aluno
                        foreach (StructHistoricoDisciplina disciplina in historico.ltDisciplina)
                        {
                            if (Convert.ToInt32(row["ahd_id"]) == disciplina.entDisciplina.ahd_id && Convert.ToInt32(row["alh_id"]) == disciplina.entDisciplina.alh_id)
                            {
                                exclui = false;
                            }
                        }
                        if (exclui)
                        {
                            disciplinasDeletadas.Add(row);
                        }
                    }
                }

                foreach (DataRow row in disciplinasDeletadas)
                {
                    dal.RetiraDisciplinas(alu_id, Convert.ToInt32(row["ahd_id"]), Convert.ToInt32(row["alh_id"]));
                }
            }
        }

        /// <summary>
        /// Salva o histórico do aluno e as disciplinas relacionadas.
        /// </summary>
        /// <param name="entityHistorico"></param>
        /// <param name="lsHistoricoDisciplina"></param>
        /// <param name="bancoGestao"></param>
        public static void SalvarHistoricoAluno(ACA_AlunoHistorico entityHistorico, List<ACA_AlunoHistoricoDisciplina> lsHistoricoDisciplina, bool gerarHistoricoTodasDisciplinas, TalkDBTransaction bancoGestao)
        {
            if (!entityHistorico.Validate())
            {
                throw new ValidationException(UtilBO.ErrosValidacao(entityHistorico));
            }

            Save(entityHistorico, bancoGestao);

            MTR_MatriculaTurma entityMatriculaTurma = new MTR_MatriculaTurma
            {
                alu_id = entityHistorico.alu_id,
                mtu_id = entityHistorico.mtu_id
            };
            MTR_MatriculaTurmaBO.GetEntity(entityMatriculaTurma, bancoGestao);

            ACA_FormatoAvaliacao entityFormato = TUR_TurmaBO.SelecionaFormatoAvaliacao(entityMatriculaTurma.tur_id, bancoGestao);

            ACA_Curso entityCurso = new ACA_Curso { cur_id = entityHistorico.cur_id };
            ACA_CursoBO.GetEntity(entityCurso);

            // Se o curso é efetivação semestral não válida se o histórico possui nota.
            if (entityFormato.fav_tipo != (byte)ACA_FormatoAvaliacaoTipo.ConceitoGlobal)
            {
                List<ACA_AlunoHistoricoDisciplina> lsHistoricoDisciplinaComNota = lsHistoricoDisciplina.Where(p => (!string.IsNullOrEmpty(p.ahd_avaliacao) || entityCurso.cur_efetivacaoSemestral || gerarHistoricoTodasDisciplinas)).ToList();

                foreach (ACA_AlunoHistoricoDisciplina entityHistDisciplina in lsHistoricoDisciplinaComNota)
                {
                    entityHistDisciplina.alh_id = entityHistorico.alh_id;

                    ACA_AlunoHistoricoDisciplinaBO.Save(entityHistDisciplina, bancoGestao);
                }
            }
        }

        public static void SalvarInclusaoAlteracaoHistoricoAluno
        (
            Int64 alu_id,
            List<ACA_AlunoHistoricoBO.StructHistorico> ltHistorico,
            List<ACA_AlunoHistoricoObservacao> ltHistoricoObservacao,
            bool salvar_sempre_maiusculo
        )
        {
            TalkDBTransaction bancoGestao = new ACA_AlunoDAO()._Banco.CopyThisInstance();
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            TalkDBTransaction bancoCore = new PES_PessoaDAO()._Banco.CopyThisInstance();
            bancoCore.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Cria uma lista com os ids de observação antigos.
                List<int> comparacao = new List<int>();
                foreach (ACA_AlunoHistoricoObservacao historicoObservacao in ltHistoricoObservacao)
                {
                    comparacao.Add(historicoObservacao.aho_id);
                }

                // Salva os dados dos históricos de observação do aluno.
                ACA_AlunoHistoricoObservacaoBO.AdicionarSalvarHistoricosObservacaoAluno
                (
                    alu_id
                    , ltHistoricoObservacao
                    , salvar_sempre_maiusculo
                    , bancoCore
                    , bancoGestao
                );

                // A partir dos novos ids de observação, atualiza a lista de históricos que estão com os ids de observação anteriores.
                for (int indiceObservacoes = 0; indiceObservacoes < ltHistoricoObservacao.Count; indiceObservacoes++)
                {
                    List<ACA_AlunoHistoricoBO.StructHistorico> historicosAtualizados = ltHistorico.Where(p => p.entHistorico.aho_id == comparacao[indiceObservacoes]).ToList();
                    for (int indiceHistoricos = 0; indiceHistoricos < historicosAtualizados.Count; indiceHistoricos++)
                    {
                        ACA_AlunoHistoricoBO.StructHistorico historico = historicosAtualizados[indiceHistoricos];
                        ltHistorico.Remove(historico);

                        historico.entHistorico.aho_id = ltHistoricoObservacao[indiceObservacoes].aho_id;
                        ltHistorico.Add(historico);
                    }
                }

                // Salva os dados dos históricos do aluno.
                ACA_AlunoHistoricoBO.AdicionarSalvarHistoricosAluno
                (
                    alu_id
                    , ltHistorico
                    , salvar_sempre_maiusculo
                    , bancoCore
                    , bancoGestao
                );
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);
                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }
        }

        /// <summary>
        /// Atualiza as notas e as frequência do histórico global e das disciplinas de um aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="anoLetivoHistorico">Ano do histórico.</param>
        /// <param name="banco">Transação.</param>
        /// <param name="mtu_resultado">Resultado final global do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public static bool CalcularNotaFrequenciaHistorico(long alu_id, int mtu_id, int anoLetivoHistorico, TalkDBTransaction banco, byte mtu_resultado, bool permiteAlterarResultado)
        {
            ACA_AlunoHistoricoDAO dao = new ACA_AlunoHistoricoDAO { _Banco = banco };
            return dao.CalcularNotaFrequenciaHistorico(alu_id, mtu_id, anoLetivoHistorico, mtu_resultado, permiteAlterarResultado);
        }

        /// <summary>
        /// Gera/atualiza o histórico por aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <returns></returns>
        public static bool GeracaoHistoricoPedagogicoPorAluno(long alu_id, int mtu_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_AlunoHistoricoDAO().GeracaoHistoricoPedagogicoPorAluno(alu_id, mtu_id) :
                new ACA_AlunoHistoricoDAO { _Banco = banco }.GeracaoHistoricoPedagogicoPorAluno(alu_id, mtu_id);
        }

        #endregion Métodos de alteração e inclusão

        #region Métodos em lote

        /// <summary>
        /// O método retorna um DataRow com as informações da entidade.
        /// </summary>
        /// <param name="entity">Entidade de historico do aluno.</param>
        /// <param name="dr">DataRow de historico.</param>
        /// <returns></returns>
        public static DataRow EntityToDataRow(ACA_AlunoHistorico entity, DataRow dr)
        {
            dr["alu_id"] = entity.alu_id;
            dr["alh_id"] = entity.alh_id;

            if (entity.eco_id > 0)
                dr["eco_id"] = entity.eco_id;
            else
                dr["eco_id"] = DBNull.Value;

            if (entity.mtu_id > 0)
                dr["mtu_id"] = entity.mtu_id;
            else
                dr["mtu_id"] = DBNull.Value;

            dr["alh_anoLetivo"] = entity.alh_anoLetivo;

            if (entity.alh_resultado > 0)
                dr["alh_resultado"] = entity.alh_resultado;
            else
                dr["alh_resultado"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.alh_resultadoDescricao))
                dr["alh_resultadoDescricao"] = entity.alh_resultadoDescricao;
            else
                dr["alh_resultadoDescricao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.alh_avaliacao))
                dr["alh_avaliacao"] = entity.alh_avaliacao;
            else
                dr["alh_avaliacao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.alh_frequencia))
                dr["alh_frequencia"] = entity.alh_frequencia;
            else
                dr["alh_frequencia"] = DBNull.Value;

            dr["alh_situacao"] = entity.alh_situacao;

            if (entity.cur_id > 0)
                dr["cur_id"] = entity.cur_id;
            else
                dr["cur_id"] = DBNull.Value;

            if (entity.crr_id > 0)
                dr["crr_id"] = entity.crr_id;
            else
                dr["crr_id"] = DBNull.Value;

            if (entity.crp_id > 0)
                dr["crp_id"] = entity.crp_id;
            else
                dr["crp_id"] = DBNull.Value;

            if (entity.esc_id > 0)
                dr["esc_id"] = entity.esc_id;
            else
                dr["esc_id"] = DBNull.Value;

            if (entity.uni_id > 0)
                dr["uni_id"] = entity.uni_id;
            else
                dr["uni_id"] = DBNull.Value;

            if (entity.alh_qtdeFaltas > 0)
                dr["alh_qtdeFaltas"] = entity.alh_qtdeFaltas;
            else
                dr["alh_qtdeFaltas"] = DBNull.Value;

            if (entity.alh_tipoControleNotas > 0)
                dr["alh_tipoControleNotas"] = entity.alh_tipoControleNotas;
            else
                dr["alh_tipoControleNotas"] = DBNull.Value;

            if (entity.aho_id > 0)
                dr["aho_id"] = entity.aho_id;
            else
                dr["aho_id"] = DBNull.Value;

            if (entity.alh_cargaHorariaBaseNacional > 0)
                dr["alh_cargaHorariaBaseNacional"] = entity.alh_cargaHorariaBaseNacional;
            else
                dr["alh_cargaHorariaBaseNacional"] = DBNull.Value;

            if (entity.alh_cargaHorariaBaseDiversificada > 0)
                dr["alh_cargaHorariaBaseDiversificada"] = entity.alh_cargaHorariaBaseDiversificada;
            else
                dr["alh_cargaHorariaBaseDiversificada"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.alh_descricaoProximoPeriodo))
                dr["alh_descricaoProximoPeriodo"] = entity.alh_descricaoProximoPeriodo;
            else
                dr["alh_descricaoProximoPeriodo"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Salva os dados do historico em lote.
        /// </summary>
        /// <param name="dtAlunoHistorico">Datatable com os dados do historico.</param>
        /// <param name="dtAlunoHistoricoDisciplina">Datatable com os dados do historico por disciplina.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarEmLote(DataTable dtAlunoHistorico, DataTable dtAlunoHistoricoDisciplina, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_AlunoHistoricoDAO().SalvarEmLote(dtAlunoHistorico, dtAlunoHistoricoDisciplina) :
                new ACA_AlunoHistoricoDAO { _Banco = banco }.SalvarEmLote(dtAlunoHistorico, dtAlunoHistoricoDisciplina);
        }

        #endregion Métodos em lote
    }
}

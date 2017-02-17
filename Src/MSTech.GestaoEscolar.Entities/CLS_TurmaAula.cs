/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using System.Data;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CLS_TurmaAula : Abstract_CLS_TurmaAula
    {
        [DataObjectField(true, false, false)]
        public override int tau_id { get; set; }

        public override int tau_sequencia { get; set; }

        [MSNotNullOrEmpty("Quantidade de tempo de aula é obrigatório e deve ser um número inteiro maior que 0 (zero).")]
        public override int tau_numeroAulas { get; set; }

        [MSDefaultValue(1)]
        public override byte tau_situacao { get; set; }

        public override DateTime tau_dataCriacao { get; set; }

        public override DateTime tau_dataAlteracao { get; set; }

        [MSNotNullOrEmpty("Posição do docente é obrigatório.")]
        public override byte tdt_posicao { get; set; }

        [MSNotNullOrEmpty]
        [MSDefaultValue(0)]
        public override bool tau_reposicao { get; set; }

        //Variável não presente na tabela CLS_TurmaAula
        //Utilizada no cadastro de aula da disciplina da turma
        public virtual long tur_id { get; set; }

        /// <summary>
        /// Variável auxiliar.
        /// </summary>
        public byte tud_tipo { get; set; }

        /// <summary>
        /// utilizado para guardar o nome do usuário que realizou a última alteração no dados
        /// </summary>
        public virtual string nomeUsuAlteracao { get; set; }

        /// <summary>
        /// Status da frequencia da aula: 1-Não Prenechida; 2-Prenechida; 3-Efetivada
        /// </summary>
        [MSNotNullOrEmpty("Status da frequencia da aula é obrigatório.")]
        [MSDefaultValue(1)]
        public override byte tau_statusFrequencia { get; set; }

        /// <summary>
        /// Status das atividade avaliativas da aula: 1-Não Prenechida; 2-Prenechida; 3-Efetivada
        /// </summary>
        [MSNotNullOrEmpty("Status das atividade avaliativas da aula é obrigatório.")]
        [MSDefaultValue(1)]
        public override byte tau_statusAtividadeAvaliativa { get; set; }

        /// <summary>
        /// Status das anotações da aula: 1-Não Prenechida; 2-Prenechida
        /// </summary>
        [MSNotNullOrEmpty("Status das anotações da aula é obrigatório.")]
        [MSDefaultValue(1)]
        public override byte tau_statusAnotacoes { get; set; }

        /// <summary>
        /// Status do plano da aula: 1-Não Prenechida; 2-Prenechida; 3-Incompleto
        /// </summary>
        [MSNotNullOrEmpty("Status do plano da aula é obrigatório.")]
        [MSDefaultValue(1)]
        public override byte tau_statusPlanoAula { get; set; }

        /// <summary>
        /// Propriedade tau_checadoAtividadeCasa.
        /// </summary>
        [MSDefaultValue(false)]
        public override bool tau_checadoAtividadeCasa { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, informa se é a mesma aula do protocolo ou se foi carregada pelo dia.
        /// </summary>
        public bool mesmaAula { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, permissão de edição da aula pelo usuário.
        /// </summary>
        public bool permiteEdicaoAula { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, permissão de edição da aula pelo usuário.
        /// </summary>
        public bool permiteFrequencia { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, permissão de edição da aula pelo usuário.
        /// </summary>
        public bool permiteAvaliacao { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, permissão de edição da aula pelo usuário.
        /// </summary>
        public bool permiteAnotacao { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, permissão de edição da aula pelo usuário.
        /// </summary>
        public bool permitePlanoAula { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, ultima data do log de alteração de aula.
        /// </summary>
        public DateTime dataLogAlteracaoAula { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, ultima data do log de alteração de plano de aula.
        /// </summary>
        public DateTime dataLogAlteracaoPlanoAula { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, ultima data do log de alteração de frequência.
        /// </summary>
        public DateTime dataLogAlteracaoFreq { get; set; }

        /// <summary>
        /// Variável auxiliar para a importação de aulas, ultima data do log de alteração de anotação.
        /// </summary>
        public DateTime dataLogAlteracaoAnotacao { get; set; }

        public static DataTable TipoTabela_TurmaAulaBusca()
        {
            DataTable dtTurmaAulaBusca = new DataTable();
            dtTurmaAulaBusca.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAulaBusca.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAulaBusca.Columns.Add("pro_id", typeof(Guid));
            dtTurmaAulaBusca.Columns.Add("tau_data", typeof(DateTime));
            dtTurmaAulaBusca.Columns.Add("tdt_posicao", typeof(Byte));
            //adicionado pro_protocolo para validação das aulas geradas no tablet
            dtTurmaAulaBusca.Columns.Add("pro_protocolo", typeof(Int64));
            dtTurmaAulaBusca.Columns.Add("usu_id", typeof(Guid));
            return dtTurmaAulaBusca;
        }

        public static DataTable TipoTabela_TurmaAula(bool criarIdAula = true)
        {
            DataTable dtTurmaAula = new DataTable();
            if (criarIdAula)
            {
                DataColumn dcId = dtTurmaAula.Columns.Add("idAula", typeof(Int64));
                dcId.AutoIncrement = true;
                dcId.AutoIncrementSeed = 1;
                dcId.AutoIncrementStep = 1;
            }
            else
            {
                dtTurmaAula.Columns.Add("idAula", typeof(Int64));
            }
            dtTurmaAula.Columns.Add("tud_id", typeof(Int64));
            dtTurmaAula.Columns.Add("tau_id", typeof(Int32));
            dtTurmaAula.Columns.Add("tpc_id", typeof(Int32));
            dtTurmaAula.Columns.Add("tau_sequencia", typeof(Int32));
            dtTurmaAula.Columns.Add("tau_data", typeof(DateTime));
            dtTurmaAula.Columns.Add("tau_numeroAulas", typeof(Int32));
            dtTurmaAula.Columns.Add("tau_planoAula", typeof(String));
            dtTurmaAula.Columns.Add("tau_diarioClasse", typeof(String));
            dtTurmaAula.Columns.Add("tau_conteudo", typeof(String));
            dtTurmaAula.Columns.Add("tau_efetivado", typeof(Boolean));
            dtTurmaAula.Columns.Add("tau_atividadeCasa", typeof(String));
            dtTurmaAula.Columns.Add("tau_situacao", typeof(Byte));
            dtTurmaAula.Columns.Add("tdt_posicao", typeof(Byte));
            dtTurmaAula.Columns.Add("pro_id", typeof(Guid));
            dtTurmaAula.Columns.Add("tau_sintese", typeof(String));
            dtTurmaAula.Columns.Add("tau_reposicao", typeof(Boolean));
            dtTurmaAula.Columns.Add("usu_id", typeof(Guid));
            dtTurmaAula.Columns.Add("tau_dataAlteracao", typeof(DateTime));            
            dtTurmaAula.Columns.Add("usu_idDocenteAlteracao", typeof(Guid));
            dtTurmaAula.Columns.Add("tau_statusFrequencia", typeof(Byte));
            dtTurmaAula.Columns.Add("tau_statusAtividadeAvaliativa", typeof(Byte));
            dtTurmaAula.Columns.Add("tau_statusAnotacoes", typeof(Byte));
            dtTurmaAula.Columns.Add("tau_statusPlanoAula", typeof(Byte));
            dtTurmaAula.Columns.Add("tau_checadoAtividadeCasa", typeof(Boolean));
            dtTurmaAula.Columns.Add("tau_dataUltimaSincronizacao", typeof(DateTime));
            return dtTurmaAula;
        }

        public int tpc_ordem { get; set; }
    }
}
/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
	public class TUR_TurmaDisciplina : AbstractTUR_TurmaDisciplina
	{
        [DataObjectField(true, true, false)]
        public override Int64 tud_id { get; set; }
        [MSValidRange(30)]
        [MSNotNullOrEmpty("Código é obrigatório.")]
        public override string tud_codigo { get; set; }
        [MSValidRange(200)]
        [MSNotNullOrEmpty("Nome é obrigatório.")]
        public override string tud_nome { get; set; }
        [MSNotNullOrEmpty("Multiseriado é obrigatório.")]
        public override bool tud_multiseriado { get; set; }
        [MSNotNullOrEmpty("Modo é obrigatório.")]
        public override byte tud_modo { get; set; }        
        [MSNotNullOrEmpty("Tipo é obrigatório.")]
        public override byte tud_tipo { get; set; }        
        [MSDefaultValue(1)]
        public override byte tud_situacao { get; set; }
        public override DateTime tud_dataCriacao { get; set; }
        public override DateTime tud_dataAlteracao { get; set; }

        // Campo auxiliar para replicar o planejamento anual para outras turmas
        // ID do tipo de disciplina.
        public int tds_id { get; set; }
        
        public override int tud_vagas { get; set; }
        public override int tud_minimoMatriculados { get; set; }
        public override byte tud_duracao { get; set; }

        public override DateTime tud_dataInicio { get; set; }
        public override DateTime tud_dataFim { get; set; }
        public override int tud_cargaHorariaSemanal { get; set; }
        public override bool tud_aulaForaPeriodoNormal { get; set; }
        public override bool tud_global { get; set; }
        [MSDefaultValue(0)]
        public override bool tud_disciplinaEspecial { get; set; }

        // Campo auxiliar para verificar se deve atualizar os campos de configuracao 
        // (tud_naoLancar.. e tud_naoExibir..) no update
        [MSDefaultValue(true)]
        public bool alterarConfiguracoes { get; set; }
    }

    /// <summary>
    /// Estrutura para salvar as entidades de turmaDisciplina.
    /// </summary>
    [Serializable]
    public class CadastroTurmaDisciplina
    {
        public TUR_TurmaDisciplina entTurmaDisciplina;
        public TUR_TurmaDocente entTurmaDocente;

        public TUR_TurmaDisciplinaRelDisciplina entTurmaDiscRelDisciplina
        {
            get
            {
                if (_listaEntTurmaDiscRelDisciplina == null)
                    return new TUR_TurmaDisciplinaRelDisciplina();
                else
                    return _listaEntTurmaDiscRelDisciplina[0];
            }
            set
            {
                if (_listaEntTurmaDiscRelDisciplina == null)
                    _listaEntTurmaDiscRelDisciplina = new List<TUR_TurmaDisciplinaRelDisciplina>();
                if (_listaEntTurmaDiscRelDisciplina.Count == 0)
                    _listaEntTurmaDiscRelDisciplina.Add(new TUR_TurmaDisciplinaRelDisciplina());
                _listaEntTurmaDiscRelDisciplina[0] = value;
            }
        }
        private List<TUR_TurmaDisciplinaRelDisciplina> _listaEntTurmaDiscRelDisciplina;
        public List<TUR_TurmaDisciplinaRelDisciplina> listaEntTurmaDiscRelDisciplina
        {
            get
            {
                return _listaEntTurmaDiscRelDisciplina ??
                    new List<TUR_TurmaDisciplinaRelDisciplina>();
            }
            set { _listaEntTurmaDiscRelDisciplina = value; }
        }

        private List<TUR_Turma_Docentes_Disciplina> _listaTurmaDocente;
        public List<TUR_Turma_Docentes_Disciplina> listaTurmaDocente
        {
            get
            {
                return _listaTurmaDocente ??
                    new List<TUR_Turma_Docentes_Disciplina>();
            }
            set { _listaTurmaDocente = value; }
        }

        public List<TUR_TurmaDisciplinaCalendario> entTurmaCalendario;
        private List<TUR_TurmaDisciplinaNaoAvaliado> _listaAvaliacoesNaoAvaliar;
        public List<TUR_TurmaDisciplinaNaoAvaliado> listaAvaliacoesNaoAvaliar
        {
            get
            {
                return _listaAvaliacoesNaoAvaliar ??
                    new List<TUR_TurmaDisciplinaNaoAvaliado>();
            }
            set { _listaAvaliacoesNaoAvaliar = value; }
        }

        /// <summary>
        /// ID da disciplina eletiva.
        /// </summary>
        public int cde_id;
    }

}
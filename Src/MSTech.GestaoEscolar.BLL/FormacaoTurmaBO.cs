using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.GestaoEscolar.DAL;
using MSTech.Validation.Exceptions;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// FormacaoTurma Business Object
    /// </summary>
    public class FormacaoTurmaBO
    {
        #region Estrutura

        /// <summary>
        /// Listas carregadas para o fechamento de matrícula ser realizado.
        /// </summary>
        [Serializable]
        public class ListasFechamentoMatricula
        {
            public string alu_id;
            public string cur_id;
            public string esc_id;
            public string tur_id;
            public string cur_crr_crp_id;
            public string cur_crr_id;
            public TalkDBTransaction banco;
           
            public List<MTR_TipoMovimentacao> listTipoMovimentacao;
            public List<AlunosComUltimaMovimentacao> listAlunosComUltimaMovimentacao;
            public List<TUR_Turma> listTurma;
            public List<TUR_TurmaCurriculoAvaliacao> listTurmaCurriculoAvaliacao;

            /// <summary>
            /// Estrutura para guardar alunos e sua última movimentação.
            /// </summary>
            public class AlunosComUltimaMovimentacao
            {
                public ACA_Aluno entAluno;
                public MTR_Movimentacao entMovimentacao;
                public ACA_AlunoCurriculo entAlunoCurriculoAtual;
                public MTR_MatriculaTurma entMatriculaTurmaAtual;
                public ACA_AlunoCurriculoAvaliacao entAlaAtual;
            }

            public List<ACA_AlunoCurriculo> _listUltimaMatriculaAtiva;
            /// <summary>
            /// Lista com a última matrícula ativa de cada aluno.
            /// </summary>
            public List<ACA_AlunoCurriculo> listUltimaMatriculaAtiva
            {
                get
                {
                    if (_listUltimaMatriculaAtiva == null && !string.IsNullOrEmpty(alu_id))
                    {
                        // Busca as matrículas ativas dos alunos.
                        _listUltimaMatriculaAtiva = ACA_AlunoCurriculoBO.SelecionaUltimaMatriculaAtiva_Alunos(alu_id, banco);
                    }

                    return _listUltimaMatriculaAtiva;
                }
            }

            private List<PES_PessoaDeficiencia> _listPessoasDeficiencias;
            /// <summary>
            /// Lista de deficiências dos alunos.
            /// </summary>
            public List<PES_PessoaDeficiencia> listPessoasDeficiencias
            {
                get
                {
                    if (_listPessoasDeficiencias == null && !string.IsNullOrEmpty(alu_id))
                    {
                        // Busca as deficiências dos alunos.
                        _listPessoasDeficiencias = ACA_AlunoBO.SelecionaAlunos_Deficiencias(alu_id);
                    }

                    return _listPessoasDeficiencias;
                }
            }

            private List<ACA_Curso> _listCursos;

            /// <summary>
            /// Lista de cursos configurados na lista de Matrículas ou na lista de AlunoCurriculo Anterior
            /// </summary>
            public List<ACA_Curso> listCursos
            {
                get
                {
                    if (_listCursos == null)
                    {
                        _listCursos = new List<ACA_Curso>();
                        if (!string.IsNullOrEmpty(cur_id))
                        {
                            // Carregar os cursos contidos na lista de matrículas.                            
                            string[] ids = cur_id.Split(',');
                            foreach (string item in ids)
                            {
                                _listCursos.Add(ACA_CursoBO.GetEntity(new ACA_Curso
                                {
                                    cur_id = Convert.ToInt32(item)
                                }, banco));
                        }}

                        if (listAlunosComUltimaMovimentacao != null)
                        {                            
                            string cur_idOrigem = string.Join(",", (from AlunosComUltimaMovimentacao aluno in listAlunosComUltimaMovimentacao select aluno.entAlunoCurriculoAtual.cur_id.ToString()).Distinct().ToArray());                            
                            if (!string.IsNullOrEmpty(cur_idOrigem))
                            {
                                // Carregar os cursos contidos na lista de AlunosComUltimaMovimentacao (entAlunoCurriculoAtual)
                                string[] ids = cur_idOrigem.Split(',');
                                foreach (string item in ids)
                                {
                                    if (!_listCursos.Exists(p => p.cur_id.ToString() == item))
                                    {
                                        _listCursos.Add(
                                            ACA_CursoBO.GetEntity(
                                                new ACA_Curso { cur_id = Convert.ToInt32(item) },
                                                banco));
                                    }
                                }
                            }
                        }
                    }

                    return _listCursos;
                }
            }

            private List<ESC_Escola> _listEscolas;

            /// <summary>
            /// Lista de escolas configuradas na lista de Matrículas.
            /// </summary>
            public List<ESC_Escola> listEscolas
            {
                get
                {
                    if (_listEscolas == null && !string.IsNullOrEmpty(esc_id))
                    {
                        // Carregar os cursos contidos na lista de matrículas.
                        _listEscolas = new List<ESC_Escola>();
                        string[] ids = esc_id.Split(',');
                        foreach (string item in ids)
                        {
                            _listEscolas.Add(ESC_EscolaBO.GetEntity(new ESC_Escola
                            {
                                esc_id = Convert.ToInt32(item)
                            }, banco));
                        }
                    }

                    return _listEscolas;
                }
            }

            private List<ACA_CurriculoPeriodo> _listCurriculoPeriodoDestinos;

            /// <summary>
            /// Lista de períodos do curso configurados na lista de Matrículas.
            /// </summary>
            public List<ACA_CurriculoPeriodo> listCurriculoPeriodoDestinos
            {
                get
                {
                    if (_listCurriculoPeriodoDestinos == null && !string.IsNullOrEmpty(cur_crr_crp_id))
                    {
                        // Carregar os cursos contidos na lista de matrículas.
                        _listCurriculoPeriodoDestinos = new List<ACA_CurriculoPeriodo>();
                        string[] ids = cur_crr_crp_id.Split(',');
                        foreach (string item in ids)
                        {
                            int cur_id = Convert.ToInt32(item.Split(';')[0]);
                            int crr_id = Convert.ToInt32(item.Split(';')[1]);
                            int crp_id = Convert.ToInt32(item.Split(';')[2]);

                            _listCurriculoPeriodoDestinos.Add(ACA_CurriculoPeriodoBO.GetEntity(new ACA_CurriculoPeriodo
                            {
                                cur_id = cur_id
                                , crr_id = crr_id
                                , crp_id = crp_id
                            }, banco));
                        }
                    }

                    return _listCurriculoPeriodoDestinos;
                }
            }

            private List<ACA_Curriculo> _listCurriculosDestinos;

            /// <summary>
            /// Lista de currículos configurados na lista de Matrículas.
            /// </summary>
            public List<ACA_Curriculo> listCurriculosDestinos
            {
                get
                {
                    if (_listCurriculosDestinos == null && !string.IsNullOrEmpty(cur_crr_id))
                    {
                        // Carregar os cursos contidos na lista de matrículas.
                        _listCurriculosDestinos = new List<ACA_Curriculo>();
                        string[] ids = cur_crr_id.Split(',');
                        foreach (string item in ids)
                        {
                            int cur_id = Convert.ToInt32(item.Split(';')[0]);
                            int crr_id = Convert.ToInt32(item.Split(';')[1]);

                            _listCurriculosDestinos.Add(ACA_CurriculoBO.GetEntity(new ACA_Curriculo
                            {
                                cur_id = cur_id
                                ,
                                crr_id = crr_id
                            }, banco));
                        }
                    }

                    return _listCurriculosDestinos;
                }
            }

            private List<TUR_TurmaRelTurmaDisciplina> _listDisciplinasTurmas;

            /// <summary>
            /// Lista de disicplinas contidas nas turmas.
            /// </summary>
            public List<TUR_TurmaRelTurmaDisciplina> listDisciplinasTurmas
            {
                get
                {
                    if (_listDisciplinasTurmas == null && !string.IsNullOrEmpty(tur_id))
                    {
                        _listDisciplinasTurmas = TUR_TurmaRelTurmaDisciplinaBO.GetSelectBy_Turmas(tur_id, banco);
                    }

                    return _listDisciplinasTurmas;
                }
            }

            private List<ACA_CalendarioAnual> _listCalendarios;

            /// <summary>
            /// Lista de calendários das diferentes turmas da lista de turmas.
            /// </summary>
            public List<ACA_CalendarioAnual> listCalendarios
            {
                get
                {
                    if (_listCalendarios == null && listTurma != null)
                    {
                        _listCalendarios = new List<ACA_CalendarioAnual>();
                        foreach (int cal_id in listTurma.Select(p => p.cal_id).Distinct())
                        {
                            _listCalendarios.Add(ACA_CalendarioAnualBO.GetEntity(new ACA_CalendarioAnual { cal_id = cal_id }, banco));
                        }
                    }

                    return _listCalendarios;
                }
            }

        }

        #endregion
    }
}

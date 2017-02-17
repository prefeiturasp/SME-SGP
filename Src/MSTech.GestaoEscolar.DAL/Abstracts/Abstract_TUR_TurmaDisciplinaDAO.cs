/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.Data.Common.Abstracts;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Classe abstrata de TUR_TurmaDisciplina.
    /// </summary>
    public abstract class Abstract_TUR_TurmaDisciplinaDAO : Abstract_DAL<TUR_TurmaDisciplina>
    {
        /// <summary>
        /// ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_codigo";
                Param.Size = 30;
                Param.Value = entity.tud_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_nome";
                Param.Size = 200;
                Param.Value = entity.tud_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_multiseriado";
                Param.Size = 1;
                Param.Value = entity.tud_multiseriado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_vagas";
                Param.Size = 4;
                if (entity.tud_vagas > 0)
                {
                    Param.Value = entity.tud_vagas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_minimoMatriculados";
                Param.Size = 4;
                if (entity.tud_minimoMatriculados > 0)
                {
                    Param.Value = entity.tud_minimoMatriculados;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_duracao";
                Param.Size = 1;
                if (entity.tud_duracao > 0)
                {
                    Param.Value = entity.tud_duracao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_modo";
                Param.Size = 1;
                Param.Value = entity.tud_modo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_tipo";
                Param.Size = 1;
                Param.Value = entity.tud_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tud_dataInicio";
                Param.Size = 20;
                if (entity.tud_dataInicio != new DateTime())
                {
                    Param.Value = entity.tud_dataInicio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tud_dataFim";
                Param.Size = 20;
                if (entity.tud_dataFim != new DateTime())
                {
                    Param.Value = entity.tud_dataFim;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_situacao";
                Param.Size = 1;
                Param.Value = entity.tud_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tud_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tud_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tud_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tud_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_cargaHorariaSemanal";
                Param.Size = 4;
                if (entity.tud_cargaHorariaSemanal > 0)
                {
                    Param.Value = entity.tud_cargaHorariaSemanal;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_aulaForaPeriodoNormal";
                Param.Size = 1;
                Param.Value = entity.tud_aulaForaPeriodoNormal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_global";
                Param.Size = 1;
                Param.Value = entity.tud_global;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_disciplinaEspecial";
                Param.Size = 1;
                Param.Value = entity.tud_disciplinaEspecial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarNota";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarNota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarFrequencia";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarFrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirNota";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirNota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirFrequencia";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirFrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_semProfessor";
                Param.Size = 1;
                Param.Value = entity.tud_semProfessor;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirBoletim";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirBoletim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarPlanejamento";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarPlanejamento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ter_id";
                Param.Size = 4;
                if (entity.ter_id > 0)
                {
                    Param.Value = entity.ter_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_permitirLancarAbonoFalta";
                Param.Size = 1;
                Param.Value = entity.tud_permitirLancarAbonoFalta;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_duplaRegencia";
                Param.Size = 1;
                Param.Value = entity.tud_duplaRegencia;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_codigo";
                Param.Size = 30;
                Param.Value = entity.tud_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_nome";
                Param.Size = 200;
                Param.Value = entity.tud_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_multiseriado";
                Param.Size = 1;
                Param.Value = entity.tud_multiseriado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_vagas";
                Param.Size = 4;
                if (entity.tud_vagas > 0)
                {
                    Param.Value = entity.tud_vagas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_minimoMatriculados";
                Param.Size = 4;
                if (entity.tud_minimoMatriculados > 0)
                {
                    Param.Value = entity.tud_minimoMatriculados;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_duracao";
                Param.Size = 1;
                if (entity.tud_duracao > 0)
                {
                    Param.Value = entity.tud_duracao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_modo";
                Param.Size = 1;
                Param.Value = entity.tud_modo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_tipo";
                Param.Size = 1;
                Param.Value = entity.tud_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tud_dataInicio";
                Param.Size = 20;
                if (entity.tud_dataInicio != new DateTime())
                {
                    Param.Value = entity.tud_dataInicio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tud_dataFim";
                Param.Size = 20;
                if (entity.tud_dataFim != new DateTime())
                {
                    Param.Value = entity.tud_dataFim;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_situacao";
                Param.Size = 1;
                Param.Value = entity.tud_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tud_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tud_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tud_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tud_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_cargaHorariaSemanal";
                Param.Size = 4;
                if (entity.tud_cargaHorariaSemanal > 0)
                {
                    Param.Value = entity.tud_cargaHorariaSemanal;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_aulaForaPeriodoNormal";
                Param.Size = 1;
                Param.Value = entity.tud_aulaForaPeriodoNormal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_global";
                Param.Size = 1;
                Param.Value = entity.tud_global;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_disciplinaEspecial";
                Param.Size = 1;
                Param.Value = entity.tud_disciplinaEspecial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarNota";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarNota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarFrequencia";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarFrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirNota";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirNota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirFrequencia";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirFrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_semProfessor";
                Param.Size = 1;
                Param.Value = entity.tud_semProfessor;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoExibirBoletim";
                Param.Size = 1;
                Param.Value = entity.tud_naoExibirBoletim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarPlanejamento";
                Param.Size = 1;
                Param.Value = entity.tud_naoLancarPlanejamento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ter_id";
                Param.Size = 4;
                if (entity.ter_id > 0)
                {
                    Param.Value = entity.ter_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_permitirLancarAbonoFalta";
                Param.Size = 1;
                Param.Value = entity.tud_permitirLancarAbonoFalta;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_duplaRegencia";
                Param.Size = 1;
                Param.Value = entity.tud_duplaRegencia;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                entity.tud_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.tud_id > 0);
            }

            return false;
        }
    }
}
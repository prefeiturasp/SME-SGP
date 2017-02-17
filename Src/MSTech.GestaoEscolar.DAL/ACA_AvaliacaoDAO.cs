/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class ACA_AvaliacaoDAO : Abstract_ACA_AvaliacaoDAO
    {
        /// <summary>
        ///  Retorna os dados da avaliação do tipo final
        ///	 ou do tipo periódica + final do formato informado
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>        
        /// <returns>DataTable com a avaliação final ou periódica m+ final</returns>
        public DataTable SelectAvaliacaoFinal_PorFormato
        (
            int fav_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectAvaliacaoFinal_PorFormato", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as avaliações
        /// que não foram excluídas logicamente, filtradas por
        ///	fav_id_ava_id, ava_situacao
        /// </summary>
        /// <param name="fav_id">ID de Formato Avaliacao</param>
        /// <param name="ava_id">ID de Avaliacao</param>
        /// <param name="ava_situacao">Situacao da avaliação</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_All
        (
            int fav_id
            , int ava_id
            , byte ava_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectBy_All", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                if (fav_id > 0)
                    Param.Value = fav_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ava_id";
                Param.Size = 4;
                if (ava_id > 0)
                    Param.Value = ava_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ava_situacao";
                Param.Size = 1;
                if (ava_situacao > 0)
                    Param.Value = ava_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Retorna também as avaliações relacionadas às avaliações períodidas.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_id">ID do tipo de período do calendário - Obrigatório</param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_Periodo_Relacionadas
        (
            int fav_id
            , string tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_Select_Efetivacao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_id";
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca o maior bimestre pelo formato de avaliação
        /// </summary>
        /// <param name="fav_id"></param>
        /// <returns></returns>
        public int SelecionaMaiorBimestre_ByFormatoAvaliacao
        (
            int fav_id
            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelecionaMaiorBimestre_ByFormatoAvaliacao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return Convert.ToInt32(qs.Return.Rows[0]["tpc_id"]);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }       

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tud_id">TudId</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="existeRecuperacaoFinal">Indica se vai trazer as avaliações de recuperação final</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <param name="efetivacaoSemestral">Indicar se para trazer as avaliações de acordo com a matriz curricular (EfetivacaoSemestral).</param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_Periodo_Efetivacao
        (
            long tur_id
            , int fav_id
            , long tud_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool existeRecuperacaoFinal
            , bool verificarRegrasRecuperacao
            , bool efetivacaoSemestral
            , int tpc_idFiltrar = -1
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_Select_Efetivacao_Todas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idPeriodicaPeriodicaFinal";
                if (string.IsNullOrEmpty(tpc_idPeriodicaPeriodicaFinal))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idPeriodicaPeriodicaFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idRecuperacao";
                if (string.IsNullOrEmpty(tpc_idRecuperacao))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idRecuperacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@existeFinal";
                Param.Size = 1;
                Param.Value = existeFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@existeRecuperacaoFinal";
                Param.Size = 1;
                Param.Value = existeRecuperacaoFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@verificarRegrasRecuperacao";
                Param.Size = 1;
                Param.Value = verificarRegrasRecuperacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@efetivacaoSemestral";
                Param.Size = 1;
                Param.Value = efetivacaoSemestral;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_idFiltrar";
                Param.Size = 4;
                if (tpc_idFiltrar > 0)
                    Param.Value = tpc_idFiltrar;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Se o curso da turma possuir efetivação semestral, retorna os períodos de acordo com a matriz do curso
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tud_id">TudId</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="existeRecuperacaoFinal">Indica se vai trazer as avaliações de recuperação final</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_Periodo_EfetivacaoSemestral
        (
            long tur_id
            , int fav_id
            , long tud_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool existeRecuperacaoFinal
            , bool verificarRegrasRecuperacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_Select_Efetivacao_Todas_EfetivacaoSemestral", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idPeriodicaPeriodicaFinal";
                if (string.IsNullOrEmpty(tpc_idPeriodicaPeriodicaFinal))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idPeriodicaPeriodicaFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idRecuperacao";
                if (string.IsNullOrEmpty(tpc_idRecuperacao))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idRecuperacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@existeFinal";
                Param.Size = 1;
                Param.Value = existeFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@existeRecuperacaoFinal";
                Param.Size = 1;
                Param.Value = existeRecuperacaoFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@verificarRegrasRecuperacao";
                Param.Size = 1;
                Param.Value = verificarRegrasRecuperacao;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as notas e frequencias efetivadas para o aluno
        /// </summary>
        /// <param name="alu_id">ID do tipo de período do calendário das avaliações</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="mtu_id">ID da matricula da turma</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        public DataTable SelecionaNotasFreqEfetivadaPorAluno
        (
            long alu_id
            , int tpc_id
            , int fav_id
            , long tud_id
            , int mtu_id
            , long tur_id
            , int tds_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_RetornaNotasFreqEfetivadas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <param name="efetivacaoSemestral">Indicar se para trazer as avaliações de acordo com a matriz curricular (EfetivacaoSemestral).</param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_Periodo_Efetivacao_TurmaDisciplinaCalendario
        (
            long tur_id
            , long tud_id
            , int fav_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool verificarRegrasRecuperacao
            , bool efetivacaoSemestral
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_Select_Efetivacao_TurmaDisciplinaCalendario", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idPeriodicaPeriodicaFinal";
                if (string.IsNullOrEmpty(tpc_idPeriodicaPeriodicaFinal))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idPeriodicaPeriodicaFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tpc_idRecuperacao";
                if (string.IsNullOrEmpty(tpc_idRecuperacao))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tpc_idRecuperacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@existeFinal";
                Param.Size = 1;
                Param.Value = existeFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@verificarRegrasRecuperacao";
                Param.Size = 1;
                Param.Value = verificarRegrasRecuperacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@efetivacaoSemestral";
                Param.Size = 1;
                Param.Value = efetivacaoSemestral;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as avaliações
        /// que não foram excluídas logicamente, filtradas por
        ///	fav_id
        /// </summary>
        /// <param name="fav_id">ID de Formato Avaliacao</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com as avaliações</returns>
        public DataTable SelectBy_fav_id
        (
           int fav_id
           , bool paginado
           , int currentPage
           , int pageSize
           , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectBy_fav_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as avaliações do tipo informado para o formato de avaliação.
        /// </summary>
        /// <param name="ava_tipo">Tipo de avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <returns></returns>
        public DataTable SelectBy_TipoAvaliacao(byte ava_tipo, int fav_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectBy_Tipo_Formato", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ava_tipo";
            Param.Value = ava_tipo;
            Param.Size = 1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as avaliações do tipo informado para o formato de avaliação.
        /// </summary>        
        /// <param name="fav_id">Formato de avaliação</param>
        /// <returns></returns>
        public DataTable SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato(int fav_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna um DataTable contendo dados sobre as avaliações,
        /// filtrados pelo tipo
        /// </summary>
        /// <param name="ava_tipos">string contendo os ids dos tipos</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <returns></returns>
        public DataTable SelectBy_TipoAvaliacao(string ava_tipos, Int64 tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectByTipoAvaliacao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@ava_tipos";
                Param.Value = ava_tipos;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                if (tur_id < 0)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca a ordem das avaliações do tipo periódica do formato informado
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <returns></returns>
        public DataTable SelectBy_FormatoAvaliacao(int fav_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectBy_FormatoAvaliacao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;                
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca avaliações do tipo periódica do formato e periodo informados
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="tpc_id">Id do periodo</param>
        /// <returns></returns>
        public DataTable SelectBy_FormatoAvaliacaoPeriodo(int fav_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Avaliacao_SelectBy_FormatoAvaliacaoPeriodo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Sobrescrita do método carregar, que traz o campo tpc_ordem.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Carregar(ACA_Avaliacao entity)
        {
            __STP_LOAD = "NEW_ACA_Avaliacao_Load";
            return base.Carregar(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Avaliacao entity)
        {
            entity.ava_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.ava_id > 0);
        }
    }
}
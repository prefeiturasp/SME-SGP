namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Web;
    using System.Linq;
    using MSTech.GestaoEscolar.BLL.Caching;
    using System.Threading;

    public class CacheEfetivacaoHandler : HttpTaskAsyncHandler
    {
        public async override Task ProcessRequestAsync(HttpContext context)
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            var banco = dao._Banco;
            var connectionString = banco.GetConnection.ConnectionString += "Asynchronous Processing=true;";
            string msg = "";
            DataTable dt = new DataTable();

            bool fechamentoAutomatico = Convert.ToBoolean(context.Request.QueryString["fechamentoAutomatico"]);
            if (fechamentoAutomatico)
            {
                long tur_id = Convert.ToInt64(context.Request.QueryString["tur_id"]);
                long tud_id = Convert.ToInt64(context.Request.QueryString["tud_id"]);
                byte tud_tipo = Convert.ToByte(context.Request.QueryString["tud_tipo"]);
                int tpc_id = Convert.ToInt32(context.Request.QueryString["tpc_id"]);
                byte tipoAvaliacao = Convert.ToByte(context.Request.QueryString["tipoAvaliacao"]);

                if (context.Request.QueryString["ava_id"] != null)
                {
                    int ava_id = Convert.ToInt32(context.Request.QueryString["ava_id"]);
                    int fav_id = Convert.ToInt32(context.Request.QueryString["fav_id"]);
                    byte tur_tipo = Convert.ToByte(context.Request.QueryString["tur_tipo"]);
                    int cal_id = Convert.ToInt32(context.Request.QueryString["cal_id"]);
                    byte tipoDocente = Convert.ToByte(context.Request.QueryString["tipoDocente"]);
                    bool disciplinaEspecial = Convert.ToBoolean(context.Request.QueryString["disciplinaEspecial"]);
                    byte permiteAlterarResultado = Convert.ToByte(context.Request.QueryString["permiteAlterarResultado"]);
                    int MinutosCacheFechamento = Convert.ToInt32(context.Request.QueryString["MinutosCacheFechamento"]);

                    bool cacheIsSet = false;

                    if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                    {
                        cacheIsSet = disciplinaEspecial ?
                            CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id)) :
                            CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_id));
                    }
                    else
                    {
                        cacheIsSet = disciplinaEspecial ?
                            CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, tpc_id)) :
                            CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_id, tpc_id));
                    }

                    if (!cacheIsSet && MinutosCacheFechamento > 0)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                await connection.OpenAsync();

                                string query = string.Empty;

                                SqlCommand command;

                                SqlParameter param;

                                DataTable dtTurma = new DataTable();
                                dtTurma.Columns.Add("tur_id", typeof(Int64));

                                if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                                {
                                    query = disciplinaEspecial ?
                                       "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia_Final" :
                                        "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento_Final";

                                    command = new SqlCommand(query, connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    param = new SqlParameter();
                                    param.ParameterName = "@tud_id";
                                    param.SqlDbType = SqlDbType.BigInt;
                                    param.Size = 8;
                                    param.Value = tud_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tur_id";
                                    param.SqlDbType = SqlDbType.BigInt;
                                    param.Size = 8;
                                    param.Value = tur_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@ava_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = ava_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@ordenacao";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = 0;
                                    command.Parameters.Add(param);
                                    param = new SqlParameter();

                                    param = new SqlParameter();
                                    param.ParameterName = "@fav_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = fav_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tur_tipo";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = tur_tipo;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@cal_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = cal_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@permiteAlterarResultado";
                                    param.SqlDbType = SqlDbType.Bit;
                                    param.Size = 1;
                                    param.Value = permiteAlterarResultado;
                                    command.Parameters.Add(param);

                                    if (disciplinaEspecial)
                                    {
                                        param = new SqlParameter();
                                        param.ParameterName = "@tdc_id";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoDocente;
                                        command.Parameters.Add(param);
                                    }

                                    SqlParameter sqlParam = new SqlParameter();
                                    sqlParam.SqlDbType = SqlDbType.Structured;
                                    sqlParam.ParameterName = "@dtTurma";
                                    sqlParam.TypeName = "TipoTabela_Turma";
                                    sqlParam.Value = dtTurma;
                                    command.Parameters.Add(sqlParam);
                                }
                                else
                                {
                                    query = disciplinaEspecial ?
                                        "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia" :
                                        "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento";

                                    command = new SqlCommand(query, connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    param = new SqlParameter();
                                    param.ParameterName = "@tud_id";
                                    param.SqlDbType = SqlDbType.BigInt;
                                    param.Size = 8;
                                    param.Value = tud_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tur_id";
                                    param.SqlDbType = SqlDbType.BigInt;
                                    param.Size = 8;
                                    param.Value = tur_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tpc_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = tpc_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@ava_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = ava_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@ordenacao";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = 0;
                                    command.Parameters.Add(param);
                                    param = new SqlParameter();

                                    param = new SqlParameter();
                                    param.ParameterName = "@fav_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = fav_id;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tipoAvaliacao";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = tipoAvaliacao;
                                    command.Parameters.Add(param);
                                    param = new SqlParameter();

                                    param = new SqlParameter();
                                    param.ParameterName = "@permiteAlterarResultado";
                                    param.SqlDbType = SqlDbType.Bit;
                                    param.Size = 1;
                                    param.Value = permiteAlterarResultado;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tur_tipo";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = tur_tipo;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@cal_id";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = cal_id;
                                    command.Parameters.Add(param);

                                    if (disciplinaEspecial)
                                    {
                                        param = new SqlParameter();
                                        param.ParameterName = "@tdc_id";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoDocente;
                                        command.Parameters.Add(param);
                                    }

                                    SqlParameter sqlParam = new SqlParameter();
                                    sqlParam.SqlDbType = SqlDbType.Structured;
                                    sqlParam.ParameterName = "@dtTurma";
                                    sqlParam.TypeName = "TipoTabela_Turma";
                                    sqlParam.Value = dtTurma;
                                    command.Parameters.Add(sqlParam);

                                }

                                //command.Parameters.Add(new SqlParameter("@variavel", variavel));

                                var result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                                dt = new DataTable();

                                dt.Load(result);

                                if (dt.Rows.Count > 0)
                                {
                                    if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                                    {
                                        List<AlunosFechamentoFinal> dados = dt.AsEnumerable().Select(p => (AlunosFechamentoFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoFinal())).ToList();
                                        CacheManager.Factory.Set(string.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_id), dados, MinutosCacheFechamento);

                                        if (dados.Any() && tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                        {
                                            var alunos = dados
                                                 .Select(p => new
                                                 {
                                                     tpc_id = p.tpc_id
                                                     ,
                                                     alu_id = p.alu_id
                                                     ,
                                                     mtu_id = p.mtu_id
                                                 })
                                                 .Where(p => p.tpc_id == -1);

                                            DataTable dtAlunos = new DataTable();
                                            dtAlunos.Columns.Add("alu_id");
                                            dtAlunos.Columns.Add("mtu_id");
                                            foreach (var aluno in alunos)
                                            {
                                                DataRow drAluno = dtAlunos.NewRow();
                                                drAluno["alu_id"] = aluno.alu_id;
                                                drAluno["mtu_id"] = aluno.mtu_id;
                                                dtAlunos.Rows.Add(drAluno);
                                            }

                                            query = "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoComponentesRegencia_Final";

                                            command = new SqlCommand(query, connection);
                                            command.CommandType = CommandType.StoredProcedure;

                                            param = new SqlParameter();
                                            param.ParameterName = "@tur_id";
                                            param.SqlDbType = SqlDbType.BigInt;
                                            param.Size = 8;
                                            param.Value = tur_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@ava_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = ava_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@fav_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = fav_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@cal_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = cal_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@alunos";
                                            param.SqlDbType = SqlDbType.Structured;
                                            param.TypeName = "TipoTabela_AlunoMatriculaTurma";
                                            param.Value = dtAlunos;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@permiteAlterarResultado";
                                            param.SqlDbType = SqlDbType.Bit;
                                            param.Size = 1;
                                            param.Value = permiteAlterarResultado;
                                            command.Parameters.Add(param);

                                            result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                                            dt = new DataTable();
                                            
                                            dt.Load(result);

                                            if (dt.Rows.Count > 0)
                                            {
                                                List<AlunosFechamentoFinalComponenteRegencia> dadosComponentes = dt.AsEnumerable().Select(p => (AlunosFechamentoFinalComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoFinalComponenteRegencia())).ToList();
                                                CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_id), dadosComponentes, MinutosCacheFechamento);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<AlunosFechamentoPadrao> dados = dt.AsEnumerable().Select(p => (AlunosFechamentoPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoPadrao())).ToList();
                                        CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_id, tpc_id), dados, MinutosCacheFechamento);

                                        if (dados.Any() && tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                        {
                                            DataTable dtAlunos = new DataTable();
                                            dtAlunos.Columns.Add("alu_id");
                                            dtAlunos.Columns.Add("mtu_id");

                                            foreach (AlunosFechamentoPadrao aluno in dados)
                                            {
                                                DataRow drAluno = dtAlunos.NewRow();
                                                drAluno["alu_id"] = aluno.alu_id;
                                                drAluno["mtu_id"] = aluno.mtu_id;
                                                dtAlunos.Rows.Add(drAluno);
                                            }

                                            query = "NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoComponentesRegencia";

                                            command = new SqlCommand(query, connection);
                                            command.CommandType = CommandType.StoredProcedure;

                                            param = new SqlParameter();
                                            param.ParameterName = "@tur_id";
                                            param.SqlDbType = SqlDbType.BigInt;
                                            param.Size = 8;
                                            param.Value = tur_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@tpc_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = tpc_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@fav_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = fav_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@tipoAvaliacao";
                                            param.SqlDbType = SqlDbType.TinyInt;
                                            param.Size = 1;
                                            param.Value = tipoAvaliacao;
                                            command.Parameters.Add(param);
                                            param = new SqlParameter();

                                            param = new SqlParameter();
                                            param.ParameterName = "@tur_tipo";
                                            param.SqlDbType = SqlDbType.TinyInt;
                                            param.Size = 1;
                                            param.Value = tur_tipo;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@ava_id";
                                            param.SqlDbType = SqlDbType.Int;
                                            param.Size = 4;
                                            param.Value = ava_id;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@permiteAlterarResultado";
                                            param.SqlDbType = SqlDbType.Bit;
                                            param.Size = 1;
                                            param.Value = permiteAlterarResultado;
                                            command.Parameters.Add(param);

                                            param = new SqlParameter();
                                            param.ParameterName = "@alunos";
                                            param.SqlDbType = SqlDbType.Structured;
                                            param.TypeName = "TipoTabela_AlunoMatriculaTurma";
                                            param.Value = dtAlunos;
                                            command.Parameters.Add(param);

                                            result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                                            dt = new DataTable();

                                            dt.Load(result);

                                            if (dt.Rows.Count > 0)
                                            {
                                                List<AlunosFechamentoPadraoComponenteRegencia> dadosComponentes = dt.AsEnumerable().Select(p => (AlunosFechamentoPadraoComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosFechamentoPadraoComponenteRegencia())).ToList();
                                                CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, tpc_id), dadosComponentes, MinutosCacheFechamento);
                                            }
                                        }
                                    }

                                    msg = "sucesso";
                                }
                                else
                                {
                                    msg = "Vazio";
                                    context.Response.StatusCode = 404;
                                }
                            }
                            catch (Exception ex)
                            {
                                msg = "Erro!";
                                context.Response.StatusCode = 500;
                                /*Handle error*/
                                ApplicationWEB._GravaErro(ex);
                            }
                        }

                        context.Response.Write(msg);
                    }
                }
            }
            else
            {
                long tud_id = Convert.ToInt64(context.Request.QueryString["tud_id"]);
                long tur_id = Convert.ToInt64(context.Request.QueryString["tur_id"]);
                int tpc_id = Convert.ToInt32(context.Request.QueryString["tpc_id"]);
                int ava_id = Convert.ToInt32(context.Request.QueryString["ava_id"]);
                int fav_id = Convert.ToInt32(context.Request.QueryString["fav_id"]);
                byte tipoAvaliacao = Convert.ToByte(context.Request.QueryString["tipoAvaliacao"]);
                int esa_id = Convert.ToInt32(context.Request.QueryString["esa_id"]);
                byte tipoEscalaDisciplina = Convert.ToByte(context.Request.QueryString["tipoEscalaDisciplina"]);
                byte tipoEscalaDocente = Convert.ToByte(context.Request.QueryString["tipoEscalaDocente"]);
                double notaMinimaAprovacao = Convert.ToDouble(context.Request.QueryString["notaMinimaAprovacao"]);
                int ordemParecerMinimo = Convert.ToInt32(context.Request.QueryString["ordemParecerMinimo"]);
                byte tipoLancamento = Convert.ToByte(context.Request.QueryString["tipoLancamento"]);
                byte fav_calculoQtdeAulasDadas = Convert.ToByte(context.Request.QueryString["fav_calculoQtdeAulasDadas"]);
                byte tur_tipo = Convert.ToByte(context.Request.QueryString["tur_tipo"]);
                int cal_id = Convert.ToInt32(context.Request.QueryString["cal_id"]);
                byte tud_tipo = Convert.ToByte(context.Request.QueryString["tud_tipo"]);
                int tpc_ordem = Convert.ToInt32(context.Request.QueryString["tpc_ordem"]);
                decimal fav_variacao = Convert.ToDecimal(context.Request.QueryString["fav_variacao"]);
                byte tipoDocente = Convert.ToByte(context.Request.QueryString["tipoDocente"]);
                bool disciplinaEspecial = Convert.ToBoolean(context.Request.QueryString["disciplinaEspecial"]);
                byte permiteAlterarResultado = Convert.ToByte(context.Request.QueryString["permiteAlterarResultado"]);
                byte exibirNotaFinal = Convert.ToByte(context.Request.QueryString["exibirNotaFinal"]);
                byte ExibeCompensacao = Convert.ToByte(context.Request.QueryString["ExibeCompensacao"]);
                int MinutosCacheFechamento = Convert.ToInt32(context.Request.QueryString["MinutosCacheFechamento"]);

                bool cacheIsSet = false;

                if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                {
                    cacheIsSet = disciplinaEspecial ?
                        CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, fav_id, ava_id, "-1")) :
                        CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_FINAL_MODEL_KEY, tud_id, fav_id, ava_id, "-1"));
                }
                else
                {
                    cacheIsSet = disciplinaEspecial ?
                        CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_id, fav_id, ava_id, "-1")) :
                        CacheManager.Factory.IsSet(String.Format(ModelCache.FECHAMENTO_BIMESTRE_MODEL_KEY, tud_id, fav_id, ava_id, "-1"));
                }

                if (!cacheIsSet && MinutosCacheFechamento > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            await connection.OpenAsync();

                            string query = string.Empty;

                            SqlCommand command;

                            SqlParameter param;

                            DataTable dtTurma = new DataTable();
                            dtTurma.Columns.Add("tur_id", typeof(Int64));

                            if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                            {
                                query = disciplinaEspecial ?
                                   "NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia_Final" :
                                    "NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final";

                                command = new SqlCommand(query, connection);
                                command.CommandType = CommandType.StoredProcedure;

                                param = new SqlParameter();
                                param.ParameterName = "@tud_id";
                                param.SqlDbType = SqlDbType.BigInt;
                                param.Size = 8;
                                param.Value = tud_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tur_id";
                                param.SqlDbType = SqlDbType.BigInt;
                                param.Size = 8;
                                param.Value = tur_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@ava_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = ava_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@ordenacao";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = 0;
                                command.Parameters.Add(param);
                                param = new SqlParameter();

                                param = new SqlParameter();
                                param.ParameterName = "@fav_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = fav_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoEscalaDisciplina";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoEscalaDisciplina;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoEscalaDocente";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoEscalaDocente;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tur_tipo";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tur_tipo;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@cal_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = cal_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoLancamento";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoLancamento;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@fav_calculoQtdeAulasDadas";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = fav_calculoQtdeAulasDadas;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoDocente";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoDocente;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@permiteAlterarResultado";
                                param.SqlDbType = SqlDbType.Bit;
                                param.Size = 1;
                                param.Value = permiteAlterarResultado;
                                command.Parameters.Add(param);

                                SqlParameter sqlParam = new SqlParameter();
                                sqlParam.SqlDbType = SqlDbType.Structured;
                                sqlParam.ParameterName = "@dtTurma";
                                sqlParam.TypeName = "TipoTabela_Turma";
                                sqlParam.Value = dtTurma;
                                command.Parameters.Add(sqlParam);
                            }
                            else
                            {
                                query = disciplinaEspecial ?
                                    "NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia" :
                                    "NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato";

                                command = new SqlCommand(query, connection);
                                command.CommandType = CommandType.StoredProcedure;

                                param = new SqlParameter();
                                param.ParameterName = "@tud_id";
                                param.SqlDbType = SqlDbType.BigInt;
                                param.Size = 8;
                                param.Value = tud_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tur_id";
                                param.SqlDbType = SqlDbType.BigInt;
                                param.Size = 8;
                                param.Value = tur_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tpc_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = tpc_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@ava_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = ava_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@ordenacao";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = 0;
                                command.Parameters.Add(param);
                                param = new SqlParameter();

                                param = new SqlParameter();
                                param.ParameterName = "@fav_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = fav_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoAvaliacao";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoAvaliacao;
                                command.Parameters.Add(param);
                                param = new SqlParameter();

                                param = new SqlParameter();
                                param.ParameterName = "@esa_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = esa_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoEscalaDisciplina";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoEscalaDisciplina;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoEscalaDocente";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoEscalaDocente;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@avaliacaoesRelacionadas";
                                param.SqlDbType = SqlDbType.VarChar;
                                param.Value = string.Empty;
                                command.Parameters.Add(param);
                                param = new SqlParameter();

                                param = new SqlParameter();
                                param.ParameterName = "@notaMinimaAprovacao";
                                param.SqlDbType = SqlDbType.Decimal;
                                param.Size = 8;
                                param.Value = notaMinimaAprovacao;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@ordemParecerMinimo";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = ordemParecerMinimo;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tipoLancamento";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tipoLancamento;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@fav_calculoQtdeAulasDadas";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = fav_calculoQtdeAulasDadas;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@permiteAlterarResultado";
                                param.SqlDbType = SqlDbType.Bit;
                                param.Size = 1;
                                param.Value = permiteAlterarResultado;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@tur_tipo";
                                param.SqlDbType = SqlDbType.TinyInt;
                                param.Size = 1;
                                param.Value = tur_tipo;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@cal_id";
                                param.SqlDbType = SqlDbType.Int;
                                param.Size = 4;
                                param.Value = cal_id;
                                command.Parameters.Add(param);

                                param = new SqlParameter();
                                param.ParameterName = "@exibirNotaFinal";
                                param.SqlDbType = SqlDbType.Bit;
                                param.Size = 1;
                                param.Value = exibirNotaFinal;
                                command.Parameters.Add(param);

                                SqlParameter sqlParam = new SqlParameter();
                                sqlParam.SqlDbType = SqlDbType.Structured;
                                sqlParam.ParameterName = "@dtTurma";
                                sqlParam.TypeName = "TipoTabela_Turma";
                                sqlParam.Value = dtTurma;
                                command.Parameters.Add(sqlParam);

                                if (disciplinaEspecial)
                                {
                                    param = new SqlParameter();
                                    param.ParameterName = "@tdc_id";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = tipoDocente;
                                    command.Parameters.Add(param);
                                }
                                else
                                {
                                    param = new SqlParameter();
                                    param.ParameterName = "@tud_tipo";
                                    param.SqlDbType = SqlDbType.TinyInt;
                                    param.Size = 1;
                                    param.Value = tud_tipo;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@tpc_ordem";
                                    param.SqlDbType = SqlDbType.Int;
                                    param.Size = 4;
                                    param.Value = tpc_ordem;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@fav_variacao";
                                    param.SqlDbType = SqlDbType.Decimal;
                                    param.Size = 8;
                                    param.Value = fav_variacao;
                                    command.Parameters.Add(param);

                                    param = new SqlParameter();
                                    param.ParameterName = "@ExibeCompensacao";
                                    param.SqlDbType = SqlDbType.Bit;
                                    param.Size = 1;
                                    param.Value = ExibeCompensacao;
                                    command.Parameters.Add(param);

                                    sqlParam = new SqlParameter();
                                    sqlParam.SqlDbType = SqlDbType.Structured;
                                    sqlParam.ParameterName = "@dtTurma";
                                    sqlParam.TypeName = "TipoTabela_Turma";
                                    sqlParam.Value = dtTurma;
                                    command.Parameters.Add(sqlParam);
                                }
                            }

                            //command.Parameters.Add(new SqlParameter("@variavel", variavel));

                            var result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                            dt = new DataTable();

                            dt.Load(result);

                            if (dt.Rows.Count > 0)
                            {
                                if (tipoAvaliacao == (byte)AvaliacaoTipo.Final)
                                {
                                    List<AlunosEfetivacaoDisciplinaFinal> dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoDisciplinaFinal)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaFinal())).ToList();
                                    CacheManager.Factory.Set(string.Format(ModelCache.FECHAMENTO_FINAL_MODEL_KEY, tud_id, fav_id, ava_id), dados, MinutosCacheFechamento);

                                    if (dados.Any() && tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                    {
                                        var alunos = dados
                                             .Select(p => new
                                             {
                                                 tpc_id = p.tpc_id
                                                 ,
                                                 alu_id = p.alu_id
                                                 ,
                                                 mtu_id = p.mtu_id
                                             })
                                             .Where(p => p.tpc_id == -1);

                                        DataTable dtAlunos = new DataTable();
                                        dtAlunos.Columns.Add("alu_id");
                                        dtAlunos.Columns.Add("mtu_id");
                                        foreach (var aluno in alunos)
                                        {
                                            DataRow drAluno = dtAlunos.NewRow();
                                            drAluno["alu_id"] = aluno.alu_id;
                                            drAluno["mtu_id"] = aluno.mtu_id;
                                            dtAlunos.Rows.Add(drAluno);
                                        }

                                        query = "NEW_MTR_MatriculaTurmaDisciplina_SelectComponentesRegenciaBy_TurmaFormato_Final";

                                        command = new SqlCommand(query, connection);
                                        command.CommandType = CommandType.StoredProcedure;

                                        param = new SqlParameter();
                                        param.ParameterName = "@tur_id";
                                        param.SqlDbType = SqlDbType.BigInt;
                                        param.Size = 8;
                                        param.Value = tur_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@ava_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = ava_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@fav_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = fav_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tipoEscalaDisciplina";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoEscalaDisciplina;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tipoEscalaDocente";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoEscalaDocente;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tur_tipo";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tur_tipo;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@cal_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = cal_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@alunos";
                                        param.SqlDbType = SqlDbType.Structured;
                                        param.TypeName = "TipoTabela_AlunoMatriculaTurma";
                                        param.Value = dtAlunos;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@permiteAlterarResultado";
                                        param.SqlDbType = SqlDbType.Bit;
                                        param.Size = 1;
                                        param.Value = permiteAlterarResultado;
                                        command.Parameters.Add(param);

                                        result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                                        dt = new DataTable();

                                        dt.Load(result);

                                        if (dt.Rows.Count > 0)
                                        {
                                            List<AlunosEfetivacaoFinalComponenteRegencia> dadosComponentes = dt.AsEnumerable().Select(p => (AlunosEfetivacaoFinalComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoFinalComponenteRegencia())).ToList();
                                            CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, fav_id, ava_id), dadosComponentes, MinutosCacheFechamento);
                                        }
                                    }
                                }
                                else
                                {
                                    List<AlunosEfetivacaoDisciplinaPadrao> dados = dt.AsEnumerable().Select(p => (AlunosEfetivacaoDisciplinaPadrao)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoDisciplinaPadrao())).ToList();
                                    CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_BIMESTRE_MODEL_KEY, tud_id, fav_id, ava_id), dados, MinutosCacheFechamento);

                                    if (dados.Any() && tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                    {
                                        DataTable dtAlunos = new DataTable();
                                        dtAlunos.Columns.Add("alu_id");
                                        dtAlunos.Columns.Add("mtu_id");

                                        foreach (AlunosEfetivacaoDisciplinaPadrao aluno in dados)
                                        {
                                            DataRow drAluno = dtAlunos.NewRow();
                                            drAluno["alu_id"] = aluno.alu_id;
                                            drAluno["mtu_id"] = aluno.mtu_id;
                                            dtAlunos.Rows.Add(drAluno);
                                        }

                                        query = "NEW_MTR_MatriculaTurmaDisciplina_SelectComponentesRegenciaBy_TurmaFormato";

                                        command = new SqlCommand(query, connection);
                                        command.CommandType = CommandType.StoredProcedure;

                                        param = new SqlParameter();
                                        param.ParameterName = "@tur_id";
                                        param.SqlDbType = SqlDbType.BigInt;
                                        param.Size = 8;
                                        param.Value = tur_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tpc_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = tpc_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@ava_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = ava_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@avaliacaoesRelacionadas";
                                        param.SqlDbType = SqlDbType.VarChar;
                                        param.Value = string.Empty;
                                        command.Parameters.Add(param);
                                        param = new SqlParameter();

                                        param = new SqlParameter();
                                        param.ParameterName = "@fav_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = fav_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tipoAvaliacao";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoAvaliacao;
                                        command.Parameters.Add(param);
                                        param = new SqlParameter();

                                        param = new SqlParameter();
                                        param.ParameterName = "@esa_id";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = esa_id;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tipoEscalaDisciplina";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoEscalaDisciplina;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tipoEscalaDocente";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tipoEscalaDocente;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@notaMinimaAprovacao";
                                        param.SqlDbType = SqlDbType.Decimal;
                                        param.Size = 8;
                                        param.Value = notaMinimaAprovacao;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@ordemParecerMinimo";
                                        param.SqlDbType = SqlDbType.Int;
                                        param.Size = 4;
                                        param.Value = ordemParecerMinimo;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@permiteAlterarResultado";
                                        param.SqlDbType = SqlDbType.Bit;
                                        param.Size = 1;
                                        param.Value = permiteAlterarResultado;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@tur_tipo";
                                        param.SqlDbType = SqlDbType.TinyInt;
                                        param.Size = 1;
                                        param.Value = tur_tipo;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@exibirNotaFinal";
                                        param.SqlDbType = SqlDbType.Bit;
                                        param.Size = 1;
                                        param.Value = exibirNotaFinal;
                                        command.Parameters.Add(param);

                                        param = new SqlParameter();
                                        param.ParameterName = "@alunos";
                                        param.SqlDbType = SqlDbType.Structured;
                                        param.TypeName = "TipoTabela_AlunoMatriculaTurma";
                                        param.Value = dtAlunos;
                                        command.Parameters.Add(param);

                                        result = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                                        dt = new DataTable();

                                        dt.Load(result);

                                        if (dt.Rows.Count > 0)
                                        {
                                            List<AlunosEfetivacaoPadraoComponenteRegencia> dadosComponentes = dt.AsEnumerable().Select(p => (AlunosEfetivacaoPadraoComponenteRegencia)GestaoEscolarUtilBO.DataRowToEntity(p, new AlunosEfetivacaoPadraoComponenteRegencia())).ToList();
                                            CacheManager.Factory.Set(String.Format(ModelCache.FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY, tur_id, fav_id, ava_id), dadosComponentes, MinutosCacheFechamento);
                                        }
                                    }
                                }

                                msg = "sucesso";
                            }
                            else
                            {
                                msg = "Vazio";
                                context.Response.StatusCode = 404;
                            }
                        }
                        catch (Exception ex)
                        {
                            msg = "Erro!";
                            context.Response.StatusCode = 500;
                            /*Handle error*/
                            ApplicationWEB._GravaErro(ex);
                        }
                    }

                    context.Response.Write(msg);
                }
            }
        }
    }
}

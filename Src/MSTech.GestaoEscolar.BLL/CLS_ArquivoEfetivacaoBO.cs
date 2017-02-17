/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.Web;
    using MSTech.GestaoEscolar.CustomResourceProviders;

    #region Estrutura

    /// <summary>
    /// Estrutura que armazena os dados analisados no arquivo de importação de fechamento.
    /// </summary>
    [Serializable]
    public struct LoteFechamento
    {
        public string codigoEscola { get; set; }
        public string codigoTurma { get; set; }
        public int numeroChamada { get; set; }
        public string raAluno { get; set; }
        public string nomeAluno { get; set; }
        public string disciplina { get; set; }
        public string codigoDisciplina { get; set; }
        public string docente { get; set; }
        public int ano { get; set; }
        public string bimestre { get; set; }
        public int qtdeAulas { get; set; }
        public int qtdeFaltas { get; set; }
        public int compensacoes { get; set; }
        public string nota { get; set; }
        public string notaPosConselho { get; set; }
        public string mensagem { get; set; }
        public LoteStatus status { get; set; }

        // Propriedades auxiliares utilizados para salvar os dados
        public string nomeEscola { get; set; }
        public int esc_id { get; set; }
        public int uni_id { get; set; }
        public int cal_id { get; set; }
        public int tpc_id { get; set; }
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public long alu_id { get; set; }
        public int mtu_id { get; set; }
        public int mtd_id { get; set; }
        public int fav_id { get; set; }
        public int ava_id { get; set; }
    }

    #endregion Estrutura

    /// <summary>
    /// Description: CLS_ArquivoEfetivacao Business Object. 
    /// </summary>
    public class CLS_ArquivoEfetivacaoBO : BusinessBase<CLS_ArquivoEfetivacaoDAO, CLS_ArquivoEfetivacao>
    {
        #region Estrutura

        /// <summary>
        /// Estrutura que armazena dados dos alunos importados.
        /// </summary>
        public struct sAlunoImportacao
        {
            public long alu_id { get; set; }
            public int mtu_id { get; set; }
            public int mtd_id { get; set; }
            public int mtu_numeroChamada { get; set; }
            public string alc_matricula { get; set; }
            public int atd_ausenciasCompensadas { get; set; }
        }

        /// <summary>
        /// Estrutura que armazena dados dos períodos do calendário da turma.
        /// </summary>
        public struct sPeriodoTurma
        {
            public string tpc_nome { get; set; }
            public int tpc_id { get; set; }
            public bool periodoVigente { get; set; }
            public int ava_id { get; set; }
        }

        /// <summary>
        /// Estrutura que armazena dados das turmas disciplinas importadas.
        /// </summary>
        public struct sTurmasAlunosEscalaAvaliacao
        {
            public long tur_id { get; set; }
            public string tur_codigo { get; set; }
            public int esc_id { get; set; }
            public int uni_id { get; set; }
            public int cal_id { get; set; }
            public string esc_codigo { get; set; }
            public string esc_nome { get; set; }
            public bool permiteImportacao { get; set; }
            public long tud_id { get; set; }
            public string tud_codigo { get; set; }
            public string tud_nome { get; set; }
            public byte tud_tipo { get; set; }
            public string nomeDocente { get; set; }
            public byte esa_tipo { get; set; }
            public string ean_variacao { get; set; }
            public string ean_menorValor { get; set; }
            public string ean_maiorValor { get; set; }
            public int fav_id { get; set; }
            public List<string> listaValorEscalaAvaliacao { get; set; }
            public List<sAlunoImportacao> listaAlunos { get; set; }
            public List<sPeriodoTurma> listaPeriodos { get; set; }
        }

        #endregion Estrutura

        #region Enumerados

        /// <summary>
        /// Situações da escola
        /// </summary>
        public enum eSituacao : byte
        {
            Ativo = 1,
            Excluido = 3
        }

        /// <summary>
        /// Índice dos campos do arquivo de importação de fechamento de bimestre.
        /// </summary>
        public enum eLoteFechamento
        {
            CodigoEscola = 0
            ,
            CodigoTurma
            ,
            Ano
            ,
            Bimestre
            ,
            CodigoDisciplina
            ,
            Disciplina
            ,
            Docente
            ,
            RaAluno
            ,
            NumeroChamada
            ,
            NomeAluno
            ,
            QtdeAulas
            ,
            QtdeFaltas
            ,
            Compensacoes
            ,
            Nota
            ,
            NotaPosConselho
        }


        /// <summary>
        /// Situações da escola
        /// </summary>
        public enum eTipoArquivoEfetivacao : byte
        {
            Importacao = 1,
            Exportacao = 2
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Monta o layout de exportação dos dados dos alunos para o fechamento de escolas que não possuem internet.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tpc_id">Id do periodo calendario</param>
        /// <param name="nomeBimestre">Nome padrão do periodo calendario no sistema</param>
        /// <param name="nomeDisciplina">Valor do parametro mensagem [MSG_DISCIPLINAS]</param>
        /// <param name="ordemAluno">Valor do parametro academico ORDENACAO_COMBO_ALUNO</param>
        /// <returns>Layout montado pronto para o arquivo</returns>
        public static DataTable ExportacaoAlunosFechamento(
            int esc_id,
            int uni_id,
            int cal_id,
            long tur_id,
            int tpc_id,
            string nomeBimestre,
            string nomeDisciplina,
            short ordemAluno
        )
        {
            CLS_ArquivoEfetivacaoDAO dao = new CLS_ArquivoEfetivacaoDAO();
            DataTable dt = dao.ExportacaoAlunosFechamento(esc_id, uni_id, cal_id, tur_id, tpc_id, nomeBimestre, nomeDisciplina, ordemAluno);

            return dt;
        }

        /// <summary>
        /// Consulta as últimas exportações/importações de acordo com os filtros
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tpc_id">Id do periodo calendario</param>
        public static DataTable SelectBy_Filtros(
            int esc_id,
            int uni_id,
            int cal_id,
            string tur_codigo,
            int tpc_id,
            bool paginado,
            int currentPage,
            int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            CLS_ArquivoEfetivacaoDAO dao = new CLS_ArquivoEfetivacaoDAO();
            DataTable dt = dao.SelectBy_Filtros(esc_id, uni_id, cal_id, tur_codigo, tpc_id, paginado, currentPage / pageSize, pageSize, out totalRecords);

            return dt;
        }

        /// <summary>
        /// Seleciona os dados para validação na análise dos registros do arquivo
        /// de importação de fechamento.
        /// </summary>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="ent_id">ID da entidade do usuário.</param>
        /// <returns></returns>
        internal static List<sTurmasAlunosEscalaAvaliacao> SelecionaDadosValidacaoAnaliseImportacao(int cal_id, int tpc_id, long tur_id, Guid ent_id)
        {
            CLS_ArquivoEfetivacaoDAO dao = new CLS_ArquivoEfetivacaoDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                DataTable dt = dao.SelecionaDadosValidacaoAnaliseImportacao(cal_id, tpc_id, tur_id, ent_id);

                // seleciona os períodos do calendário.
                var turmaPeriodos = from turma in dt.Rows.Cast<DataRow>().GroupBy(dr => new { tur_id = Convert.ToInt64(dr["tur_id"]), cal_id = Convert.ToInt32(dr["cal_id"]) }).Select(p => p.Key)
                                    select new
                                    {
                                        tur_id = turma.tur_id
                                        ,
                                        periodos = (from DataRow dr in ACA_TipoPeriodoCalendarioBO.SelecionaTodosPor_EventoEfetivacao(turma.cal_id, -1, turma.tur_id, ent_id, banco).Rows
                                                    join tur in dt.Rows.Cast<DataRow>().GroupBy(g => new { tur_id = Convert.ToInt64(g["tur_id"]), tpc_id = Convert.ToInt64(g["tpc_id"]) }).Where(p => p.Key.tur_id == turma.tur_id)
                                                    on Convert.ToInt32(dr["tpc_id"]) equals tur.Key.tpc_id
                                                    select new sPeriodoTurma
                                                    {
                                                        tpc_nome = dr["tpc_descricao"].ToString()
                                                        ,
                                                        tpc_id = Convert.ToInt32(dr["tpc_id"])
                                                        ,
                                                        periodoVigente = Convert.ToBoolean(dr["Vigente"])
                                                        ,
                                                        ava_id = Convert.ToInt32(tur.First()["ava_id"])
                                                    }).ToList()
                                    };

                //Retorna estrutura com todos os dados para validção dos registros do arquivo importado.
                return (from DataRow dr in dt.Rows
                        group dr
                        by new
                            {
                                tur_id = Convert.ToInt64(dr["tur_id"])
                                ,
                                tud_id = Convert.ToInt64(dr["tud_id"])
                            }
                            into grupo
                            let alunos = (from DataRow g in grupo
                                          group g by new { alu_id = Convert.ToInt64(g["alu_id"]), mtu_id = Convert.ToInt32(g["mtu_id"]), mtd_id = Convert.ToInt32(g["mtd_id"]) }
                                          into gAlunos
                                          select new sAlunoImportacao
                                          {
                                              alu_id = gAlunos.Key.alu_id
                                              ,
                                              mtu_id = gAlunos.Key.mtu_id
                                              ,
                                              mtd_id = gAlunos.Key.mtd_id
                                              ,
                                              mtu_numeroChamada = Convert.ToInt32(gAlunos.First()["mtu_numeroChamada"])
                                              ,
                                              alc_matricula = gAlunos.First()["alc_matricula"].ToString()
                                              ,
                                              atd_ausenciasCompensadas = Convert.ToInt32(gAlunos.First()["atd_ausenciasCompensadas"])
                                          }).ToList()
                            let esa_tipo = Convert.ToByte(grupo.First()["esa_tipo"])
                            let ean_menorValor = esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica ? grupo.First()["ean_menorValor"].ToString() : ""
                            let ean_maiorValor = esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica ? grupo.First()["ean_maiorValor"].ToString() : ""
                            let ean_variacao = esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica ? grupo.First()["ean_variacao"].ToString() : ""
                            let valoresEscalaAvalacao = esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres ?
                                                                    (from DataRow row in grupo
                                                                     orderby Convert.ToInt32(row["eap_ordem"])
                                                                     group row by row["eap_valor"].ToString()
                                                                     into gEscala
                                                                     select gEscala.Key).ToList()
                                                                    : new List<string>()
                            select new sTurmasAlunosEscalaAvaliacao
                            {
                                tur_id = grupo.Key.tur_id
                                ,
                                tur_codigo = grupo.First()["tur_codigo"].ToString()
                                ,
                                esc_id = Convert.ToInt32(grupo.First()["esc_id"])
                                ,
                                uni_id = Convert.ToInt32(grupo.First()["uni_id"])
                                ,
                                cal_id = Convert.ToInt32(grupo.First()["cal_id"])
                                ,
                                esc_codigo = grupo.First()["esc_codigo"].ToString()
                                ,
                                esc_nome = grupo.First()["esc_nome"].ToString()
                                ,
                                permiteImportacao = Convert.ToBoolean(grupo.First()["permiteImportacao"])
                                ,
                                tud_id = grupo.Key.tud_id
                                ,
                                tud_codigo = grupo.First()["tud_codigo"].ToString()
                                ,
                                tud_nome = grupo.First()["tud_nome"].ToString()
                                ,
                                tud_tipo = Convert.ToByte(grupo.First()["tud_tipo"])
                                ,
                                nomeDocente = grupo.First()["nomeDocente"].ToString()
                                ,
                                esa_tipo = esa_tipo
                                ,
                                ean_menorValor = ean_menorValor
                                ,
                                ean_maiorValor = ean_maiorValor
                                ,
                                ean_variacao = ean_variacao
                                ,
                                fav_id = Convert.ToInt32(grupo.First()["fav_id"])
                                ,
                                listaValorEscalaAvaliacao = valoresEscalaAvalacao
                                ,
                                listaAlunos = alunos
                                ,
                                listaPeriodos = turmaPeriodos.Where(p => p.tur_id == grupo.Key.tur_id).First().periodos
                            }).ToList();
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        #endregion

        #region Importacao de arquivo de fechamento

        /// <summary>
        /// Valida os dados presentes no nome do arquivo.
        /// </summary>
        /// <param name="nomeDisciplina">Nome disciplina.</param>
        /// <param name="nomeArquivo">Nome do arquivo.</param>
        /// <param name="listaTurmaAlunoEscalaAvaliacao">Lista de dados para validação.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns></returns>
        private static bool ValidarNomeArquivo(string nomeDisciplina, string nomeArquivo, List<sTurmasAlunosEscalaAvaliacao> listaTurmaAlunoEscalaAvaliacao, Guid ent_id)
        {
            string[] partesNome = nomeArquivo.Split('_');
            
            // Código da escola
            string parteNome = partesNome[0];
            if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.esc_codigo.Replace(' ','-') == parteNome))
                throw new ValidationException("Código da escola no nome do arquivo é inválido ou não corresponde à escola da turma selecionada.");

            // Código da turma
            parteNome = partesNome[1];
            if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.tur_codigo.Replace(' ', '-') == parteNome))
                throw new ValidationException("Código da turma no nome do arquivo é inválido ou não corresponde à turma selecionada.");

            // Ano e bimestre
            parteNome = partesNome[2];
            string sAno = parteNome.Split('-')[0];
            string sBimestre = string.Join(" ", parteNome.Split('-').Skip(1).ToArray());
            int ano;

            if (!Int32.TryParse(sAno, out ano))
                throw new ValidationException("Ano inválido no nome do arquivo.");
            else if (ano != DateTime.Now.Year)
                throw new ValidationException("Arquivo deve ser do ano vigente.");

            string nomeBimestre = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(ent_id);

            if (string.IsNullOrEmpty(sBimestre) || !listaTurmaAlunoEscalaAvaliacao.Any(p => p.listaPeriodos.Any(q => q.tpc_nome == sBimestre)))
                throw new ValidationException(String.Format("{0} inválido no nome do arquivo.", nomeBimestre));

            return true;
        }

        /// <summary>
        /// Analisa os registro da importação referente ao cadastro em
        /// lote de fechamento de bimestre
        /// </summary>
        /// <param name="nomeDisciplina">Nome padrão da disciplina.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tpc_id">ID do período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="arquivo">Arquivo de importação</param>
        /// <param name="nomeArquivo">Nome do arquivo selecionado.</param>
        /// <param name="sucesso">Quantidade de registros processados com sucesso.</param>
        /// <param name="erro">Quantidade de registros processados com erro.</param>
        /// <returns></returns>
        public static List<LoteFechamento> AnalisarRegistrosLote(string nomeDisciplina, Guid ent_id, int cal_id, int tpc_id, long tur_id, Stream arquivo, string nomeArquivo, out int sucesso, out int erro)
        {
            sucesso = erro = 0;

            List<LoteFechamento> lista = new List<LoteFechamento>();

            List<sTurmasAlunosEscalaAvaliacao> listaTurmaAlunoEscalaAvaliacao = SelecionaDadosValidacaoAnaliseImportacao(cal_id, tpc_id, tur_id, ent_id);

            //ValidarNomeArquivo(nomeDisciplina, nomeArquivo, listaTurmaAlunoEscalaAvaliacao);

            using (StreamReader reader = new StreamReader(arquivo, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                // Lê registro para pular cabeçalho/primeira linha
                if (!reader.EndOfStream)
                    reader.ReadLine();

                LoteFechamento fechamento;

                // Não inverter ordem das validações!
                while (!reader.EndOfStream)
                {
                    fechamento = new LoteFechamento();

                    try
                    {
                        char separator;
                        string line = reader.ReadLine();

                        // Verifica a estrutura do registro
                        GestaoEscolarUtilBO.VerificarEstruturaRegistro(line.Replace(",","."), 15, out separator);

                        string[] array = (from item in line.Split(separator)
                                          select item.Trim()).ToArray();

                        fechamento.codigoEscola = array[(byte)eLoteFechamento.CodigoEscola];

                        #region Validação código escola

                        //if (string.IsNullOrEmpty(fechamento.codigoEscola))
                        //    fechamento.mensagem += "- Código da escola é obrigatório.<br />";
                        //else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.esc_codigo == fechamento.codigoEscola))
                        //    fechamento.mensagem += "- Código da escola é inválido ou não corresponde à escola do arquivo.<br />";

                        if (string.IsNullOrEmpty(fechamento.codigoEscola))
                            fechamento.mensagem += "- Código da escola é obrigatório.<br />";
                        else
                        {
                            Dictionary<long, string> dicCodigos = new Dictionary<long, string>();

                            foreach (string esc_codigo in listaTurmaAlunoEscalaAvaliacao.Select(p => p.esc_codigo))
                            {
                                long codigoEscola;

                                if (Int64.TryParse(esc_codigo, out codigoEscola) && !dicCodigos.Any(p => p.Key == codigoEscola))
                                {
                                    dicCodigos.Add(codigoEscola, esc_codigo);
                                }
                            }

                            long codigoEscolaArquivo;

                            if (Int64.TryParse(fechamento.codigoEscola, out codigoEscolaArquivo))
                            {
                                if (!(dicCodigos.Any(p => p.Key == codigoEscolaArquivo) || listaTurmaAlunoEscalaAvaliacao.Any(p => p.esc_codigo == fechamento.codigoEscola)))
                                    fechamento.mensagem += "- Código da escola é inválido ou não corresponde à escola do arquivo.<br />";
                                else
                                    fechamento.codigoEscola = dicCodigos.Where(p => p.Key == codigoEscolaArquivo).First().Value;
                            }
                            else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.esc_codigo == fechamento.codigoEscola))
                                fechamento.mensagem += "- Código da escola é inválido ou não corresponde à escola do arquivo.<br />";
                        }

                        #endregion Validação código escola

                        fechamento.codigoTurma = array[(byte)eLoteFechamento.CodigoTurma];

                        #region Validacao código turma

                        if (string.IsNullOrEmpty(fechamento.codigoTurma))
                            fechamento.mensagem += "- Código da turma é obrigatório.<br />";
                        else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.tur_codigo == fechamento.codigoTurma))
                            fechamento.mensagem += "- Código da turma é inválido ou não pertence à escola do arquivo.<br />";

                        #endregion Validacao código turma

                        fechamento.disciplina = array[(byte)eLoteFechamento.Disciplina];

                        #region Validação Disciplina

                        if (string.IsNullOrEmpty(fechamento.disciplina))
                            fechamento.mensagem += String.Format("- {0} é obrigatório.<br />", nomeDisciplina);
                        else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.tud_nome == fechamento.disciplina))
                            fechamento.mensagem += String.Format("- {0} é inválido.<br />", nomeDisciplina);

                        #endregion Validação Disciplina

                        fechamento.codigoDisciplina = array[(byte)eLoteFechamento.CodigoDisciplina];

                        #region Validação código disciplina

                        if (string.IsNullOrEmpty(fechamento.codigoDisciplina))
                            fechamento.mensagem += String.Format("- Código de {0} é obrigatório.<br />", nomeDisciplina);
                        else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.tud_codigo == fechamento.codigoDisciplina && p.tur_codigo == fechamento.codigoTurma && p.esc_codigo == fechamento.codigoEscola))
                            fechamento.mensagem += String.Format("- Código de {0} é inválido ou não pertence à turma.<br />", nomeDisciplina);

                        if (string.IsNullOrEmpty(fechamento.codigoDisciplina))
                            fechamento.mensagem += String.Format("- Código de {0} é obrigatório.<br />", nomeDisciplina);
                        else
                        {
                            Dictionary<int, string> dicCodigos = new Dictionary<int, string>();

                            foreach (string tud_codigo in listaTurmaAlunoEscalaAvaliacao.Select(p => p.tud_codigo))
                            {
                                int codigoDisciplina;

                                if (Int32.TryParse(tud_codigo, out codigoDisciplina) && !dicCodigos.Any(p => p.Key == codigoDisciplina))
                                {
                                    dicCodigos.Add(codigoDisciplina, tud_codigo);
                                }
                            }

                            int codigoDisciplinaArquivo;

                            if (Int32.TryParse(fechamento.codigoDisciplina, out codigoDisciplinaArquivo))
                            {
                                if (!(dicCodigos.Any(p => p.Key == codigoDisciplinaArquivo) || listaTurmaAlunoEscalaAvaliacao.Any(p => p.tud_codigo == fechamento.codigoDisciplina && p.tur_codigo == fechamento.codigoTurma && p.esc_codigo == fechamento.codigoEscola)))
                                    fechamento.mensagem += String.Format("- Código de {0} é inválido ou não pertence à turma.<br />", nomeDisciplina);
                                else
                                    fechamento.codigoDisciplina = dicCodigos.Where(p => p.Key == codigoDisciplinaArquivo).First().Value;
                            }
                            else if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.tud_codigo == fechamento.codigoDisciplina && p.tur_codigo == fechamento.codigoTurma && p.esc_codigo == fechamento.codigoEscola))
                                fechamento.mensagem += String.Format("- Código de {0} é inválido ou não pertence à turma.<br />", nomeDisciplina);
                        }

                        #endregion Validação código disciplina

                        fechamento.docente = array[(byte)eLoteFechamento.Docente];

                        //#region Validação docente

                        //if (!string.IsNullOrEmpty(fechamento.docente))
                        //{
                        //    if (!listaTurmaAlunoEscalaAvaliacao.Any(p => p.nomeDocente == fechamento.docente))
                        //        fechamento.mensagem += "- O docente não está vigente na turma.<br />";
                        //}
                        //else
                        //    fechamento.mensagem += "- Docente é obrigatório.<br />";

                        //#endregion

                        sTurmasAlunosEscalaAvaliacao turmaAlunoEscalaAvaliacao = listaTurmaAlunoEscalaAvaliacao.Find(p => p.tur_codigo == fechamento.codigoTurma &&
                                                                                                                           p.esc_codigo == fechamento.codigoEscola &&
                                                                                                                           p.tud_nome == fechamento.disciplina);// &&
                                                                                                                           //p.nomeDocente == fechamento.docente);

                        if (turmaAlunoEscalaAvaliacao.esc_id > 0)
                        {
                            fechamento.nomeEscola = turmaAlunoEscalaAvaliacao.esc_nome;
                            fechamento.esc_id = turmaAlunoEscalaAvaliacao.esc_id;
                            fechamento.uni_id = turmaAlunoEscalaAvaliacao.uni_id;

                            if (!turmaAlunoEscalaAvaliacao.permiteImportacao)
                                fechamento.mensagem = String.Format("- A escola não pode importar dados para o {0}.<br />", GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(ent_id));
                        }

                        if (turmaAlunoEscalaAvaliacao.tur_id > 0)
                        {
                            fechamento.tur_id = turmaAlunoEscalaAvaliacao.tur_id;
                            fechamento.cal_id = turmaAlunoEscalaAvaliacao.cal_id;
                            fechamento.fav_id = turmaAlunoEscalaAvaliacao.fav_id;
                        }

                        if (turmaAlunoEscalaAvaliacao.tud_id > 0)
                            fechamento.tud_id = turmaAlunoEscalaAvaliacao.tud_id;

                        #region Validação número de chamada

                        int numeroChamada;
                        if (Int32.TryParse(array[(byte)eLoteFechamento.NumeroChamada], out numeroChamada))
                        {
                            fechamento.numeroChamada = numeroChamada;
                            if (turmaAlunoEscalaAvaliacao.listaAlunos != null && !turmaAlunoEscalaAvaliacao.listaAlunos.Any(a => a.mtu_numeroChamada == numeroChamada))
                                fechamento.mensagem += String.Format("- Número de chamada {0} não pertence à turma {1}.<br />", numeroChamada.ToString(), fechamento.codigoTurma);
                        }
                        else
                            fechamento.mensagem += "- Número de chamada é inválido.<br />";

                        #endregion Validação número de chamada

                        fechamento.raAluno = array[(byte)eLoteFechamento.RaAluno];

                        #region Validação número de matrícula

                        if (string.IsNullOrEmpty(fechamento.raAluno))
                            fechamento.mensagem += String.Format("- {0} é obrigatório.<br />", CustomResource.GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA]"));
                        else if (turmaAlunoEscalaAvaliacao.listaAlunos != null && !turmaAlunoEscalaAvaliacao.listaAlunos.Any(a => a.alc_matricula == fechamento.raAluno))
                            fechamento.mensagem += String.Format("- {0} é inválido ou não pertence à turma.<br />", CustomResource.GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA"));

                        #endregion Validação número de matrícula

                        fechamento.nomeAluno = array[(byte)eLoteFechamento.NomeAluno];

                        #region Validação nome do aluno

                        if (string.IsNullOrEmpty(fechamento.nomeAluno))
                            fechamento.mensagem += "- Nome do aluno é obrigatório.<br />";

                        #endregion Validação nome do aluno

                        #region Validação ano
                        int ano;
                        if (Int32.TryParse(array[(byte)eLoteFechamento.Ano], out ano))
                        {
                            fechamento.ano = ano;
                            if (ano != DateTime.Now.Year)
                                fechamento.mensagem += "- Ano deve ser referente ao ano vigente.<br />";
                        }
                        else
                            fechamento.mensagem += "- Ano é inválido.<br />";

                        #endregion Validação ano

                        fechamento.bimestre = array[(byte)eLoteFechamento.Bimestre];

                        #region Validação bimestre

                        string nomeBimestre = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(ent_id);

                        if (!string.IsNullOrEmpty(fechamento.bimestre))
                        {
                            if (turmaAlunoEscalaAvaliacao.listaPeriodos != null && !turmaAlunoEscalaAvaliacao.listaPeriodos.Any(p => p.tpc_nome == fechamento.bimestre))
                                fechamento.mensagem += String.Format("- {0} é inválido.<br />", nomeBimestre);
                            else if (turmaAlunoEscalaAvaliacao.listaPeriodos != null)
                            {
                                sPeriodoTurma periodo = turmaAlunoEscalaAvaliacao.listaPeriodos.Find(p => p.tpc_nome == fechamento.bimestre);

                                if (!(periodo.periodoVigente))
                                    fechamento.mensagem += String.Format("- O {0} deve estar vigente (aberto).<br />", nomeBimestre);

                                fechamento.tpc_id = periodo.tpc_id;
                                fechamento.ava_id = periodo.ava_id;
                            }
                        }
                        else
                            fechamento.mensagem += String.Format("- {0} é obrigatório.<br />", nomeBimestre);

                        #endregion Validação bimestre

                        #region Validação aluno
                        sAlunoImportacao aluno = new sAlunoImportacao();
                        if (turmaAlunoEscalaAvaliacao.listaAlunos != null)
                        {
                            aluno = turmaAlunoEscalaAvaliacao.listaAlunos.Find(q => q.alc_matricula == fechamento.raAluno &&
                                                                                    q.mtu_numeroChamada == fechamento.numeroChamada);

                            if (aluno.alu_id > 0)
                            {
                                fechamento.alu_id = aluno.alu_id;
                                fechamento.mtu_id = aluno.mtu_id;
                                fechamento.mtd_id = aluno.mtd_id;
                            }
                            else
                            {
                                fechamento.mensagem += "- O aluno não foi encontrado no sistema.<br />";
                            }
                        }

                        #endregion

                        #region Validação aulas

                        int qtdeAulas;
                        if (!string.IsNullOrEmpty(array[(byte)eLoteFechamento.QtdeAulas]))
                        {
                            if (array[(byte)eLoteFechamento.QtdeAulas].Length > 5)
                            {
                                fechamento.mensagem += "- Quantidade de aulas deve possuir até 5 dígitos.<br />";
                            }
                            else if (Int32.TryParse(array[(byte)eLoteFechamento.QtdeAulas], out qtdeAulas))
                            {
                                if (qtdeAulas > 0)
                                    fechamento.qtdeAulas = qtdeAulas;
                                else
                                    fechamento.mensagem += "- Quantidade de aulas deve ser maior do que 0.<br />";
                            }
                            else
                                fechamento.mensagem += "- Quantidade de aulas é inválido.<br />";
                        }
                        else if (turmaAlunoEscalaAvaliacao.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia)
                            fechamento.mensagem += "- Quantidade de aulas é obrigatório.<br />";

                        #endregion Validação aulas

                        #region Validação faltas

                        if ((string.IsNullOrEmpty(array[(byte)eLoteFechamento.QtdeFaltas]) || array[(byte)eLoteFechamento.QtdeFaltas] == "-"))
                            fechamento.qtdeFaltas = -1;
                        else if (turmaAlunoEscalaAvaliacao.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia)
                        {
                            int qtdeFaltas;
                            if (Int32.TryParse(array[(byte)eLoteFechamento.QtdeFaltas], out qtdeFaltas))
                                fechamento.qtdeFaltas = qtdeFaltas;
                            else
                                fechamento.mensagem += "- Quantidade de faltas é inválido.<br />";
                        }
                        else
                            fechamento.mensagem += "- Quantidade de faltas não deve ser informado para componente de regência.<br />";

                        #endregion Validação faltas

                        #region Validação compensacoes

                        if (string.IsNullOrEmpty(array[(byte)eLoteFechamento.Compensacoes]) || array[(byte)eLoteFechamento.Compensacoes] == "-")
                            fechamento.compensacoes = 0;
                        else if (turmaAlunoEscalaAvaliacao.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia)
                        {
                            int compensacoes;
                            if (Int32.TryParse(array[(byte)eLoteFechamento.Compensacoes], out compensacoes))
                            {
                                fechamento.compensacoes = compensacoes + (aluno.alu_id > 0 ? aluno.atd_ausenciasCompensadas : 0);

                                if (fechamento.compensacoes > fechamento.qtdeFaltas)
                                    fechamento.mensagem += "- Quantidade de ausências compensadas deve ser menor ou igual à quantidade de faltas.<br />";
                            }
                            else
                                fechamento.mensagem += "- Quantidade de ausências compensadas é inválido.<br />";
                        }
                        else
                            fechamento.mensagem += "- Quantidade de ausências compensadas não deve ser informado para componente de regência.<br />";

                        #endregion Validação compensacoes

                        if (fechamento.qtdeFaltas > fechamento.qtdeAulas)
                            fechamento.mensagem += "- Quantidade de faltas deve ser menor que quantidade de aulas.<br />";

                        fechamento.nota = turmaAlunoEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Relatorios ?
                            array[(byte)eLoteFechamento.Nota] :
                            array[(byte)eLoteFechamento.Nota].Replace('.', ',');

                        #region Validação nota

                        if (string.IsNullOrEmpty(fechamento.nota) || fechamento.nota == "-")
                            fechamento.nota = "";
                        else if (turmaAlunoEscalaAvaliacao.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            switch (turmaAlunoEscalaAvaliacao.esa_tipo)
                            {
                                case (byte)EscalaAvaliacaoTipo.Numerica:
                                    {
                                        decimal nota;
                                        if (decimal.TryParse(fechamento.nota, out nota))
                                        {
                                            decimal ean_menorValor;
                                            decimal ean_maiorValor;
                                            decimal ean_variacao;

                                            if (decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_menorValor, out ean_menorValor) &&
                                                decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_maiorValor, out ean_maiorValor))
                                                fechamento.mensagem += nota >= ean_menorValor && nota <= ean_maiorValor ? "" :
                                                                       String.Format("- Nota está fora do intervalo entre {0} e {1}.<br />", ean_menorValor, ean_maiorValor);

                                            ean_variacao = decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_variacao, out ean_variacao) ? ean_variacao : (decimal)1;
                                            string variacao = Convert.ToDouble(ean_variacao).ToString();
                                            int numeroCasasDecimais = 1;
                                            if (variacao.IndexOf(",") >= 0)
                                            {
                                                numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                                            }

                                            if (nota % ean_variacao != 0)
                                                fechamento.mensagem += String.Format("- Nota deve ter variação de {0}.<br />", ean_variacao);

                                            fechamento.nota = nota.ToString(String.Format("N{0}", numeroCasasDecimais));
                                        }
                                        else
                                            fechamento.mensagem += "- Nota é inválida.<br />";

                                        break;
                                    }
                                case (byte)EscalaAvaliacaoTipo.Pareceres:
                                    {
                                        if (turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao != null)
                                        {
                                            fechamento.mensagem += turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao.Any(p => p.ToUpper() == fechamento.nota.ToUpper()) ?
                                                "" : String.Format("- Nota não está entre os valores permitidos ({0}).<br />", string.Join(",",
                                                                                                                                    turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao.ToArray()));
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        else
                            fechamento.mensagem += String.Format("- Nota não deve ser informada para {0} do tipo \"Regência\".<br />", nomeDisciplina.ToLower());

                        #endregion Validação nota

                        fechamento.notaPosConselho = turmaAlunoEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Relatorios ?
                            array[(byte)eLoteFechamento.NotaPosConselho] :
                            array[(byte)eLoteFechamento.NotaPosConselho].Replace('.', ',');

                        #region Validação nota pós-conselho

                        if (string.IsNullOrEmpty(fechamento.notaPosConselho) || fechamento.notaPosConselho == "-")
                            fechamento.notaPosConselho = "";
                        else if (turmaAlunoEscalaAvaliacao.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            switch (turmaAlunoEscalaAvaliacao.esa_tipo)
                            {
                                case (byte)EscalaAvaliacaoTipo.Numerica:
                                    {
                                        decimal nota;
                                        if (decimal.TryParse(fechamento.notaPosConselho, out nota))
                                        {
                                            decimal ean_menorValor;
                                            decimal ean_maiorValor;
                                            decimal ean_variacao;

                                            if (decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_menorValor, out ean_menorValor) &&
                                                decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_maiorValor, out ean_maiorValor))
                                                fechamento.mensagem += nota >= ean_menorValor && nota <= ean_maiorValor ? "" :
                                                                       String.Format("- Nota pós-conselho está fora do intervalo entre {0} e {1}.<br />", ean_menorValor, ean_maiorValor);

                                            ean_variacao = decimal.TryParse(turmaAlunoEscalaAvaliacao.ean_variacao, out ean_variacao) ? ean_variacao : (decimal)1;
                                            string variacao = Convert.ToDouble(ean_variacao).ToString();
                                            int numeroCasasDecimais = 1;
                                            if (variacao.IndexOf(",") >= 0)
                                            {
                                                numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                                            }

                                            if (nota % ean_variacao != 0)
                                                fechamento.mensagem += String.Format("- Nota pós-conselho deve ter variação de {0}.<br />", ean_variacao);

                                            fechamento.notaPosConselho = nota.ToString(String.Format("N{0}", numeroCasasDecimais));
                                        }
                                        else
                                            fechamento.mensagem += "- Nota pós-conselho é inválida.<br />";

                                        break;
                                    }
                                case (byte)EscalaAvaliacaoTipo.Pareceres:
                                    {
                                        if (turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao != null)
                                        {
                                            fechamento.mensagem += turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao.Any(p => p.ToUpper() == fechamento.notaPosConselho.ToUpper()) ?
                                                "" : String.Format("- Nota pós-conselho não está entre os valores permitidos ({0}).<br />", string.Join(",",
                                                                                                                                    turmaAlunoEscalaAvaliacao.listaValorEscalaAvaliacao.ToArray()));
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        else
                            fechamento.mensagem += String.Format("- Nota pós-conselho não deve ser informada para {0} do tipo \"Regência\".<br />", nomeDisciplina.ToLower());

                        #endregion Validação nota pós-conselho

                    }
                    catch (Exception ex)
                    {
                        fechamento.mensagem += String.Concat("- Não foi possível completar a análise do registro. ", ex.Message, "<br/>");
                    }
                    finally
                    {
                        if (string.IsNullOrEmpty(fechamento.mensagem))
                        {
                            sucesso++;
                            fechamento.status = LoteStatus.Sucess;
                        }
                        else
                        {
                            erro++;
                            fechamento.status = LoteStatus.Error;
                        }

                        lista.Add(fechamento);
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// O método fecha o bimestre com os dados analisados na importação.
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="listaProcessada">Lista de registros processados com sucesso.</param>
        /// <param name="entityArquivo">Entidade de arquivo selecionado para importação.</param>
        /// <param name="sucesso">Quantidade de registros importados.</param>
        /// <param name="erro">Quantidade de registros com erro.</param>
        /// <returns></returns>
        public static List<LoteFechamento> SalvarRegistrosLote(Guid ent_id, List<LoteFechamento> listaProcessada, SYS_Arquivo entityArquivo, out int sucesso, out int erro)
        {
            TalkDBTransaction banco = new CLS_ArquivoEfetivacaoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            
            sucesso = erro = 0;

            try
            {
                DataTable dtAvaliacaoTurmaDisciplina = CLS_AlunoAvaliacaoTurmaDisciplina.TipoTabela_AlunoAvaliacaoTurmaDisciplina();

                List<DataRow> ltAvaliacaoTurmaDisciplina = (from LoteFechamento lote in listaProcessada
                                                            select LoteFechamentoToDataRow(lote, dtAvaliacaoTurmaDisciplina.NewRow())).ToList();

                if (ltAvaliacaoTurmaDisciplina.Count > 0)
                    dtAvaliacaoTurmaDisciplina = ltAvaliacaoTurmaDisciplina.CopyToDataTable();

                if (!(dtAvaliacaoTurmaDisciplina.Rows.Count > 0 &&
                      CLS_AlunoAvaliacaoTurmaDisciplinaBO.ImportarDadosFechamento(dtAvaliacaoTurmaDisciplina, listaProcessada.First().tpc_id, banco)))
                {
                    throw new ValidationException("Não há registros para importar.");
                }
                else
                {
                    sucesso = dtAvaliacaoTurmaDisciplina.Rows.Count;
                    erro = 0;

                    if (SYS_ArquivoBO.Save(entityArquivo, banco))
                    {
                        CLS_ArquivoEfetivacao entity = new CLS_ArquivoEfetivacao
                        {
                            aef_id = -1,
                            esc_id = listaProcessada.First().esc_id,
                            uni_id = listaProcessada.First().uni_id,
                            cal_id = listaProcessada.First().cal_id,
                            tpc_id = listaProcessada.First().tpc_id,
                            tur_id = listaProcessada.First().tur_id,
                            arq_id = entityArquivo.arq_id,
                            aef_tipo = (short)CLS_ArquivoEfetivacaoBO.eTipoArquivoEfetivacao.Importacao,
                            aef_situacao = (short)CLS_ArquivoEfetivacaoBO.eSituacao.Ativo,
                            IsNew = true
                        };

                        CLS_ArquivoEfetivacaoBO.Save(entity, banco);
                    }
                }

                return listaProcessada;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método converte uma registro da importação de fechamento em um DataRow.
        /// </summary>
        /// <param name="lote">Registro de importação.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns></returns>
        private static DataRow LoteFechamentoToDataRow(LoteFechamento lote, DataRow dr)
        {
            dr["tud_id"] = lote.tud_id;
            dr["alu_id"] = lote.alu_id;
            dr["mtu_id"] = lote.mtu_id;
            dr["mtd_id"] = lote.mtd_id;
            dr["atd_id"] = -1;
            dr["fav_id"] = lote.fav_id;
            dr["ava_id"] = lote.ava_id;

            if (!string.IsNullOrEmpty(lote.nota))
                dr["atd_avaliacao"] = lote.nota.ToUpper();
            else
                dr["atd_avaliacao"] = DBNull.Value;

            if (!string.IsNullOrEmpty(lote.notaPosConselho))
                dr["atd_avaliacaoPosConselho"] = lote.notaPosConselho.ToUpper();
            else
                dr["atd_avaliacaoPosConselho"] = DBNull.Value;

            if (lote.qtdeAulas > 0)
                dr["atd_numeroAulas"] = lote.qtdeAulas;
            else
                dr["atd_numeroAulas"] = DBNull.Value;

            if (lote.qtdeFaltas > -1)
                dr["atd_numeroFaltas"] = lote.qtdeFaltas;
            else
                dr["atd_numeroFaltas"] = DBNull.Value;

            if (lote.compensacoes > 0)
                dr["atd_ausenciasCompensadas"] = lote.compensacoes;
            else
                dr["atd_ausenciasCompensadas"] = DBNull.Value;

            dr["atd_registroexterno"] = true;

            dr["atd_frequencia"] = DBNull.Value;
            dr["atd_comentarios"] = DBNull.Value;
            dr["atd_relatorio"] = DBNull.Value;
            dr["atd_semProfessor"] = DBNull.Value;
            dr["atd_situacao"] = 1;
            dr["arq_idRelatorio"] = DBNull.Value;
            dr["atd_justificativaPosConselho"] = DBNull.Value;
            dr["atd_frequenciaFinalAjustada"] = DBNull.Value;

            return dr;
        }

        #endregion Importacao de arquivo de fechamento
    }
}
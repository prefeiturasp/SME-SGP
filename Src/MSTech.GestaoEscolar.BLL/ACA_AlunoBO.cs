using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.WebServices.Consumer;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using Newtonsoft.Json.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Situações do aluno
    /// </summary>
    public enum ACA_AlunoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Inativo = 4
        ,

        Formado = 5
        ,

        Cancelado = 6
        ,

        EmMatricula = 7
        ,

        Excedente = 8
        ,

        Evadido = 9
        ,

        EmMovimentacao = 10
        ,

        EmPreMatricula = 11
    }
    
    /// <summary>
    /// Enumerador para guardar o tipo de dado a ser carregado para correção da migração.
    /// </summary>
    public enum eTipoDadoCorrecao
    {
        Endereco
        ,

        Historico
        ,

        Telefone
        ,

        Responsavel
        ,

        Nenhuma
    }

    /// <summary>
    /// Tipo de meio de transporte do aluno.
    /// </summary>
    public enum ACA_Aluno_MeioTransporteTipo
    {
        Pedestre = 1
        ,

        Onibus = 2
        ,

        Trem = 3
        ,

        Carro = 4
        ,

        Metro = 5
        ,

        Outros = 6
    }

    /// <summary>
    /// Tipo de tempo de deslocamento do aluno.
    /// </summary>
    public enum ACA_Aluno_TempoDeslocamentoTipo
    {
        QuinzeMinutos = 1
        ,

        TrintaMinutos = 2
        ,

        UmaHora = 3
        ,

        MaisDeUmaHora = 4
    }

    /// <summary>
    /// Situações do aluno
    /// </summary>
    public enum ACA_AlunoTipoMensagem : byte
    {
        QuantidadeVagasTurma = 1
        ,

        AlunoExcedente = 2
        ,

        MovimentacaoRetroativa = 3
        ,

        DadosMovimentacao = 4
        ,

        ValidacaoAlunoOriundoCrecheConveniada = 5
    }

    #endregion Enumeradores

    #region Estruturas

    public struct DadosAlunoPessoa
    {
        public long alu_id { get; set; }

        public string pes_nome { get; set; }

        public DateTime pes_dataNascimento { get; set; }

        public string pesMae_nome { get; set; }

        public DateTime pes_dataCriacao { get; set; }

        public DateTime pes_dataAlteracao { get; set; }

        public byte alu_situacao { get; set; }
    }

    /// <summary>
    /// Estrutura utilizada para redimensionar as fotos dos alunos.
    /// </summary>
    public struct FotoAluno
    {
        public long alu_id { get; set; }

        public Guid pes_id { get; set; }

        public long arq_idFoto { get; set; }

        public string arq_typeMIME { get; set; }
    }

    public class AlunoSCA
    {
        public DadosPessoais dadosPessoais;
        public List<HistoricoEscolar> historicoEscolar;
        public List<ObservacoesDoHistorico> observacoesDoHistorico;
        public List<MateriasDoHistorico> materiasDoHistorico;
        public List<SituacaoDoAnoAnterior> situacaoDoAnoAnterior;
        public DependenciasDoAluno dependenciasDoAluno;
        public RodapeArquivo rodapeArquivo;

        #region Estruturas

        public struct DadosPessoais
        {
            public string NOME_DO_ALUNO { get; set; }

            public string CODIGO_DO_ALUNO { get; set; }

            public string DATA_DE_NASCIMENTO { get; set; }

            public string COR { get; set; }

            public string NIS_ALUNO { get; set; }

            public string NIS_RESPONSAVEL { get; set; }

            public string TIPO_CERTIDAO { get; set; }

            public string NUMERO_CERTIDAO { get; set; }

            public string NUM_MATRICULA_CERT { get; set; }

            public string FOLHA_CERTIDAO { get; set; }

            public string LIVRO_CERTIDAO { get; set; }

            public string DATA_CERTIDAO { get; set; }

            public string UF_CARTORIO { get; set; }

            public string NOME_CARTORIO { get; set; }

            public string SEXO { get; set; }

            public string NACIONALIDADE { get; set; }

            public string UF { get; set; }

            public string CIDADE { get; set; }

            public string NOME_PAI { get; set; }

            public string NOME_MAE { get; set; }

            public string PROFISSAO_PAI { get; set; }

            public string PROFISSAO_MAE { get; set; }

            public string RELIGIAO { get; set; }

            public string PARENTESCO_DO_RESPONSAVEL { get; set; }

            public string NOME_DO_RESPONSAVEL { get; set; }

            public string ENDEREÇO { get; set; }

            public string BAIRRO { get; set; }

            public string CEP { get; set; }

            public string TELEFONE { get; set; }

            public string EDUCACAO_ESPECIAL { get; set; }

            public string ATENDIMENTO_ESPECIAL { get; set; }

            public string TEMPO_DE_DESLOCAMENTO { get; set; }

            public string MEIO_DE_TRANSPORTE { get; set; }

            public string FORMA_DE_REGRESSO { get; set; }

            public string PROBLEMAS_DE_SAUDE { get; set; }

            public string CONTATO { get; set; }

            public string FONE_CONTATO { get; set; }

            public string AULA_RELIGIAO { get; set; }

            public string FREQUENTOU_EI { get; set; }

            public string PAI_FALECIDO { get; set; }

            public string MAE_FALECIDA { get; set; }

            public string MAE_MORA_COM_ALUNO { get; set; }

            public string INSTRUCAO_PAI { get; set; }

            public string INSTRUCAO_MAE { get; set; }

            public string PAI_MORA_COM_ALUNO { get; set; }

            public string DESCRICAO_INST_ESPECIAL { get; set; }

            public string RESULTADO { get; set; }

            public string FREQ1 { get; set; }

            public string FREQ2 { get; set; }

            public string COC_RECURSO { get; set; }
        }

        public struct HistoricoEscolar
        {
            public string CODIGO_DO_ALUNO { get; set; }

            public string CODIGO_DA_SERIE { get; set; }

            public string ANO_LETIVO { get; set; }

            public string NOME_DA_ESCOLA { get; set; }

            public string RESULTADO { get; set; }

            public string DEPENDENCIA { get; set; }

            public string DATA_DE_INCLUSAO { get; set; }

            public string CODIGO_DO_USUARIO_INCLUSAO { get; set; }

            public string CODIGO_DE_DESIGNACAO { get; set; }

            public string DATA_DE_ALTERACAO { get; set; }

            public string CODIGO_DO_USUÁRIO_ALTERACAO { get; set; }

            public string NUM_OBS { get; set; }

            public string UF { get; set; }

            public string IND_DEF_CONCEI { get; set; }
        }

        public struct ObservacoesDoHistorico
        {
            public string CODIGO_DO_ALUNO { get; set; }

            public string NUMERO_OBS { get; set; }

            public string DESCRICAO_DA_OBSERVACAO { get; set; }

            public string IND_DEF_CONCEI { get; set; }

            public string STATUS { get; set; }

            public string DATA_DE_INCLUSAO { get; set; }

            public string CODIGO_DO_USUARIO_INCLUSAO { get; set; }

            public string DATA_DE_ALTERACAO { get; set; }

            public string CODIGO_DO_USUARIO_ALTERACAO { get; set; }

            public string COD_OBS { get; set; }

            public string IND_BLOQUEIO { get; set; }
        }

        public struct MateriasDoHistorico
        {
            public string CODIGO_DO_ALUNO { get; set; }

            public string ANO_LETIVO { get; set; }

            public string CODIGO_DA_MATERIA { get; set; }

            public string INDICACAO_DE_DEPENDENCIA { get; set; }

            public string CODIGO_DA_AVALIACAO { get; set; }

            public string PERCENTUAL_DE_FREQUENCIA { get; set; }

            public string DATA_DE_INCLUSAO { get; set; }

            public string CODIGO_DO_USUARIO_INCLUSAO { get; set; }

            public string DATA_DE_ALTERACAO { get; set; }

            public string CODIGO_DO_USUARIO_ALTERACAO { get; set; }

            public string INDICACAO_DO_RESULTADO { get; set; }

            public string INDICACAO_DE_FREQUENCIA_SUPERIOR_A_75 { get; set; }

            public int ESC_ID { get; set; }
        }

        public struct SituacaoDoAnoAnterior
        {
            public string CODIGO_DA_ESCOLA { get; set; }

            public string ANO_LETIVO { get; set; }

            public string CODIGO_DO_ALUNO { get; set; }

            public string CODIGO_DA_SERIE { get; set; }

            public string CODIGO_DA_MATERIA { get; set; }

            public string NUM_COC_REFERENCIA { get; set; }

            public string QUANTIDADE_DE_FALTAS { get; set; }

            public string DEPENDENCIA { get; set; }

            public string DATA_DE_INCLUSAO { get; set; }

            public string CODIGO_DO_USUARIO_INCLUSAO { get; set; }

            public string DATA_DE_ALTERACAO { get; set; }

            public string CODIGO_DO_USUARIO_ALTERACAO { get; set; }

            public string CODIGO_CONCEITO { get; set; }

            public string PRC_FREQ { get; set; }

            public string IND_RESULTADO { get; set; }

            public string NUM_NOTA { get; set; }

            public string cur_id { get; set; }

            public string crr_id { get; set; }

            public string crp_id { get; set; }
        }

        public struct DependenciasDoAluno
        {
            public string CODIGO_DA_MATERIA { get; set; }

            public string IND_DEPEND { get; set; }
        }

        public struct RodapeArquivo
        {
            public string CODIGO_DA_ULTIMA_SERIE_CURSADA { get; set; }

            public string CIDADE { get; set; }

            public string SERIE { get; set; }

            public string NAOCONSTAPAI { get; set; }

            public string CONSTAMAE { get; set; }

            public string NUMERO_UP_PEJA { get; set; }

            public string CODIGO_MOVIMENTACAO { get; set; }

            public string CODIGO_ESCOLA_ORIGEM { get; set; }
        }

        #endregion Estruturas
    }

    public class DadosAluno
    {
        public ACA_Aluno aluno { get; set; }

        public string NIS { get; set; }

        public int esc_id { get; set; }

        public int cur_id { get; set; }

        public int crr_id { get; set; }

        public int crp_id { get; set; }

        public long tur_id { get; set; }

        public string alc_matricula { get; set; }
    }

    public struct AlunosTds
    {
        public long alu_id { get; set; }

        public int tds_id { get; set; }
    }

    #endregion Estruturas

    #region Excessões

    /// <summary>
    /// Classe de excessão referente à entidade ACA_Aluno.
    /// Utilizada nas telas de cadastro, para identificar se houve erro de validação
    /// na entidade do aluno.
    /// </summary>
    public class ACA_Aluno_ValidationException : ValidationException
    {
        public ACA_Aluno_ValidationException(string message)
            : base(message)
        {
        }
    }

    public class EditarAluno_ValidationException : ValidationException
    {
        public EditarAluno_ValidationException(string message)
            : base(message)
        {
        }
    }

    public class ParametroNisException : Exception
    {
        public ParametroNisException(string Mensagem)
            : base(Mensagem)
        {
        }
    }

    public class AlunoExistenteException : Exception
    {
        public AlunoExistenteException(string Mensagem)
            : base(Mensagem)
        {
        }

        public AlunoExistenteException()
        {
        }
    }

    public class DuplicidadeBuscaFoneticaException : Exception
    {
        public DuplicidadeBuscaFoneticaException(string Mensagem)
            : base(Mensagem)
        {
        }

        public DuplicidadeBuscaFoneticaException()
        {
        }
    }

    public class AlunoExcedenteException : Exception
    {
        public AlunoExcedenteException(string Mensagem)
            : base(Mensagem)
        {
        }
    }

    #endregion Excessões

    public class ACA_AlunoBO : BusinessBase<ACA_AlunoDAO, ACA_Aluno>
    {
        #region Temporário

        /// <summary>
        /// O método redimensiona as fotos cadastradas para os alunos.
        /// </summary>
        /// <param name="proporcaoHorizontal">Se verdadeiro, calcula a proporção do redimensionamento pela largura.</param>
        /// <returns></returns>
        public static bool RedimensionarFotoAluno(bool proporcaoHorizontal, FotoAluno foto)
        {
            bool salvou = false;

            TalkDBTransaction bancoGestao = new ACA_AlunoDAO()._Banco.CopyThisInstance();
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            TalkDBTransaction bancoCore = new CFG_ArquivoDAO()._Banco.CopyThisInstance();
            bancoCore.Open(IsolationLevel.ReadCommitted);
            try
            {
                ACA_AlunoDAO daoAluno = new ACA_AlunoDAO { _Banco = bancoGestao };

                CFG_Arquivo entFoto = new CFG_Arquivo { arq_id = foto.arq_idFoto };
                CFG_ArquivoBO.GetEntity(entFoto, bancoCore);

                entFoto.arq_data = RedimensionaFoto(entFoto.arq_data, proporcaoHorizontal);

                salvou = SalvarArquivo(entFoto, bancoCore);
                if (salvou)
                {
                    // Marca o arquivo na tabela como redimensionado.
                    daoAluno.InsereArquivoRedimensionado(foto.arq_idFoto);
                }
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                    bancoGestao.Close();

                if (bancoCore.ConnectionIsOpen)
                    bancoCore.Close();
            }
            return salvou;
        }

        /// <summary>
        /// Método que redimensiona a foto passada
        /// </summary>
        /// <param name="bufferData">Dados da foto</param>
        /// <param name="proporcaoHorizontal">Proporção da foto horizontal</param>
        /// <returns></returns>
        public static byte[] RedimensionaFoto(byte[] bufferData, bool proporcaoHorizontal)
        {
            const int larguraFinal = 200;
            const int alturaFinal = 265;

            byte[] fotoRedimensionada;

            using (Image imgOriginal = Image.FromStream(new MemoryStream(bufferData)))
            {
                float proporcao = proporcaoHorizontal ? ((float)larguraFinal / (float)imgOriginal.Width) : ((float)alturaFinal / (float)imgOriginal.Height);
                int altura = (int)(imgOriginal.Height * proporcao);
                int largura = (int)(imgOriginal.Width * proporcao);

                using (Bitmap bmpRedimensionado = new Bitmap(largura, altura, PixelFormat.Format24bppRgb))
                {
                    bmpRedimensionado.SetResolution(imgOriginal.HorizontalResolution, imgOriginal.VerticalResolution);
                    using (Graphics grImagem = Graphics.FromImage(bmpRedimensionado))
                    {
                        grImagem.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        grImagem.DrawImage(imgOriginal,
                            new Rectangle(0, 0, largura, altura),
                            new Rectangle(0, 0, imgOriginal.Width, imgOriginal.Height),
                            GraphicsUnit.Pixel);
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        bmpRedimensionado.Save
                            (
                                stream,
                                System.Drawing.Imaging.ImageFormat.Jpeg
                            );

                        stream.Seek(0, SeekOrigin.Begin);

                        fotoRedimensionada = stream.GetBuffer();
                    }

                    return fotoRedimensionada;
                }
            }
        }

        /// <summary>
        /// Método que redimensiona a foto passada
        /// </summary>
        /// <param name="bufferData">Dados da foto</param>
        /// <param name="proporcaoHorizontal">Proporção da foto horizontal</param>
        /// <returns></returns>
        public static byte[] RedimensionaFoto(byte[] bufferData, int largura, int altura)
        {
            byte[] fotoRedimensionada;

            using (Image imgOriginal = Image.FromStream(new MemoryStream(bufferData)))
            {
                using (Bitmap bmpRedimensionado = new Bitmap(largura, altura, PixelFormat.Format24bppRgb))
                {
                    bmpRedimensionado.SetResolution(imgOriginal.HorizontalResolution, imgOriginal.VerticalResolution);
                    using (Graphics grImagem = Graphics.FromImage(bmpRedimensionado))
                    {
                        grImagem.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        grImagem.DrawImage(imgOriginal,
                            new Rectangle(0, 0, largura, altura),
                            new Rectangle(0, 0, imgOriginal.Width, imgOriginal.Height),
                            GraphicsUnit.Pixel);
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        bmpRedimensionado.Save
                            (
                                stream,
                                System.Drawing.Imaging.ImageFormat.Jpeg
                            );

                        stream.Seek(0, SeekOrigin.Begin);

                        fotoRedimensionada = stream.GetBuffer();
                    }

                    return fotoRedimensionada;
                }
            }
        }

        /// <summary>
        /// Salva o arquivo (foto do aluno) na CFG_Arquivo.
        /// </summary>
        /// <param name="bmp">Imagem a ser salva como arquivo</param>
        /// <param name="entFoto">Entidade da foto.</param>
        /// <returns>ID do arquivo gerado</returns>
        public static bool SalvarArquivo(CFG_Arquivo entFoto, TalkDBTransaction banco)
        {
            string nome = entFoto.arq_nome;

            int indexExtensao = nome.LastIndexOf('.');
            int tamanhoNome = nome.Length;

            entFoto.arq_tamanhoKB = entFoto.arq_data.Length;
            entFoto.arq_situacao = 1; // Volta a situação para 1.

            if (indexExtensao > 0)
                entFoto.arq_nome = nome.Replace(nome.Substring(indexExtensao, tamanhoNome - indexExtensao), ".jpg");

            return CFG_ArquivoBO.Save(entFoto, banco);
        }

        /// <summary>
        /// Seleciona IDs da pessoa e da foto relacionados ao aluno. (todos alunos ativos)
        /// </summary>
        /// <returns></returns>
        public static List<FotoAluno> SelecionaPessoaFotoAtivos(int qtTop, TalkDBTransaction banco = null)
        {
            ACA_AlunoDAO dao = banco == null ? new ACA_AlunoDAO() : new ACA_AlunoDAO { _Banco = banco };
            return dao.SelecionaPessoaFotoAtivos(qtTop).Rows.Cast<DataRow>()
                                                  .Select(row =>
                                                          new FotoAluno
                                                          {
                                                              alu_id = Convert.ToInt64(row["alu_id"])
                                                              ,
                                                              pes_id = new Guid(row["pes_id"].ToString())
                                                              ,
                                                              arq_idFoto = Convert.ToInt64(row["arq_idFoto"])
                                                              ,
                                                              arq_typeMIME = row["arq_typeMIME"].ToString()
                                                          }).ToList();
        }

        #endregion Temporário

        #region Estruturas
               
        /// <summary>
        /// Estrutura dos alunos duplicados
        /// </summary>
        [Serializable]
        public struct AlunoDuplicado
        {
            public string numeroRegistro { get; set; }

            public string codDesignacao { get; set; }

            public string nomeEscola { get; set; }

            public string codAluno { get; set; }

            public string nomeAluno { get; set; }

            public string sexoAluno { get; set; }

            public string nascimentoAluno { get; set; }

            public string nascionalidadeAluno { get; set; }

            public string ufAluno { get; set; }

            public string cidadeAluno { get; set; }

            public string racaCorAluno { get; set; }

            public string religiaoAluno { get; set; }

            public string meioTransporte { get; set; }

            public string tempoDeslocamento { get; set; }

            public string regressaSozinho { get; set; }

            public string freqOutraInstituicao { get; set; }

            public string serie { get; set; }

            public string turma { get; set; }

            public string numeroChamada { get; set; }

            public string turno { get; set; }

            public string up { get; set; }

            public string serieOrigem { get; set; }

            public string aprovado { get; set; }

            public string endereco { get; set; }

            public string bairro { get; set; }

            public string cidade { get; set; }

            public string cep { get; set; }

            public string telefone { get; set; }

            public string contatoNome { get; set; }

            public string telefoneContato { get; set; }

            public string nomePai { get; set; }

            public string naoConstaPai { get; set; }

            public string falecidoPai { get; set; }

            public string moraComAlunoPai { get; set; }

            public string grauInstrucaoPai { get; set; }

            public string profissaoPai { get; set; }

            public string nomeMae { get; set; }

            public string naoConstaMae { get; set; }

            public string falecidoMae { get; set; }

            public string moraComalunoMae { get; set; }

            public string grauInstrucaoMae { get; set; }

            public string profissaoMae { get; set; }

            public string nomeResponsavel { get; set; }

            public string tipoResponsavel { get; set; }

            public string nisResponsavel { get; set; }

            public string nroCertidao { get; set; }

            public string tipoCertidao { get; set; }

            public string folha { get; set; }

            public string livro { get; set; }

            public string ufCartorio { get; set; }

            public string dataEmissao { get; set; }

            public string nomeCartorio { get; set; }

            public string grupoSanguineo { get; set; }

            public string fatorRh { get; set; }

            public string reacoesAlergicas { get; set; }

            public string doencasCongenitas { get; set; }

            public string ocorrenciasMedicas { get; set; }

            public Int64 tur_id { get; set; }

            public int cur_id { get; set; }

            public int crr_id { get; set; }

            public int crp_id { get; set; }

            public int cur_idOrigem { get; set; }

            public int crr_idOrigem { get; set; }

            public int crp_idOrigem { get; set; }

            public string unf_idOrigem { get; set; }

            public string unf_idCartorio { get; set; }

            public string rlg_id { get; set; }

            public string tes_idPai { get; set; }

            public string tes_idMae { get; set; }
        }

        /// <summary>
        /// Estrutura dos dados do boletim do aluno
        /// </summary>
        [Serializable]
        public struct BoletimDadosAluno
        {
            public long alu_id { get; set; }

            public int mtu_id { get; set; }

            public int ava_id { get; set; }

            public int fav_id { get; set; }

            public int alc_id { get; set; }

            public long tur_id { get; set; }

            public int mtu_numeroChamada { get; set; }

            public int cal_id { get; set; }

            public int cal_ano { get; set; }

            public string cur_nome { get; set; }

            public long arq_idFoto { get; set; }

            public string qualidade { get; set; }

            public string desempenho { get; set; }

            public string recomendacaoAluno { get; set; }

            public string recomendacaoResponsavel { get; set; }

            public string cpe_atividadeFeita { get; set; }

            public string cpe_atividadePretendeFazer { get; set; }

            public string ava_nome { get; set; }

            public string uad_nome { get; set; }

            public string esc_nome { get; set; }

            public string pes_nome { get; set; }

            public string pes_nomeOficial { get; set; }

            public string pes_nomeRegistro { get; set; }

            public string pes_nome_abreviado { get; set; }

            public string tur_codigo { get; set; }

            public int tci_id { get; set; }

            public string tci_nome { get; set; }

            public string tci_layout { get; set; }

            public bool tci_exibirBoletim { get; set; }

            public bool fechamentoPorImportacao { get; set; }

            public string recuperacaoParalela { get; set; }

            public string alc_matriculaEstadual { get; set; }

            public string alc_matricula { get; set; }

            public int cur_id { get; set; }

            public int crr_id { get; set; }

            public int crp_id { get; set; }

            public int tne_id { get; set; }

            public List<BoletimAluno> listaNotasEFaltas { get; set; }

            public string linhaTerritorioSaber { get; set; }

            public string territorioSaber { get; set; }

            public string ParecerConclusivo { get; set; }

            public string justificativaAbonoFalta { get; set; }
        }

        /// <summary>
        /// Estrutura dos dados do boletim do aluno
        /// </summary>
        [Serializable]
        public struct BoletimDadosAlunoFechamento
        {
            public long alu_id { get; set; }

            public int mtu_id { get; set; }

            public int cur_id { get; set; }

            public int crr_id { get; set; }

            public int crp_id { get; set; }

            public List<DadosFechamento> listaNotasEFaltas { get; set; }
        }

        /// <summary>
        /// Estrutura dos dados de alunos turma multisseriada do docente
        /// </summary>
        [Serializable]
        public struct DadosAlunoMultisseriadaDocente
        {
            public long alu_id { get; set; }

            public int mtu_id { get; set; }

            public int mtd_id { get; set; }
        }

        #endregion Estruturas

        #region Propriedades

        public static int numeroCursosPeja;
        public static bool VerificaBuscaMatriculaIgual { get; } = false;

        #endregion Propriedades

        #region Constantes

        private const int QTD_GERACAO_SEQ_PROTOCOLO_EXCEDENTE = 10;
       
        #endregion

        #region Conexao

        /// <summary>
        /// Retorna a ConnectionStringName
        /// </summary>
        /// <returns>String de conexão</returns>
        public static string getConnectionStringName()
        {
            return new ACA_AlunoDAO()._Banco.GetConnection.ConnectionString;
        }

        #endregion Conexao

        #region Métodos de consulta

        /// <summary>
        /// Retorna as deficiências dos alunos informados
        /// </summary>
        /// <param name="alu_id">IDs dos alunos</param>
        /// <returns></returns>
        public static List<PES_PessoaDeficiencia> SelecionaAlunos_Deficiencias(string alu_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            DataTable dt = dao.SelectAlunos_Deficiencias(alu_id);
            List<PES_PessoaDeficiencia> retorno =
            (from DataRow dr in dt.Rows
             select new PES_PessoaDeficiencia
             {
                 pes_id = new Guid(dr["pes_id"].ToString())
                 ,
                 tde_id = new Guid(dr["tde_id"].ToString())
             }).ToList();

            return retorno;
        }

        /// <summary>
        /// Retorna os dados do aluno para a exibição do boletim na area do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matricula turma a trazer os dados</param>
        /// <returns>Dados do aluno</returns>
        public static DataTable AreaAluno_DadosTurmaAtualBoletim(long alu_id, int mtu_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.AreaAluno_DadosTurmaAtualBoletim(alu_id, mtu_id);
        }

        #region Acesso Boletim Online

        public static DataTable SelectView()
        {
            return new ACA_AlunoDAO().SelectView();
        }

        /// <summary>
        /// Retorna todas as disciplinas da turma e aluno selecionados
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <returns>DataTable de disciplinas</returns>
        public static DataTable SelecionaDisciplinasPorTurmaAlunoView(long tur_id, long alu_id, int mtu_id)
        {
            return new ACA_AlunoDAO().SelectDisciplinasPorTurmaAlunoView(tur_id, alu_id, mtu_id);
        }

        /// <summary>
        /// Retorna todas as avalições da turma selecionada
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <returns>DataTable de avalições</returns>
        public static DataTable SelecionaAvaliacoesPorTurmaAlunoView(long tur_id, long alu_id, int mtu_id)
        {
            return new ACA_AlunoDAO().SelectAvaliacoesPorTurmaAlunoView(tur_id, alu_id, mtu_id);
        }

        /// <summary>
        /// Retorna todas as turmas do aluno selecionado
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>DataTable de turmas</returns>
        public static DataTable SelecionaTurmasPorAlunoView(long alu_id)
        {
            return new ACA_AlunoDAO().SelectTurmasPorAlunoView(alu_id);
        }

        /// <summary>
        /// Seleciona aluno por número de matricula na view VW_Alunos_Acesso_Boletim
        /// </summary>
        /// <param name="numMatricula">Numero de matricula</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunoAcessoBoletimView(string numMatricula)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectAlunoAcessoBoletimView(numMatricula);
        }

        #endregion Acesso Boletim Online
                
        /// <summary>
        /// Realiza a busca dos alunos de acordo com os filtros informados, escolhendo o melhor caminho de acordo
        /// os filtros que forem passados ou não.
        /// Ordem de preferência:
        /// 1 - Número de matrícula, 2 - Nome do aluno, 3 - Todos os filtros
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="tipoBusca">Tipo de busca do aluno. 1 - Contém, 2 - Começa por, 3 - Fonética</param>
        /// <param name="pes_dataNascimento">Data de nascimento do aluno</param>
        /// <param name="pes_nomeMae">Nome da mãe do aluno</param>
        /// <param name="alc_matricula">Número de matrícula</param>
        /// <param name="alc_matriculaEstadual">Número da matrícula estadual</param>
        /// <param name="alu_situacao">Situação do aluno</param>
        /// <param name="alu_dataCriacao">Data de criação do aluno</param>
        /// <param name="alu_dataAlteracao">Última data de criação do aluno</param>
        /// <param name="adm">Indica se visão do usuário é Administração</param>
        /// <param name="podeVisualizarTodos">Indica se usuário checou a opção para ver todos os alunos da rede</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo do usuário</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_PorFiltroPreferencial
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , string pes_nome
            , byte tipoBusca
            , string pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , byte alu_situacao
            , string alu_dataCriacao
            , string alu_dataAlteracao
            , bool adm
            , bool podeVisualizarTodos
            , Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , bool retornaExcedentes
            , bool retornaPreMatricula
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial = false
        )
        {
            return BuscaAlunos_PorFiltroPreferencial(
                uad_idSuperior
                , esc_id
                , uni_id
                , pes_nome
                , tipoBusca
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , alu_situacao
                , alu_dataCriacao
                , alu_dataAlteracao
                , false
                , Guid.Empty
                , adm
                , podeVisualizarTodos
                , false
                , false
                , usu_id
                , gru_id
                , ent_id
                , retornaExcedentes
                , retornaPreMatricula
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , documentoOficial
            );
        }

        /// <summary>
        /// Realiza a busca dos alunos de acordo com os filtros informados, escolhendo o melhor caminho de acordo
        /// os filtros que forem passados ou não.
        /// Ordem de preferência:
        /// 1 - Número de matrícula, 2 - Nome do aluno, 3 - Todos os filtros
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="tipoBusca">Tipo de busca do aluno. 1 - Contém, 2 - Começa por, 3 - Fonética</param>
        /// <param name="pes_dataNascimento">Data de nascimento do aluno</param>
        /// <param name="pes_nomeMae">Nome da mãe do aluno</param>
        /// <param name="alc_matricula">Número de matrícula</param>
        /// <param name="alc_matriculaEstadual">Número da matrícula estadual</param>
        /// <param name="alu_situacao">Situação do aluno</param>
        /// <param name="alu_dataCriacao">Data de criação do aluno</param>
        /// <param name="alu_dataAlteracao">Última data de criação do aluno</param>
        /// <param name="apenasDeficiente">Se busca apenas alunos com deficiencia</param>
        /// <param name="deficiencia">Deficiencia a procurar</param>
        /// <param name="adm">Indica se visão do usuário é Administração</param>
        /// <param name="podeVisualizarTodos">Indica se usuário checou a opção para ver todos os alunos da rede</param>
        /// <param name="apenasIntencaoTransferencia">Indica se serão buscados apenas alunos com intenção de transferência</param>
        /// <param name="apenasGemeo">Indica se serão buscados apenas alunos com irmão gemeo</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo do usuário</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_PorFiltroPreferencial
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , string pes_nome
            , byte tipoBusca
            , string pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , byte alu_situacao
            , string alu_dataCriacao
            , string alu_dataAlteracao
            , bool apenasDeficiente
            , Guid deficiencia
            , bool adm
            , bool podeVisualizarTodos
            , bool apenasIntencaoTransferencia
            , bool apenasGemeo
            , Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , bool retornaExcedentes
            , bool retornaPreMatricula
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial = false
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            DateTime dtNascimento = (string.IsNullOrEmpty(pes_dataNascimento)
                             ? new DateTime()
                             : Convert.ToDateTime(pes_dataNascimento));
            DateTime dtCriacao = (string.IsNullOrEmpty(alu_dataCriacao)
                                         ? new DateTime()
                                         : Convert.ToDateTime(alu_dataCriacao));
            DateTime dtAlteracao = (string.IsNullOrEmpty(alu_dataAlteracao)
                                         ? new DateTime()
                                         : Convert.ToDateTime(alu_dataAlteracao));


            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;

            if (uad_idSuperior == Guid.Empty
                && esc_id <= 0 && uni_id <= 0 && string.IsNullOrEmpty((pes_nome ?? "").Trim())
                && string.IsNullOrEmpty((pes_nomeMae ?? "").Trim())
                && dtNascimento == new DateTime() && string.IsNullOrEmpty((alc_matricula ?? "").Trim())
                && string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) && dtCriacao == new DateTime()
                && dtAlteracao == new DateTime()
                )
            {
                throw new ValidationException("É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos ativos ou inativos.");
            }

            /* Busca preferencial
             * 1 - Número de matrícula
             * 2 - Nome do aluno
             * 3 - Todos os filtros
             */
            byte buscaPreferencial = 3;

            if (!string.IsNullOrEmpty((alc_matricula ?? "").Trim()))
            {
                buscaPreferencial = 1;

                // Seta nulo na matrícula estadual, para não adicionar ele na hora de inserir os parâmetros da procedure.
                // pois essa procedure não recebe esse parâmetro.
                alc_matriculaEstadual = null;
            }
            else if (!string.IsNullOrEmpty(pes_nome))
            {
                // Se passou o nome, busca pelo nome.
                buscaPreferencial = 2;
            }

            return dao.BuscaAlunos_PorFiltroPreferencial
                (
                    uad_idSuperior
                    , esc_id
                    , uni_id
                    , pes_nome
                    , tipoBusca
                    , dtNascimento
                    , pes_nomeMae
                    , alc_matricula
                    , alc_matriculaEstadual
                    , alu_situacao
                    , dtCriacao
                    , dtAlteracao
                    , apenasDeficiente
                    , deficiencia
                    , adm
                    , podeVisualizarTodos
                    , apenasGemeo
                    , usu_id
                    , gru_id
                    , ent_id
                    , buscaMatriculaIgual
                    , buscaPreferencial
                    , retornaExcedentes
                    , retornaPreMatricula
                    , LinhasPorPagina
                    , Pagina
                    , SortDirection
                    , SortExpression
                    , out totalRecords
                    , documentoOficial
                );
        }

        /// <summary>
        /// Verifica se o campo possui valor inteiro
        /// </summary>
        /// <param name="campo"></param>
        /// <returns></returns>
        private static int VerificaInteiroVazio(string campo)
        {
            int inteiro;
            int.TryParse(campo, out inteiro);

            return inteiro;
        }

        /// <summary>
        /// Retorna string truncada
        /// </summary>
        /// <param name="valor">string a ser truncada</param>
        /// <param name="largura">tamanho máximo da string</param>
        /// <returns></returns>
        public static string RetornaStringTruncada(string valor, int largura)
        {
            if (!String.IsNullOrEmpty(valor))
            {
                if (valor.Length > largura)
                    return valor.Substring(0, largura);
                return valor;
            }
            return string.Empty;
        }

        /// <summary>
        /// Carrega os dados na estrutura
        /// </summary>
        /// <param name="numRegistro">Número do registro</param>
        /// <param name="dados">Itens</param>
        /// <returns>Estrutura com os dados</returns>
        private static AlunoDuplicado CarregarEstrutura(int numRegistro, DataRow dados)
        {
            AlunoDuplicado aluno = new AlunoDuplicado();

            aluno.numeroRegistro = numRegistro.ToString();
            aluno.codDesignacao = dados["esc_codigo"].ToString();
            aluno.nomeEscola = dados["esc_nome"].ToString();
            aluno.codAluno = dados["COD_ALUNO"].ToString();
            aluno.nomeAluno = dados["NOM_ALUNO"].ToString();
            aluno.sexoAluno = dados["IND_SEXO"].ToString();
            aluno.nascimentoAluno = Convert.ToDateTime(dados["DAT_NASCIM"]).ToString("dd/MM/yyyy");
            aluno.nascionalidadeAluno = dados["DSC_NACION"].ToString();
            aluno.ufAluno = dados["SIG_UF_NATUR"].ToString();
            aluno.cidadeAluno = dados["DSC_CIDADE_NASCIM"].ToString();
            aluno.racaCorAluno = dados["IND_COR"].ToString();
            aluno.religiaoAluno = dados["DSC_RELIGIAO"].ToString();
            aluno.meioTransporte = dados["IND_TRANSP_DESLOC"].ToString();
            aluno.tempoDeslocamento = dados["IND_TEMPO_DESLOC"].ToString();
            aluno.regressaSozinho = dados["IND_FORMA_REGR"].ToString();
            aluno.serie = dados["COD_SERIE"].ToString();
            aluno.turma = dados["NUM_TURMA"].ToString();
            aluno.numeroChamada = dados["NUM_CHAMADA"].ToString();
            aluno.turno = dados["IND_TURNO"].ToString();
            aluno.up = dados["NUM_UP_PEJ"].ToString();
            aluno.endereco = dados["END_RUA_RESP"].ToString();
            aluno.bairro = dados["END_BAIRRO_RESP"].ToString();
            aluno.cidade = dados["END_CIDADE_RESP"].ToString();
            aluno.cep = dados["END_CEP_RESP"].ToString();
            aluno.telefone = dados["NUM_FONE_RESP"].ToString();
            aluno.contatoNome = dados["NOM_CONTAT_1"].ToString();
            aluno.telefoneContato = dados["NUM_FONE_CONTAT_1"].ToString();
            aluno.nomePai = dados["NOM_PAI"].ToString();
            aluno.naoConstaPai = dados["IND_CONSTA_PAI"].ToString();
            aluno.falecidoPai = dados["IND_FALEC_PAI"].ToString();
            aluno.moraComAlunoPai = dados["IND_MORADIA_PAI"].ToString();
            aluno.grauInstrucaoPai = dados["IND_INSTR_PAI"].ToString();
            aluno.profissaoPai = dados["DSC_PROF_PAI"].ToString();
            aluno.nomeMae = dados["NOM_MAE"].ToString();
            aluno.naoConstaMae = dados["IND_CONSTA_MAE"].ToString();
            aluno.falecidoMae = dados["IND_FALEC_MAE"].ToString();
            aluno.moraComalunoMae = dados["IND_MORADIA_MAE"].ToString();
            aluno.grauInstrucaoMae = dados["IND_INSTR_MAE"].ToString();
            aluno.profissaoMae = dados["DSC_PROF_MAE"].ToString();
            aluno.nomeResponsavel = dados["NOM_RESP"].ToString();
            aluno.tipoResponsavel = dados["IND_RESP"].ToString();
            aluno.nisResponsavel = dados["NIS_RESP"].ToString();
            aluno.nroCertidao = dados["NUM_CERT"].ToString();
            aluno.tipoCertidao = dados["IND_TIPO_CERT"].ToString();
            aluno.folha = dados["FOLHA_CERT"].ToString();
            aluno.livro = dados["LIVRO_CERT"].ToString();
            aluno.ufCartorio = dados["UF_CARTORIO"].ToString();
            aluno.dataEmissao = !String.IsNullOrEmpty(dados["DAT_CERT"].ToString()) ? Convert.ToDateTime(dados["DAT_CERT"]).ToString("dd/MM/yyyy") : dados["DAT_CERT"].ToString();
            aluno.nomeCartorio = dados["NOM_CARTORIO"].ToString();
            aluno.grupoSanguineo = dados["GRUPO_SANGUINEO"].ToString();
            aluno.fatorRh = dados["FATOR_RH"].ToString();
            aluno.doencasCongenitas = dados["DSC_PROBL_SAUDE"].ToString();
            aluno.tur_id = VerificaInteiroVazio(dados["tur_id"].ToString());
            aluno.crp_id = VerificaInteiroVazio(dados["crp_id"].ToString());
            aluno.crr_id = VerificaInteiroVazio(dados["crr_id"].ToString());
            aluno.cur_id = VerificaInteiroVazio(dados["cur_id"].ToString());
            aluno.crp_idOrigem = VerificaInteiroVazio(dados["crp_idOrigem"].ToString());
            aluno.crr_idOrigem = VerificaInteiroVazio(dados["crr_idOrigem"].ToString());
            aluno.cur_idOrigem = VerificaInteiroVazio(dados["cur_idOrigem"].ToString());
            aluno.unf_idOrigem = dados["unf_idOrigem"].ToString();
            aluno.unf_idCartorio = dados["unf_idCartorio"].ToString();
            aluno.rlg_id = dados["rlg_id"].ToString();
            aluno.tes_idPai = dados["tes_idPai"].ToString();
            aluno.tes_idMae = dados["tes_idMae"].ToString();

            return aluno;
        }

        /// <summary>
        /// Seleciona os registro do aluno duplicado, de acordo com o id passado.
        /// </summary>
        /// <param name="pes_id">Id da pessoa.</param>
        /// <returns>DataTable contendo os dados de cada registro do aluno duplicado.</returns>
        public static List<AlunoDuplicado> SelecionarAlunosDuplicados
        (
            Guid pes_id
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            DataTable dtAlunosDuplicados = dao.SelecionarAlunosDuplicados(pes_id);

            List<AlunoDuplicado> listaAlunos = new List<AlunoDuplicado>();

            int numRegistro = 1;
            foreach (DataRow alunoDuplicado in dtAlunosDuplicados.Rows)
            {
                listaAlunos.Add(CarregarEstrutura(numRegistro, alunoDuplicado));
                numRegistro++;
            }

            return listaAlunos;
        }
        
        /// <summary>
        /// Seleciona os alunos ativos e matriculados na turma, ordenados por nome.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns>DataTable contendo dados dos alunos ativos e matriculados.</returns>
        public static DataTable SelecionarAlunosAtivosPorTurma
        (
            Int64 tur_id
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelecionarAlunosAtivosPorTurma(tur_id);
        }

        /// <summary>
        /// Seleciona os alunos matriculados na turma e ordena por nome ou matricula.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="order">Bit de ordenação 0 - Por matricula / 1 - Por Nome</param>
        /// <returns>DataTable contendo dados dos alunos matriculados.</returns>
        public static DataTable SelecionarAlunosPorTurma
        (
            Int64 tur_id
            , byte order
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelecionarAlunosPorTurma(tur_id, order);
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma multisseriada
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionarAlunosMatriculadosTurmaMultisseriada
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            List<MTR_MatriculaTurmaDisciplina> list = new List<MTR_MatriculaTurmaDisciplina>();

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.SelectBy_MatriculaMultisseriaAluno_TurmaMultisseriada(esc_id, uni_id, tur_id, cal_id, ent_id, adm, usu_id, gru_id);
            foreach (DataRow dr in dt.Rows)
            {
                MTR_MatriculaTurmaDisciplinaDAO daoMTD = new MTR_MatriculaTurmaDisciplinaDAO();
                list.Add(daoMTD.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina()));
            }
            return list;
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma multisseriada do docente
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionarAlunosMatriculadosTurmaMultisseriadaDocente
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , int tds_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            List<MTR_MatriculaTurmaDisciplina> list = new List<MTR_MatriculaTurmaDisciplina>();

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.SelectBy_MatriculaMultisseriadaDocente_TurmaDisciplinaMultisseriada(esc_id, uni_id, tur_id, cal_id, tds_id, ent_id, adm, usu_id, gru_id);
            foreach (DataRow dr in dt.Rows)
            {
                MTR_MatriculaTurmaDisciplinaDAO daoMTD = new MTR_MatriculaTurmaDisciplinaDAO();
                list.Add(daoMTD.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina { IsNew = false }));
            }
            return list;
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas com o mesmo curso e curriculo da turma multisseriada
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionarAlunosPeloCursoCurriculoTurmaMultisseriada
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , int tds_id
            , byte tud_tipo
            , string pes_nome
            , string tur_codigo
            , string alu_ids
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            List<MTR_MatriculaTurmaDisciplina> list = new List<MTR_MatriculaTurmaDisciplina>();
            DataTable dtAlunos = ACA_Aluno.TipoTabela_Aluno();

            if (!string.IsNullOrEmpty(alu_ids))
            {
                List<string> listaAlunos = alu_ids.Split(';').ToList();

                DataRow drAluno;
                listaAlunos.ForEach(p =>
                {
                    drAluno = dtAlunos.NewRow();
                    drAluno["alu_id"] = p;
                    dtAlunos.Rows.Add(drAluno);
                });
            }

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.SelectBy_CursoPeriodoTurmaMultisseriada(esc_id, uni_id, tur_id, cal_id, tds_id, tud_tipo, pes_nome, tur_codigo, dtAlunos, ent_id, adm, usu_id, gru_id);
            foreach (DataRow dr in dt.Rows)
            {
                MTR_MatriculaTurmaDisciplinaDAO daoMTD = new MTR_MatriculaTurmaDisciplinaDAO();
                list.Add(daoMTD.DataRowToEntity(dr, new MTR_MatriculaTurmaDisciplina()));
            }
            return list;
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas com o mesmo curso e curriculo para disciplina educação fisica
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<DadosAlunoMultisseriadaDocente> SelecionarAlunosPeloCursoCurriculoTurmaMultisseriadaDocente
        (
                int esc_id
                , int uni_id
                , Int64 tur_id
                , int cal_id
                , int tds_id
                , Guid ent_id
                , bool adm
                , Guid usu_id
                , Guid gru_id
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = new DataTable();

            dt = dao.SelectBy_CursoPeriodoTurmaMultisseriadaDocente(esc_id, uni_id, tur_id, cal_id, tds_id, ent_id, adm, usu_id, gru_id);
            List<DadosAlunoMultisseriadaDocente> lt = new List<DadosAlunoMultisseriadaDocente>();

            if (dt.Rows.Count > 0)
            {
                lt = (from dr in dt.AsEnumerable()
                      select new DadosAlunoMultisseriadaDocente
                      {
                          alu_id = Convert.ToInt64(dr["alu_id"].ToString()),
                          mtu_id = Convert.ToInt32(dr["mtu_id"].ToString()),
                          mtd_id = Convert.ToInt32(dr["mtd_id"].ToString())
                      }).ToList();
            }
            return lt;
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas eletivas por disciplina turma
        /// </summary>
        /// <param name="tud_id">ID da disciplina turma</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        public static List<MTR_MatriculaTurmaDisciplina> SelecionaAlunosMatriculadosTurmasEletivas
        (
            long tud_id
            , TalkDBTransaction banco
        )
        {
            List<MTR_MatriculaTurmaDisciplina> list = new List<MTR_MatriculaTurmaDisciplina>();

            ACA_AlunoDAO dao = new ACA_AlunoDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_MatriculaEletivasAluno_TurmaEletiva(tud_id);

            foreach (DataRow dr in dt.Rows)
            {
                MTR_MatriculaTurmaDisciplinaDAO daoMTD = new MTR_MatriculaTurmaDisciplinaDAO { _Banco = banco };
                MTR_MatriculaTurmaDisciplina ent = new MTR_MatriculaTurmaDisciplina();
                list.Add(daoMTD.DataRowToEntity(dr, ent));
            }

            return list;
        }

        
        /// <summary>
        /// Carrega a entidade aluno através do nome do aluno, data
        /// de nascimento e nome da mãe.
        /// </summary>
        /// <param name="entity">Entidade ACA_Aluno.</param>
        /// <param name="pes_nome">Nome do aluno.</param>
        /// <param name="pes_dataNascimento">Data de nascimento.</param>
        /// <param name="pes_nomeMae">Nome da mãe.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>True = Encontrou o aluno. / False = Não encontrou.</returns>
        public static bool ConsultaAluno
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , Guid ent_id
            , out ACA_Aluno entity
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Nome_DataNasc_NomeMae(pes_nome, pes_dataNascimento, pes_nomeMae, ent_id, out entity);
        }
                
        /// <summary>
        /// Retorna um datatable contendo todos os alunos que não foram excluídos logicamente,
        /// filtrados principalmente pelo docente e pelo tipo de busca por nome do aluno: 1 - Contém / 2- Começa por
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id"></param>
        /// <param name="tipobusca">Tipo de busca</param>
        /// <param name="pes_nome">Nome</param>
        /// <param name="pes_nomeMae">Nome da Mã</param>
        /// <param name="pes_dataNascimento">Data de nascimento</param>
        /// <param name="alc_matricula">numero de matricula</param>
        /// <param name="alc_matriculaEstadual">numero de matricula estadual</param>
        /// <param name="alu_dataCriacao">data de criacao</param>
        /// <param name="alu_dataAlteracao">data de alteracao</param>
        /// <param name="apenasDeficiente">apenas deficiente</param>
        /// <param name="apenasGemeo">apenas alunos que possuam irmao(s) gemeo(s)</param>
        /// <param name="deficiencia">deficiencia</param>
        /// <returns>DataTable com os alunos</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Docente
        (
            Guid ent_id
            , long doc_id
            , byte tipobusca
            , string pes_nome
            , string pes_nomeMae
            , string pes_dataNascimento
            , string alc_matricula
            , string alc_matriculaEstadual
            , DateTime alu_dataCriacao
            , DateTime alu_dataAlteracao
            , bool apenasDeficiente
            , bool apenasGemeo
            , Guid deficiencia
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
        )
        {
            totalRecords = 0;

            DateTime dataNascimento = string.IsNullOrEmpty(pes_dataNascimento) ? new DateTime() : Convert.ToDateTime(pes_dataNascimento);

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Pesquisa_Docente(ent_id, doc_id, tipobusca, pes_nome, pes_nomeMae, dataNascimento,
                                                 alc_matricula, alc_matriculaEstadual, alu_dataCriacao,
                                                 alu_dataAlteracao, apenasDeficiente, apenasGemeo, deficiencia,
                                                 LinhasPorPagina, Pagina, SortDirection, SortExpression,
                                                 out totalRecords, documentoOficial);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente, filtrados por
        /// Escola/UA, usuario, grupo, entidade, nome do aluno, matricula, situação
        /// </summary>
        public static DataTable SelecionaPorParametros
        (
            Guid ent_id
            , Guid uad_idSuperior
            , Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , string nome_aluno
            , byte tipoBusca
            , bool trazExcedentes
            , bool verificaPermissao
            , string pes_dataNascimento
            , Guid tdo_idCPF
            , string tdo_nomeCPF
            , Guid tdo_idRG
            , string tdo_nomeRG
            , string pes_nomePai
            , string pes_nomeMae
            , string ctc_numeroTermo
            , string ctc_folha
            , string ctc_livro
            , string ctc_dataEmissao
            , string alc_matricula
            , string alc_matriculaEstadual
            , byte alu_situacao

        )
        {
            totalRecords = 0;

            DateTime dataNascimento = string.IsNullOrEmpty(pes_dataNascimento) ? new DateTime() : Convert.ToDateTime(pes_dataNascimento);
            DateTime dataEmissao = string.IsNullOrEmpty(ctc_dataEmissao) ? new DateTime() : Convert.ToDateTime(ctc_dataEmissao);

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_ParametrosAluno(ent_id, uad_idSuperior, usu_id, gru_id, esc_id, uni_id, nome_aluno, tipoBusca, trazExcedentes, verificaPermissao, dataNascimento, tdo_idCPF, tdo_nomeCPF, tdo_idRG, tdo_nomeRG, pes_nomePai, pes_nomeMae, ctc_numeroTermo, ctc_folha, ctc_livro, dataEmissao, alc_matricula, alc_matriculaEstadual, alu_situacao, out totalRecords);
        }

        /// <summary>
        /// Utilizado na tela de Transferência.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="esc_id">id da escola</param>
        /// <param name="uni_id">id da unidade administrativa</param>
        /// <param name="cur_id">id do curso</param>
        /// <param name="tur_codigo">código da turma</param>
        /// <param name="tipoBusca"></param>
        /// <param name="nome_aluno">nome do aluno</param>
        /// <param name="ent_id">id entidade</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// Retorna um datatable contendo todos os alunos com a situação "Ativo",
        /// que possuem matrícula (AlunoCurriculo) também com a situação "Ativo".
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Transferencia
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , string tur_codigo
            , byte tipoBusca
            , string nome_aluno
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Transferencia(uad_idSuperior, esc_id, uni_id, cur_id, tur_codigo, tipoBusca, nome_aluno, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as pessoas no sistema,
        /// com uma flag dizendo se os alunos da entidade serão incluídos ou não na busca.
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="nome">nome da pessoa</param>
        /// <param name="cpf">cpf da pessoa</param>
        /// <param name="rg">rg da pessoa</param>
        /// <param name="nis"></param>
        /// <param name="consultaAlunos">Indica se irá incluir alunos na busca ou não</param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaPessoas
        (
            Guid ent_id
            , string nome
            , string cpf
            , string rg
            , string nis
            , bool consultaAlunos
            , int currentPage
            , int pageSize
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            // ID do tipo de documento CPF.
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            Guid tdo_idCPF = String.IsNullOrEmpty(docPadraoCPF) ? Guid.Empty : new Guid(docPadraoCPF);

            // ID do tipo de documento RG.
            string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);
            Guid tdo_idRG = String.IsNullOrEmpty(docPadraoRG) ? Guid.Empty : new Guid(docPadraoRG);

            // ID do tipo de documento NIS.
            Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, ent_id);

            return dao.SelectBuscaPessoas(ent_id, nome, cpf, rg, nis, consultaAlunos, tdo_idCPF, tdo_idRG, tdo_idNis, currentPage, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os dados para declaração de
        /// um ou mais alunos que não foram excluidos logicamente, filtrados por
        /// id do aluno e tipo de documento de RG padrão do sistema
        /// </summary>
        /// <param name="alu_ids"> string que contem os ids dos alunos</param>
        /// <param name="tipo_doc_rg"> Valor de identificação única do tipo de documento de rg padrão</param>
        /// <returns>DataTable com os alunos</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosDeclaracaoPorAluno
        (
            string alu_ids
            , Guid tipo_doc_rg
        )
        {
            ACA_AlunoDAO dal = new ACA_AlunoDAO();
            return dal.SelectBy_DadosDeclaracaoporAluno(alu_ids, tipo_doc_rg);
        }

        /// <summary>
        /// Seleciona os alunos que possuem dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="nomeAluno">Nome do aluno.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaAlunos_DadosCorrecao
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , string nomeAluno
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosCorrecao
                        (
                            uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , nomeAluno
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }

        /// <summary>
        /// Seleciona as escolas que tenham alunos com dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaQtdAlunos_DadosCorrecaoPorEscola
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosCorrecaoPorEscola
                        (
                            uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }

        /// <summary>
        /// Seleciona a quantidade a alunos com dados a corrigir nas unidades adm devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="tua_id"></param>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaQtdAlunos_DadosCorrecaoPorUA
        (
            Guid tua_id
            , Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosCorrecaoPorUA
                        (
                            tua_id
                            , uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }

        /// <summary>
        /// Seleciona os alunos que possuem dados duplicados na migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaAlunos_DadosDuplicados
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosDuplicados
                        (
                            uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , NivelSME
                            , NivelCRE
                            , NivelEscola
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }

        /// <summary>
        /// Seleciona as escolas que tenham alunos com dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaQtdAlunos_DadosDuplicadosPorEscola
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosDuplicadosPorEscola
                        (
                            uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , NivelSME
                            , NivelCRE
                            , NivelEscola
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }

        /// <summary>
        /// Seleciona a quantidade a alunos com dados duplicados nas unidades adm na migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="tua_id"></param>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public DataTable BuscaQtdAlunos_DadosDuplicadosPorUA
        (
            Guid tua_id
            , Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            return dao.SelectBy_PesquisaDadosDuplicadosPorUA
                        (
                            tua_id
                            , uad_id
                            , esc_id
                            , uni_id
                            , cur_id
                            , crr_id
                            , crp_id
                            , ent_id
                            , usu_id
                            , gru_id
                            , adm
                            , NivelSME
                            , NivelCRE
                            , NivelEscola
                            , paginado
                            , currentPage / pageSize
                            , pageSize
                            , out totalRecords
                        );
        }
                    

        /// <summary>
        /// Se encotrar a pessoa (pes_id) no cadastro de aluno
        /// retorna o codigo do aluno (alu_id) referente à essa
        /// pessoa
        /// </summary>
        /// <param name="pes_id">Id da pessoa pesquisada</param>
        /// <returns>Int64 - Id do aluno ou 0 se nao achar</returns>
        public static Int64 SelectAlunoby_pes_id(Guid pes_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectAlunoby_pes_id(pes_id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <returns></returns>
        public static DataTable SelecionaSexoDataNascPorAluno(Int64 alu_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelecionarSexoDataNascPorAluno(alu_id);
        }

        /// <summary>
        /// Seleciona alunos ativos que possuam NIS.
        /// </summary>
        /// <param name="tdo_idNIS">Tipo de documento NIS</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns></returns>
        public static List<DadosAluno> SelecionaPorNIS(Guid tdo_idNIS, Guid ent_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.SelecionaPorNIS(tdo_idNIS, ent_id);
            List<DadosAluno> lista = new List<DadosAluno>();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add
                (
                    new DadosAluno
                    {
                        aluno = dao.DataRowToEntity(dr, new ACA_Aluno())
                        ,
                        NIS = dr["NIS"].ToString()
                        ,
                        esc_id = Convert.ToInt32(dr["esc_id"])
                        ,
                        cur_id = Convert.ToInt32(dr["cur_id"])
                        ,
                        crr_id = Convert.ToInt32(dr["crr_id"])
                        ,
                        crp_id = Convert.ToInt32(dr["crp_id"])
                        ,
                        tur_id = Convert.ToInt64(dr["tur_id"])
                        ,
                        alc_matricula = dr["alc_matricula"].ToString()
                    }
                );
            }
            return lista;
        }

        public static List<DadosAluno> SelecionaPorNumeroMatricula(string alc_matricula, Guid ent_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.SelecionaPorNumeroMatricula(alc_matricula, ent_id);
            List<DadosAluno> lista = new List<DadosAluno>();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add
                (
                    new DadosAluno
                    {
                        aluno = dao.DataRowToEntity(dr, new ACA_Aluno())
                        ,
                        alc_matricula = dr["alc_matricula"].ToString()
                    }
                );
            }

            return lista;
        }

        public static bool VerificaMovimentacaoDuplicidade(string alc_matricula, Guid ent_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelecionaPorNumeroMatriculaMovimentacaoDuplicidade(alc_matricula, ent_id);
        }

        public static List<ACA_Aluno> SelecionaAlunosPorCidadeMovimentacaoAno(Guid cid_id, int ano, Guid ent_id, TalkDBTransaction bancoGestao)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO { _Banco = bancoGestao };
            DataTable dt = dao.SelecionaAlunosPorCidadeMovimentacaoAno(cid_id, ano, ent_id);
            List<ACA_Aluno> lista = new List<ACA_Aluno>();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(dao.DataRowToEntity(dr, new ACA_Aluno()));
            }

            return lista;
        }

        /// <summary>
        /// Seleciona os dados da pessoa e da matrícula do aluno por id do aluno ou escola.
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <param name="esc_id">codigo da escola</param>
        /// <returns></returns>
        public static DataTable SelecionaDadosAlunoMatricula(string alu_ids, int esc_id, DateTime dataBase)
        {
            return new ACA_AlunoDAO().SelecionaDadosAlunoMatricula(alu_ids, esc_id, dataBase);
        }

        /// <summary>
        /// retorna registros de usuário dos alunos pelos ids concatenados
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <returns></returns>
        public static DataTable SelecionaDadosFotoAlunos(string alu_ids)
        {
            return new ACA_AlunoDAO().SelecionaDadosFotoAlunos(alu_ids);
        }

        /// <summary>
        /// Seleciona os dados do aluno da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro">Parâmetro: Nome do aluno OU matrícula</param>
        /// <returns>Retorna dados do aluno</returns>
        public static DataSet VisualizaConteudo(string parametro)
        {
            return new ACA_AlunoDAO().SelecionaVisualizaConteudo(parametro);
        }


        /// <summary>
        /// Retorna os alunos matriculados que estão vinculados a uma turma.
        /// Para efetuar matricula nas turmas selecionada
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID currículo período</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static DataTable SelecionaAlunosMatriculadosSemTurma
        (
            int pfi_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Alunos_MatriculadosSemTurma(pfi_id, esc_id, uni_id, cur_id, crr_id, crp_id, ent_id, adm, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Justificativa de abono de falta".
        /// de calendários ativos
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_JustificativaAbonoFalta
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , long tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_JustificativaAbonoFalta(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords);

            return dt;
        }

        /// <summary>
        /// Seleciona os dados de pessoa do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public static DadosAlunoPessoa GetDadosAluno(long alu_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.GetDadosAluno(alu_id);
            List<DadosAlunoPessoa> lista = new List<DadosAlunoPessoa>();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add
                (
                    new DadosAlunoPessoa
                    {
                        alu_id = Convert.ToInt64(dr["alu_id"])
                        ,
                        pes_nome = dr["pes_nome"].ToString()
                        ,
                        pesMae_nome = dr["pesMae_nome"].ToString()
                        ,
                        pes_dataNascimento = Convert.ToDateTime(dr["pes_dataNascimento"])
                        ,
                        pes_dataCriacao = Convert.ToDateTime(dr["pes_dataCriacao"])
                        ,
                        pes_dataAlteracao = Convert.ToDateTime(dr["pes_dataAlteracao"])
                        ,
                        alu_situacao = Convert.ToByte(dr["alu_situacao"])
                    }
                );
            }
            return lista.Any() ? lista.First() : new DadosAlunoPessoa();
        }

        #endregion Métodos de consulta

        #region Métodos de consulta - Documentos do Aluno

        /// <summary>
        /// Retorna um datatable contendo todos os alunos e suas matriculas
        /// que não foram excluídos logicamente de calendários ativos
        /// Utilizado na tela de busca de "Documentos dos Alunos", apenas para o relatório de boletim escolar.
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_BoletimEscolar
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();
            return dal.SelectBy_PesquisaDocumentoAluno_BoletimEscolar(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos e suas matriculas
        /// que não foram excluídos logicamente de calendários ativos
        /// Utilizado na tela de busca de "Documentos dos Alunos", apenas para o relatório de HistoricoEscolarPedagogico.
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_HistoricoEscolarPedagogico
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , string alc_matricula
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();
            return dal.SelectBy_PesquisaDocumentoAluno_HistoricoEscolarPedagogico(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , alc_matricula
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , emitirDocAnoAnt
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords
                , documentoOficial);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e traz o último curriculo
        /// inativo do aluno por permissão do usuário
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// de calendários ativos
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_HistoricoEscolar
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();
            return dal.SelectBy_PesquisaDocumentoAluno_HistoricoEscolar(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// de calendários ativos
        /// para o documento de certificado de conclusao de etapa de ensino
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_CertificadoConclusaoCurso
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();
            return dal.SelectBy_PesquisaDocumentoAluno_CertificadoConclusaoCurso(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , emitirDocAnoAnt, buscaMatriculaIgual, MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_AcompanhamentoIndividual
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaAcompanhamentoIndividualAluno(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual, MostraCodigoEscola
                , out totalRecords);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<int>("CursoPeja") == 1
                                select dr).ToList().Count;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Grafico individual de notas".
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_GraficoIndividualNotas
        (
            int cal_id
            , int tpc_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaGraficoIndividualNotas(
                cal_id
                , tpc_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual, MostraCodigoEscola
                , out totalRecords);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<int>("CursoPeja") == 1
                                select dr).ToList().Count;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// de calendários ativos
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_Documentos
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaDocumentoAluno(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , emitirDocAnoAnt
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , documentoOficial
                , out totalRecords);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<bool>("CursoPeja")
                                select dr).ToList().Count;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// de calendários ativos
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_Documentos_GraficoIndividualNotas
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaDocumentoAluno_GraficoIndividualNotas(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , buscaMatriculaIgual
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords
                , documentoOficial);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<bool>("CursoPeja")
                                select dr).ToList().Count;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos docentes".
        /// de calendários ativos
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_Anotacoes
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , int cap_id
            , Int64 tud_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 && cap_id <= 0 && tud_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaAnotacoesAluno(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , cap_id
                , tud_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , emitirDocAnoAnt, buscaMatriculaIgual, MostraCodigoEscola
                , LinhasPorPagina, Pagina, SortDirection, SortExpression
                , out totalRecords
                , documentoOficial);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<bool>("CursoPeja")
                                select dr).ToList().Count;

            return dt;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e que possuem matrícula (AlunoCurriculo)
        /// Utilizado na tela de busca de "Documentos dos Alunos".
        /// de calendários ativos
        /// nao paginado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaAlunos_FichaIndividual
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool buscaMatriculaIgual = VerificaBuscaMatriculaIgual;
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            // Verificar se o usuário informou pelo menos um filtro da tela, pois se não informou, a procedure não
            // trará resultados (ficou assim pela melhoria de performance - antes dava timeout).
            if (esc_id <= 0 && uni_id <= 0 && cal_id <= 0 && cur_id <= 0 &&
                crr_id <= 0 && crp_id <= 0 && tur_id <= 0 && uad_idSuperior == Guid.Empty &&
                string.IsNullOrEmpty((pes_nome ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matricula ?? "").Trim()) &&
                string.IsNullOrEmpty((alc_matriculaEstadual ?? "").Trim()) &&
                string.IsNullOrEmpty((pes_nomeMae ?? "").Trim()) &&
                pes_dataNascimento == new DateTime()
                )
            {
                throw new ValidationException(
                    "É necessário selecionar/preencher pelo menos uma opção de filtro para pesquisar alunos.");
            }

            totalRecords = 0;
            ACA_AlunoDAO dal = new ACA_AlunoDAO();

            DataTable dt = dal.SelectBy_PesquisaDocumentoAluno_FichaIndividual(
                cal_id
                , esc_id
                , uni_id
                , cur_id
                , crr_id
                , crp_id
                , tur_id
                , tipoBusca
                , pes_nome
                , pes_dataNascimento
                , pes_nomeMae
                , alc_matricula
                , alc_matriculaEstadual
                , ent_id
                , uad_idSuperior
                , adm, usu_id, gru_id
                , emitirDocAnoAnt, buscaMatriculaIgual, MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords);

            //ve se todos sao peja
            numeroCursosPeja = (from DataRow dr in dt.AsEnumerable()
                                where dr.Field<bool>("CursoPeja")
                                select dr).ToList().Count;

            return dt;
        }

        #endregion Métodos de consulta - Documentos do Aluno

        #region Métodos de verificação
        
        /// <summary>
        /// Valida o Número de Identificação Social
        /// </summary>
        /// <param name="Nis">Número de Identificação Social</param>
        public static Boolean NISInvalido(string Nis)
        {
            int Digito, Cont;
            Int64 x;

            //Verifica  a quantidade de digitos
            if (Nis.Length != 11)
            {
                return true;
            }

            //Verifica se possui apenas números
            if (!Int64.TryParse(Nis, out x))
            {
                return true;
            }

            //Valida dígito verificador
            Digito = 0;

            for (Cont = 2; Cont <= 9; Cont++)
            {
                Digito = Digito + Convert.ToInt32(Nis.Substring(11 - Cont, 1)) * Cont;
            }

            for (Cont = 2; Cont <= 3; Cont++)
            {
                Digito = Digito + Convert.ToInt32(Nis.Substring(3 - Cont, 1)) * Cont;
            }

            Digito = 11 - Digito % 11;

            if (Digito > 9)
            {
                Digito = 0;
            }

            if (Digito != Convert.ToInt32(Nis.Substring(10, 1)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// -Metodo retorna true ou false da DAO SelectBy_Entidade_MatriculaEstadual
        /// -que é responsavel por verificar se existe uma matricula existente na
        /// -entidade que esta logado.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="alc_matriculaEstadual">Numero da matricula Estadual do aluno</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Boolean verifica_RA_Aluno(long alu_id, Guid ent_id, string alc_matriculaEstadual)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Entidade_MatriculaEstadual(alu_id, ent_id, alc_matriculaEstadual);
        }
                
        /// <summary>
        /// -Metodo retorna true ou false da DAO SelectBy_Entidade_MatriculaEstadual
        /// -que é responsavel por verificar se existe uma matricula existente na
        /// -entidade que esta logado.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="alc_matricula">Numero da matricula Estadual do aluno</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Boolean verifica_Numero_Matricula(long alu_id, Guid ent_id, string alc_matricula)
        {
            // Se o alc_matricula for nulo, tem que setar o valor do campo para vazio
            if (string.IsNullOrEmpty(alc_matricula))
                alc_matricula = string.Empty;

            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Entidade_NumeroMatricula(alu_id, ent_id, alc_matricula);
        }

        /// <summary>
        /// Verifica se ja existe aluno cadastrado com o mesmo numero de protocolo.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="protocolo">Numero do protocolo</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Boolean verifica_ProtocoloExcedente(long alu_id, Guid ent_id, string protocolo)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            return dao.SelectBy_Entidade_ProtocoloExcedente(alu_id, ent_id, protocolo);
        }

        /// <summary>
        /// Verifica se já existe um aluno cadastrado com as mesma informações (NomeAluno, NomeMae,Dta Nasc)
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable VerificaUnicidadeAluno
        (
            Guid ent_id
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , long alu_id
            , TalkDBTransaction bancoGestao
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            if (bancoGestao != null)
                dao._Banco = bancoGestao;

            return dao.SelectBy_VerificaUnicidadeAluno(ent_id, pes_nome, pes_dataNascimento, pes_nomeMae, alu_id);
        }

        /// <summary>
        /// Verifica se já existe um aluno cadastrado com o mesmo nome e data de nascimento
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable VerificaIntegridadePorNomeDataNasc
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , long alu_id
            , Guid ent_id
            , TalkDBTransaction bancoGestao
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            if (bancoGestao != null)
                dao._Banco = bancoGestao;

            return dao.SelectBy_VerificaIntegridadeNomeDataNasc(pes_nome, pes_dataNascimento, alu_id, ent_id);
        }

        /// <summary>
        /// Verifica se o aluno possui matrícula filtrado
        /// por processo e número de matrícula.
        /// </summary>
        /// <param name="alc_matricula">Matrícula do aluno.</param>
        /// <param name="pfi_id">Id do processo.</param>
        /// <param name="entityAluno"></param>
        /// <returns></returns>
        public static bool VerificaAlunoExistentePorMatriculaProcesso(string alc_matricula, int pfi_id, out ACA_Aluno entityAluno)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            DataTable dt = dao.VerificaAlunoExistentePorMatriculaProcesso(alc_matricula, pfi_id);

            entityAluno = new ACA_Aluno();

            if (dt.Rows.Count > 0)
            {
                entityAluno = dao.DataRowToEntity(dt.Rows[0], entityAluno);
            }

            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// Realiza busca fonética do nome do aluno e nome da mãe.
        /// Retorna o dataTable com as possíveis duplicidades encontradas.
        /// </summary>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="pes_dataNascimento">Dt nascimento do aluno</param>
        /// <param name="pes_nomeMae">Nome da mãe</param>
        /// <param name="alu_id">ID do aluno - para não buscar o mesmo</param>
        /// <param name="ent_id">Entidade - valida somente na entidade</param>
        /// <param name="bancoGestao">Transação</param>
        /// <returns>O dataTable com as possíveis duplicidades encontradas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable VerificaUnicidadeSom
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , long alu_id
            , Guid ent_id
            , TalkDBTransaction bancoGestao
        )
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();
            if (bancoGestao != null)
                dao._Banco = bancoGestao;
            return dao.SelectBy_VerificaUnicidadeSom(pes_nome, pes_dataNascimento, pes_nomeMae, alu_id, ent_id);
        }

        /// <summary>
        /// Valida a permissão do usuário de editar o aluno em questão.
        /// Se o aluno não possuir matrículas ativas na escola que o usuário tem acesso,
        /// ele não pode alterar o aluno.
        /// </summary>
        private static bool ValidaPermissaoUsuario(Guid usu_id, Guid gru_id, ACA_Aluno aluno, TalkDBTransaction banco)
        {
            bool isValid = aluno.IsNew;

            if (!aluno.IsNew)
            {
                isValid = ACA_AlunoCurriculoBO.AlunoPossuiMatricula_PorPermissaoUsuario(aluno.alu_id, usu_id, gru_id, banco);
            }

            return isValid;
        }

        public static bool VerficaAlunoExistentePelaMatriculaLista(string alc_matricula, List<DadosAluno> ltAlunos, out ACA_Aluno entityAluno)
        {
            entityAluno = new ACA_Aluno();

            List<ACA_Aluno> aluno = (from DadosAluno al in ltAlunos
                                     where al.alc_matricula.Equals(alc_matricula)
                                     select al.aluno).ToList();

            if (aluno.Count > 0)
                entityAluno = aluno[0];

            return aluno.Count > 0;
        }

        /// <summary>
        /// Valida associação com deficiência, caso o aluno esteja na educação especial.
        /// </summary>
        /// <param name="entityAluno">Entidade ACA_Aluno</param>
        /// <param name="cadMovimentacao">Estrutura contendo os dados da movimentação</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        public static void ValidaEducacaoEspecial(ACA_Aluno entityAluno, MTR_Movimentacao_Cadastro cadMovimentacao, Guid ent_id)
        {
            // Verifica se aluno de classe especial.
            ACA_Curso cursoAnterior = new ACA_Curso { cur_id = cadMovimentacao.entAluCurAnterior.cur_id };
            ACA_CursoBO.GetEntity(cursoAnterior);

            if (cursoAnterior.cur_exclusivoDeficiente)
            {
                ACA_Curso cursoNovo = new ACA_Curso { cur_id = cadMovimentacao.entAluCurNovo.cur_id };
                ACA_CursoBO.GetEntity(cursoNovo);

                if ((cursoNovo.cur_id > 0 && cursoNovo.cur_exclusivoDeficiente) || (cursoNovo.cur_id <= 0))
                {
                    throw new ValidationException(
                        CustomResource.GetGlobalResourceObject("BLL", "Aluno.ValidaPreenchimentoDeficienciaClasseEspecial")
                        );
                }
            }
        }

        /// <summary>
        /// Verifica se já existe o aluno matriculado para a escola no ano de início do processo de fechamento/início do ano letivo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do curso.</param>
        /// <param name="pfi_id">ID do processo de fechamento/início do ano letivo.</param>
        /// <param name="alc_matricula">Número de matrícula do aluno.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public static bool VerificaAlunoMatriculadoProcessoEscolaCurso
        (
            int esc_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int pfi_id,
            string alc_matricula,
            string pes_nome,
            string pes_nomeMae,
            DateTime pes_dataNascimento,
            Guid ent_id,
            TalkDBTransaction banco = null
        )
        {
            ACA_AlunoDAO dao = banco == null ?
                new ACA_AlunoDAO() :
                new ACA_AlunoDAO { _Banco = banco };

            return dao.VerificaAlunoMatriculadoProcessoEscolaCurso(esc_id, cur_id, crr_id, crp_id, pfi_id, alc_matricula, pes_nome, pes_nomeMae, pes_dataNascimento, ent_id);
        }

        #endregion Métodos de verificação

        #region Métodos de alteração e inclusão

        /// <summary>
        /// Metodo para processar os protocolos referente a foto do aluno
        /// </summary>
        /// <param name="ltProtocolo">Lista de protocolos em processamento.</param>
        /// <param name="tentativasProtocolo">Quantidade máxima de tentativas para processar protocolos.</param>
        /// <returns>Retorna se o protocolo foi ou não processado.</returns>
        public static bool ProcessarProtocoloFoto(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
        {
            // DataTable de protocolos
            DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa > tentativasProtocolo))
            {
                protocolo.pro_statusObservacao = String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                , tentativasProtocolo, protocolo.pro_statusObservacao);
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                protocolo.tur_id = -1;
                protocolo.tud_id = -1;
                protocolo.tau_id = -1;
                protocolo.pro_qtdeAlunos = -1;
                dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
            }

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa <= tentativasProtocolo))
            {
                TalkDBTransaction bancoGestao = new ACA_AlunoDAO()._Banco.CopyThisInstance();
                TalkDBTransaction bancoCore = new PES_PessoaDAO()._Banco.CopyThisInstance();

                bancoGestao.Open();
                bancoCore.Open();

                bool processou = false;
                try
                {
                    if (protocolo.pro_tentativa <= tentativasProtocolo)
                    {
                        JObject json = JObject.Parse(protocolo.pro_pacote);
                        JArray fotos = ((JArray)json.SelectToken("Fotos") ?? new JArray());

                        foreach (JToken registro in fotos.Children())
                        {
                            ACA_Aluno aluno = new ACA_Aluno
                            {
                                alu_id = (Int64)(registro.SelectToken("alu_id") ?? 0)
                            };

                            GetEntity(aluno, bancoGestao);

                            PES_Pessoa pessoa = new PES_Pessoa
                            {
                                pes_id = aluno.pes_id
                            };

                            PES_PessoaBO.GetEntity(pessoa, bancoCore);

                            CFG_Arquivo foto = null;

                            byte[] bytes = ACA_AlunoBO.RedimensionaFoto(Convert.FromBase64String(registro.SelectToken("fot_data").ToString()), true);

                            if (pessoa.arq_idFoto > 0)
                            {
                                DateTime dataAlteracao = (DateTime)(registro.SelectToken("Fot_dataAlteracao") ?? new DateTime());

                                foto = PES_PessoaBO.RetornaFotoPor_Pessoa(pessoa.pes_id, bancoCore);

                                // so vai atualizar caso a data de alteração do diario for posterior
                                if (dataAlteracao.CompareTo(foto.arq_dataAlteracao) == 1)
                                {
                                    foto.arq_dataAlteracao = dataAlteracao;
                                    foto.arq_data = bytes;
                                    foto.arq_tamanhoKB = bytes.Length;
                                }
                            }
                            else
                            {
                                foto = new CFG_Arquivo
                                {
                                    arq_nome = pessoa.pes_nome,
                                    arq_situacao = 1,
                                    arq_typeMime = "Image/jpeg",
                                    arq_dataCriacao = DateTime.Now,
                                    arq_dataAlteracao = DateTime.Now,
                                    arq_tamanhoKB = bytes.Length,
                                    arq_data = bytes,
                                    IsNew = true
                                };
                            }

                            // validações da foto.
                            if (!foto.Validate())
                            {
                                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(foto));
                            }

                            processou = CFG_ArquivoBO.Save(foto, bancoCore);

                            pessoa.arq_idFoto = foto.arq_id;
                            PES_PessoaBO.Save(pessoa, bancoCore);

                            DCL_ProtocoloAluno protocoloAluno = new DCL_ProtocoloAluno
                            {
                                pro_id = protocolo.pro_id,
                                alu_id = aluno.alu_id
                            };

                            // relaciona protocolo com aluno
                            DCL_ProtocoloAlunoBO.Save(protocoloAluno, bancoGestao);
                        }
                    }

                    if (processou)
                    {
                        // Processou com sucesso.
                        protocolo.pro_statusObservacao = String.Format("Protocolo processado com sucesso ({0}).",
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso;
                    }
                    else
                    {
                        if (protocolo.pro_tentativa > tentativasProtocolo)
                        {
                            throw new ValidationException(String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                , tentativasProtocolo, protocolo.pro_statusObservacao));
                        }

                        // Não processou sem erro - volta o protocolo para não processado.
                        protocolo.pro_statusObservacao = String.Format("Protocolo não processado ({0}).",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                        protocolo.tur_id = -1;
                        protocolo.tud_id = -1;
                        protocolo.tau_id = -1;
                        protocolo.pro_qtdeAlunos = -1;
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.NaoProcessado;
                    }

                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                }
                catch (ArgumentException ex)
                {
                    // Se ocorrer uma excessão de validação, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoGestao.Close(ex);
                    bancoCore.Close(ex);
                }
                catch (ValidationException ex)
                {
                    // Se ocorrer uma excessão de validação, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoGestao.Close(ex);
                    bancoCore.Close(ex);
                }
                catch (Exception ex)
                {
                    // Se ocorrer uma excessão de erro, guardar novo status.
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro;
                    protocolo.pro_statusObservacao = ex.Message;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    bancoGestao.Close(ex);
                    bancoCore.Close(ex);
                }
                finally
                {
                    if (bancoGestao.ConnectionIsOpen)
                        bancoGestao.Close();

                    if (bancoCore.ConnectionIsOpen)
                        bancoCore.Close();
                }
            }

            DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);

            return true;
        }

        /// <summary>
        /// Altera a foto da pessoa ligada ao aluno, usando o arquivo.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="arq_id">ID do arquivo que contém a foto</param>
        /// <returns></returns>
        public static bool SalvarFotoAluno(long alu_id, long arq_id)
        {
            TalkDBTransaction bancoGestao = new ACA_AlunoDAO()._Banco.CopyThisInstance();
            TalkDBTransaction bancoCore = new PES_PessoaDAO()._Banco.CopyThisInstance();

            bancoGestao.Open(IsolationLevel.ReadCommitted);
            bancoCore.Open(IsolationLevel.ReadCommitted);

            try
            {
                ACA_Aluno entAluno = new ACA_Aluno
                {
                    alu_id = alu_id
                };
                GetEntity(entAluno, bancoGestao);

                SYS_Arquivo entArquivo = new SYS_Arquivo
                {
                    arq_id = arq_id
                };
                SYS_ArquivoBO.GetEntity(entArquivo, bancoGestao);

                PES_Pessoa entPessoa = new PES_Pessoa
                {
                    pes_id = entAluno.pes_id
                };
                PES_PessoaBO.GetEntity(entPessoa, bancoCore);

                // Seta a foto da pessoa.
                CFG_Arquivo entArquivoFoto = new CFG_Arquivo
                {
                    // Se a pessoa já possuir foto, só altera o valor do registro.
                    arq_id = entPessoa.arq_idFoto
                                                     ,
                    arq_nome = entArquivo.arq_nome
                                                     ,
                    arq_tamanhoKB = entArquivo.arq_tamanhoKB
                                                     ,
                    arq_typeMime = entArquivo.arq_typeMime
                                                     ,
                    arq_situacao = (byte)CFG_ArquivoSituacao.Ativo
                                                     ,
                    arq_data = entArquivo.arq_data
                                                     ,
                    IsNew = entPessoa.arq_idFoto <= 0
                };
                CFG_ArquivoBO.Save(entArquivoFoto, bancoCore);

                entPessoa.arq_idFoto = entArquivoFoto.arq_id;

                bool res = PES_PessoaBO.Save(entPessoa, bancoCore);

                // Exclui o arquivo que não será mais utilizado.
                SYS_ArquivoBO.Delete(entArquivo, bancoGestao);

                return res;
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);

                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                    bancoGestao.Close();

                if (bancoCore.ConnectionIsOpen)
                    bancoCore.Close();
            }
        }

        /// <summary>
        /// Salva os dados do aluno
        /// </summary>
        /// <param name="entityAluno"></param>
        /// <param name="entityAlunoFichaMedica"></param>
        /// <param name="dtFichaMedicaContato"></param>
        /// <returns></returns>
        public static bool Salvar
        (
            ACA_Aluno entityAluno
            , ACA_AlunoFichaMedica entityAlunoFichaMedica
            , DataTable dtFichaMedicaContato
        )
        {
            TalkDBTransaction bancoGestao = new ACA_AlunoDAO()._Banco.CopyThisInstance();
            

            try
            {
                bancoGestao.Open(IsolationLevel.ReadCommitted);

                ACA_Aluno auxEntityAluno = new ACA_Aluno
                {
                    alu_id = entityAluno.alu_id
                };
                GetEntity(auxEntityAluno, bancoGestao);

                if (auxEntityAluno.alu_dataAlteracao != entityAluno.alu_dataAlteracao)
                    throw new EditarAluno_ValidationException("Esta ação não pode ser realizada, pois o aluno foi alterado. Entre novamente no cadastro desse aluno para atualizar as informações.");

                // Verifica se os dados do aluno serão sempre salvos em maiúsculo.
                string param = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.SALVAR_SEMPRE_MAIUSCULO);
                bool salvar_sempre_maiusculo = !string.IsNullOrEmpty(param) && Convert.ToBoolean(param);

                if (salvar_sempre_maiusculo)
                {
                    entityAlunoFichaMedica.afm_alergias = entityAlunoFichaMedica.afm_alergias == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_alergias.ToUpper();

                    entityAlunoFichaMedica.afm_convenioMedico = entityAlunoFichaMedica.afm_convenioMedico == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_convenioMedico.ToUpper();

                    entityAlunoFichaMedica.afm_doencasConhecidas = entityAlunoFichaMedica.afm_doencasConhecidas == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_doencasConhecidas.ToUpper();

                    entityAlunoFichaMedica.afm_fatorRH = entityAlunoFichaMedica.afm_fatorRH == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_fatorRH.ToUpper();

                    entityAlunoFichaMedica.afm_hospitalRemocao = entityAlunoFichaMedica.afm_hospitalRemocao == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_hospitalRemocao.ToUpper();

                    entityAlunoFichaMedica.afm_medicacoesPodeUtilizar = entityAlunoFichaMedica.afm_medicacoesPodeUtilizar == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_medicacoesPodeUtilizar.ToUpper();

                    entityAlunoFichaMedica.afm_medicacoesUsoContinuo = entityAlunoFichaMedica.afm_medicacoesUsoContinuo == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_medicacoesUsoContinuo.ToUpper();

                    entityAlunoFichaMedica.afm_outrasRecomendacoes = entityAlunoFichaMedica.afm_outrasRecomendacoes == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_outrasRecomendacoes.ToUpper();

                    entityAlunoFichaMedica.afm_tipoSanguineo = entityAlunoFichaMedica.afm_tipoSanguineo == null
                        ? string.Empty
                        : entityAlunoFichaMedica.afm_tipoSanguineo.ToUpper();
                }

                if (entityAlunoFichaMedica.Validate())
                {
                    ACA_AlunoFichaMedicaDAO daoAlunoFichaMedica = new ACA_AlunoFichaMedicaDAO { _Banco = bancoGestao };
                    daoAlunoFichaMedica.Salvar(entityAlunoFichaMedica);
                }
                else
                {
                    throw new ValidationException(entityAlunoFichaMedica.PropertiesErrorList[0].Message);
                }

                // Salva os dados dos contatos da ficha médica do aluno.
                ACA_FichaMedicaContatoBO.SalvarFichaMedicaContatosAluno
                (
                    bancoGestao
                    , entityAluno
                    , salvar_sempre_maiusculo
                    , dtFichaMedicaContato
                );

                return true;
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                    bancoGestao.Close();
            }
        }

        #endregion Métodos de alteração e inclusão

        #region Métodos de exclusão

        /// <summary>
        /// Exclui a foto da pessoa.
        /// </summary>
        /// <param name="pes_id">ID da pessoa</param>
        /// <param name="arq_id">ID do arquivo que contém a foto</param>
        /// <returns></returns>
        public static bool ExcluirFotoAluno(Guid pes_id, long arq_id)
        {
            TalkDBTransaction bancoCore = new PES_PessoaDAO()._Banco.CopyThisInstance();

            bancoCore.Open(IsolationLevel.ReadCommitted);

            try
            {
                CFG_Arquivo entArquivo = new CFG_Arquivo
                {
                    arq_id = arq_id
                };
                CFG_ArquivoBO.GetEntity(entArquivo, bancoCore);

                PES_Pessoa entPessoa = new PES_Pessoa
                {
                    pes_id = pes_id
                };
                PES_PessoaBO.GetEntity(entPessoa, bancoCore);

                entPessoa.arq_idFoto = -1;

                bool res = PES_PessoaBO.Save(entPessoa, bancoCore);

                // Exclui fisícamente o arquivo que não será mais utilizado.
                CFG_ArquivoBO.Delete(entArquivo, bancoCore);

                return res;
            }
            catch (Exception ex)
            {
                bancoCore.Close(ex);

                throw;
            }
            finally
            {
                if (bancoCore.ConnectionIsOpen)
                {
                    bancoCore.Close();
                }
            }
        }

        #endregion Métodos de exclusão
              
        #region Métodos Boletim do Aluno

        /// <summary>
        /// Retorna todos os dados do boletim de todos os alunos informados na lista
        /// em suas respectivas matriculas turmas
        /// </summary>
        /// <param name="alu_ids">Ids dos alunos (separados por ',')</param>
        /// <param name="mtu_ids">Ids das matriculas turmas (separados por ',')</param>
        /// <param name="tpc_id">Id do Bimestre</param>
        /// <returns></returns>
        public static List<BoletimDadosAluno> BuscaBoletimAlunos(string alu_ids, string mtu_ids, int tpc_id, Guid ent_id)
        {
            ACA_AlunoDAO dao = new ACA_AlunoDAO();

            DataTable dtAlunoMatriculaTurma = MTR_MatriculaTurma.TipoTabela_AlunoMatriculaTurma();
            List<string> listaAlunos = alu_ids.Split(',').ToList();

            for (int i = 0; i < listaAlunos.Count; i++)
            {
                long alu_id = Convert.ToInt64(listaAlunos[i]);
                if (!dtAlunoMatriculaTurma.AsEnumerable().Any(row => row.Field<Int64>("alu_id") == alu_id))
                {
                    DataRow dr = dtAlunoMatriculaTurma.NewRow();
                    dr["alu_id"] = alu_id;

                    if (string.IsNullOrEmpty(mtu_ids))
                    {
                        dr["mtu_id"] = DBNull.Value;
                    }
                    else
                    {
                        int mtu_id;
                        if (int.TryParse(mtu_ids.Split(',')[i], out mtu_id)
                            && mtu_id > 0)
                        {
                            dr["mtu_id"] = mtu_id.ToString();
                        }
                        else
                        {
                            dr["mtu_id"] = DBNull.Value;
                        }
                    }

                    dtAlunoMatriculaTurma.Rows.Add(dr);
                }
            }

            // Busca os dados gerais dos alunos
            DataTable dt = dao.BuscaBoletimAlunos(dtAlunoMatriculaTurma, tpc_id);

            dtAlunoMatriculaTurma = MTR_MatriculaTurma.TipoTabela_AlunoMatriculaTurma();
            foreach (DataRow dr in dt.Rows)
            {
                dtAlunoMatriculaTurma.Rows.Add(new[] { dr["alu_id"], dr["mtu_id"] });
            }

            // Busca os dados do boletim de todos os alunos
            List<BoletimAluno> listaBoletimAlunos = AjustaDadosExibicaoBoletim(CLS_AlunoAvaliacaoTurmaBO.RetornaBoletimAluno(dtAlunoMatriculaTurma), ent_id, dtAlunoMatriculaTurma);

            List<BoletimDadosAluno> lista = new List<BoletimDadosAluno>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BoletimDadosAluno boletim = new BoletimDadosAluno();
                boletim = (BoletimDadosAluno)GestaoEscolarUtilBO.DataRowToEntity(dt.Rows[i], boletim);
                boletim.listaNotasEFaltas = listaBoletimAlunos.FindAll(p => p.alu_id == boletim.alu_id);

                if (boletim.listaNotasEFaltas.Any(b => !string.IsNullOrEmpty(b.ParecerConclusivo)))
                {
                    string parecer = boletim.listaNotasEFaltas.Where(c => !string.IsNullOrEmpty(c.ParecerConclusivo)).First().ParecerConclusivo;
                    boletim.ParecerConclusivo = parecer;
                    boletim.listaNotasEFaltas.ForEach(b => b.ParecerConclusivo = parecer);
                }

                lista.Add(boletim);
            }

            return lista;
        }

        /// <summary>
        /// Ajusta os dados para exibição correta do boletim, todas as disciplinas devem aparecer em todos os bimestres
        /// para não quebrar o layout da table no repeater.
        /// </summary>
        /// <param name="listaBoletimAlunos"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        private static List<BoletimAluno> AjustaDadosExibicaoBoletim(List<BoletimAluno> listaBoletimAlunos, Guid ent_id, DataTable dtAlunoMatriculaTurma)
        {

            List<BoletimAluno> lstAdd = new List<BoletimAluno>();
            List<long> lstAluIds = listaBoletimAlunos.GroupBy(p => p.alu_id).Select(p => p.Key).ToList();
            foreach (long alu_id in lstAluIds)
            {
                List<int> lstTpcIds = listaBoletimAlunos.Where(p => p.alu_id == alu_id)
                                      .GroupBy(p => new { tpc_id = p.tpc_id, tpc_ordem = p.tpc_ordem })
                                      .OrderBy(p => p.Key.tpc_ordem).Select(p => p.Key.tpc_id).ToList();
                int qtdPeriodos = lstTpcIds.Count();
                int tds_id = 0;
                foreach (BoletimAluno item in listaBoletimAlunos.Where(p => p.alu_id == alu_id))
                {
                    if (tds_id == item.tds_id)
                        continue;

                    tds_id = item.tds_id;

                    if (listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id).GroupBy(p => p.tpc_id).Count() == qtdPeriodos)
                        continue;

                    foreach (int tpc_id in lstTpcIds)
                    {
                        if (listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id).Any(p => p.tpc_id == tpc_id))
                            continue;

                        BoletimAluno itemCopiar = listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tds_id == tds_id).FirstOrDefault();
                        BoletimAluno itemTpc = listaBoletimAlunos.Where(p => p.alu_id == alu_id && p.tpc_id == tpc_id).FirstOrDefault();
                        lstAdd.Add(new BoletimAluno
                        {
                            tds_id = tds_id,
                            alu_id = alu_id,
                            mtu_id = itemCopiar.mtu_id,
                            tur_id = itemCopiar.tur_id,
                            tur_codigo = itemCopiar.tur_codigo,
                            mtd_id = itemCopiar.mtd_id,
                            tud_id = itemCopiar.tud_id,
                            tud_global = itemCopiar.tud_global,
                            Disciplina = itemCopiar.Disciplina,
                            DisciplinaEspecial = itemCopiar.DisciplinaEspecial,
                            tud_disciplinaEspecial = itemCopiar.tud_disciplinaEspecial,
                            ava_id = itemTpc.ava_id,
                            ava_tipo = itemCopiar.ava_tipo,
                            fav_tipo = itemCopiar.fav_tipo,
                            ava_exibeSemProfessor = itemCopiar.ava_exibeSemProfessor,
                            ava_exibeNaoAvaliados = itemCopiar.ava_exibeNaoAvaliados,
                            naoAvaliado = itemCopiar.naoAvaliado,
                            semProfessor = itemCopiar.semProfessor,
                            tpc_id = tpc_id,
                            tpc_ordem = itemTpc.tpc_ordem,
                            tpc_nome = itemTpc.tpc_nome,
                            ava_mostraConceito = itemCopiar.ava_mostraConceito,
                            ava_mostraNota = itemCopiar.ava_mostraNota,
                            fav_variacao = itemCopiar.fav_variacao,
                            mostraConceito = itemCopiar.mostraConceito,
                            mostraNota = itemCopiar.mostraNota,
                            mostraFrequencia = itemCopiar.mostraFrequencia,
                            naoLancarNota = itemCopiar.naoLancarNota,
                            naoExibirNota = itemCopiar.naoExibirNota,
                            naoExibirFrequencia = itemCopiar.naoExibirFrequencia,
                            MostrarLinhaDisciplina = itemCopiar.MostrarLinhaDisciplina,
                            esc_codigo = itemCopiar.esc_codigo,
                            esc_nome = itemCopiar.esc_nome,
                            tds_ordem = itemCopiar.tds_ordem,
                            tud_tipo = itemCopiar.tud_tipo,
                            esa_tipo = itemCopiar.esa_tipo,
                            nomeDisciplina = itemCopiar.nomeDisciplina,
                            EnriquecimentoCurricular = itemCopiar.EnriquecimentoCurricular,
                            cur_id = itemCopiar.cur_id,
                            crr_id = itemCopiar.crr_id,
                            crp_id = itemCopiar.crp_id,
                            disRelacionadas = itemCopiar.disRelacionadas,
                            NotaResultado = itemCopiar.NotaResultado,
                            Recuperacao = itemCopiar.Recuperacao
                        });
                    }
                }
            }


            if (lstAdd.Any())
            {
                listaBoletimAlunos.AddRange(lstAdd);

                bool controleOrdemDisc = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, ent_id);

                if (controleOrdemDisc)
                    listaBoletimAlunos = listaBoletimAlunos.OrderBy(p => p.tds_ordem).ThenBy(p => p.tpc_ordem).ToList();
                else
                    listaBoletimAlunos = listaBoletimAlunos.OrderBy(p => p.Disciplina).ThenBy(p => p.tpc_ordem).ToList();
            }


            List<AlunosTds> lstAluTds = new List<AlunosTds>();

            foreach (DataRow dr in dtAlunoMatriculaTurma.Rows)
            {

                lstAluTds.AddRange(listaBoletimAlunos
                    .Where(p => p.alu_id == Convert.ToInt64(dr["alu_id"].ToString()) && p.mtu_id == Convert.ToInt32(dr["mtu_id"].ToString()))
                    .Select(s => new AlunosTds { alu_id = s.alu_id, tds_id = s.tds_id }).ToList());
            }


            listaBoletimAlunos = (from Boletim in listaBoletimAlunos
                                  join AluTds in lstAluTds on
                                  new
                                  {
                                      AluId = Boletim.alu_id,
                                      TdsId = Boletim.tds_id
                                  }
                                  equals
                                  new
                                  {
                                      AluId = AluTds.alu_id,
                                      TdsId = AluTds.tds_id
                                  }
                                  select Boletim).Distinct().ToList();


            return listaBoletimAlunos;
        }

        #endregion Métodos Boletim do Aluno

        #region Métodos Histórico Escolar

        /// <summary>
        /// Busca os dados do aluno para o histórico escolar.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns></returns>
        public static DataTable BuscaDadosAluno(Int64 alu_id, bool documentoOficial)
        {
            return new ACA_AlunoDAO().BuscaDadosAluno(alu_id, documentoOficial);
        }

        /// <summary>
        /// Para cada alu_id informado, retorna o mtu_id correspondente a cada tpc_id do calendário.
        /// </summary>
        /// <param name="alu_ids">Lista com o alu_id de cada aluno</param>
        /// <param name="mtu_ids">Lista com o mtu_id de cada aluno. Deve estar vazia (será considerado o mtu_id mais recente de cada aluno) ou ter a mesma quantidade de itens de <paramref name="alu_ids"/>.</param>
        /// <returns>Tabela com os campos alu_id, mtu_id, tpc_id, tpc_ordem.</returns>
        public static DataTable BuscarMatriculasPeriodos(long[] alu_ids, int[] mtu_ids = null, TalkDBTransaction banco = null)
        {
            if (alu_ids.Length == 0 ||
                (mtu_ids.Length > 0 && mtu_ids.Length != alu_ids.Length))
                throw new ArgumentException();

            var dtAlunos = MTR_MatriculaTurma.TipoTabela_AlunoMatriculaTurma();
            for (int i = 0; i < alu_ids.Length; i++)
            {
                if (!dtAlunos.AsEnumerable().Any(row => row.Field<Int64>("alu_id") == alu_ids[i]))
                {
                    var dr = dtAlunos.NewRow();
                    dr["alu_id"] = alu_ids[i];
                    if (mtu_ids.Length == 0 || mtu_ids[i] <= 0)
                        dr["mtu_id"] = DBNull.Value;
                    else
                        dr["mtu_id"] = mtu_ids[i];
                    dtAlunos.Rows.Add(dr);
                }
            }

            var dao = new ACA_AlunoDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.BuscarMatriculasPeriodos(dtAlunos);
        }

        #endregion
    }
}
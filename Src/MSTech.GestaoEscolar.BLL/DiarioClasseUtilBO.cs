using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    public class ParametrosInvalidosException : Exception
    {
        public ParametrosInvalidosException(string método, string mensagem) :
            base(string.Format("{0} - {1}", método, mensagem))
        { }
    }

    public class k1VaziaException : Exception
    {
        public k1VaziaException() :
            base("k1 vazia")
        { }
    }

    public class uadCodigoVazioException : Exception
    {
        public uadCodigoVazioException() :
            base("Código da escola vazio")
        { }
    }

    public class EscolaVaziaException : Exception
    {
        public EscolaVaziaException() :
            base("Escola vazia.")
        { }
    }

    public class k4VaziaException : Exception
    {
        public k4VaziaException() :
            base("k4 vazia")
        { }
    }

    public class EntidadeVaziaException : Exception
    {
        public EntidadeVaziaException() :
            base("Entidade vazia")
        { }
    }

    public class AnoLetivoVazioException : Exception
    {
        public AnoLetivoVazioException() :
            base("Ano letivo vazio")
        { }
    }

    /// <summary>
    /// Parametro uad_id foi passado vazio
    /// </summary>
    public class UnidadeAdministrativaVaziaException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnidadeAdministrativaVaziaException() :
            base("Unidade Administrativa vazia")
        { }
    }

    public class ProtocolosNaoEnviadosException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ProtocolosNaoEnviadosException() :
            base("Nenhum protocolo foi enviado")
        { }
    }

    /// <summary>
    /// Parametro pacote foi passado nulo ou vazio
    /// </summary>
    public class PacoteVazioException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public PacoteVazioException() :
            base("Pacote vazio")
        { }
    }

    /// <summary>
    /// Parametro k1 foi passado nulo ou vazio
    /// </summary>
    public class TurmaVaziaException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public TurmaVaziaException() :
            base("Turma foi enviada vazia")
        { }
    }

    /// <summary>
    /// Escola informada não foi encontrada.
    /// </summary>
    public class EscolaNaoEncontradaException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uad_codigo">Código da Escola</param>
        public EscolaNaoEncontradaException(string uad_codigo)
            : base(string.Format("Escola não encontrada (Código Escola: {0})", uad_codigo))
        { }
    }

    /// <summary>
    /// Nenhuma escola foi encontrada para o usuário do professor
    /// </summary>
    public class NenhumaEscolaEncontradaException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu_login">Login do professor</param>
        public NenhumaEscolaEncontradaException(string usu_login)
            : base(string.Format("Nenhuma escola encontrada (Login: {0})", usu_login))
        { }
    }

    /// <summary>
    /// Entidade informada não foi encontrada.
    /// </summary>
    public class EntidadeNãoEncontradaException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="k1">Chave do sistema para a entidade</param>
        public EntidadeNãoEncontradaException(string k1)
            : base(string.Format("Entidade não encontrada (K1: {0})", k1))
        { }
    }

    /// <summary>
    /// Nenhum equipamento foi encontrado
    /// </summary>
    public class EquipamentoNaoEncontradoException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="k4">Identificador do equipamento</param>
        /// <param name="ent_id">Id da entidade</param>
        public EquipamentoNaoEncontradoException(string k4, Guid ent_id)
            : base(string.Format("Equipamento não encontrado (k4: {0} - ent_id: {1})", k4, ent_id))
        { }

        public EquipamentoNaoEncontradoException(string k4)
            : base(string.Format("Equipamento não encontrado (k4: {0})", k4))
        { }
    }

    /// <summary>
    /// Nenhum protocolo foi encontrado
    /// </summary>
    public class ProtocolosNaoEncontradosException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ProtocolosNaoEncontradosException()
            : base(string.Format("Nenhum protocolo encontrado"))
        { }
    }

    /// <summary>
    /// Nenhuma turma foi encontrada
    /// </summary>
    public class TurmasNaoEncontradasException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public TurmasNaoEncontradasException()
            : base(string.Format("Nenhuma turma foi encontrada"))
        { }
    }

    /// <summary>
    /// Nenhum tipo atividade foi encontrado
    /// </summary>
    public class NaoEncontradoNenhumTipoAtividadeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public NaoEncontradoNenhumTipoAtividadeException()
            : base(string.Format("Nenhum tipo atividade foi encontrado"))
        { }
    }

    /// <summary>
    /// Nenhum formato de avaliação encontrado
    /// </summary>
    public class NaoEncontradoNenhumFormatoAvaliacao : Exception
    {
        public NaoEncontradoNenhumFormatoAvaliacao()
            : base(string.Format("Nenhum formato de avaliação encontado para os parâmetros informados"))
        { }
    }

    /// <summary>
    /// Nenhum aluno foi encontrado na turma
    /// </summary>
    public class AlunosTurmaNaoEncontradosException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        public AlunosTurmaNaoEncontradosException(Int64 tur_id)
            : base(string.Format("Nenhum aluno por turma foi encontrada - tur_id: {0}", tur_id))
        { }
    }

    /// <summary>
    /// Nenhum usuário foi encontrado
    /// </summary>
    public class UsuariosNaoEncontradosException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UsuariosNaoEncontradosException()
            : base(string.Format("Nenhum usuário foi encontrado"))
        { }
    }

    /// <summary>
    /// Usuário não encontrado
    /// </summary>
    public class UsuarioNaoEncontradosException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu_login">Login do usuário</param>
        public UsuarioNaoEncontradosException(string usu_login)
            : base(string.Format("Usuário não encontrado - usu_login: {0}", usu_login))
        { }
    }

    public class ProtocoloNãoEncontrado : Exception
    {
        public ProtocoloNãoEncontrado() :
            base("Protocolo não encontrado.")
        { }

        public ProtocoloNãoEncontrado(long numeroProtocolo) :
            base(string.Format("Protocolo de número {0} não foi encontrado.", numeroProtocolo))
        { }
    }

    public class ReprocessarProtocoloSemErro : Exception
    {
        public ReprocessarProtocoloSemErro() :
            base("Não é possível reprocessar um protocolo com status diferente de \"Processado com erro\".")
        { }
    }

    #endregion

    public class DiarioClasseUtilBO
    {

    }
}

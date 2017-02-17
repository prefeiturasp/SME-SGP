/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_ConfiguracaoAtividadeQualificador Business Object. 
	/// </summary>
	public class CLS_ConfiguracaoAtividadeQualificadorBO : BusinessBase<CLS_ConfiguracaoAtividadeQualificadorDAO, CLS_ConfiguracaoAtividadeQualificador>
	{
        public enum EnumQuantificadoes
        { 
            AtividadeDiversificada = 1,
            InstrumentoAvaliacao = 2,
            RecuperacaoAtividadeDiversificada = 3,
            RecuperacaoInstrumentoAvaliacao = 4,
            LicaoCasa = 5
            
        }

        public static new bool Save(CLS_ConfiguracaoAtividadeQualificador entity, TalkDBTransaction banco)
        {
            if (!entity.Validate())
            {
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
            }

            CLS_ConfiguracaoAtividadeQualificadorDAO dao = new CLS_ConfiguracaoAtividadeQualificadorDAO { _Banco = banco };
            return dao.Salvar(entity);
        }
	}
}
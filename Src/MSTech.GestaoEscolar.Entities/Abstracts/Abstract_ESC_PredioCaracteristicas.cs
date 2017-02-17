/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities.Abstracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.ComponentModel;
	using MSTech.Data.Common.Abstracts;
	using MSTech.Validation;
	
	/// <summary>
	/// Description: .
	/// </summary>
	[Serializable]
    public abstract class AbstractESC_PredioCaracteristicas : Abstract_Entity
    {
		
		/// <summary>
		/// PK da tabela ESC_Predio.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int prd_id { get; set; }

		/// <summary>
		/// PK da tabela.
		/// </summary>
		[MSNotNullOrEmpty]
		[DataObjectField(true, false, false)]
		public virtual int prc_id { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Prédio escolar. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncPredioEscolar { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Templo/Igreja. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncTemploIgreja { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Salas de empresa. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncSalasEmpresa { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Casa do professor. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncCasaProfessor { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Salas em outra escola. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncSalasOutraEscola { get; set; }

		/// <summary>
		/// Local de funcionamento da escola – Galpão/ Rancho/ Paiol/ Barracão. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncGalpao { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Unidade de internação/Prisional. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncUnidadeInternacaoPrisional { get; set; }

		/// <summary>
		/// Local de funcionamento da escola - Outros. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_localFuncOutros { get; set; }

		/// <summary>
		/// Forma de ocupação do prédio. Valores permitidos: 1 – Próprio, 2 – Alugado, 3 – Cedido.
		/// </summary>
		public virtual short prc_formaOcupacaoPredio { get; set; }

		/// <summary>
		/// Água consumida pelos alunos. Valores permitidos: 1 – Não filtrada, 2 – Filtrada.
		/// </summary>
		public virtual short prc_aguaConsumida { get; set; }

		/// <summary>
		/// Abastecimento de água – Rede pública. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasAguaRedePublica { get; set; }

		/// <summary>
		/// Abastecimento de água – Poço artesiano. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasAguaPocoArtesiano { get; set; }

		/// <summary>
		/// Abastecimento de água – Cacimba/ cisterna / poço. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasAguaCacimba { get; set; }

		/// <summary>
		/// Abastecimento de água – Fonte/ rio / igarapé/ riacho/ córrego. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasAguaFonte { get; set; }

		/// <summary>
		/// Abastecimento de água – Inexistente. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasAguaInexistente { get; set; }

		/// <summary>
		/// Abastecimento de energia elétrica – Rede pública. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasEnergiaRedePublica { get; set; }

		/// <summary>
		/// Abastecimento de energia elétrica – Gerador. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasEnergiaGerador { get; set; }

		/// <summary>
		/// Abastecimento de energia elétrica – Outros (energia alternativa). Ex: eólica, solar etc. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasEnergiaOutros { get; set; }

		/// <summary>
		/// Abastecimento de energia elétrica – Inexistente. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_abasEnergiaInexistente { get; set; }

		/// <summary>
		/// Esgoto sanitário – Rede pública. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_esgotoRedePublica { get; set; }

		/// <summary>
		/// Esgoto sanitário – Fossa. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_esgotoFossa { get; set; }

		/// <summary>
		/// Esgoto sanitário – Inexistente. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_esgotoInexistente { get; set; }

		/// <summary>
		/// Destinação do lixo – Coleta periódica. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoColeta { get; set; }

		/// <summary>
		/// Destinação do lixo – Queima. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoQueima { get; set; }

		/// <summary>
		/// Destinação do lixo – Joga em outra área. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoJoga { get; set; }

		/// <summary>
		/// Destinação do lixo – Recicla. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoRecicla { get; set; }

		/// <summary>
		/// Destinação do lixo – Enterra. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoEnterra { get; set; }

		/// <summary>
		/// Destinação do lixo – Outros. Valores permitidos: 0 – Não, 1 – Sim.
		/// </summary>
		public virtual bool prc_destLixoOutros { get; set; }

		/// <summary>
		/// Situação do registro: 1 - Ativo, 3 - Excluido.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual short prc_situacao { get; set; }

		/// <summary>
		/// Data de criação do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime prc_dataCriacao { get; set; }

		/// <summary>
		/// Data de alteração do registro.
		/// </summary>
		[MSNotNullOrEmpty]
		public virtual DateTime prc_dataAlteracao { get; set; }

    }
}
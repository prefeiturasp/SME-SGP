namespace MSTech.GestaoEscolar.Entities
{
    using System;
using System.ComponentModel;
using System.Data;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DCL_Protocolo : Abstract_DCL_Protocolo
    {
        /// <summary>
        /// Propriedade pro_id.
        /// </summary>
        [DataObjectField(true, false, false)]
        public override Guid pro_id { get; set; }
        
        /// <summary>
        /// Propriedade pro_tipo.
        /// </summary>
        [MSNotNullOrEmpty("Tipo de protocolo é obrigatório.")]
        public override byte pro_tipo { get; set; }

        /// <summary>
        /// Propriedade pro_protocolo.
        /// </summary>
        public override long pro_protocolo { get; set; }

        /// <summary>
        /// Propriedade pro_pacote.
        /// </summary>
        [MSNotNullOrEmpty("Pacote é obrigatório.")]
        public override string pro_pacote { get; set; }

        /// <summary>
        /// Propriedade pro_status.
        /// </summary>
        [MSNotNullOrEmpty("Status é obrigatório.")]
        public override byte pro_status { get; set; }

        /// <summary>
        /// Propriedade pro_situacao.
        /// </summary>
        [MSDefaultValue(1)]
        public override byte pro_situacao { get; set; }

        /// <summary>
        /// Propriedade pro_dataCriacao.
        /// </summary>
        public override DateTime pro_dataCriacao { get; set; }

        /// <summary>
        /// Propriedade pro_dataalteracao.
        /// </summary>
        public override DateTime pro_dataalteracao { get; set; }

        /// <summary>
        /// Propriedade pro_pacote.
        /// </summary>
        [MSValidRange(100, "Versão do aplicativo deve conter até 100 caracteres.")]
        public override string pro_versaoAplicativo { get; set; }

        public long idAula { get; set; }

        public static DataTable TipoTabela_Protocolo()
        {
            DataTable dtProtocolo = new DataTable();
            dtProtocolo.Columns.Add("idAula", typeof(Int64));
            dtProtocolo.Columns.Add("pro_id", typeof(Guid));
            dtProtocolo.Columns.Add("pro_status", typeof(Byte));
            dtProtocolo.Columns.Add("pro_statusObservacao", typeof(String));
            dtProtocolo.Columns.Add("tur_id", typeof(Int64));
            dtProtocolo.Columns.Add("tud_id", typeof(Int64));
            dtProtocolo.Columns.Add("tau_id", typeof(Int32));
            dtProtocolo.Columns.Add("pro_qtdeAlunos", typeof(Int32));
            dtProtocolo.Columns.Add("pro_situacao", typeof(Byte));
            dtProtocolo.Columns.Add("pro_tentativa", typeof(Int32));
            dtProtocolo.Columns.Add("esc_id", typeof(Int32));
            return dtProtocolo;
        }
    }
}

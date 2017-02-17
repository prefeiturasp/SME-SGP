namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    using System;
    using System.Collections.Generic;
    using MSTech.GestaoEscolar.Entities;
    using Newtonsoft.Json;

    public class ESC_EscolaDTO : ESC_Escola
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDTO> listaTurma { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TUR_TurmaDocenteDTO> listaTurmaDocente { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ACA_CurriculoPeriodoDTO> listaCurriculoPeriodo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string uad_nomeSuperiorGestao { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string end_logradouro { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string end_bairro { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string end_cep { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cid_nome { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string chr_cargaHorariaSemanal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string chr_horasAula { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string chr_horasComplementares { get; set; }

        public class Referencia
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public long esc_id
            {
                get;
                set;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }

        public struct EscolaEndereco
        {
            public string Uad_codigo { get; set; }
            public string Uad_nome { get; set; }
            public string Uad_nomeSuperior { get; set; }
            public string End_logradouro { get; set; }
            public string End_bairro { get; set; }
            public string End_cep { get; set; }
            public string Uae_numero { get; set; }
            public string Cid_nome { get; set; }
            public string esc_nome { get; set; }
            public string esc_codigo { get; set; }
            public Guid Ent_id { get; set; }
            public Guid Uad_id { get; set; }
            public int esc_id { get; set; }
            public string TipoEscola { get; set; }
        }

        public struct EscolaDadosBasicos
        {
            public int esc_id { get; set; }
            public Guid ent_id { get; set; }
            public Guid uad_id { get; set; }
            public string uad_codigo { get; set; }
            public string uad_nome { get; set; }
        }

        /*
            Tais comentários abaixo serão utilizados futuramente no método POST
            já adequando ao Save do EscolaBO.
        */
        #region Atributos método POST

        //  
        //public Escola ESC_Escola { get; set; }
        //public ESC_UnidadeEscolaDTO ESC_UnidadeEscola { get; set; }
        //public ESC_PredioDTO ESC_Predio { get; set; }
        //public END_EnderecoDTO END_Endereco { get; set; }
        //public ESC_PredioEnderecoDTO ESC_PredioEndereco { get; set; }
        //public ESC_UnidadeEscolaPredioDTO ESC_UnidadeEscolaPredio { get; set; }
        //public List<ESC_EscolaPapeisDTO> List_ESC_EscolaPapeis { get; set; }
        //public List<ESC_EscolaOrgaoSupervisaoDTO> List_ESC_EscolaOrgaoSupervisao { get; set; }
        //public List<ESC_UnidadeEscolaContatoDTO> List_ESC_UnidadeEscolaContato { get; set; }
        //public ESC_EscolaClassificacaoVigenciaDTO ESC_EscolaClassificacaoVigencia { get; set; }
        //public List<ESC_EscolaClassificacaoDTO> List_ESC_EscolaClassificacao { get; set; }
        //public List<ACA_CursoDTO> List_ACA_Curso { get; set; }
        //public List<ACA_CurriculoEscolaPeriodoDTO> List_ACA_CurriculoEscolaPeriodo { get; set; }
        //public List<ESC_EscolaEmpresaDTO> List_ESC_EscolaEmpresa { get; set; }

        //public ESC_EscolaDTO()
        //{
        //    this.ESC_Escola = new Escola();
        //    this.ESC_UnidadeEscola = new ESC_UnidadeEscolaDTO();
        //    this.ESC_Predio = new ESC_PredioDTO();
        //    this.END_Endereco = new END_EnderecoDTO();
        //    this.ESC_PredioEndereco = new ESC_PredioEnderecoDTO();
        //    this.ESC_UnidadeEscolaPredio = new ESC_UnidadeEscolaPredioDTO();
        //    this.List_ESC_EscolaPapeis = new List<ESC_EscolaPapeisDTO>();
        //    this.List_ESC_EscolaOrgaoSupervisao = new List<ESC_EscolaOrgaoSupervisaoDTO>();
        //    this.List_ESC_UnidadeEscolaContato = new List<ESC_UnidadeEscolaContatoDTO>();
        //    this.ESC_EscolaClassificacaoVigencia = new ESC_EscolaClassificacaoVigenciaDTO();
        //    this.List_ESC_EscolaClassificacao = new List<ESC_EscolaClassificacaoDTO>();
        //    this.List_ACA_Curso = new List<ACA_CursoDTO>();
        //    this.List_ACA_CurriculoEscolaPeriodo = new List<ACA_CurriculoEscolaPeriodoDTO>();
        //}

        #endregion Atributos método POST

    }
}

namespace GestaoAcademica.WebApi.Controllers
{
    using Authentication;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;
    /// <summary>
    /// Recurso para renderizar gráficos de atendimento.
    /// </summary>
    public class graficoAtendimentoController : ApiController
    {
        /// <summary>
        /// Descrição: Retorna os dados para renderizar o gráfico de atendimento
        /// </summary>
        /// <param name="gra_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication(false)]
        [JWTAuthenticationFilter()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public GraficoAtendimentoSaidaDTO Get
        (
            int gra_id,
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id
        )
        {
            try
            {
                return REL_GraficoAtendimentoBO.SelecionarDadosGrafico
                    (
                        gra_id,
                        esc_id,
                        uni_id,
                        cur_id,
                        crr_id,
                        crp_id
                    );
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                GraficoAtendimentoSaidaDTO saidaDTO = new GraficoAtendimentoSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                saidaDTO.Dados = new List<GraficoAtendimentoDadoDTO>();
                return saidaDTO;
            }
        }
    }
}

using GestaoAcademica.WebApi.Security;
using MSQuartz.Core;
using MSQuartz.Core.SchedulerProviders;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.Web.WebProject;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class servicosController : ApiController
    {
        private const string username = "admin_services";
        private const string password = "'ne1')>ZRN";

        /// <summary>
        /// Seleciona o status dos serviços configurados
        /// -- Utilização: URL_API/servicos/GetStatusServicos/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServicosAuthentication(username, password)]
        public List<StatusServicosDTO> GetStatusServicos()
        {
            try
            {
                List<StatusServicosDTO> status = ApiBO.SelecionarStatusServicosAPI();
                return status;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Salva o serviço (reativa a trigger no Quartz)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServicosAuthentication(username, password)]
        public HttpResponseMessage Post()
        {
            try
            {
                string json = Request.Content.ReadAsStringAsync().Result;

                if (SalvarServico(json))
                    return Request.CreateResponse(HttpStatusCode.Created, "");
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro: " + e.Message);
            }
        }

        /// <summary>
        /// Salva o serviço (reativa a trigger no Quartz)
        /// </summary>
        /// <param name="json">Parâmetros</param>
        /// <returns></returns>
        private static bool SalvarServico(string json)
        {
            JArray listaDados = (JArray.Parse(json) ?? new JArray());

            foreach (var dados in listaDados)
            {
                string jobName = dados.ToString();

                const string groupName = "DEFAULT";
                string cronExpression = SYS_ServicosBO.GeraCronExpression(jobName);
                
                if (string.IsNullOrEmpty(cronExpression))
                    throw new Exception("Expressão de agendamento de serviço não encontrada. " + jobName);

                string triggerName = "Trigger_" + jobName;
                byte ser_ativo = SYS_ServicosBO.SelecionaStatusServico(jobName);

                if (ser_ativo != (byte)GestaoEscolarServicosBO.eServicoAtivo.Ativo)
                    throw new Exception("Serviço não está ativo. " + jobName);

                var jobKey = new JobKey(jobName, groupName);
                
                // Verifica se a trigger já está criada 
                if (ApplicationWEB.SchedulerProvider.Scheduler.CheckExists(jobKey))
                {
                    // Caso a trigger já exista, deleta para cria-la com as configurações novas
                    ApplicationWEB.SchedulerDataProvider.DeleteTrigger(new TriggerKey(triggerName, groupName));
                }

                // Verifica se o serviço está habilitado
                if (ser_ativo == (byte)GestaoEscolarServicosBO.eServicoAtivo.Ativo)
                {
                    // Cria a trigger com as configurações novas
                    ApplicationWEB.SchedulerDataProvider.ScheduleCronTriggerForJob(jobKey, triggerName, cronExpression);
                }
            }

            return true;
        }

    }
}

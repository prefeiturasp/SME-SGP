using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using System.ComponentModel.DataAnnotations;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.Entities;

namespace AreaAluno
{
    public partial class Index : MotherPageLogado
    {
        #region Métodos

        /// <summary>
        /// Gets or sets the arguments mensagem log.
        /// </summary>
        /// <value>
        /// The arguments mensagem log.
        /// </value>
        /// <author>juliano.real</author>
        /// <datetime>12/10/2013-09:20</datetime>
        public string sMensagemLog { get; set; }

        /// <summary>
        /// Rertona o Id do modulo da query string
        /// </summary>
        /// <value>
        /// The get modulo unique identifier.
        /// </value>
        protected int GetModuloId
        {
            get
            {
                int mod_id = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["mod_id"]))
                {
                    Int32.TryParse(Request.QueryString["mod_id"], out mod_id);
                }
                //Retorna zero para trazer todos os menus inclusive o nó do sistema
                return mod_id;
            }
        }

        #endregion Métodos

        #region Eventos

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <author>juliano.real</author>
        /// <datetime>12/10/2013-09:20</datetime>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">Usuário não autorizado a exibir o area aluno.</exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String sMensagemLog = "";

                divResponsavel.Visible = __SessionWEB.__UsuarioWEB.responsavel;

                Int64 alu_id = 0;
                long arq_idFoto = 0;
                if (__SessionWEB.__UsuarioWEB.responsavel && __SessionWEB.__UsuarioWEB.alu_id > 0)
                {
                    alu_id = __SessionWEB.__UsuarioWEB.alu_id;
                    PES_Pessoa pesAluno = new PES_Pessoa { pes_id = __SessionWEB.__UsuarioWEB.pes_idAluno };
                    PES_PessoaBO.GetEntity(pesAluno);
                    arq_idFoto = pesAluno.arq_idFoto;
                }
                else if (__SessionWEB.__UsuarioWEB.responsavel)
                {
                    alu_id = ACA_AlunoBO.SelectAlunoby_pes_id(__SessionWEB.__UsuarioWEB.pes_idAluno);
                    PES_Pessoa pesAluno = new PES_Pessoa { pes_id = __SessionWEB.__UsuarioWEB.pes_idAluno };
                    PES_PessoaBO.GetEntity(pesAluno);
                    arq_idFoto = pesAluno.arq_idFoto;
                }
                else
                {
                    alu_id = ACA_AlunoBO.SelectAlunoby_pes_id(__SessionWEB.__UsuarioWEB.Usuario.pes_id);
                    PES_Pessoa pesAluno = new PES_Pessoa { pes_id = __SessionWEB.__UsuarioWEB.Usuario.pes_id };
                    PES_PessoaBO.GetEntity(pesAluno);
                    arq_idFoto = pesAluno.arq_idFoto;
                }

                if (alu_id <= 0)
                {
                    sMensagemLog = "Usuário não autorizado a exibir Area do Aluno: usu_id: " + __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString();
                    throw new ValidationException("Usuário não autorizado a exibir o Area do Aluno.");
                }

                ACA_Aluno entityAluno = ACA_AlunoBO.GetEntity(new ACA_Aluno { alu_id = alu_id });
                bool boletimBloqueado = false;
                bool compromissoEstudoBloqueado = !ACA_TipoCicloBO.VerificaSeExibeCompromissoAluno(alu_id);

                if (entityAluno.alu_possuiInformacaoSigilosa && entityAluno.alu_bloqueioBoletimOnline)
                {
                    if (__SessionWEB.__UsuarioWEB.responsavel)
                    {
                        Fieldset2.Visible = true;
                        lblBoletimNaoDisponivel.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("AreaAluno", "Index.lblBoletimNaoDisponivel.Text").ToString(), UtilBO.TipoMensagem.Informacao);
                        Fieldset1.Visible = false;
                        return;
                    }
                    else
                    {
                        boletimBloqueado = true;
                    }
                }

                if (arq_idFoto > 0)
                {
                    string imagem = "";
                    CFG_Arquivo arquivo = new CFG_Arquivo { arq_id = arq_idFoto };
                    CFG_ArquivoBO.GetEntity(arquivo);
                    byte[] bufferData = arquivo.arq_data;

                    using (MemoryStream stream = new MemoryStream(bufferData))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        imagem = Convert.ToBase64String(stream.ToArray());
                    }

                    imgFotoAluno.ImageUrl = "data:" + arquivo.arq_typeMime + ";base64," + imagem;
                }

                DataTable dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);

                if (dtCurriculo.Rows.Count <= 0)
                {
                    sMensagemLog = "Aluno não possui dados para a Area do Aluno: alu_id: " + alu_id.ToString();
                    throw new ValidationException("Aluno não possui dados para a Area do Aluno.");
                }

                string nomeAluno = dtCurriculo.Rows[0]["pes_nome"].ToString();
                string matriculaEstadual = dtCurriculo.Rows[0]["alc_matriculaEstadual"].ToString();
                string numeroMatricula = dtCurriculo.Rows[0]["alc_matricula"].ToString();

                //Nome Aluno
                lblInformacaoAluno.Text = "Aluno: <b>" + nomeAluno + "</b><br/>";

                //Matricula
                if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                {
                    if (!string.IsNullOrEmpty(matriculaEstadual))
                        lblInformacaoAluno.Text += " <b>" + GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": " + "</b>" + matriculaEstadual + "&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    if (!string.IsNullOrEmpty(numeroMatricula))
                        lblInformacaoAluno.Text += GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA") + ": <b>" + numeroMatricula + "</b>" + "&nbsp;&nbsp;&nbsp;";
                }

                __SessionWEB.__UsuarioWEB.alu_id = Convert.ToInt64(dtCurriculo.Rows[0]["alu_id"].ToString());
                __SessionWEB.__UsuarioWEB.esc_id = Convert.ToInt32(dtCurriculo.Rows[0]["esc_id"].ToString());
                __SessionWEB.__UsuarioWEB.uni_id = Convert.ToInt32(dtCurriculo.Rows[0]["uni_id"].ToString());
                __SessionWEB.__UsuarioWEB.mtu_id = Convert.ToInt32(dtCurriculo.Rows[0]["mtu_id"].ToString());
                __SessionWEB.__UsuarioWEB.tpc_id = Convert.ToInt32(string.IsNullOrEmpty(dtCurriculo.Rows[0]["tpc_id"].ToString()) ? "-1" : dtCurriculo.Rows[0]["tpc_id"].ToString());

                int mod_id = GetModuloId;

                string menuXml = SYS_ModuloBO.CarregarSiteMapXML2(
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        __SessionWEB.__UsuarioWEB.Grupo.sis_id,
                        __SessionWEB.__UsuarioWEB.Grupo.vis_id,
                        mod_id
                        );
                if (String.IsNullOrEmpty(menuXml))
                    menuXml = "<menus/>";
                menuXml = menuXml.Replace("url=\"~/", String.Concat("url=\"", ApplicationWEB._DiretorioVirtual));

                // Verifica se o aluno está com o boletim bloqueado. Se estiver, retiro do menu.
                int indiceBoletim = menuXml.IndexOf("<menu id=\"Boletim");
                if (boletimBloqueado && indiceBoletim >= 0)
                {
                    menuXml = menuXml.Remove(indiceBoletim, menuXml.IndexOf("/>", indiceBoletim) - indiceBoletim + 2);
                }

                IDictionary<string, ICFG_Configuracao> configuracao;
                MSTech.GestaoEscolar.BLL.CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                if (configuracao.ContainsKey("AppURLAreaAlunoInfantil") && configuracao["AppURLAreaAlunoInfantil"].cfg_valor != null)
                {
                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    string configInfantil = configuracao["AppURLAreaAlunoInfantil"].cfg_valor;

                    if (url.Contains(configInfantil))
                        menuXml = menuXml.Replace("menu id=\"Boletim Online\"", "menu id=\"" + (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "MenuBoletimInfantil") + "\"");
                }

                // Verifica se o aluno está com o compromisso estudo bloqueado. Se estiver, retiro do menu.
                int indiceCompromissoEstudo = menuXml.IndexOf("<menu id=\"Compromisso de Estudo");
                if (compromissoEstudoBloqueado && indiceCompromissoEstudo >= 0)
                {
                    menuXml = menuXml.Remove(indiceCompromissoEstudo, menuXml.IndexOf("/>", indiceCompromissoEstudo) - indiceCompromissoEstudo + 2);
                }

                XmlTextReader reader = new XmlTextReader(new StringReader(menuXml));
                XPathDocument treeDoc = new XPathDocument(reader);
                XslCompiledTransform siteMap = new XslCompiledTransform();

                if (__SessionWEB.__UsuarioWEB.responsavel)
                {
                    siteMap.Load(String.Concat(__SessionWEB._AreaAtual._DiretorioIncludes, "SiteMapResponsavel.xslt"));
                }
                else
                {
                    siteMap.Load(String.Concat(__SessionWEB._AreaAtual._DiretorioIncludes, "SiteMap.xslt"));
                }

                StringWriter sw = new StringWriter();
                siteMap.Transform(treeDoc, null, sw);
                string result = sw.ToString();

                List<CFG_ModuloClasse> lstModClasse = CFG_ModuloClasseBO.SelecionaAtivos(ApplicationWEB.AreaAlunoSistemaID);

                if (lstModClasse.Any())
                {
                    //Carrega a lista de link e moduloId
                    Dictionary<string, string> linkModulo = new Dictionary<string, string>();
                    string[] linkMenusXml = menuXml.Split(new[] { "<menu id=\"" }, StringSplitOptions.None);
                    if (linkMenusXml.Length > 0)
                    {
                        bool primeiroItem = true;
                        foreach (string item in linkMenusXml)
                        {
                            if (!primeiroItem)
                            {
                                string link = item.Substring(item.IndexOf("url=\"") + 5, item.Substring(item.IndexOf("url=\"") + 5).IndexOf("\""));
                                string modulo = item.Substring(0, item.IndexOf("\""));
                                linkModulo.Add(link, modulo);
                            }
                            primeiroItem = false;
                        }
                    }

                    //Carrega a lista de link e classe css atual
                    Dictionary<string, string> linkClasse = new Dictionary<string, string>();
                    string[] linkMenus = result.Split(new[] { "<li class=\"txtSubMenu\"><a " }, StringSplitOptions.None);
                    if (linkMenus.Length > 0)
                    {
                        bool primeiroItem = true;
                        foreach (string item in linkMenus)
                        {
                            if (!primeiroItem)
                            {
                                string link = item.Substring(item.IndexOf("href=\"") + 6, item.Substring(item.IndexOf("href=\"") + 6).IndexOf("\""));
                                string classe = item.Substring(item.IndexOf("class=\"") + 7, item.Substring(item.IndexOf("class=\"") + 7).IndexOf("\""));
                                linkClasse.Add(link, "class=\"" + classe + "\" " + "href=\"" + link);
                            }
                            primeiroItem = false;
                        }
                    }

                    //Troca a classe css atual do link conforme o que está configurado na tabela filtrando pelo modulo
                    if (linkModulo.Count > 0 && linkClasse.Count > 0)
                    {
                        foreach (var item in linkClasse)
                        {
                            string modulo = linkModulo[item.Key];
                            if (!string.IsNullOrEmpty(modulo) && lstModClasse.Any(p => p.mod_nome == modulo))
                            {
                                string classeCfg = lstModClasse.Where(p => p.mod_nome == modulo).FirstOrDefault().mdc_classe;
                                if (!string.IsNullOrEmpty(classeCfg))
                                    result = result.Replace(item.Value, "class=\"link " + classeCfg + "\" " + "href=\"" + item.Key);
                            }
                        }
                    }
                }

                //Control ctrl = Page.ParseControl(result);
                _lblSiteMap.Text = result;

                if (!string.IsNullOrEmpty(ApplicationWEB.UrlAcessoExternoBoletimOnline))
                {
                    string[] crp_ordem = ApplicationWEB.Crp_ordem_AcessoExternoBoletimOnline;
                    // Só exibe o ícone caso o aluno esteja em alguma das séries parametrizadas.
                    if (crp_ordem.Contains(dtCurriculo.Rows[0]["crp_ordem"].ToString()))
                    {
                        // Seta um nó de menu para acesso ao site externo.
                        ulItemAcessoExterno.Visible = true;
                        lblAcessoExterno.Text = GetGlobalResourceObject("AreaAluno", "Index.lblAcessoExternoNome").ToString();
                        lnkAcessoExterno.HRef = ApplicationWEB.UrlAcessoExternoBoletimOnline;
                        h2TituloAcessoExterno.InnerHtml = GetGlobalResourceObject("AreaAluno", "Index.lblAcessoExternoNome").ToString();
                    }
                }

                sMensagemLog = "Area do Aluno exibida para aluno: alu_id: " + alu_id.ToString();
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Query, sMensagemLog);
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                btnVoltar.PostBackUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";
                btnVoltar.Visible = true;
                divInformacao.Visible = false;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao exibir a Area do aluno", UtilBO.TipoMensagem.Erro);
                btnVoltar.PostBackUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";
                btnVoltar.Visible = true;
                divInformacao.Visible = false;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

        }

        #endregion
    }
}
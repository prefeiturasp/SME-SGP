using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;

namespace AreaAluno
{
    public partial class MasterPageAluno : MotherMasterPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Exibe o título no navegador
            Page.Title = __SessionWEB.TituloGeral;

            #region Adiciona links de favicon

            HtmlLink link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon.ico");
            link.Attributes["rel"] = "shortcut icon";
            link.Attributes["sizes"] = "";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-57x57.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "57x57";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-114x114.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "114x114";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-72x72.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "72x72";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-144x144.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "144x144";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-60x60.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "60x60";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-120x120.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "120x120";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-76x76.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "76x76";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/apple-touch-icon-152x152.png");
            link.Attributes["rel"] = "apple-touch-icon";
            link.Attributes["sizes"] = "152x152";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-196x196.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "196x196";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-160x160.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "160x160";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-96x96.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "96x96";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-16x16.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "16x16";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-32x32.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "32x32";
            Page.Header.Controls.Add(link);

            link = new HtmlLink();
            link.Href = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/favicon-32x32.png");
            link.Attributes["rel"] = "icon";
            link.Attributes["sizes"] = "32x32";
            Page.Header.Controls.Add(link);

            HtmlMeta meta = new HtmlMeta();
            meta.Name = "msapplication-TileImage";
            meta.Content = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/mstile-144x144.png");
            Page.Header.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Name = "msapplication-config";
            meta.Content = ResolveUrl("~/App_Themes/" + TemaAtual + "/images/favicons/browserconfig.xml");
            Page.Header.Controls.Add(meta);

            #endregion Adiciona links de favicon


            if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LISTA_SISTEMAS_AREA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                 && !ApplicationWEB.LoginProprioDoSistema)
            {
                //Carrega logo geral do sistema
                imgGeral.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoGeral);
                imgGeral.ToolTip = __SessionWEB.TituloGeral;
                imgGeral.AlternateText = __SessionWEB.TituloGeral;
                ImgLogoGeral.ToolTip = __SessionWEB.TituloGeral;
                ImgLogoGeral.NavigateUrl = __SessionWEB.UrlCoreSSO + "/Sistema.aspx";
            }
            else
            {
                h1Logo.Visible = ImgLogoGeral.Visible = false;
                if (UCSistemas1 != null)
                {
                    UCSistemas1.Visible = false;
                }
            }

            //Atribui o caminho do logo do sistema atual, caso ele exista no Sistema Administrativo
            if (string.IsNullOrEmpty(__SessionWEB.UrlLogoSistema))
                ImgLogoSistemaAtual.Visible = false;
            else
            {
                //Carrega logo do sistema atual
                imgSistemaAtual.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoSistema);
                imgSistemaAtual.AlternateText = __SessionWEB.TituloSistema;
                imgSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
                ImgLogoSistemaAtual.ToolTip = __SessionWEB.TituloSistema;
                ImgLogoSistemaAtual.NavigateUrl = "~/Index.aspx";
            }

            try
            {
                // Busca dados do aluno

                Int64 alu_id = 0;
                if (__SessionWEB.__UsuarioWEB.responsavel && __SessionWEB.__UsuarioWEB.alu_id > 0)
                {
                    alu_id = __SessionWEB.__UsuarioWEB.alu_id;
                }
                else if (__SessionWEB.__UsuarioWEB.responsavel)
                {
                    alu_id = ACA_AlunoBO.SelectAlunoby_pes_id(__SessionWEB.__UsuarioWEB.pes_idAluno);
                }
                else
                {
                    alu_id = ACA_AlunoBO.SelectAlunoby_pes_id(__SessionWEB.__UsuarioWEB.Usuario.pes_id);
                }

                bool boletimBloqueado = false;
                bool compromissoEstudoBloqueado = false;
                if (alu_id > 0)
                {
                    compromissoEstudoBloqueado = !ACA_TipoCicloBO.VerificaSeExibeCompromissoAluno(alu_id);
                    DataTable dtCurriculo = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(alu_id);
                    if (dtCurriculo.Rows.Count > 0)
                    {
                        string nomeAluno = dtCurriculo.Rows[0]["pes_nome"].ToString();
                        string parametroMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        string matriculaEstadual = dtCurriculo.Rows[0]["alc_matriculaEstadual"].ToString();
                        string numeroMatricula = dtCurriculo.Rows[0]["alc_matricula"].ToString();

                        litNomeAluno.Text = __SessionWEB.__UsuarioWEB.responsavel ? (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "litNomeAluno.Text.Responsavel") : (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "litNomeAluno.Text.Aluno");
                        lblNomeAluno.Text = nomeAluno;
                        lblMatricula.Text = string.IsNullOrEmpty(parametroMatriculaEstadual) ? numeroMatricula : matriculaEstadual;
                        lblNroMatricula.Text = string.IsNullOrEmpty(parametroMatriculaEstadual)
                            ? GetGlobalResourceObject("AreaAluno", "MasterPageAluno.lblNroMatricula.Text").ToString()
                            : parametroMatriculaEstadual + ":";
                    }

                    ACA_Aluno entityAluno = ACA_AlunoBO.GetEntity(new ACA_Aluno { alu_id = alu_id });                 
                    if (entityAluno.alu_possuiInformacaoSigilosa && entityAluno.alu_bloqueioBoletimOnline)
                    {
                        boletimBloqueado = true;
                    }
                }

                if (__SessionWEB.__UsuarioWEB.responsavel && boletimBloqueado)
                {
                    Response.Redirect("~/Index.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                // Se ja tiver logado mostra o linkbutton pra selecionar o aluno, senão esconde
                if (Session["Pes_Id_Responsavel"] != null)
                {
                    if (Convert.ToInt32(Session["Qtde_Filhos_Responsavel"]) > 1)
                    {
                        lbSelecaoAlunos.Visible = true;
                    }
                    else
                    {
                        lbSelecaoAlunos.Visible = false;
                    }
                    imgLogoAreaAluno.Visible = false;
                    imgLogoAreaResp.Visible = true;
                }
                else
                {
                    lbSelecaoAlunos.Visible = false;
                    imgLogoAreaAluno.Visible = true;
                    imgLogoAreaResp.Visible = false;
                }

                // Busca dados do menu

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
                    siteMap.Load(String.Concat(__SessionWEB._AreaAtual._DiretorioIncludes, "SiteMapPagesResponsavel.xslt"));
                }
                else
                {
                    siteMap.Load(String.Concat(__SessionWEB._AreaAtual._DiretorioIncludes, "SiteMapPages.xslt"));
                }               

                string page = Page.Request.Url.ToString();

                StringWriter sw = new StringWriter();
                siteMap.Transform(treeDoc, null, sw);

                string result = sw.ToString();


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
                            string link2 = item.Substring(item.IndexOf("url=\"") + 5, item.Substring(item.IndexOf("url=\"") + 5).IndexOf("\""));
                            string modulo = item.Substring(0, item.IndexOf("\""));
                            linkModulo.Add(link2, modulo);
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
                            string link2 = item.Substring(item.IndexOf("href=\"") + 6, item.Substring(item.IndexOf("href=\"") + 6).IndexOf("\""));
                            string classe = item.Substring(item.IndexOf("class=\"") + 7, item.Substring(item.IndexOf("class=\"") + 7).IndexOf("\""));
                            linkClasse.Add(link2, "class=\"" + classe + "\" " + "href=\"" + link2);
                        }
                        primeiroItem = false;
                    }
                }

                //Troca a classe css atual do link conforme o que está configurado na tabela filtrando pelo modulo
                List<CFG_ModuloClasse> lstModClasse = new List<CFG_ModuloClasse>();
                if (linkModulo.Count > 0 && linkClasse.Count > 0)
                {
                    lstModClasse = CFG_ModuloClasseBO.SelecionaAtivos(ApplicationWEB.AreaAlunoSistemaID);
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


                int indexPagina = result.IndexOf(Page.Request.Url.ToString()) > 0 ? result.IndexOf(Page.Request.Url.ToString()) : result.IndexOf(Page.Request.UrlReferrer.ToString());
                int indexClasse = 0;
                if (indexPagina > 0)
                {
                    indexClasse = result.IndexOf("txtSubMenu", (indexPagina - 60), result.Length - indexPagina);
                }

                string resultClasse = result.Substring(indexClasse);

                // Busca menu que está sendo chamado, para adicionar a classe ativo para a aba selecionada
                if (indexClasse > 0)
                {
                    result = result.Replace(resultClasse, "ativo " + resultClasse);
                }

                Control ctrl = Page.ParseControl(result);
                menuAreaAlunoComponentes.Controls.Add(ctrl);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }
    }
}
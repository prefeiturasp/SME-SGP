using System.Web;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.Entities;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Academico.Aluno.CapturaFoto
{
    public partial class Default : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// True - Retorna para a tela de busca
        /// False - Retorna para a tela de cadastro
        /// </summary>
        private bool busca
        {
            get
            {
                if (ViewState["busca"] != null)
                    return Convert.ToBoolean(ViewState["busca"]);
                return true;
            }
            set
            {
                ViewState["busca"] = value;
            }
        }

        /// <summary>
        /// Propriedade que seta a url de retorno da página.
        /// </summary>
        private string VS_PaginaRetorno
        {
            get
            {
                if (ViewState["VS_PaginaRetorno"] != null)
                    return ViewState["VS_PaginaRetorno"].ToString();

                return "";
            }
            set
            {
                ViewState["VS_PaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno.
        /// </summary>
        private object VS_DadosPaginaRetorno
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno Minhas turmas.
        /// </summary>
        private object VS_DadosPaginaRetorno_MinhasTurmas
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno_MinhasTurmas"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno_MinhasTurmas"] = value;
            }
        }

        /// <summary>
        /// ID do aluno.
        /// </summary>
        private long VS_alu_id
        {
            get
            {
                if (ViewState["VS_alu_id"] != null)
                    return Convert.ToInt64(ViewState["VS_alu_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// ID do aluno.
        /// </summary>
        private long VS_arq_id
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnArq.Value))
                    return Convert.ToInt64(hdnArq.Value);
                
                if (ViewState["VS_arq_id"] != null)
                    return Convert.ToInt64(ViewState["VS_arq_id"]);

                return -1;
            }
            set
            {
                ViewState["VS_arq_id"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Redireciona para a página de busca.
        /// </summary>
        private void Redireciona()
        {
            if (busca)
            {
                Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
            }
            else if (!string.IsNullOrEmpty(VS_PaginaRetorno))
            {
                Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
                Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
                Response.Redirect(VS_PaginaRetorno, false);
            }
            else
            {

                Session.Add("aluno", VS_alu_id);
                Session.Add("permissao", true);
                Response.Redirect("~/Academico/Aluno/Cadastro.aspx", false);
            }
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Deleta os arquivos temporários que foram criados
        /// </summary>
        private void DeletaArquivoTemporario()
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnArqExcluir.Value) && (!hdnArqExcluir.Value.Equals(";")))
                {
                    int i = 1;
                    while (i != 0)
                    {
                        if (!String.IsNullOrEmpty(hdnArqExcluir.Value.Split(';')[i]))
                        {
                            int id = Convert.ToInt32(hdnArqExcluir.Value.Split(';')[i]);

                            SYS_Arquivo arq = new SYS_Arquivo
                            {
                                arq_id = id
                            };
                            SYS_ArquivoBO.GetEntity(arq);

                            SYS_ArquivoBO.ExcluiFisicamente(arq);

                            i++;
                        }
                        else
                            i = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        /// <summary>
        /// Gera o endereço da imagem atual do aluno
        /// </summary>
        /// <param name="entPessoa">Pessoa do aluno</param>
        /// <param name="entFoto">Entidade da foto da pessoa</param>
        /// <returns>Retorna o endereço da imagem atual do aluno</returns>
        private static string CriaFotoAluno(PES_Pessoa entPessoa, out CFG_Arquivo entFoto)
        {
            entFoto = CFG_ArquivoBO.GetEntity(new CFG_Arquivo { arq_id = entPessoa.arq_idFoto });
            byte[] foto = entFoto.arq_data;

            return (foto == null || foto.Length == 0) ? string.Empty : "~/Academico/Aluno/CapturaFoto/Imagem.ashx?idfoto=" + entFoto.arq_id;
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                }
                
                string mensagemFlash = CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.CAPTURA_REQUERFLASH);
                lblMessageFlash.Text = UtilBO.GetErroMessage(mensagemFlash, UtilBO.TipoMensagem.Informacao);
                lblMessageFlash.Visible = !String.IsNullOrEmpty(mensagemFlash);
                if (Session["alu_id"] != null)
                {
                    if (Session["PaginaRetorno_CapturaFoto"] != null)
                    {
                        VS_PaginaRetorno = Session["PaginaRetorno_CapturaFoto"].ToString();
                        Session.Remove("PaginaRetorno_CapturaFoto");
                        VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                        Session.Remove("DadosPaginaRetorno");

                        VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                        Session.Remove("VS_DadosTurmas");
                    }

                    VS_alu_id = Convert.ToInt64(Session["alu_id"].ToString());
                    Session.Remove("alu_id");
                    busca = false;
                }

                if (((PreviousPage != null) && (PreviousPage.EditItem > 0)) || VS_alu_id != -1)
                {
                    if (VS_alu_id == -1)
                        VS_alu_id = PreviousPage.EditItem;

                    InfoComplementarAluno1.InformacaoComplementarAluno(VS_alu_id);
                    
                    ACA_Aluno alu = new ACA_Aluno
                    {
                        alu_id = VS_alu_id
                    };
                    ACA_AlunoBO.GetEntity(alu);

                    PES_Pessoa pes = new PES_Pessoa
                    {
                        pes_id = alu.pes_id
                    };
                    PES_PessoaBO.GetEntity(pes);

                    CFG_Arquivo entFoto;
                    string src = CriaFotoAluno(pes, out entFoto);

                    imgAntiga.Visible = !string.IsNullOrEmpty(src) && string.IsNullOrEmpty(hdnArqExcluir.Value);
                    lblDataFoto.Visible = imgAntiga.Visible;

                    if (imgAntiga.Visible)
                    {
                        const string script = "var existeImagem = true;";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "confirm", script, true);

                        imgAntiga.Src = src;
                        imgAntiga.Style.Remove("display");
                        lblDataFoto.Text = @"<br />Última alteração da foto: " + entFoto.arq_dataAlteracao.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        imgAntiga.Src = "";
                        imgAntiga.Style.Add("display", "none");
                    }
                }
                else
                {
                    Redireciona();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(hdnArq.Value) && string.IsNullOrEmpty(fupAnexo.FileName)) // Nao foi tirada nenhuma foto ou carregada.
            {
                if (imgAntiga.Visible)
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Foto do aluno salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    Redireciona();
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Nenhuma imagem foi carregada.", UtilBO.TipoMensagem.Alerta);
            }
            }
            else
            {
                try
                {
                    // Se houver imagem capturada e escolhida para upload, salvará a que foi capturada.
                    if (!string.IsNullOrEmpty(fupAnexo.FileName) && string.IsNullOrEmpty(hdnArq.Value)) 
                    {
                        HttpPostedFile arquivo = fupAnexo.PostedFile;

                        if (arquivo != null && arquivo.ContentLength > 0)
                        {
                            string fileNameApplication = System.IO.Path.GetFileName(arquivo.FileName);

                            if (fileNameApplication != String.Empty)
                            {

                                if (fupAnexo.PostedFile.FileName.Substring(fupAnexo.PostedFile.FileName.Length - 3, 3).ToUpper() != "JPG")
                                {
                                    throw new ValidationException("Foto tem que estar no formato \".jpg\".");
                                }

                                SYS_Arquivo entArquivo = SYS_ArquivoBO.CriarAnexo(arquivo);
                                entArquivo.arq_data = ACA_AlunoBO.RedimensionaFoto(entArquivo.arq_data, true);

                                SYS_ArquivoBO.Save(entArquivo);
                                VS_arq_id = entArquivo.arq_id;
                            }
                        }


                    string tam = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_MAX_FOTO_PESSOA);

                    if (!string.IsNullOrEmpty(tam))
                    {
                            if (fupAnexo.PostedFile.ContentLength > Convert.ToInt32(tam)*1000)
                        {
                            throw new ValidationException("Foto é maior que o tamanho máximo permitido.");
                        }

                        if (fupAnexo.PostedFile.FileName.Substring(fupAnexo.PostedFile.FileName.Length - 3, 3).ToUpper() != "JPG")
                        {
                            throw new ValidationException("Foto tem que estar no formato \".jpg\".");
                        }
                    }
                    }

                    if (ACA_AlunoBO.SalvarFotoAluno(VS_alu_id, VS_arq_id))
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "alu_id: " + VS_alu_id + ", arq_id: " + VS_arq_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Foto do aluno salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        DeletaArquivoTemporario();

                        Redireciona();
                    }
                    else
                    {
                        lblMessage.Text = UtilBO.GetErroMessage("Não foi possível salvar a foto do aluno.", UtilBO.TipoMensagem.Erro);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar foto do aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            hdnArqExcluir.Value += hdnArq.Value + ";";
            DeletaArquivoTemporario();
            Redireciona();
        }

        #endregion
    }
}

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Collections.Generic;
using MSTech.Validation.Exceptions;
using System.Text.RegularExpressions;

namespace AreaAluno
{
    public partial class Login : MotherPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string provider = IdentitySettingsConfig.IDSSettings.AuthenticationType;
                    Context.GetOwinContext().Authentication.Challenge(provider);
                }
                else
                {
                    if (!UserIsAuthenticated())
                        throw new Exception("Usuário não encontrado!");

                    string prefixoRESP = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_RESPONSAVEL_AREA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool responsavel = __SessionWEB.__UsuarioWEB.Usuario.usu_login.Contains(prefixoRESP);

                    // Se selecionou para logar como responsável, verifica se esse ele é responsável por um aluno só, 
                    //  ou caso tenha mais, redireciona para uma tela de selação de alunos
                    if (responsavel)
                    {
                        __SessionWEB.__UsuarioWEB.responsavel = true;
                        string prefixoAluno = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_ALUNO_AREA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        //Troca o prefixo de responsável por aluno
                        var regex = new Regex(Regex.Escape(prefixoRESP));
                        var loginAluno = regex.Replace(__SessionWEB.__UsuarioWEB.Usuario.usu_login, prefixoAluno, 1);

                        SYS_Usuario entityUsuarioAluno = new SYS_Usuario
                        {
                            ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                            usu_login = loginAluno
                        };

                        SYS_UsuarioBO.GetSelectBy_ent_id_usu_login(entityUsuarioAluno);
                        __SessionWEB.__UsuarioWEB.pes_idAluno = entityUsuarioAluno.pes_id;

                        DataTable dtAlunosDoResponsavel = ACA_AlunoResponsavelBO.SelecionaAlunosPorResponsavel(__SessionWEB.__UsuarioWEB.Usuario.pes_id);
                        Session["Pes_Id_Responsavel"] = __SessionWEB.__UsuarioWEB.Usuario.pes_id.ToString();
                        Session["Qtde_Filhos_Responsavel"] = dtAlunosDoResponsavel.Rows.Count;
                        if (dtAlunosDoResponsavel.Rows.Count > 1)
                        {
                            RedirecionarLogin(true);
                            return;
                        }
                    }
                    RedirecionarLogin(false);
                    return;
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar entrar no sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Redireciona para a tela de seleção do sistema
        /// </summary>
        private void RedirecionarLogin(bool RedirecionaIndexSelecaoAluno)
        {
            if (RedirecionaIndexSelecaoAluno)
            {
                Response.Redirect("~/IndexSelecaoAluno.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
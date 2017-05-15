using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Classe_Efetivacao_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de ava_id
    /// </summary>
    public int _VS_ava_id
    {
        get
        {
            return UCEfetivacaoNotas1._VS_ava_id;
        }
        set
        {
            UCEfetivacaoNotas1._VS_ava_id = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
    /// </summary>
    public int _VS_fav_id
    {
        get
        {
            return UCEfetivacaoNotas1._VS_fav_id;
        }
        set
        {
            UCEfetivacaoNotas1._VS_fav_id = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
    /// </summary>
    public long _VS_tur_id
    {
        get
        {
            return UCEfetivacaoNotas1._VS_tur_id;
        }
        set
        {
            UCEfetivacaoNotas1._VS_tur_id = value;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UCEfetivacaoNotas1.VisibleBotaoCancelar = true;
            UCEfetivacaoNotas1.VisibleNavegacao = true;

            if (!IsPostBack)
            {
                if (Session["tur_idEfetivacao"] != null
                    && Session["tud_idEfetivacao"] != null
                    && Session["fav_idEfetivacao"] != null
                    && Session["ava_idEfetivacao"] != null
                    && Session["URL_Retorno_Efetivacao"] != null)
                {
                    UCEfetivacaoNotas1._VS_tur_id = Convert.ToInt64(Session["tur_idEfetivacao"]);
                    long tud_id = Convert.ToInt64(Session["tud_idEfetivacao"]);
                    UCEfetivacaoNotas1._VS_fav_id = Convert.ToInt32(Session["fav_idEfetivacao"]);
                    UCEfetivacaoNotas1._VS_ava_id = Convert.ToInt32(Session["ava_idEfetivacao"]);
                    UCEfetivacaoNotas1._VS_URL_Retorno_Efetivacao = Convert.ToByte(Session["URL_Retorno_Efetivacao"]);

                    UCEfetivacaoNotas1.SetaTurmaDisciplina(tud_id, UCEfetivacaoNotas1._VS_tur_id, -1);

                    // Tud_id que será enviado de volta para a tela que chamou (necessário na tela de cadastro de aulas).
                    if (Session["tud_id_Retorno"] != null)
                    {
                        UCEfetivacaoNotas1._VS_tud_id_Retorno = Convert.ToInt64(Session["tud_id_Retorno"]);
                        Session.Remove("tud_id_Retorno");
                    }

                    Session.Remove("tur_idEfetivacao");
                    Session.Remove("tud_idEfetivacao");
                    Session.Remove("fav_idEfetivacao");
                    Session.Remove("ava_idEfetivacao");
                    Session.Remove("URL_Retorno_Efetivacao");
                }
                else
                {
                    UCEfetivacaoNotas1.RedirecionaBusca(UtilBO.GetErroMessage("É necessário selecionar a turma e a avaliação.", UtilBO.TipoMensagem.Alerta));
                }
            }
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            UCEfetivacaoNotas1.MensagemTela = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }


    #endregion

    #region Métodos

    [WebMethod]
    public static bool VerificarIntegridadeParecerEOL(string CodigoEOLTurma, string CodigoEOLAluno, string resultado)
    {
        return GestaoEscolar.WebControls.AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacaoGeral.VerificarIntegridadeParecerEOL(CodigoEOLTurma, CodigoEOLAluno, resultado);
    }

    #endregion
}

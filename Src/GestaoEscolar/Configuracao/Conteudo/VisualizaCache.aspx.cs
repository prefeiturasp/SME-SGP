using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL.Caching;

namespace GestaoEscolar.Configuracao.Conteudo
{
    public partial class VisualizaCache : MotherPageLogado
    {
        protected IDictionary<String, String> PegaChaves(bool atualizar = false)
        {
            if (atualizar || ViewState["Chaves"] == null)
            {
                System.Collections.IDictionaryEnumerator myCache = Cache.GetEnumerator();
                IDictionary<String, String> listaPorValor = CacheManager.Factory.GetAllKey().ToDictionary(x => x.Key, x => x.GetType().ToString());

                myCache.Reset();

                while (myCache.MoveNext())
                {
                    listaPorValor.Add(myCache.Key.ToString(), myCache.Key.GetType().ToString());
                }
                ViewState["Chaves"] = listaPorValor;
            }
            return ViewState["Chaves"] as Dictionary<String, String>;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (__SessionWEB.__UsuarioWEB == null ||
                __SessionWEB.__UsuarioWEB.Usuario == null ||
                __SessionWEB.__UsuarioWEB.Usuario.usu_login != "admin")
            {
                Response.Redirect("~/logout.ashx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        protected void btnLimpaCache_Click(object sender, EventArgs e)
        {
            try
            {
                rptTipos.Visible = false;
                int keys = 0;
                System.Collections.IDictionaryEnumerator myCache = Cache.GetEnumerator();

                myCache.Reset();

                while (myCache.MoveNext())
                {
                    keys++;
                    Cache.Remove(myCache.Key.ToString());
                }

                keys += CacheManager.Factory.GetAllKey().Count;
                CacheManager.Factory.Clear();

                ViewState["Chaves"] = null;

                lblInformacao.Text = UtilBO.GetErroMessage("Quantidade de caches removidos: " + keys, UtilBO.TipoMensagem.Sucesso);
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o cache: " + ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
        protected void btnVerCache_Click(object sender, EventArgs e)
        {
            try
            {
                rptTipos.Visible = false;
                lblInformacao.Text = UtilBO.GetErroMessage("Quantidade de chaves no cache: " + PegaChaves(true).Count, UtilBO.TipoMensagem.Informacao);
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ver o cache: " + ex.Message
                                                        , UtilBO.TipoMensagem.Erro);
            }
        }
        protected void btnListarCache_Click(object sender, EventArgs e)
        {
            try
            {
                rptTipos.Visible = true;
                rptTipos.DataSource = PegaChaves(true).GroupBy(t => t.Value).ToDictionary(grp => grp.Key, grp => grp.Count());
                rptTipos.DataBind();

                if (rptTipos.Items.Count <= 0)
                {
                    lblInformacao.Text = UtilBO.GetErroMessage("Não há itens para exibir.", UtilBO.TipoMensagem.Informacao);
                    rptTipos.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ver o cache: " + ex.Message, UtilBO.TipoMensagem.Erro);
            }
        }
        protected void rptTipos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var rptChaves = e.Item.FindControl("rptChaves") as Repeater;
            rptChaves.Visible = !rptChaves.Visible;
            if (e.CommandName == "ListarChaves")
            {
                var chaves = PegaChaves();
                var btnChave = e.Item.FindControl("btnChave") as LinkButton;
                var textoCommand = btnChave.CommandArgument;
                var select = chaves.Where(x => x.Value.ToString() == textoCommand).ToDictionary(x => x.Key, x => x.Value);
                rptChaves.DataSource = select.Keys;
                rptChaves.DataBind();
            }
        }
        protected void rptTipos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataitem = (KeyValuePair<String, int>)e.Item.DataItem;
                var btnChave = e.Item.FindControl("btnChave") as LinkButton;
                btnChave.CommandArgument = dataitem.Key.ToString();
            }
        }
    }
}
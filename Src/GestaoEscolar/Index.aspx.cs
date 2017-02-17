using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Index : MotherPageLogadoCompressedViewState
{
    #region Métodos

    /// <summary>
    /// Rertona o Id do modulo da query string
    /// </summary>
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                _lblMessage.Text = message;
            }
            _lblMessage.Visible = !(string.IsNullOrEmpty(message));

            string menuXml = SYS_ModuloBO.CarregarSiteMapXML2(
                __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                __SessionWEB.__UsuarioWEB.Grupo.sis_id,
                __SessionWEB.__UsuarioWEB.Grupo.vis_id,
                GetModuloId
                );
            if (String.IsNullOrEmpty(menuXml))
                menuXml = "<menus/>";
            menuXml = menuXml.Replace("url=\"~/", "url=\"");

            XmlTextReader reader = new XmlTextReader(new StringReader(menuXml));
            XPathDocument treeDoc = new XPathDocument(reader);
            XslCompiledTransform siteMap = new XslCompiledTransform();
            siteMap.Load(Server.MapPath("Includes/SiteMap.xslt"));

            StringWriter sw = new StringWriter();
            siteMap.Transform(treeDoc, null, sw);
            string result = sw.ToString();

            Control ctrl = Page.ParseControl(result);
            _lblSiteMap.Controls.Add(ctrl);

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
        }
    }

    #endregion Eventos
}
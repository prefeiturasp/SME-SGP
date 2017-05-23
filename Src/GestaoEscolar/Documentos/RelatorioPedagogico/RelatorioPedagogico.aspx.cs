namespace GestaoEscolar.Documentos.RelatorioPedagogico
{
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;

    public partial class RelatorioPedagogico : MotherPageLogado
    {
        public long alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["alu_id"] ?? "-1");
            }
            set
            {
                ViewState["alu_id"] = value;
            }
        }

        public string URLRetorno
        {
            get
            {
                if (ViewState["URLRetorno"] == null)
                {
                    ViewState["URLRetorno"] = Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/Aluno/Busca.aspx");
                }

                return ViewState["URLRetorno"].ToString();
            }
            set
            {
                ViewState["URLRetorno"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["alu_id_RelatorioPedagogico"] != null)
                {
                    alu_id = Convert.ToInt32(Session["alu_id_RelatorioPedagogico"]);
                    Session.Remove("alu_id_RelatorioPedagogico");
                    if (Session["PaginaRetorno_RelatorioPedagogico"] != null)
                    {
                        URLRetorno = Session["PaginaRetorno_RelatorioPedagogico"].ToString();
                        Session.Remove("PaginaRetorno_RelatorioPedagogico");
                    }
                    else
                    {
                        URLRetorno = Path.Combine(MSTech.Web.WebProject.ApplicationWEB._DiretorioVirtual, "Academico/Aluno/Busca.aspx");
                    }
                }

                if (alu_id > 0)
                {
                    UCRelatorioPedagogico.Carregar(alu_id, URLRetorno);
                }
                else
                {
                    RedirecionarPagina(URLRetorno);
                }
            }
        }
    }
}
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;

[WebService(Namespace = "WSServicos")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[System.Web.Script.Services.ScriptService]
public class WSServicos : System.Web.Services.WebService
{
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BuscaEnderecosLogradouro(string end_cep, string end_logradouro)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        string str = js.Serialize(MSTech.CoreSSO.BLL.END_EnderecoBO.GetSelectBy_end_cep_end_logradouro(end_cep, end_logradouro));
        return str;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BuscaCidades(string cid_nome)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        string str = js.Serialize(MSTech.CoreSSO.BLL.END_CidadeBO.GetSelectBy_PesquisaIncremental(cid_nome));
        return str;
    }
}



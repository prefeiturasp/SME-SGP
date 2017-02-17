<%@ WebHandler Language="C#" Class="Imagem" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;

public class Imagem : IHttpHandler 
{     
    public void ProcessRequest (HttpContext context) 
    { 
        context.Response.Clear(); 
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"])) 
        {
            Guid id = new Guid(context.Request.QueryString["id"].ToString()); 
            PES_Pessoa pes = new PES_Pessoa()
            {
                pes_id = id
            };
            PES_PessoaBO.GetEntity(pes);

            CFG_Arquivo entArquivo = PES_PessoaBO.RetornaFotoPor_Pessoa(id);
            
            if (entArquivo.arq_data != null)
            {
                try
                {
                    context.Response.Clear();
                    context.Response.Expires = 0; //Expira o cache do Browser.
                    context.Response.ContentType = "image/jpeg";
                    context.Response.OutputStream.Write(entArquivo.arq_data, 0, entArquivo.arq_data.Length - 1);
                }
                catch (Exception e)
                {
                    MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(e);
                }           
            }
        }         
    } 
    public bool IsReusable 
    { 
        get 
        { 
            return false; 
        } 
   }       
}
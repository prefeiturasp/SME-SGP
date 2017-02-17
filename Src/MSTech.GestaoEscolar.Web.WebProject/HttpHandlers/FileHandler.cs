using System;
using System.Web;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    /// <summary>
    /// Classe que implementa a interface IHttpHandler para fazer download de arquivos.
    /// Busca o arquivo passado por parâmetro na pasta padrão de arquivos. 
    /// Para buscar um arquivo em uma subpasta, é só passar no nome da pasta junto com o 
    ///     nome do arquivo. 
    ///     Ex: file=Subpasta\nomearquivo.txt
    /// Para chamar a página:
    ///     arquivo.ashx
    /// Passar parâmetro:
    ///     file=Pasta\nomearquivo.txt
    /// Ex:
    /// arquivo.ashx?file=nomearquivo.txt
    /// </summary>
    public class FileHandler : IHttpHandler
    {
        /// <summary>
        /// 10K bytes
        /// </summary>
        const int size = 10000;
        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that 
        /// implements the IHttpHandler interface.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            if (!String.IsNullOrEmpty(context.Request.QueryString["file"]))
            {
                try
                {
                    string sArq_id = context.Server.UrlDecode(context.Request.QueryString["file"]);
                    long arq_id;

                    if ((Int64.TryParse(sArq_id, out arq_id)) && (arq_id > 0))
                    {
                        SYS_Arquivo arquivo = RetornaArquivoPorID(arq_id);
                        if (!arquivo.IsNew)
                        {                            
                            try
                            {
                                context.Response.Clear();
                                context.Response.ClearHeaders();
                                context.Response.ClearContent();

                                context.Response.Buffer = false;
                                context.Response.BufferOutput = false;     

                                context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + arquivo.arq_nome + "\"");
                                context.Response.ContentType = "application/octet-stream";
                                context.Response.AddHeader("Content-Length", arquivo.arq_tamanhoKB.ToString());

                                int length = 0;
                                // Total bytes to read:
                                long dataToRead = arquivo.arq_data.LongLength;   
                                byte[] bufferData = arquivo.arq_data;         
                                
                                // Read the bytes.
                                while (dataToRead > 0)
                                {
                                    // Verify that the client is connected.
                                    if (context.Response.IsClientConnected)
                                    {
                                        context.Response.OutputStream.Write(bufferData, length, (size > dataToRead ? (int)dataToRead : size));
                                        context.Response.Flush(); 
                                        length += size;  
                                        dataToRead = dataToRead - size;
                                    }
                                    else
                                    {
                                        //prevent infinite loop if user disconnects
                                        dataToRead = -1;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ApplicationWEB._GravaErro(ex);
                                context.ApplicationInstance.CompleteRequest();
                                context.Server.ClearError();
                            }
                            finally
                            {
                                context.Response.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Retorna a entidade pelo ID.
        /// </summary>
        /// <param name="arq_id">ID do arquivo.</param>
        /// <returns></returns>
        private SYS_Arquivo RetornaArquivoPorID(long arq_id)
        {
            SYS_Arquivo arquivo = new SYS_Arquivo {arq_id = arq_id};
            arquivo = SYS_ArquivoBO.GetEntity(arquivo);

            return arquivo;
        }

        #endregion
    }
}

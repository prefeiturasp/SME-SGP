using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;


namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    /// <summary>
    /// Classe que implementa a interface IHttpHandler para visualizar imagens.
    /// Busca a imagem passada por parâmetro na pasta padrão de arquivos. 
    /// Para chamar a página:
    ///     imagem.ashx?picture=imagem.jpg
    /// Para buscar uma imagem em uma subpasta, é só passar no nome da pasta junto com o 
    ///     nome da imagem. 
    ///     Ex: picture=Pasta\imagem.jpg
    ///     
    /// Para visualizar a imagem com dimensões customizadas passar:
    ///     w=100 (Width - Largura)
    ///     h=200 (Height - Altura)
    ///     
    /// Exemplo: imagem.ashx?picture=Pasta\imagem.jpg&w=100&h=200
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        #region Constantes

        private static readonly List<string> extensoesValidas =
            new List<string> { ".png", ".jpg" };

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that 
        /// implements the IHttpHandler interface.
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
            {
                string fileName = UtilBO.DecodificaQueryString(context.Request.QueryString["id"]) + ".jpg";

                bool redimensionar = ((!String.IsNullOrEmpty(context.Request.QueryString["w"])) &&
                                      (!String.IsNullOrEmpty(context.Request.QueryString["h"])));

                try
                {
                    string folder = context.Server.MapPath("~/App_Themes");

                    FileInfo info = new FileInfo(folder + Path.DirectorySeparatorChar + fileName);

                    if (info.Directory.FullName.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((info.Exists) && IsImage(info))
                        {
                            context.Response.Cache.SetCacheability(HttpCacheability.Public);
                            context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));

                            if (redimensionar)
                            {
                                int heigth = Convert.ToInt32(context.Request.QueryString["h"]);
                                int width = Convert.ToInt32(context.Request.QueryString["w"]);

                                Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
                                Image img = Image.FromFile(info.FullName);

                                // Criando objeto redimencionado.
                                Image thumb = img.GetThumbnailImage
                                    (
                                    width,
                                    heigth,
                                    myCallback,
                                    IntPtr.Zero
                                    );

                                // ContentType sempre JPG.
                                context.Response.ContentType = "image/png";

                                // Enviando email para Response.
                                thumb.Save
                                    (
                                    context.Response.OutputStream,
                                    System.Drawing.Imaging.ImageFormat.Jpeg
                                    );

                                img.Dispose();
                                thumb.Dispose();
                            }
                            else
                            {
                                int index = fileName.LastIndexOf(".") + 1;
                                string extension = fileName.Substring(index).ToUpperInvariant();

                                // Fix for IE not handling jpg image types
                                if (string.Compare(extension, "JPG") == 0)
                                    context.Response.ContentType = "image/png";
                                else
                                    context.Response.ContentType = "image/" + extension;

                                context.Response.TransmitFile(info.FullName);
                            }

                            context.Response.Flush();
                        }
                        else
                        {
                            // Limpa o response.
                            context.Response.Flush();
                            context.Response.Clear();
                            context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    else
                    {
                        context.Response.Redirect("~/Manutencao.aspx?erro=404", false);
                        context.ApplicationInstance.CompleteRequest();
                    }

                    //string folder = ApplicationWEB._DiretorioVirtual;

                    //string i = VirtualPathUtility.Combine(folder, fileName);


                    //folder += !folder.EndsWith("\\") ? "\\" : "";

                    //string s = context.Server.MapPath(fileName);

                    //FileInfo fi = new FileInfo(context.Server.MapPath(folder) + fileName);

                    //fi = new FileInfo(
                    //    folder +
                    //    (fileName.StartsWith("\\", StringComparison.OrdinalIgnoreCase)
                    //         ? ""
                    //         : Path.DirectorySeparatorChar.ToString()) +
                    //    fileName);


                }
                catch (Exception)
                {
                    try
                    {
                        // Limpa o response.
                        context.Response.Flush();
                        context.Response.Clear();
                        context.ApplicationInstance.CompleteRequest();
                    }
                    catch
                    {
                    }
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica pela extensão se é uma imagem válida.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        private bool IsImage(FileInfo fi)
        {
            string ext = fi.Extension;

            return extensoesValidas.Exists(p => p.Equals(ext, StringComparison.OrdinalIgnoreCase));
        }

        private bool ThumbnailCallback()
        {
            return true;
        }

        #endregion
    }
}

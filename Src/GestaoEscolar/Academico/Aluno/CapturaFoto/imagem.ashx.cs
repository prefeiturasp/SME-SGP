using System;
using System.Drawing;
using System.Web;
using System.IO;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.Academico.Aluno.CapturaFoto
{
    /// <summary>
    /// 
    /// </summary>
    public class imagem : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (!String.IsNullOrEmpty(context.Request.Params["conteudoXML"]))
                {
                    string retorno = createImage(171, context.Request.Params["conteudoXML"].Split(';'));
                    context.Response.Write("retorno=" + retorno);
                }
                else if (!String.IsNullOrEmpty(context.Request.QueryString["file"]))
                {
                    EnviaImagem(context.Server.UrlDecode(context.Request.QueryString["file"]), context);
                }
                else if (!String.IsNullOrEmpty(context.Request.QueryString["idFoto"]))
                {
                    EnviaImagemPessoa(context.Server.UrlDecode(context.Request.QueryString["idFoto"]), context);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                context.ApplicationInstance.CompleteRequest();
                context.Server.ClearError();
            }
        }

        private void EnviaImagemPessoa(string sArq_id, HttpContext context)
        {
            long arq_id;

            if ((Int64.TryParse(sArq_id, out arq_id)) && (arq_id > 0))
            {
                CFG_Arquivo arquivo = CFG_ArquivoBO.GetEntity(new CFG_Arquivo {arq_id = arq_id});

                if (!arquivo.IsNew)
                {
                    try
                    {
                        byte[] bufferData = arquivo.arq_data;

                        MemoryStream stream = new MemoryStream(bufferData);
                        Image img = Image.FromStream(stream);

                        context.Response.Clear();
                        context.Response.ContentType = arquivo.arq_typeMime;
                        context.Response.BinaryWrite(bufferData);
                        context.Response.Flush();

                        img.Dispose();
                        stream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        context.ApplicationInstance.CompleteRequest();
                        context.Server.ClearError();
                    }
                    finally
                    {
                        context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        /// <summary>
        /// Envia o conteúdo da imagem no response.
        /// </summary>
        /// <param name="sArq_id">ID do arquivo que representa a imagem.</param>
        /// <param name="context">HttpContext</param>
        private static void EnviaImagem(string sArq_id, HttpContext context)
        {
            long arq_id;

            if ((Int64.TryParse(sArq_id, out arq_id)) && (arq_id > 0))
            {
                SYS_Arquivo arquivo = RetornaArquivoPorID(arq_id);
                if (!arquivo.IsNew)
                {
                    try
                    {
                        byte[] bufferData = arquivo.arq_data;

                        MemoryStream stream = new MemoryStream(bufferData);
                        Image img = Image.FromStream(stream);

                        context.Response.Clear();
                        context.Response.ContentType = arquivo.arq_typeMime;
                        context.Response.BinaryWrite(bufferData);
                        context.Response.Flush(); 
                                    
                        img.Dispose();
                        stream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        context.ApplicationInstance.CompleteRequest();
                        context.Server.ClearError();
                    }
                    finally
                    {
                        context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        /// <summary>
        /// Retorna a entidade pelo ID.
        /// </summary>
        /// <param name="arq_id">ID do arquivo.</param>
        /// <returns></returns>
        private static SYS_Arquivo RetornaArquivoPorID(long arq_id)
        {
            SYS_Arquivo arquivo = new SYS_Arquivo { arq_id = arq_id };
            arquivo = SYS_ArquivoBO.GetEntity(arquivo);

            return arquivo;
        }

        /// <summary>
        /// Cria um bitmap da imagem de acordo com os pixels.
        /// </summary>
        /// <param name="width">Tamanho da imagem</param>
        /// <param name="array">Array de pixels da imagem</param>
        /// <returns></returns>
        private static string createImage(int width, string[] array)
        {
            int height = array.Length / width;
            Bitmap bmp = new Bitmap(width, height);
            int i = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(array[i])));
                    i++;
                }
            }
            
            string retorno = SalvarArquivo(bmp);

            bmp.Dispose();

            return retorno;
        }

        /// <summary>
        /// Salva o arquivo na SYS_Arquivo, como arquivo temporário.
        /// </summary>
        /// <param name="bmp">Imagem a ser salva como arquivo</param>
        /// <returns>ID do arquivo gerado</returns>
        private static string SalvarArquivo(Bitmap bmp)
        {
            MemoryStream stream = new MemoryStream();

            // Enviando email para Response.
            bmp.Save
                (
                stream,
                System.Drawing.Imaging.ImageFormat.Jpeg
                );

            stream.Seek(0, SeekOrigin.Begin);

            byte[] arquivo = stream.GetBuffer();
            stream.Dispose();

            string nome = DateTime.Now.ToString("yyyyMMdd-HH_mm_ss") + ".jpg";

            SYS_Arquivo entArquivo = new SYS_Arquivo
                                         {
                                             arq_nome = nome
                                             , arq_tamanhoKB = arquivo.Length
                                             , arq_typeMime = "Image/jpeg"
                                             , arq_data = arquivo
                                             , arq_situacao = (byte)SYS_ArquivoSituacao.Temporario
                                         };

            SYS_ArquivoBO.Save(entArquivo);

            return entArquivo.arq_id.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

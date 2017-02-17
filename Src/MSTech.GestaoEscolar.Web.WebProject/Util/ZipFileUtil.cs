using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System.IO.Compression;
using Ionic.Zip;

namespace MSTech.GestaoEscolar.Web.WebProject.Util
{
    /// <summary>
    /// Classe de utilitários para trabalhar com arquivos compactados.
    /// </summary>
    public class ZipFileUtil
    {
                        
        #region

        /// <summary>
        /// Gerar um arquivo compactado.
        /// </summary>
        /// <param name="conteudoArquivo">Conteúdo do arquivo de texto a ser compactado.</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado.</param>
        /// <returns>Stream com o arquivo para ser enviado no response.</returns>
        public static MemoryStream CompactaArquivoString(string conteudoArquivo, string nomeArquivo, Dictionary<string, byte[]> arquivos)
        {
            MemoryStream streamRetorno = new MemoryStream();
            using (ZipOutputStream s = new ZipOutputStream(streamRetorno))
            {
                s.PutNextEntry(nomeArquivo + ".tmp");
                byte[] buffer = Encoding.GetEncoding("iso-8859-1").GetBytes(conteudoArquivo);
                s.Write(buffer, 0, buffer.Length);

                foreach (var itemArquivos in arquivos)
                {
                    s.PutNextEntry(itemArquivos.Key);
                    s.Write(itemArquivos.Value, 0, itemArquivos.Value.Length);
                }
                
                s.Close();
            }

            return streamRetorno;
        }

        #endregion
    }
}

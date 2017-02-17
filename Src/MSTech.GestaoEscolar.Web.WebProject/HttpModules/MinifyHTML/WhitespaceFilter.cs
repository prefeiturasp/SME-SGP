using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MSTech.GestaoEscolar.Web.WebProject.HttpModules.MinifyHTML
{
    internal class WhitespaceFilter : Stream
    {
        /// <summary>
        /// A regular expression to localize all whitespace preceeding HTML tag endings.
        /// </summary>
        //Regex(@">(?! )\s+" 
        private static readonly Regex RegexBetweenTags = new Regex(@">\s+", RegexOptions.Compiled);

        /// <summary>
        /// A regular expression to localize all whitespace preceeding a line break.
        /// </summary>
        //Regex(@"([\n\s])+?(?<= {2,})<"
        private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2})<", RegexOptions.Compiled);

        private Encoding encoding = Encoding.Default;

        private readonly Stream _stream;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position { get; set; }

        public override void Flush()
        {
            _stream.Flush();
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Close()
        {
            _stream.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];

            Buffer.BlockCopy(buffer, offset, data, 0, count);

            string html = encoding.GetString(buffer);
            
            html = RegexBetweenTags.Replace(html, ">");
            html = RegexLineBreaks.Replace(html, "<");
            //[TODO]: html = RemoveWhiteSpaceFromStylesheets(html);

            byte[] output = encoding.GetBytes(html);
            _stream.Write(output, 0, output.GetLength(0));
        }

        public WhitespaceFilter(Stream stream)
        {
            _stream = stream;
        }


        /// <summary>
        /// Strips the whitespace from any .css file.
        /// </summary>
        /// <param name="body">The body/contents of the CSS file.</param>
        /// <returns>The body of the CSS file stripped from whitespace and comments.</returns>
        public static string RemoveWhiteSpaceFromStylesheets(string body)
        {
            body = Regex.Replace(body, @"[a-zA-Z]+#", "#");
            body = Regex.Replace(body, @"[\n\r]+\s*", string.Empty);
            body = Regex.Replace(body, @"\s+", " ");
            body = Regex.Replace(body, @"\s?([:,;{}])\s?", "$1");
            body = body.Replace(";}", "}");
            body = Regex.Replace(body, @"([\s:]0)(px|pt|%|em)", "$1");

            // Remove comments from CSS
            body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty);

            return body;
        }

        //public static string RemoveWhiteSpaceFromJavaScript(string body)
        //{
        //    JavaScriptMinifier jsmin = new JavaScriptMinifier();
        //    return jsmin.Minify(body);
        //}
    }
}

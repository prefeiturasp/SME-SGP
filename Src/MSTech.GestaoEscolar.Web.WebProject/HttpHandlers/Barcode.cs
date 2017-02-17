using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnoRiver.BarcodeDeveloper;
using System.Drawing;


namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    public class Barcode : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";

            int fontSize = 9;
            if (context.Request.QueryString["fontSize"] != null)
                fontSize = Convert.ToInt32(context.Request.QueryString["fontSize"]);

            BarcodeControl barcode = new BarcodeControl();
            barcode.BarcodeData = context.Request.QueryString["code"];
            barcode.Symbology = BarcodeControl.BarcodeSymbology.CODE39;
            barcode.DisplayCheckDigitText = BarcodeControl.BarcodeDisplayCheckDigitText.No;
            barcode.DisplayStartStopCharacter = BarcodeControl.BarcodeDisplayStartStopCharacter.No;
            barcode.Font = new Font("Verdana, Arial, Helvetica, sans-serif", fontSize);

            byte[] b = barcode.GetImageJPG();
            context.Response.OutputStream.Write(b, 0, b.Length - 1);
        }

        #endregion
    }
}

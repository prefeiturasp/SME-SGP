using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GestaoEscolar.Api.Areas.HelpPage.Attributes
{
    public class ResponseCodesAttribute : Attribute
    {
        public ResponseCodesAttribute(params HttpStatusCode[] statusCodes)
        {
            ResponseCodes = statusCodes;
        }

        public IEnumerable<HttpStatusCode> ResponseCodes { get; private set; }
    }
}
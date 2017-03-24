using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace GestaoEscolar.Api.Areas.HelpPage.Attributes
{
    // the attribute itself - to put against our action parameters.
    // combines the model binding engine with our value provider factory.
    public class FromHeaderAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            var httpConfig = parameter.Configuration;
            var binder = new ModelBinderAttribute()
                .GetModelBinder(httpConfig, parameter.ParameterType);
            return new ModelBinderParameterBinding(
                parameter, binder,
                new ValueProviderFactory[] { new HeaderValueProviderFactory() });
        }
    }

    // factory for creating our custom value provider class given a specific action
    // context.  this is where we capture and keep reference to the HTTP headers
    // for the request .
    public class HeaderValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new HeaderValueProvider(actionContext.Request.Headers);
        }
    }

    // Our value provider, which handles the bulk of the work 
    public class HeaderValueProvider : IValueProvider
    {
        private readonly HttpRequestHeaders _headers;

        public HeaderValueProvider(HttpRequestHeaders headers)
        {
            _headers = headers;
        }

        public bool ContainsPrefix(string prefix)
        {
            // all prefixes are flattened - all members and sub-members
            // considered equally 
            return true;
        }

        // the heart of the approach.  this will be called for each property of the
        // model we’re binding to – we need only find and return the appropriate
        // values
        public ValueProviderResult GetValue(string key)
        {
            IEnumerable<string> values;

            var propName = RemovePrefixes(key);
            var headerName = MakeHeaderName(propName);

            if (!_headers.TryGetValues(headerName, out values))
            {
                return null;
            }

            var data = string.Join(",", values.ToArray());
            return new ValueProviderResult(values, data, CultureInfo.InvariantCulture);
        }

        private static string RemovePrefixes(string key)
        {
            var lastDot = key.LastIndexOf('.');
            if (lastDot == -1) return key;

            return key.Substring(lastDot + 1);
        }

        // here’s the simple algorithm for making a HTTP header name out of our members:
        // iterate through the characters, inserting a dash before uppercase letters,
        // with the exception of the first character
        private static string MakeHeaderName(string key)
        {
            var headerBuilder = new StringBuilder();

            for (int i = 0; i < key.Length; i++)
            {
                if (char.IsUpper(key[i]) && i > 0)
                {
                    headerBuilder.Append('-');
                }
                headerBuilder.Append(key[i]);
            }

            return headerBuilder.ToString();
        }
    }
}
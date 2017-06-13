using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GestaoEscolar.Api.App_Start;
using GestaoEscolar.Api.Models;
using System.Reflection;
using System.ComponentModel;

namespace GestaoEscolar.Api.Controllers.Base
{
    [AuthenticationFilter()]
    public class BaseApiController : ApiController 
    {
        public UsuarioWEB __userLogged { get; set; }

        public static jsonObject[] EnumToJsonObject<T>()
        {
            List<jsonObject> lst = new List<jsonObject>();

            Type objType = typeof(T);
            FieldInfo[] propriedades = objType.GetFields();
            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    lst.Add(new jsonObject { id = Convert.ToString(objField.GetRawConstantValue()) , text = attributes[0].Description });
                }
            }

            return lst.ToArray();
        }
    }
}
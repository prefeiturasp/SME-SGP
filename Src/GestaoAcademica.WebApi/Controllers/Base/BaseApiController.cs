using GestaoAcademica.WebApi.Models;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers.Base
{
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
                    lst.Add(new jsonObject { id = Convert.ToString(objField.GetRawConstantValue()), text = attributes[0].Description });
                }
            }

            return lst.ToArray();
        }
    }
}

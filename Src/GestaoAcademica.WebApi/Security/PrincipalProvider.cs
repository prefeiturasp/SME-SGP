using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Rest.Core.Interface;
using MSTech.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace GestaoAcademica.WebApi.Security
{
    public class PrincipalProvider : IProvidePrincipal
    {
        /// <summary>
        /// Usuário configurado para autenticação da WebApi.
        /// </summary>
        private string UserName
        {
            get
            {
                return ApplicationWEB.Api_UserName;
            }
        }

        /// <summary>
        /// Senha configurada para autenticação da WebApi.
        /// </summary>
        private string Password
        {
            get
            {
                try
                {
                    string senha = ApplicationWEB.Api_Password;
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                    return sa.Decrypt(senha, System.Text.Encoding.UTF8);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IPrincipal CreatePrincipal(string userName, string password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                throw new Exception("Usuário não configurado.");
            }

            string senhaDescriptografada = "";

            try
            {
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                senhaDescriptografada = sa.Decrypt(password, System.Text.Encoding.UTF8);
            }
            catch
            {
                throw new Exception("Acesso não autorizado.");
            }

            if (userName != UserName || senhaDescriptografada != Password)
            {
                throw new Exception("Acesso não autorizado.");
            }

            var identity = new GenericIdentity(UserName);
            IPrincipal principal = new GenericPrincipal(identity, new[] { "User" });
            return principal;
        }

    }
}
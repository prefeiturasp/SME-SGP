using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using MSTech.GestaoEscolar.Web.WebProject;
using Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(AreaAluno.Startup))]
namespace AreaAluno
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var IDSSettings = IdentitySettingsConfig.IDSSettings;
            if (!string.IsNullOrEmpty(IdentitySettingsConfig.Msg))
            {
                app.Run(async context =>
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(IdentitySettingsConfig.Msg);
                });
            }
            else
            {
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = IDSSettings.Cookies_AuthenticationType,
                    LoginPath = new PathString(IDSSettings.Cookies_LoginPath),
                    CookieName = IDSSettings.Cookies_CookieName
                });

                app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    AuthenticationType = IDSSettings.AuthenticationType,
                    SignInAsAuthenticationType = IDSSettings.SignInAsAuthenticationType,
                    Authority = IDSSettings.Authority,
                    RedirectUri = IDSSettings.RedirectUri,
                    ClientId = IDSSettings.ClientId,
                    ClientSecret = IDSSettings.ClientSecret,
                    Scope = IDSSettings.Scope,
                    ResponseType = IDSSettings.ResponseType,
                    PostLogoutRedirectUri = IDSSettings.RedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = async n =>
                        {
                            var claims_to_exclude = new[] { "aud", "iss", "nbf", "exp", "nonce", "iat", "at_hash" };

                            //Exclude unnecessary claims
                            var claims_to_keep =
                                   n.AuthenticationTicket.Identity.Claims
                                   .Where(x => false == claims_to_exclude.Contains(x.Type)).ToList();

                            //Add id_token to user claims
                            claims_to_keep.Add(new Claim("id_token", n.ProtocolMessage.IdToken));

                            //Search userinfo from IS Claims
                            if (n.ProtocolMessage.AccessToken != null)
                            {
                                claims_to_keep.Add(new Claim("access_token", n.ProtocolMessage.AccessToken));
                                var userInfoClient = new UserInfoClient(new System.Uri(IDSSettings.EndpointUserInfo), n.ProtocolMessage.AccessToken);
                                var userInfoResponse = await userInfoClient.GetAsync();
                                var userInfoClaims = userInfoResponse.Claims
                                    .Select(x => new Claim(x.Item1, x.Item2));
                                claims_to_keep.AddRange(userInfoClaims);
                            }

                            var ci = new ClaimsIdentity(
                                n.AuthenticationTicket.Identity.AuthenticationType,
                                "name", "role");
                            ci.AddClaims(claims_to_keep);

                            n.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(
                                ci, n.AuthenticationTicket.Properties
                            );
                        },
                        RedirectToIdentityProvider = n =>
                        {
                            if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                            {
                                var id_token = n.OwinContext.Authentication.User.FindFirst("id_token")?.Value;
                                n.ProtocolMessage.IdTokenHint = id_token;
                            }

                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = context =>
                        {
                            if (context.Exception is OpenIdConnectProtocolInvalidNonceException)
                            {
                                // Handle Microsoft.IdentityModel.Protocols.OpenIdConnectProtocolInvalidNonceException:
                                // IDX10311: RequireNonce is 'true' (default) but validationContext.Nonce is null. A nonce
                                // cannot be validated. If you don't need to check the nonce, set
                                // OpenIdConnectProtocolValidator.RequireNonce to 'false'.
                                if (context.Exception.Message.Contains("IDX10311"))
                                {
                                    context.SkipToNextMiddleware();
                                }
                            }
                            return Task.FromResult(0);
                        }
                    }
                });
            }
        }
    }
}

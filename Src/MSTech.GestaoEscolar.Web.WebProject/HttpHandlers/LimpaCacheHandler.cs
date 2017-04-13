using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.BLL.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    public enum eTipoCache: byte
    {
        Fechamento = 1
        , RelatorioPendencias = 2
        , Geral = 3
    }

    public class LimpaCacheHandler : HttpTaskAsyncHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        protected List<string> RetornarListaCahveCacheRelatorioPendencias(HttpContext context)
        {
            string[] esc_ids = context.Request.QueryString["esc_ids"].ToString().Split(';');
            string[] uni_ids = context.Request.QueryString["uni_ids"].ToString().Split(';');
            string[] cal_ids = context.Request.QueryString["cal_ids"].ToString().Split(';');
            string[] tud_ids = context.Request.QueryString["tud_ids"].ToString().Split(';');

            List<string> ltChaves = new List<string>();

            for (int i = 0; i < tud_ids.Length; i++)
            {
                ltChaves.Add(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, esc_ids[i], uni_ids[i], cal_ids[i], tud_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_ids[i]));
                ltChaves.Add(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, esc_ids[i], uni_ids[i], cal_ids[i], tud_ids[i]));
            }

            return ltChaves;
        }

        protected List<string> RetornarListaCahveCacheFechamento(HttpContext context)
        {
            string[] tur_ids = context.Request.QueryString["tur_ids"].ToString().Split(';');
            string[] tud_ids = context.Request.QueryString["tud_ids"].ToString().Split(';');
            string[] fav_ids = context.Request.QueryString["fav_ids"].ToString().Split(';');
            string[] ava_ids = context.Request.QueryString["ava_ids"].ToString().Split(';');
            string[] tpc_ids = context.Request.QueryString["tpc_ids"].ToString().Split(';');

            List<string> ltChaves = new List<string>();

            for (int i = 0; i < tud_ids.Length; i++)
            {
                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_RECUPERACAO_FINAL_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_RECUPERACAO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY
                         , tud_ids[i]
                         , fav_ids[i]
                         , ava_ids[i]));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_BIMESTRE_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]
                        , string.Empty));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]
                        , string.Empty));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_FINAL_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]
                        , string.Empty));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]
                        , string.Empty));

                ltChaves.Add(String.Format(
                    ModelCache.FECHAMENTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY
                        , tud_ids[i]
                        , fav_ids[i]
                        , ava_ids[i]));

                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_MODEL_KEY, tud_ids[i], tpc_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_MODEL_KEY, tud_ids[i], tpc_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_MODEL_KEY,tur_ids[i], tpc_ids[i]));

                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_MODEL_KEY, tud_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_MODEL_KEY, tud_ids[i]));
                ltChaves.Add(String.Format(ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_MODEL_KEY, tur_ids[i]));
            }

            return ltChaves;
        }

        protected List<string> LimparCacheGeral(HttpContext context)
        {
            List<string> ltChaves = CacheManager.Factory.GetAllKey().Select(p => p.Key).ToList();
            return ltChaves;
        }

        public async override Task ProcessRequestAsync(HttpContext context)
        {
            if (!String.IsNullOrEmpty(context.Request.QueryString["tipoCache"]))
            {
                byte tipoCache = 0;
                List<string> ltChaves = new List<string>();

                if (Byte.TryParse(context.Request.QueryString["tipoCache"].ToString(), out tipoCache))
                {
                    try
                    {
                        await Task.Factory.StartNew(() =>
                            {
                                switch (tipoCache)
                                {
                                    case (byte)eTipoCache.Fechamento:
                                        ltChaves = RetornarListaCahveCacheFechamento(context);
                                        break;
                                    case (byte)eTipoCache.RelatorioPendencias:
                                        ltChaves = RetornarListaCahveCacheRelatorioPendencias(context);
                                        break;
                                    case (byte)eTipoCache.Geral:
                                        ltChaves = LimparCacheGeral(context);
                                        break;
                                    default:
                                        break;
                                }

                                ltChaves.ForEach(p => CacheManager.Factory.RemoveByPattern(p));
                            });
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                    }
                }
            }
        }
    }
}

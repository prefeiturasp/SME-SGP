namespace MSTech.GestaoEscolar.RelatoriosDevExpress
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.XtraCharts;
    using System.Drawing;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;

    class RelatoriosDevUtil
    {
        public static Palette CarregarPaletaCoresRelaorio(int rlt_id)
        {
            List<string> lstCoresRelatorio = CFG_CorRelatorioBO.SelecionaCoresRelatorio(rlt_id).Select(p => p.cor_corPaleta).ToList();

            Palette PaletaCores = new Palette("Gestao", PaletteScaleMode.Repeat);

            List<PaletteEntry> ltPaletaCores = lstCoresRelatorio.Select(p => new PaletteEntry(ColorTranslator.FromHtml(p))).ToList();

            foreach (PaletteEntry Paleta in ltPaletaCores)
            {
                PaletaCores.Add(Paleta);
            }
            
            return PaletaCores;
        }

        /// <summary>
        /// Carregars the paleta cores relaorio.
        /// </summary>
        /// <param name="lstCoresRelatorio">Lista de cores que serão carregadas na paleta</param>
        /// <returns>Palette</returns>
        public static Palette CarregarPaletaCoresRelatorio(List<string> lstCoresRelatorio)
        {
            Palette PaletaCores = new Palette("Gestao", PaletteScaleMode.Repeat);

            List<PaletteEntry> ltPaletaCores = lstCoresRelatorio.Select(p => new PaletteEntry(ColorTranslator.FromHtml(p))).ToList();

            foreach (PaletteEntry Paleta in ltPaletaCores)
            {
                PaletaCores.Add(Paleta);
            }
            
            return PaletaCores;
        }

        /// <summary>
        /// Carrega a lista de cores para o relatório
        /// </summary>
        /// <param name="rlt_id">Id do relatório</param>
        /// <returns></returns>
        public static List<CFG_CorRelatorio> SelecionaCoresRelatorio(int rlt_id)
        {
            return CFG_CorRelatorioBO.SelecionaCoresRelatorio(rlt_id);
        }

    }
}

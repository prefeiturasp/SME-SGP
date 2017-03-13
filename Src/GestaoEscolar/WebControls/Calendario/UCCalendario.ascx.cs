namespace GestaoEscolar.WebControls.Calendario
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;    
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;

    public partial class UCCalendario : MotherUserControl
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/fullcalendar/lib/moment.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/fullcalendar/fullcalendar.min.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/fullcalendar/lang-all.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCalendario.js"));
            }
        }
        #endregion Eventos

        #region Metodos

        public void CarregarCalendarioSemanal(List<CalendarioBO.EventTime> eventos, CalendarioBO.OptionsCalendar option, bool inicializar = true, int slotMin = -1, int slotMax = -1)
        {
            if (inicializar)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Script" + divCalendar.ClientID,
                    "loadCalendar(" +
                    CalendarioBO.GetJSonConfigEvents(option) + "," +
                    CalendarioBO.ConvertEventsToJson(eventos) + "," +
                    option.onClickEvent.ToString() + "," +
                    slotMin + "," +
                    slotMax +
                    ");", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScriptA" + divCalendar.ClientID,
                    "reLoadCalendar(" +
                    CalendarioBO.GetJSonConfigEvents(option) + "," +
                    CalendarioBO.ConvertEventsToJson(eventos) +
                    ");", true);
            }
        }

        public void RefreshEvents(string calendarClass)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScriptB" + divCalendar.ClientID,
                "refreshEvents(" + calendarClass + ");", true);
        }

        #endregion Metodos
    }
}
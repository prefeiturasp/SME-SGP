namespace MSTech.GestaoEscolar.BLL
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CalendarioBO
    {
        #region Enumeradores

        public enum optionView
        {
            month
            ,
            basicWeek
            ,
            basicDay
            ,
            agendaWeek
            ,
            agendaDay
        }

        #endregion Enumeradores

        #region Estruturas

        /// <summary>
        /// Estrutura principal evento para serializar
        /// </summary>
        [Serializable]
        private struct EventObject
        {
            public string id { get; set; }
            public string title { get; set; }
            public bool allDay { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string url { get; set; }
            public string className { get; set; }
            public bool editable { get; set; }
            public bool startEditable { get; set; }
            public bool durationEditable { get; set; }
            public bool overlap { get; set; }
            public string color { get; set; }
            public string backgroundColor { get; set; }
            public string borderColor { get; set; }
            public string textColor { get; set; }
        }

        /// <summary>
        /// Estrutura principal evento
        /// </summary>
        [Serializable]
        public struct Event
        {
            public string id { get; set; }
            public string title { get; set; }
            public bool allDay { get; set; }
            public string url { get; set; }
            public string className { get; set; }
            public bool editable { get; set; }
            public bool startEditable { get; set; }
            public bool durationEditable { get; set; }
            public bool overlap { get; set; }
            public string color { get; set; }
            public string backgroundColor { get; set; }
            public string borderColor { get; set; }
            public string textColor { get; set; }
        }

        /// <summary>
        /// Estrutura de entrada para eventos do calendario com data
        /// </summary>
        [Serializable]
        public struct EventDate
        {
            public Event evento { get; set; }
            public DateTime inicio { get; set; }
            public DateTime fim { get; set; }
        }

        /// <summary>
        /// Estrutura de entrada para eventos do calendario por hora
        /// </summary>
        [Serializable]
        public struct EventTime
        {
            public Event evento { get; set; }
            public eDiasSemana diaSemana { get; set; }
            public TimeSpan inicio { get; set; }
            public TimeSpan fim { get; set; }
        }

        /// <summary>
        /// Estrutura de entrada para configurações do calendario
        /// </summary>
        [Serializable]
        private struct OptionsObject
        {
            public string header { get; set; }
            public string defaultView { get; set; }
            public bool allDaySlot { get; set; }
            public bool slotEventOverlap { get; set; }
            public bool selectable { get; set; }

            public string columnFormat { get; set; }
            public string timeFormat { get; set; }

            public string slotDuration { get; set; }
            public string minTime { get; set; }
            public string maxTime { get; set; }
            public string scrollTime { get; set; }

            public float aspectRatio { get; set; }

            public string lang { get; set; }

            public string defaultDate { get; set; }

            public string calendarClass { get; set; }
        }

        /// <summary>
        /// Estrutura de entrada para configurações do calendario
        /// </summary>
        [Serializable]
        public struct OptionsCalendar
        {
            public string header { get; set; }
            public optionView defaultView { get; set; }
            public bool allDaySlot { get; set; }
            public bool slotEventOverlap { get; set; }
            public bool selectable { get; set; }

            public string columnFormat { get; set; }
            public string timeFormat { get; set; }

            public TimeSpan slotDuration { get; set; }
            public TimeSpan minTime { get; set; }
            public TimeSpan maxTime { get; set; }
            public TimeSpan scrollTime { get; set; }

            public float aspectRatio { get; set; }

            public string lang { get; set; }

            public DateTime defaultDate { get; set; }

            public string calendarClass { get; set; }
            public string onClickEvent { get; set; }
        }

        #endregion Estruturas

        #region Métodos

        private static TimeSpan TruncateTimeMinutes(TimeSpan hour, int min)
        {
            return TimeSpan.FromMinutes(min * Math.Truncate(hour.TotalMinutes / min));
        }

        public static string ConvertEventsToJson(List<EventTime> lstEvents)
        {
            if (lstEvents == null)
                return JsonConvert.SerializeObject(new EventObject());

            DateTime defaultDate = new DateTime(2000, 10, 01);

            List<EventObject> data = lstEvents.Select(p =>
                new EventObject
                {
                    id = p.evento.id
                    ,
                    title = p.evento.title
                    ,
                    allDay = p.evento.allDay
                    ,
                    start = defaultDate.AddDays((int)p.diaSemana - 1).Add(p.inicio).ToString("s")
                    ,
                    end = defaultDate.AddDays((int)p.diaSemana - 1).Add(p.fim).ToString("s")
                    ,
                    url = p.evento.url
                    ,
                    className = p.evento.className
                    ,
                    editable = p.evento.editable
                    ,
                    startEditable = p.evento.startEditable
                    ,
                    durationEditable = p.evento.durationEditable
                    ,
                    overlap = p.evento.overlap
                    ,
                    color = p.evento.color
                    ,
                    backgroundColor = p.evento.backgroundColor
                    ,
                    borderColor = p.evento.borderColor
                    ,
                    textColor = p.evento.textColor

                }).ToList();

            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Convert a lista de eventos do calendario para o Json no formato para exibição no calendario.
        /// </summary>
        /// <param name="lstEvents"></param>
        /// <returns>Json(string)</returns>
        public static string ConvertEventsToJson(List<EventDate> lstEvents)
        {
            if (lstEvents == null)
                return JsonConvert.SerializeObject(new EventObject());

            List<EventObject> data = lstEvents.Select(p =>
                new EventObject
                {
                    id = p.evento.id
                    ,
                    title = p.evento.title
                    ,
                    allDay = p.evento.allDay
                    ,
                    start = p.inicio.ToString(@"hh\:mm")
                    ,
                    end = p.fim.ToString(@"hh\:mm")
                    ,
                    url = p.evento.url
                    ,
                    className = p.evento.className
                    ,
                    editable = p.evento.editable
                    ,
                    startEditable = p.evento.startEditable
                    ,
                    durationEditable = p.evento.durationEditable
                    ,
                    overlap = p.evento.overlap
                    ,
                    color = p.evento.color
                    ,
                    backgroundColor = p.evento.backgroundColor
                    ,
                    borderColor = p.evento.borderColor
                    ,
                    textColor = p.evento.textColor

                }).ToList();

            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Retorna as configurações iniciais de carregamento do calendario
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Json(string)</returns>
        public static string GetJSonConfigEvents(OptionsCalendar options = new OptionsCalendar())
        {

            OptionsObject config = new OptionsObject
            {
                header = string.IsNullOrEmpty(options.header) ? "false" : options.header
                ,
                defaultView = options.defaultView.ToString()
                ,
                allDaySlot = options.allDaySlot
                ,
                slotEventOverlap = options.slotEventOverlap
                ,
                selectable = options.selectable
                ,
                columnFormat = options.columnFormat
                ,
                timeFormat = options.timeFormat
                ,
                slotDuration = options.slotDuration.ToString("hh':'mm':'ss")
                ,
                minTime = TruncateTimeMinutes(options.minTime, options.slotDuration.Minutes).ToString("hh':'mm':'ss")
                ,
                maxTime = options.maxTime.ToString("hh':'mm':'ss")
                ,
                scrollTime = options.scrollTime.ToString("hh':'mm':'ss")
                ,
                aspectRatio = options.aspectRatio
                ,
                lang = options.lang
                ,
                defaultDate = options.defaultDate.ToString("s")
                ,
                calendarClass = options.calendarClass
            };

            return JsonConvert.SerializeObject(config);
        }

        #endregion Métodos
    }
}

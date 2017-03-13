var calendar;
var config;
var events;

function loadCalendar(Config, Events, eClick, slotMin, slotMax) {
    var objCalendar = $('.' + Config.calendarClass);
    objCalendar.fullCalendar('destroy');
    objCalendar.fullCalendar({
        header: Config.header,
        defaultView: Config.defaultView,
        allDaySlot: Config.allDaySlot,
        slotEventOverlap: Config.slotEventOverlap,
        selectable: Config.selectable,

        columnFormat: Config.columnFormat,
        timeFormat: Config.timeFormat,

        slotDuration: Config.slotDuration,
        minTime: Config.minTime,
        maxTime: Config.maxTime,
        scrollTime: Config.scrollTime,

        aspectRatio: Config.aspectRatio,

        lang: Config.lang,

        defaultDate: Config.defaultDate,

        events: Events,

        eventClick: function (calEvent, jsEvent, view) {
            eClick(calEvent);
        },

        eventRender: function (event, element) {
            element.find('div.fc-event-title').html(element.find('div.fc-event-title').text());
        }
    });

    var slotsCalendar = $('.calendar').find("[class^='fc-slot']");
    for (var i = 0; i < slotsCalendar.length; i++) {
        if (slotMin == -1 || slotMax == -1 || (i + 1) < slotMin || (i + 1) > slotMax) {
            var slot = $(slotsCalendar[i]);
            slot.addClass('horarioFora');
        }
    }
};

function reLoadCalendar(Config, Events) {
    events = Events;
    var objCalendar = $('.' + Config.calendarClass);
    objCalendar.fullCalendar('removeEvents');
    objCalendar.fullCalendar('addEventSource', Events);
    objCalendar.fullCalendar('refetchEvents');
    objCalendar.fullCalendar('rerenderEvents')
}

function refreshEvents(calendarClass) {
    var objCalendar = $('.' + calendarClass);
    objCalendar.fullCalendar('refetchEvents');
    objCalendar.fullCalendar('rerenderEvents')
}
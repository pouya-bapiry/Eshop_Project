$(document).ready(function() {

    $('#calendar').fullCalendar({
      header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
      },
      defaultDate: '2018-03-12',
      navLinks: true, // can click day/week names to navigate views
      selectable: true,
      selectHelper: true,
      select: function(start, end) {
        var title = prompt('عنوان رویداد:');
        var eventData;
        if (title) {
          eventData = {
            title: title,
            start: start,
            end: end
          };
          $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
        }
        $('#calendar').fullCalendar('unselect');
      },
      editable: true,
      eventLimit: true, // allow "more" link when too many events
      events: [
        {
          title: 'رویداد یک روز کامل',
          start: '2018-03-01'
        },
        {
          title: 'رویداد طولانی',
          start: '2018-03-07',
          end: '2018-03-10'
        },
        {
          id: 999,
          title: 'تکرار رویداد',
          start: '2018-03-09T16:00:00'
        },
        {
          id: 999,
          title: 'ر',
          start: '2018-03-16T16:00:00'
        },
        {
          title: 'کنفرانس',
          start: '2018-03-11',
          end: '2018-03-13'
        },
        {
          title: 'ملاقات',
          start: '2018-03-12T10:30:00',
          end: '2018-03-12T12:30:00'
        },
        {
          title: 'ناهار',
          start: '2018-03-12T12:00:00'
        },
        {
          title: 'ملاقات',
          start: '2018-03-12T14:30:00'
        },
        {
          title: 'ساعت شاد',
          start: '2018-03-12T17:30:00'
        },
        {
          title: 'شام',
          start: '2018-03-12T20:00:00'
        },
        {
          title: 'جشن تولد',
          start: '2018-03-13T07:00:00'
        },
        {
          title: 'برای گوگل کلیک کنید',
          url: 'http://google.com/',
          start: '2018-03-28'
        }
      ]
    });

  });
/* Modified Paul Irish's approach (http://www.paulirish.com/2009/markup-based-unobtrusive-comprehensive-dom-ready-execution/) */
UTIL = {
    fire: function (func, funcname, args) {
        var namespace = App;  // indicate your obj literal namespace here

        funcname = (funcname === undefined) ? 'init' : funcname;
        if (func !== '' && namespace[func] && typeof namespace[func][funcname] == 'function') {
            namespace[func][funcname](args);
        }
    },

    loadEvents: function () {

        var pageId = $('#page').val();
        if (pageId) {
            UTIL.fire(pageId);
        }
    }
};

// kick it all off here 
$(document).ready(UTIL.loadEvents);
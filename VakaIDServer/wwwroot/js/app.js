! function(i) {
    "use strict";
    var t = function() {};
    t.prototype.initNavbar = function() {
        i(".navbar-toggle").on("click", function(t) {
            i(this).toggleClass("open"), i("#navigation").slideToggle(400)
        }), i(".navigation-menu>li").slice(-2).addClass("last-elements"), i('.navigation-menu li.has-submenu a[href="#"]').on("click", function(t) {
            i(window).width() < 992 && (t.preventDefault(), i(this).parent("li").toggleClass("open").find(".submenu:first").toggleClass("open"))
        })
    }, t.prototype.initScrollbar = function() {
        i(".slimscroll").slimScroll({
            height: "auto",
            position: "right",
            size: "7px",
            color: "#9ea5ab",
            wheelStep: 5,
            touchScrollStep: 50
        })
    }, t.prototype.initMenuItem = function() {
        i(".navigation-menu a").each(function() {
            var t = window.location.href.split(/[?#]/)[0];
            this.href == t && (i(this).parent().addClass("active"), i(this).parent().parent().parent().addClass("active"), i(this).parent().parent().parent().parent().parent().addClass("active"))
        })
    }, t.prototype.initComponents = function() {
        i('[data-toggle="tooltip"]').tooltip(), i('[data-toggle="popover"]').popover()
    }, t.prototype.init = function() {
        this.initNavbar(), this.initScrollbar(), this.initMenuItem(), this.initComponents(), Waves.init()
    }, i.MainApp = new t, i.MainApp.Constructor = t
}(window.jQuery),
function(t) {
    "use strict";
    window.jQuery.MainApp.init()
}();
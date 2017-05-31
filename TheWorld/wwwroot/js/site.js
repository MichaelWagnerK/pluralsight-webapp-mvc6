(function () {
    var $sidebarAndWrapper = $("#sidebar", "#wrapper");
    var $icon = $("#sidebarToggle i.fa");

    var $sidebar = $("#sidebar");
    var $wrapper = $("#wrapper");

    $("#sidebarToggle").on("click",
        function() {
            //$sidebarAndWrapper.toggleClass("hide-sidebar");

            $sidebar.toggleClass("hide-sidebar");
            $wrapper.toggleClass("hide-sidebar");

            if ($sidebar.hasClass("hide-sidebar")) {
                $icon.removeClass("fa-angle-left");
                $icon.addClass("fa-angle-right");
            } else {
                $icon.addClass("fa-angle-left");
                $icon.removeClass("fa-angle-right");
            }
        });

})();
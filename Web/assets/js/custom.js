
// TABS JS
// ¸ü¶àÏÂÔØ£ºhttp://www.bootstrapmb.com
function makeTabActive() {

    var url = window.location.href;

    var indexof = url.indexOf("#");

    if (indexof > 0) {

        var activeTab = url.substring(indexof + 1);

        if (typeof activeTab != 'undefined' && activeTab != '' && activeTab != '#') {

            // to dispaly give tab content

            jQuery(".tab-pane").removeClass("active in show");

            jQuery("#" + activeTab).addClass("active in show")

            // to make active given tab

            jQuery(".nav-tabs li a").removeClass("active");

            jQuery(".nav-tabs li a[href='#" + activeTab + "']").addClass("active");
        }
    }
}
// Pop Up On Image

$("#fixed-form-container .body").hide();
$("#fixed-form-container .image").on("click", function(e){
    $(this).next("#fixed-form-container div").slideToggle(400);
    $(this).toggleClass("expanded");
});
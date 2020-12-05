var baseUrl = "/shop";
baseUrl += (window.location.href.includes("sales")) ? "/sales" : "";
var redirect = ($(".PagedList-skipToPrevious").length > 0) ? $(".pagination li:first-of-type a").attr("href").replace(/\/([0-9]+)/, "") : "";

$(".pagination li:first-of-type a").attr("href", redirect);

if ($(".PagedList-skipToPrevious").length > 0) {
    $(".pagination li:nth-of-type(2) a").attr("href", redirect);
}

$("#search").on("input", function () {
    if ($(this).val() == "" && window.location.href.includes("?")) {
        window.location.href = baseUrl;
    }
});

$("#search").hover(function () {
    if ($(this).val() != "") {
        $(this).attr("title", "Clear search");
    }
    else {
        $(this).attr("title", "Search products");
    }
})

$("[name='reset']").click(function () {
    if (($("#search").val() != "") && (window.location.href.includes("collection") || window.location.href.includes("order"))) {
        let url = new URL(window.location.href);
        let params = new URLSearchParams(url.search.slice(1));
        params.delete("order");
        window.location.href = baseUrl + "?" + params;
    }
    else if ($("#search").val() == "") {
        window.location.href = baseUrl;
    }
});

$("[name='apply']").click(function () {
    if ($("#category option:selected").val() != "" || $("#order option:selected").val() != "") {
        var url = baseUrl;
        url += ($("#category option:selected").val() != "") ? "/collection/" + $("#category").val().toLowerCase() : "";

        var searchParams = new URLSearchParams(window.location.search);

        if ($("[name='order'] option:selected").val() != "" || $("[name='search']").val() != "") {
            url += "?";
        }
        if ($("[name='order'] option:selected").val() != "") {
            searchParams.set("order", $("[name='order'] option:selected").val());
        }

        if ($("#search").val() != "") {
            searchParams.set("search", $("[name='search']").val());
        }
        window.location.href = url + searchParams.toString();
    }
});

$("[name='size']").on("input", function () {
    var searchParams = new URLSearchParams(window.location.search);

    if ($("[name='size'] option:selected").val() != "6") {
        searchParams.set("size", $("[name='size'] option:selected").val());
    }
    else {
        searchParams.delete("size");
    }

    if (searchParams != "") {
        window.location.href = window.location.href.split("?")[0].replace(/\/([0-9]+)/, "") + "?" + searchParams.toString();
    }
    else {
        window.location.href = window.location.href.split("?")[0];
    }
});

const CreateSale = flatpickr("[name='create'] #Sale, .newSale", {
    altInput: true,
    altFormat: "F j, Y",
    dateFormat: "m-d-Y",
    mode: "range",
    minDate: "today"
});

const EditSale = flatpickr(".existingSale", {
    altInput: true,
    altFormat: "F j, Y",
    dateFormat: "m-d-Y",
    mode: "range",
    minDate: $(".existingSale").val()
});

if (!$("#OnSale").prop("checked")) {
    $(".novisible").hide();
}

$("#OnSale").change(function () {
    $(".novisible").fadeToggle(700);
    $("#Sale + span").fadeToggle(700);
    $("#Sale + input").next().html("");
    $("#percentInput + span").html("");

    if (!$("#OnSale").prop("checked")) {
        if ($("[name='create']").length > 0 || $(".newSale").length > 0) {
            CreateSale.clear();
        }
        else {
            EditSale.clear();
        }
        $("#Discount").val("");
        $(".numberWrapper input").parent().find("span").css("color", "#8E8E8E");
    }
});

$(".numberWrapper input").each(function () {
    if ($(this).val() != "") {
        $(this).parent().find("span").css("color", "#555");
    }
});

$(".numberWrapper input").each(function () {
    $(this).on("input", function () {
        if ($(this).val() != "") {
            $(this).parent().find("span").css("color", "#555");
        }
        else {
            $(this).parent().find("span").css("color", "#8E8E8E");
        }

        if ($(this).attr("id") === "Discount") {
            if ($(this).val() != "" && $(this).val() < 10) {
                $(this).parent().find("span").css("right", "247px");
            }
            else {
                $(this).parent().find("span").css("right", "240px");
            }
        }
    });
})

if ($("#Image").length > 0) {
    $("#Image").css({ "color": "transparent", "width": "95px" });
    $("#Image + span").css("display", "inline");
}

if ($("#HiddenImage").length > 0 && $("#HiddenImage").val() != "") {
    $("#Image").attr("title", $("#HiddenImage").val());
    $("#Image + span").html($("#HiddenImage").val());
}

$("#preview").on("error", function () {
    $(this).hide();
});

$("#Image").change(function () {
    var file = $(this)[0].files[0];

    $("#Image + span").html(file.name);

    if (file.type.includes("png") || file.type.includes("jpg") || file.type.includes("jpeg")) {
        $("#preview").show();

        if ($("#preview").length == 0) {
            var img = document.createElement("img");
            img.setAttribute("src", URL.createObjectURL(file));
            img.setAttribute("id", "preview");
            $(".image").append(img);
        }
        else {
            $("#preview").attr("src", URL.createObjectURL(file));
        }
    }
    else {
        $("#preview").hide();
    }
});

$("#zoom").mousemove(function (e) {
    var zoomer = e.currentTarget;
    e.offsetX ? offsetX = e.offsetX : offsetX = e.touches[0].pageX;
    e.offsetY ? offsetY = e.offsetY : offsetX = e.touches[0].pageX;
    x = offsetX / zoomer.offsetWidth * 100;
    y = offsetY / zoomer.offsetHeight * 100;
    zoomer.style.backgroundPosition = x + '% ' + y + '%';
});

while ($("figure").length % 3 != 0) {
    var figure = document.createElement("figure");
    figure.setAttribute("aria-hidden", "true")
    $("#gallery").append(figure);
}
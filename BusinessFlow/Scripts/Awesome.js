var $ae = new function () {
    this.applyAwesomeStyles = function () {
        $(".ae-lookup-openbtn").empty().prepend('<span class="ui-icon ui-icon-newwin"></span>');
        $(".ae-lookup-clearbtn").empty().prepend('<span class="ui-icon ui-icon-gear"></span>');
        $ae.mybutton(".ae-lookup-openbtn");
        $ae.mybutton(".ae-lookup-clearbtn");
    }

    this.mybutton = function (sel) {
        $(sel).unbind('mousedown mouseup mouseleave')
        .hover(function () { $(this).addClass("ui-state-hover"); },
	            function () { $(this).removeClass("ui-state-hover"); })
        .bind({ 'mousedown mouseup': function () { $(this).toggleClass('ui-state-active'); } })
        .addClass("ui-state-default").addClass("ui-corner-all")
        .bind('mouseleave', function () { $(this).removeClass('ui-state-active') });
    }

    this.interactive = function (o) {
        $(o + ' input:text').keyup(function (e) {
            if ($ae.isKeyChange(e.which)) {
                $(o).submit();
            }
        });
        $(o + ' input[type="hidden"], ' + o + ' .ae-array').change(function () {
            $(o).submit();
        });
    }

    this.isKeyChange = function (w) {
        if (w < 9 || w > 45 && w < 91 || w > 93 && w < 112 || w > 185 || w == 32) return true;
        return false;
    }

    this.fullscreen = function (o) {
        $(window).bind("resize", function (e) { $(o).dialog("option", { height: $(window).height() - 50, width: $(window).width() - 50 }).trigger('dialogresize'); });
    }

    loadRadioList = function (o, url, parent) {
        var data = new Array();
        addval(data, parent, "parent");
        addval(data, o, "key");
        $.post(url, data,
        function (d) {
            $("#" + o + "list").empty();
            if (typeof (d) == 'object') {
                var found = false;
                $.each(d, function (i, j) {
                    var sel = "";
                    if (j.Selected == true) { sel = "checked = 'checked'"; $('#' + o).val(j.Value).change(); found = true; };
                    $("#" + o + "list").append("<li><input type='radio' " + sel + " value='" + j.Value + "' name='ae" + o + "' id='ae" + o + i + "'/><label for='ae" + o + i + "' >" + j.Text + "</label></li>");
                    if (!found) $('#' + o).val('').change();
                });
            }
            else { $('#' + o).val('').change(); }
        });
    }

    this.radioList = function (o, url, parent) {
        loadRadioList(o, url, parent);
        $('#' + o + 'list input').live('change',
        function () {
            $('#' + o).val($('#' + o + 'list input:checked').val()).change();
        });
        if (parent) $('#' + parent).change(function () { loadRadioList(o, url, parent); });
    }

    this.checkBoxList = function (o, url, parent, prop) {
        loadCheckBoxList(o, url, parent, prop);
        $('#' + o + 'list input').live('change', function () { syncCheckBoxList(o, prop); });
        if (parent) $('#' + parent).change(function () { loadCheckBoxList(o, url, parent, prop); });
    }

    syncCheckBoxList = function (o, prop) {
        $('#' + o).empty();
        $('#' + o + 'list input:checked').each(function () {
            $('#' + o).prepend('<input type="hidden" name="' + prop + '" value="' + $(this).val() + '" />');
        });
        $('#' + o).change();
    }

    loadCheckBoxList = function (o, url, parent, prop) {
        var data = new Array();
        addval(data, o, "keys");
        addval(data, parent, "parent");
        $.post(url, data, function (d) {
            $('#' + o + 'list').empty();
            if (typeof d == 'object') {
                $.each(d, function (i, j) {
                    var sel = j.Selected ? 'checked="checked"' : "";
                    $('#' + o + 'list').append('<li><input type="checkbox" name="' + prop + '" value="' + j.Value + '" ' + sel + ' id="ae' + o + i + '" /><label for="ae' + o + i + '">' + j.Text + '</label></li>');
                });
            }
            $('#' + o + 'list').change();
            syncCheckBoxList(o, prop);
        });
    }

    addval = function (arr, id, key) {
        if (!id) return;
        var o = $('#' + id);
        if (o.hasClass('ae-array')) {
            $('#' + id + ' input').each(function () { arr.push({ name: key, value: $(this).val() }); });
        }
        else {
            arr.push({ name: key, value: o.val() });
        }
    }


    this.ajaxDropdown = function (o, p, url, keys, values, pkeys, pvals) {
        this.loadAjaxDropdown(o, p, url, false, keys, values, pkeys, pvals);

        $("#" + o + "dropdown").keyup(function () { $(this).change(); }).change(function () { $('#' + o).val($('#' + o + 'dropdown').val()).trigger('change'); });

        if (p) $('#' + p).change(function () { $ae.loadAjaxDropdown(o, p, url, true, keys, values, pkeys, pvals); });
        $.each(keys, function (i, k) {
            $('#' + k).change(function () { $ae.loadAjaxDropdown(o, p, url, true, keys, values, pkeys, pvals); });
        });
        //if keys foreach key change same 
    }

    this.loadAjaxDropdown = function (o, p, url, c, keys, values, pkeys, pvals) {
        if (c) $('#' + o).val(null);
        var data = new Array();
        addval(data, o, "key");
        addval(data, p, "parent");

        $.each(pkeys, function (i, k) {
            data.push({ name: k, value: pvals[i] });
        });

        $.each(keys, function (i, k) {
            addval(data, k, values[i]);
        });

        $.post(url, data,
        function (d) {
            $("#" + o + "dropdown").empty();
            if (typeof (d) == 'object')
                $.each(d, function (i, j) {
                    var sel = "";
                    if (j.Selected == true) sel = "selected = 'selected'";
                    $("#" + o + "dropdown").append("<option " + sel + " value=\"" + j.Value + "\">" + j.Text + "</option>");
                });
            if (c) $("#" + o + "dropdown").trigger('change');
        });
    }

    this.autocomplete = function (o, k, p, u, mr, delay, minLen, keys, values) {
        $('#' + o).autocomplete({
            delay: delay,
            minLength: minLen,
            source: function (request, response) {
                var data = new Array();
                data.push({ name: 'searchText', value: request.term });
                data.push({ name: 'maxResults', value: mr });
                addval(data, p, "parent");

                $.each(keys, function (i, k) {
                    addval(data, k, values[i]);
                });

                $.ajax({
                    url: u, type: "POST", dataType: "json",
                    data: data,
                    success: function (d) { response($.map(d, function (o) { return { label: o.Text, value: o.Text, id: o.Id} })); }
                });
            }
        });

        $('#' + o).bind("autocompleteselect", function (e, ui) {
            $('#' + k).val(ui.item ? ui.item.id : null).trigger('change');
            $('#' + o).trigger('change');
        });

        $('#' + o).keyup(function (e) { if (e.which != '13') $("#" + k).val(null).trigger('change'); });
    }

    this.popup = function (o, w, h, title, modal, pos, res, btns, fulls) {
        var dragg = true;
        if (fulls) { res = false; modal = true; dragg = false; }
        $("#" + o).dialog({
            show: "fade",
            width: fulls ? $(window).width() - 50 : w,
            height: fulls ? $(window).height() - 50 : h,
            title: title,
            modal: modal,
            position: pos,
            resizable: res,
            draggable: dragg,
            buttons: btns,
            autoOpen: false,
            close: function (e, ui) { $("#" + o).find('*').remove(); }
        });
        if (modal || fulls) $("#" + o).dialog("option", { dialogClass: 'ae-fixed' });
        if (fulls) this.fullscreen("#" + o);
    }

    this.loadLookupDisplay = function (o, url) {
        $('#ld' + o).val('');
        var id = $('#' + o).val();
        if (id) $.get(url, { id: id }, function (d) { $("#ld" + o).val(d); });
    }

    this.loadMultiLookupDisplay = function (o, url) {
        var ids = $("#" + o + " input").map(function () { return $(this).attr("value"); }).get();
        $("#ld" + o).html('');
        if (ids.length != 0) $.post(url, $.param({ selected: ids }, true),
        function (d) {
            $.each(d, function () { $("#ld" + o).append('<li>' + this.Text + '</li>') });
        });
    }

    this.lookupChoose = function (o, url, sel) {
        $('#' + o).val('');
        $('#' + o).val($('#' + o + 'ls .' + sel).attr("data-value")).change();
        this.loadLookupDisplay(o, url);
        $("#lp" + o).dialog('close');
    }

    this.multiLookupChoose = function (o, loadUrl, prop) {
        $("#" + o).empty();
        $.each($("#" + o + "se li").map(function () { return $(this).attr("data-value"); }).get(), function () {
            $("#" + o).append($("<input type='hidden' name='" + prop + "' \>").attr("value", this));
        });
        $("#" + o).change();
        this.loadMultiLookupDisplay(o, loadUrl);
        $("#lp" + o).dialog('close');
    }

    this.lookupClear = function (o) {
        $("#lc" + o).click(function () {
            $("#" + o).val("").change();
            $("#ld" + o).val("");
        });
    }

    this.multiLookupClear = function (o) {
        $("#lc" + o).click(function () {
            $("#" + o + ",#ld" + o).empty();
            $("#" + o).change();
        });
    }

    this.confirm = function (o, f, h, w, yes, no) {
        $("#dialog-confirm-" + o).dialog({
            show: "fade",
            hide: "fade",
            resizable: false,
            height: h,
            width: w,
            modal: true,
            autoOpen: false,
            dialogClass: 'ae-fixed'
        })
    .dialog("option", "buttons", [
    {
        text: yes,
        click: function () { $(this).dialog("close"); f.submit(); }
    },
    {
        text: no,
        click: function () { $(this).dialog("close"); }
    },
    ]);

        $("." + o).live('click', function () {
            f = $(this).closest('form');
            $("#dialog-confirm-" + o).dialog('open');
            return false;
        });
    }

    this.lookupPopupOpenClick = function (o, lck, url, paging, multi, datak, datav, park, parv) {
        $("#lpo" + o).click(function (e) {
            e.preventDefault();
            if (lck != null) return;
            lck = true;
            var data = [{ name: 'prop', value: o}];
            data.push({ name: 'paging', value: paging });
            data.push({ name: 'multi', value: multi });

            addarr(data, datak, "datak");
            addarr(data, datav, "datav");
            addarr(data, park, "park");
            addarr(data, parv, "parv");

            $.get(url, $.param(data), function (d) {
                $("#lp" + o).html(d).dialog('open'); lck = null;
            });
        });
    }

    addarr = function (arr, v, k) {
        for (var i in v) arr.push({ name: k, value: v[i] });
    }

    this.takevals = function (a, b, w) {
        $.each(a, function (i, v) {
            var e = $('#' + v);
            var t = e.hasClass('ae-array') ? e.find('input') : e;
            t.clone().removeAttr('id').attr('name', b[i]).appendTo('#' + w);
        });
    }
};

$(function () {
    $(".ae-pagination-current").addClass('ui-state-highlight');
    $ae.mybutton(".ae-pagination a");
    $ae.applyAwesomeStyles();
    $("body").ajaxComplete($ae.applyAwesomeStyles);
});

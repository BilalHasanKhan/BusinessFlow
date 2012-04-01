<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%var o = ViewData["prop"].ToString();
  var pageable = (bool)ViewData["paging"];
  var datak = ViewData["datak"] as string[];
  var datav = ViewData["datav"] as string[];
  var moreText = ViewData["moreText"].ToString();
  const string ai = "ui-icon-circle-plus";
  const string ri = "ui-icon-circle-arrow-n";
%>
<form id="lsf<%=o %>" action="<%=Url.Action("Search") %>" method="post">
<%=JsTools.MakePars(ViewData) %>
<% Html.RenderAction("SearchForm"); %>

<%if (datak != null)
  {%>
<div id='lsfv<%=o %>' style="display: none;">
</div>
<%}%>
</form>
<div id='<%=o %>lsh'>
    <% Html.RenderAction("header"); %>
</div>
<ul id="<%=o%>ls" class="ae-lookup-list ae-lookup-searchlist">
</ul>
<div id='<%=o %>seh'>
    <% Html.RenderAction("header"); %>
</div>
<ul id="<%=o %>se" class="ae-lookup-list ae-lookup-selectedlist">
</ul>
<script type="text/javascript">
<%if(null != datak) {%>
$ae.takevals([<%=AwesomeTools.MakeIdJsArray(datak) %>],[<%=AwesomeTools.MakeJsArray(datav) %>], 'lsfv<%=o %>');
<%}%>
    $('#lsf<%=o %>').submit(function (e) {
        e.preventDefault();       
        var lfm = $('#lsf<%=o %>').serializeArray();
    <% if(pageable){%>        
        var clfm = lfm.slice(0);
        clfm.push({ name: "page", value: "1" });
        
        var a = $('#<%=o %>se > li').map(function () { return $(this).attr("data-value"); }).get();       
        for(var k in a) clfm.push({name: "selected", value: a[k]});        

        $.post('<%=Url.Action("search") %>', clfm, 
        function(d){
        var x = $(d.rows);
        x.find('.ae-lookup-mbtn').prepend("<a title='+' class='ui-icon <%=ai%>'>+</a>");
        $('#<%=o%>ls').html(x);
        $('#<%=o%>ls > li').draggable({ cancel: "a", revert: "invalid", helper: "clone", cursor: "move" });        
        if (d.more) {        
        var page = 1;
        
        $('<a class="ae-lookup-morebtn"><%=moreText %></a>').click(function() {
        page++;
        var clfm = lfm.slice(0);
        clfm.push({ name: "page", value: page });
        
        var a = $('#<%=o %>se li').map(function () { return $(this).attr("data-value"); }).get();       
        for(var k in a) clfm.push({name: "selected", value: a[k]});
        
            $.post('<%=Url.Action("search") %>', clfm, function (d) {
                var x = $(d.rows);
                x.find('.ae-lookup-mbtn').prepend("<a title='+' class='ui-icon <%=ai%>'>+</a>");
                $("#<%=o%>ls .ae-lookup-morebtn").before(x.css('opacity', 0).animate({ opacity: 1 }, 300, 'easeInCubic')); 
                $('#<%=o%>ls > li').draggable({ cancel: "a", revert: "invalid", helper: "clone", cursor: "move" });
                if (!d.more) $('#<%=o%>ls .ae-lookup-morebtn').fadeOut('slow');
                lay<%=o %>();
            });
        }).appendTo('#<%=o%>ls');
        }
        });

    <%} else
       {%>
        var a = $('#<%=o %>se li').map(function () { return $(this).attr("data-value"); }).get();       
        for(var k in a) lfm.push({name: "selected", value: a[k]});
           
           $.post('<%=Url.Action("search") %>', lfm, function(d){
           $('#<%=o%>ls').html(d);

            $('#<%=o%>ls > li').draggable({ cancel: "a", revert: "invalid", helper: "clone", cursor: "move" });
            $('#<%=o%>ls > li .ae-lookup-mbtn').prepend("<a title='+' class='ui-icon <%=ai%>'>+</a>");

           lay<%=o %>(); });           
       <%
       }%>
    }); 


    $('#<%=o %>se').droppable({
        accept: "#<%=o %>ls li",
        activeClass: "ui-state-highlight",
        drop: function (e, ui) { add<%=o %>(ui.draggable);}
    });

    $('#<%=o %>ls').droppable({
        accept: "#<%=o %>se li",
        activeClass: "ui-state-highlight",
        drop: function (e, ui) { rem<%=o %>(ui.draggable); }
    });
    
    function add<%=o %>(o) {
        o.prependTo($('#<%=o %>se')).find('a.<%=ai %>').removeClass('<%=ai %>').addClass('<%=ri %>');
        $(document).trigger('awesome');
    }    
    
    function rem<%=o %>(o) {
        o.prependTo($('#<%=o %>ls')).find('a.<%=ri %>').removeClass('<%=ri %>').addClass('<%=ai %>');
        $(document).trigger('awesome');
    }

    $.post('<%=Url.Action("selected") %>', $.param({ selected: $("#<%=o%> input").map(function () { return $(this).attr("value"); }).get() }, true), 
    function (d) {
        $('#<%=o %>se').html(d);        
        $('#<%=o %>se > li .ae-lookup-mbtn').prepend("<a title='^' class='ui-icon <%=ri %>'>^</a>");
        $('#lsf<%=o %>').submit();
        $('#lsf<%=o %> input:first').focus();
        $('#<%=o %>se > li').draggable({ cancel: "a", revert: "invalid", helper: "clone", cursor: "move" });
        lay<%=o %>();
    });

    $('#<%=o %>ls a').live('click', function () { add<%=o %>($(this).closest('.ae-lookup-item')); });
    $('#<%=o %>se a').live('click', function () { rem<%=o %>($(this).closest('.ae-lookup-item')); });
        
    function lay<%=o %>() {
        var av = $("#lp<%=o %>").height() - $('#lsf<%=o %>').height() - $('#<%=o %>lsh').height() - $('#<%=o %>seh').height() - 3;
        $('#<%=o %>ls').css('height', (av * 0.5)+'px');
        $('#<%=o %>se').css('height', (av * 0.5)+'px');
    }
    $("#lp<%=o %>").bind( "dialogresize", lay<%=o %>);

    $('#lsf<%=o %> input').keypress(function (e) { if (e.which == 13) { e.preventDefault();$('#lsf<%=o %>').submit(); }});
    <%if(Settings.Lookup.Interactive) {%>
       $ae.interactive('#lsf<%=o %>');
    <%}%>
</script>

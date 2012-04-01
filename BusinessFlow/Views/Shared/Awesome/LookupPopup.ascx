<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = ViewData["prop"].ToString();
    var pageable = (bool)ViewData["paging"];
    var datak = ViewData["datak"] as string[];
    var datav = ViewData["datav"] as string[];
    var moreText = ViewData["moreText"].ToString();
%>

<form id="lsf<%=o %>" action="<%=Url.Action("search") %>" method="post">
<%=JsTools.MakePars(ViewData) %>
<% Html.RenderAction("SearchForm"); %>
<%if(datak != null) {%>
<div id='lsfv<%=o %>' style="display:none;">
</div>
<%}%>
</form>
<div id='<%=o %>lsh'>
    <% Html.RenderAction("header"); %>
</div>
<ul id="<%=o%>ls" class="ae-lookup-list ae-lookup-searchlist">
</ul>
<script type="text/javascript">
<%if(datak != null) {%>
$ae.takevals([<%=AwesomeTools.MakeIdJsArray(datak) %>],[<%=AwesomeTools.MakeJsArray(datav) %>], 'lsfv<%=o %>');
<%}%>
function lay<%=o %>() {
    var av = $("#lp<%=o %>").height() - $('#lsf<%=o %>').height() - $('#<%=o %>lsh').height();
    $('#<%=o %>ls').css('height', av+'px');     
} 
$("#lp<%=o %>").bind( "dialogresize", lay<%=o %>);
$('#lsf<%=o %> input').keypress(function(e){ if(e.which == 13){ e.preventDefault(); $('#lsf<%=o %>').submit(); } });
<%if(Settings.Lookup.Interactive) {%>
$ae.interactive('#lsf<%=o %>');
    <%}%>

$('#lsf<%=o %>').submit(function(e){
    e.preventDefault();    
    var lfm = $('#lsf<%=o %>').serializeArray();

    <% if(pageable){%>
        lfm.push({ name: "page", value: "1" });

        $.post('<%=Url.Action("search") %>', lfm, 
        function(d){
        $('#<%=o%>ls').html(d.rows);        
        $("#<%=o%>ls li").click(lgc<%=o %>);
        lay<%=o %>();        
        if (d.more) {
        lfm.pop();
        var page =1;

        $('<a class="ae-lookup-morebtn"><%=moreText %></a>').click(function() {
        page++;
        lfm.push({ name: "page", value: page });

            $.post('<%=Url.Action("search") %>', lfm, function (d) {
                
                $("#<%=o%>ls .ae-lookup-morebtn").before($(d.rows).css('opacity', 0).animate({ opacity: 1 }, 300, 'easeInCubic'));                
                $("#<%=o%>ls li").click(lgc<%=o %>);

                if (!d.more) $('#<%=o%>ls .ae-lookup-morebtn').fadeOut('slow');      
                lay<%=o %>();
            });
        lfm.pop();
        }).appendTo('#<%=o%>ls');
        }

        });

    <%} else
       {%>   
           $.post('<%=Url.Action("search") %>', lfm, function(d){
           $('#<%=o%>ls').html(d);
           $("#<%=o%>ls li").click(lgc<%=o %>); 
           lay<%=o %>(); });           
       <%
       }%>
});

$('#lsf<%=o %>').submit();
$('#lsf<%=o %> input:first').focus();
</script>

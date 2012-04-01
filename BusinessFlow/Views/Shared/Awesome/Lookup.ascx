<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.LookupInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = AwesomeTools.MakeId(Model.Prop, Model.Prefix);
    var sel = Settings.Lookup.SelectedRowCssClass;
%>
<script type="text/javascript">
    
    function lgc<%=o %>() {        
        $('li',$(this).parent()).removeClass('<%=sel %>').unbind('click').click(lgc<%=o %>);        
        $(this).addClass('<%=sel %>').click(function(){$ae.lookupChoose('<%=o %>', '<%=Url.Action("Get", Model.Controller, new{Model.Area}) %>', '<%=sel %>');});
    }
    
    $(function () {
        var o = '<%=o %>';
        $("#ld"+o).addClass("ae-lookup-textbox");
        $ae.loadLookupDisplay(o, '<%=Url.Action("Get", Model.Controller, new{Model.Area}) %>');
        $("."+o+"ie8").remove();
        $("#lp"+o).addClass(o+"ie8");

        $ae.popup('lp'+o, <%=Model.Width %>, <%=Model.Height %>, '<%=AwesomeTools.JsEncode(Model.Title) %>', true, 'center', true, {'<%=AwesomeTools.JsEncode(Model.ChooseText) %>': function () {$ae.lookupChoose('<%=o %>', '<%=Url.Action("Get", Model.Controller, new{Model.Area}) %>', '<%=sel %>');},'<%=AwesomeTools.JsEncode(Model.CancelText) %>': function () { $(this).dialog('close'); }}, <%=Model.Fullscreen.ToString().ToLower() %>);

        var lck<%=o%> = null;
        $ae.lookupPopupOpenClick(o, lck<%=o %>, 
        '<%=Url.Action("index", Model.Controller, new{Model.Area}) %>', 
        <%=Model.Paging.ToString().ToLower() %>, 
        <%=Model.Multiselect.ToString().ToLower() %>, 
        [<%=Model.Data  != null ? AwesomeTools.MakeJsArray(Model.Data.Keys) :""%>], 
        [<%=Model.Data != null ? AwesomeTools.MakeIdJsArray(Model.Data.Values) :""%>],
        [<%=Model.Parameters != null ? AwesomeTools.MakeJsArray(Model.Parameters.Keys) :""%>],
        [<%=Model.Parameters != null ? AwesomeTools.MakeJsArrayObj(Model.Parameters.Values) :""%>]);        
        <%if(Model.ClearButton){%>
        $ae.lookupClear(o);        
        <%} %>
    });    
</script>
<div id='lp<%=o%>'>
</div>
<input type="hidden" id="<%=o %>" name="<%=Model.Prop %>" value="<%=Model.Value %>" />
<input type="text" id="ld<%=o%>" disabled="disabled" <%=Model.HtmlAttributes %> />
<a class="ae-lookup-openbtn" id="lpo<%=o%>" ></a>
<%if (Model.ClearButton)
  {%>
<a class="ae-lookup-clearbtn" id="lc<%=o%>" ></a>
<%} %>
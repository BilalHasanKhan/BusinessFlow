<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.AjaxDropdownInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = AwesomeTools.MakeId(Model.Prop, Model.Prefix);
    var p = AwesomeTools.MakeId(Model.ParentId, null);
%>
<input type="hidden" name="<%=Model.Prop %>" id="<%=o %>" value="<%=Model.Value %>"/>
<select id='<%=o %>dropdown' <%=Model.HtmlAttributes %> ></select>
<script type="text/javascript">   
$ae.ajaxDropdown('<%=o %>','<%=p %>','<%=Url.Action("GetItems", Model.Controller, new {Model.Area}) %>',[<%=Model.Data  != null ? AwesomeTools.MakeJsArray(Model.Data.Keys) :""%>], [<%=Model.Data != null ? AwesomeTools.MakeIdJsArray(Model.Data.Values) :""%>],[<%=Model.Parameters  != null ? AwesomeTools.MakeJsArray(Model.Parameters.Keys) :""%>], [<%=Model.Parameters  != null ? AwesomeTools.MakeJsArrayObj(Model.Parameters.Values) :""%>]);
</script>

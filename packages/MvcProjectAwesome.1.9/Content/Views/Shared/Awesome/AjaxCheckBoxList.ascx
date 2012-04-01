<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.AjaxCheckBoxListInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = AwesomeTools.MakeId(Model.Prop, Model.Prefix);
%>
<div class='ae-array' style='display:none;' id="<%=o%>">
<% if (Model.Value != null && Model.Value is IEnumerable) foreach (var oo in Model.Value as IEnumerable)
{%>
<input type="hidden" name="<%=Model.Prop%>" value="<%=oo %>" />
<%
}%>
</div>
<ul id="<%=o %>list" <%=Model.HtmlAttributes %>>
</ul>
<script type="text/javascript">
    $(function () { $ae.checkBoxList('<%=o %>', '<%=Url.Action("GetItems", Model.Controller, new{Model.Area}) %>', '<%=Model.ParentId %>', '<%=Model.Prop %>'); });
</script>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.AjaxRadioListInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = AwesomeTools.MakeId(Model.Prop, Model.Prefix);
%>
<input type="hidden" name="<%=Model.Prop %>" id="<%=o %>" value="<%=Model.Value %>" />
<ul id="<%=o %>list" <%=Model.HtmlAttributes %>>
</ul>
<script type="text/javascript">
    $(function () { $ae.radioList('<%=o %>', '<%=Url.Action("GetItems", Model.Controller, new{Model.Area}) %>', '<%=Model.ParentId %>'); });
</script>

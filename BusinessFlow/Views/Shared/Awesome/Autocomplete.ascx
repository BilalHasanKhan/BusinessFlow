<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AutocompleteInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = AwesomeTools.MakeId(Model.Prop, Model.Prefix);
    var k = AwesomeTools.MakeId(Model.PropId, Model.Prefix);
    var p = AwesomeTools.MakeId(Model.ParentId, null);
%>
<input type="text" id="<%=o %>" name="<%=Model.Prop %>" value="<%=Model.Value %>"  <%=Model.HtmlAttributes %> />
<%if (Model.GeneratePropId)
  { %><input type="hidden" id="<%=k %>" name="<%=Model.PropId %>"
    value="<%=Model.PropIdValue %>" /><%} %>
<script type="text/javascript">
    $ae.autocomplete('<%=o %>','<%=k %>','<%=p %>','<%=Url.Action("Search", Model.Controller,new{Model.Area}) %>',<%=Model.MaxResults %>,<%=Model.Delay %>,<%=Model.MinLength %>,
    [<%=Model.Data  != null ? AwesomeTools.MakeJsArray(Model.Data.Keys) :""%>], [<%=Model.Data != null ? AwesomeTools.MakeIdJsArray(Model.Data.Values) :""%>]);
</script>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.ConfirmInfo>" %>
<%@ Import Namespace="Omu.Awesome.Mvc" %>
<%
    var o = Model.CssClass; %>
<script type="text/javascript">
        var currentForm<%=Model.CssClass %>;      
        $(function () {
            $ae.confirm('<%=o %>', currentForm<%=Model.CssClass %>, <%=Model.Height %>, <%=Model.Width %>, '<%=AwesomeTools.JsEncode(Model.YesText) %>', '<%=AwesomeTools.JsEncode(Model.NoText) %>');
        });
</script>
<div id="dialog-confirm-<%=o %>" title="<%=Model.Title %>">
    <%=Model.Message %>
</div>

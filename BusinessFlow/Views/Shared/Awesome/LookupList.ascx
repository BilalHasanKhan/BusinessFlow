<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<object>>" %>

    <% 
        if (Model != null)
        {
            foreach (var o in Model)
            {
    %>
        <% Html.RenderPartial("item", o); %>
    <%
            }
        }%>


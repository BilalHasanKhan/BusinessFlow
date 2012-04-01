<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Omu.Awesome.Mvc.Helpers.PopupFormInfo>" %>
<%
    var o = "pf" + Model.Prefix + (Model.Action + Model.Controller + Model.Area).ToLower();
%>
<script type="text/javascript">
        $(function() {$ae.popup('<%=o %>',<%=Model.Width %>, <%=Model.Height %>, '<%=Model.Title %>', true, <%=Model.Position %>, true, 
        {"<%=Model.OkText %>": function () { $("#<%=o %> form").submit(); }, "<%=Model.CancelText %>": function () { $(this).dialog('close'); } }, 
        <%=Model.FullScreen.ToString().ToLower() %>);});

        var l<%=o %> = null;
        function call<%=o %>(<%=JsTools.MakeParameters(Model.Parameters) %>) { 
            if(l<%=o %> != null) return;
            l<%=o %> = true;
            $.get('<%=Url.Action(Model.Action, Model.Controller, new {area = Model.Area}) %>',
            <%=JsTools.JsonParam(Model.Parameters) %>
            update<%=o %>
            );
        }

        function OnSuccess<%=o %>(result) {
            if (result == 'ok' || typeof(result) == 'object') {
                $("#<%=o %>").dialog('close');
                <%if(Model.RefreshOnSuccess){%>
                    location.reload(true);
                <%} %>
                <% if(Model.SuccessFunction != null)
                   {%>
                   <%=Model.SuccessFunction %>(result);
                <%}%>
            }
            else update<%=o %>(result);            
        }        

        function update<%=o %>(data) {
            l<%=o %> = null;
            $("#<%=o %>").html(data);
            $("#<%=o %> form").ajaxForm({            
            <% if(Model.ClientSideValidation){%>
                beforeSubmit: function () { return $("#<%=o %> form").validate().valid(); },
            <%} %>
                success: OnSuccess<%=o %>
            }); 
            $("#<%=o %>").dialog('open');
            $("#<%=o %> form input:visible:first").focus();
        }
</script>
<div id="<%=o %>">
</div>

<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="EventCategories.aspx.cs" Inherits="Ignite.Organisations.Events.EventCategories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    
    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <asp:LinkButton ID="btnEventCategories" ForeColor="white" runat="server" Text="Categories" CssClass="backTo" OnClick="btnEventCategories_OnClick"/>
            <asp:LinkButton ID="btnEventCoordinator" ForeColor="white" runat="server" Text="Coordinator" CssClass="backTo" OnClick="btnEventCoordinator_OnClick"/>
            <asp:LinkButton ID="btnEventInterests" ForeColor="white" runat="server" Text="Interests" CssClass="backTo" OnClick="btnEventInterests_OnClick"/>
            <asp:LinkButton ID="btnEventSessions" ForeColor="white" runat="server" Text="Sessions" Visible="False" CssClass="backTo" OnClick="btnEventSessions_OnClick"/>
            <asp:LinkButton ID="btnEventSocialMediaPage" runat="server" ForeColor="white" Text="Social Media Page" CssClass="backTo" OnClick="btnEventSocialMediaPage_OnClick"/>
            <asp:LinkButton ID="btnScheduleAlerts" ForeColor="white" runat="server" Text="Schedule Alerts" CssClass="backTo" OnClick="btnScheduleAlerts_OnClick"/>
            <asp:LinkButton ID="btnEventPricing" ForeColor="white" runat="server" Text="Pricing" CssClass="backTo" OnClick="btnEventPricing_OnClick"/>
            <asp:LinkButton ID="btnViewAttendance" ForeColor="white" runat="server" Text="View Attendance" CssClass="backTo" OnClick="btnViewAttendance_OnClick"/>
            <asp:LinkButton ID="btnFeedback" ForeColor="white" runat="server" Text="Respond to Feedback" CssClass="backTo" OnClick="btnFeedback_OnClick"/>
            <asp:LinkButton ID="btnMonitirinterest" ForeColor="white" runat="server" Text="Monitor Interest" CssClass="backTo" OnClick="btnMonitirinterest_OnClick" />
            
        </div>
    </div>

        <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Assign Categories <asp:Label runat="server" ID="eventName" Text=" "/><asp:LinkButton ForeColor="white" runat="server" ID="btnAdd" Text="+Add" CssClass="btn btn-outline btn-link pull-right" OnClick="btnAdd_OnClick"/><asp:DropDownList runat="server" ID="cbCategories" CssClass="pull-right" ForeColor="black"/></h5>
       
             </div>
        <div class="panel-body">
            <asp:GridView ID="dgvCategories" EmptyDataText="Please select at least one category." runat="server" DataKeyNames="Key" OnRowDeleting="dgvCategories_OnRowDeleting" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField  DataField="Key" HeaderText="Id" SortExpression="Key" Visible="False"/>
                    <asp:BoundField DataField="Value" HeaderText="Name" SortExpression="Value"/>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="">
                        <ControlStyle CssClass="fa fa-trash-o" ForeColor="red"/>
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>

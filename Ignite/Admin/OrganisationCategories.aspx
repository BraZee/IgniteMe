<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="OrganisationCategories.aspx.cs" Inherits="Ignite.Admin.OrganisationCategories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    
        <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Assign Categories <asp:Label runat="server" ID="userGroup" Text=" "/><asp:LinkButton ForeColor="white" runat="server" ID="btnSave" Text="Save" CssClass="btn btn-outline btn-link pull-right" OnClick="btnSave_OnClick"/><asp:LinkButton ForeColor="white" runat="server" ID="btnAdd" Text="+Add" CssClass="btn btn-outline btn-link pull-right" OnClick="btnAdd_OnClick"/><asp:DropDownList runat="server" ID="cbCategories" CssClass="pull-right" ForeColor="black"/></h5>
       
             </div>
        <div class="panel-body">
            <asp:GridView ID="dgvCategories" runat="server" DataKeyNames="Key" OnRowDeleting="dgvCategories_OnRowDeleting" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
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

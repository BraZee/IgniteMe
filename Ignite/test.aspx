<%@ Page Title="" Language="C#" MasterPageFile="~/AuthMaster.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Ignite.test1" %>
<asp:Content runat="server" ContentPlaceHolderID="NestedContent">
    <asp:Button runat="server" Text="Button" CssClass="btn btn-outline btn-success"/>
    <asp:Label ID="hash" runat="server"></asp:Label>
</asp:Content>

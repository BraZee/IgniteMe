<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="IgniteWeb.test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="server">
    <asp:TextBox runat="server" ID="txtTest"/>
    <asp:TextBox runat="server" ID="hash"/>
    
    <asp:Button runat="server" ID="btn" OnClick="btn_OnClick"/>
    <asp:Label runat="server" ID="lbl"></asp:Label>
</asp:Content>

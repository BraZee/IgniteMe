<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ignite.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="margin-bottom: 100px; margin-top: 200px">
        <div class="col-md-4"></div>
        <div class="col-md-4">
            <div class="well well-lg center-block">
                <div class="form-group input-group">
                    <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                    <asp:TextBox CssClass="form-control" ID="txtUsername" placeholder="Username..." runat="server" />
                </div>
                <div class="form-group input-group">
                    <span class="input-group-addon"><span class="fa fa-key"></span></span>
                    <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtPassword" placeholder="Password..." runat="server" />
                </div>
                <div class="form-group">
                    <label id="orgLabel" class="control-label">Organisation:</label>
                    <asp:DropDownList runat="server" CssClass="form-control" ID="cbOrganisations" />
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" CssClass="btn btn-block btn-success" Text="Login" runat="server" OnClick="btnLogin_OnClick"/>
                    <asp:LinkButton ID="btnForgotPassword" runat="server" Text="Forgot Password..." CssClass="pull-left" OnClick="btnForgotPassword_OnClick"></asp:LinkButton>
                </div>
               
            </div>
        </div>
        <div class="col-md-4"></div>
        <div class="col-md-12">
            <asp:Label ID="org" runat="server"></asp:Label>
            <hr />
            <footer>
                <p class="text-center">&copy; <%: DateTime.Now.Year %> - Powered by theSOFTtribe</p>
            </footer>
        </div>
    </div>
</asp:Content>

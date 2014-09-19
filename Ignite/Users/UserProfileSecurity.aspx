<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="UserProfileSecurity.aspx.cs" Inherits="Ignite.Users.UserProfileSecurity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Profile Security</h5>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group col-md-12">
                        <div class="col-md-2">
                            <label class="control-label">Security Question:</label>
                        </div>
                        <div class="col-md-9">
                            <asp:DropDownList runat="server" ID="cbSecurityQuestions" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-2">
                            <label class="control-label">Security Answer:</label>
                        </div>
                        <div class="col-md-9">
                            <textarea class="form-control" runat="server" id="txtSecurityAnswer"></textarea>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSecurityAnswer" ErrorMessage="Security answer required!" CssClass="label label-danger" ValidationGroup="userProfileValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-1 pull-left">
                            <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*" />
                        </div>
                    </div>
                    <hr />
                    <div class="form-group col-md-12">
                        <div class="col-md-2">
                            <label class="control-label">Password:</label>
                        </div>
                        <div class="col-md-9">
                            <asp:TextBox TextMode="Password" runat="server" ID="txtPassword" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password required!" CssClass="label label-danger" ValidationGroup="userProfileValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPassword" ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{4,}$" runat="server" ErrorMessage="Minimum 4 characters required." ValidationGroup="userProfileValidation" CssClass="label label-danger"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-1 pull-left">
                            <asp:Label ID="Label2" runat="server" ForeColor="red" Text="*" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-2">
                            <label class="control-label">Confirm Password:</label>
                        </div>
                        <div class="col-md-9">
                            <asp:TextBox TextMode="Password" runat="server" ID="txtConfirmPassword" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm password!" CssClass="label label-danger" ValidationGroup="userProfileValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtConfirmPassword" Display="Dynamic" ControlToCompare="txtPassword" CssClass="label label-danger" ErrorMessage="Passwords do not match!" ValidationGroup="userProfileValidation"></asp:CompareValidator>
                        </div>
                        <div class="col-md-1 pull-left">
                            <asp:Label ID="Label3" runat="server" ForeColor="red" Text="*" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-2"></div>
                        <div class="col-md-10">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-success pull-right" OnClick="btnSave_OnClick" Text="Save" ValidationGroup="userProfileValidation" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

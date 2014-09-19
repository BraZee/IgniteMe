<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="UserProfileDetails.aspx.cs" Inherits="Ignite.Users.UserProfileDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Profile Details</h5>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Last Name:</label>
                        </div>
                        <div class="col-md-7">
                            <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Last Name Required!" runat="server" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-1 pull-left">
                            <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">First Name:</label>
                        </div>
                        <div class="col-md-7">
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <hr />
                    <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Email:</label>
                        </div>
                        <div class="col-md-7">
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Email Required!" runat="server" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Wrong email format!" ControlToValidate="txtEmail" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ValidationExpression="\w+([-+.’]\w+)*@\w+([-.]
\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-1 pull-left">
                            <asp:Label ID="Label2" runat="server" ForeColor="red" Text="*" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Username:</label>
                        </div>
                        <div class="col-md-7">
                            <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Username Required!" runat="server" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                        </div>
                         <div class="col-md-1 pull-left">
                                    <asp:Label ID="Label3" runat="server" ForeColor="red" Text="*"/>
                                </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-4"></div>
                        <div class="col-md-8">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-success pull-right" Text="Save" OnClick="btnSave_OnClick" ValidationGroup="userValidation" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Ignite.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="row" style="margin-bottom: 100px; margin-top: 200px">
        <div class="col-md-12">
            <div class="col-md-3"></div>
            <div class="col-md-6">
                    <fieldset>
                        <legend><strong>Forgot Password</strong></legend>
                <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Email:</label>
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Email Required!" runat="server" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Wrong email format!" ControlToValidate="txtEmail" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ValidationExpression="\w+([-+.’]\w+)*@\w+([-.]
\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Security Question:</label>
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList runat="server" ID="cbSecurityQuestions" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                        <div class="col-md-4">
                            <label class="control-label">Security Answer:</label>
                        </div>
                        <div class="col-md-8">
                            <textarea class="form-control" runat="server" id="txtSecurityAnswer"></textarea>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSecurityAnswer" ErrorMessage="Security answer required!" CssClass="label label-danger" ValidationGroup="userProfileValidation" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                 <div class="form-group col-md-12">
                        <div class="col-md-4"></div>
                        <div class="col-md-8">
                            <asp:Button runat="server" ID="btnSave" Text="Send Password" CssClass="btn btn-success pull-right" OnClick="btnSave_OnClick"/>
                        </div>
                    </div>
                    </fieldset>
                </div>
            <div class="col-md-3"></div>
            
           
            </div>
        <div class="col-md-12">
             <hr />
            <footer>
                <p class="text-center">&copy; <%: DateTime.Now.Year %> - Powered by theSOFTtribe</p>
            </footer>
        </div>
       
       </div>
            
        

</asp:Content>

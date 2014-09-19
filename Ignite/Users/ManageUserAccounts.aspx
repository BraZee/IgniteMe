<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageUserAccounts.aspx.cs" Inherits="Ignite.Users.ManageUserAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white" >User Account Management<asp:LinkButton runat="server" ForeColor="white" ClientIDMode="Static" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New User</asp:LinkButton></h5>
        </div>
        <div class="panel-body">
            <!--Table here-->
            <asp:GridView ID="dgvUsers" runat="server" AllowPaging="true" EmptyDataText="No Users to display" DataKeyNames="UserId" OnRowEditing="dgvUsers_OnRowEditing" OnRowDeleting="dgvUsers_OnRowDeleting" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Ridge" EnableSortingAndPagingCallbacks="True">
                <Columns>
                    <asp:BoundField DataField="UserId" HeaderText="UserId"  SortExpression="UserId" Visible="False" />
                    <asp:BoundField DataField="Logon" HeaderText="UserName" SortExpression="UserName" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="Organisation" HeaderText="Organisation" SortExpression="Organisation" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"  />
                    <asp:BoundField DataField="StatusId" HeaderText="StatusId" SortExpression="StatusId" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:CommandField ShowEditButton="True" EditText="">
                        <ControlStyle CssClass="fa fa-edit" ForeColor="blue"/>
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="">
                        <ControlStyle CssClass="fa fa-trash-o" ForeColor="red" />
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 700px">
                <div class="modal-header">
                   <h4 class="modal-title" id="myModalLabel">Add/Edit User Account</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Last Name:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control" ValidationGroup="userValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Last Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-md-1 pull-left">
                                    <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">First Name:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control" ValidationGroup="userValidation" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="First Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1 pull-left">
                                    <asp:Label runat="server" ForeColor="red" Text="*"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Email:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" ValidationGroup="userValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Email Required!" runat="server" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Wrong email format!" ControlToValidate="txtEmail" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ValidationExpression="\w+([-+.’]\w+)*@\w+([-.]
\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </div>
                                 <div class="col-md-1 pull-left">
                                    <asp:Label ID="Label2" runat="server" ForeColor="red" Text="*"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Username:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" ValidationGroup="userValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Username Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userValidation" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-md-1 pull-left">
                                    <asp:Label ID="Label3" runat="server" ForeColor="red" Text="*"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="usergroupCB">
                                <div class="col-md-3">
                                    <label class="control-label">Usergroup:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList runat="server" ID="cbUsergroup" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="organisationCB">
                                <div class="col-md-3">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:CheckBox runat="server" ID="chkActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>
                            <label id="newModal"></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" OnClick="btnCloseModal_OnClick" CssClass="btn btn-default" ClientIDMode="Static" runat="server">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveUser" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="userValidation" OnClick="btnSaveUser_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageCurrency.aspx.cs" Inherits="Ignite.Organisations.ManageCurrency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Currency Management<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Currency</asp:LinkButton></h5>
        </div>
        <div class="panel-body">
            <!--Table here-->
            <asp:GridView ID="dgvCurrency" runat="server" DataKeyNames="Id" OnRowDeleting="dgvCurrency_OnRowDeleting" OnRowEditing="dgvCurrency_OnRowEditing" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Symbol" HeaderText="Symbol" SortExpression="Symbol" />
                    <asp:BoundField DataField="ISOCode" HeaderText="ISO Code" SortExpression="ISOCode" />
                    <asp:BoundField DataField="UnitName" HeaderText="Unit Name" SortExpression="UnitName" />
                    <asp:BoundField DataField="SubUnitName" HeaderText="Sub Unit Name" SortExpression="SubUnitName" />
                    <asp:BoundField DataField="UnitNameSingle" HeaderText="UnitNameSingle" SortExpression="UnitNameSingle" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="SubUnitNameSingle" HeaderText="SubUnitNameSingle" SortExpression="SubUnitNameSingle" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="BaseCurrency" HeaderText="Base Currency" SortExpression="BaseCurrency" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                    <asp:CommandField ShowEditButton="True" EditText="">
                        <ControlStyle CssClass="fa fa-edit" />
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
            <div class="modal-content" style="width: 800px">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Currency</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Symbol:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtSymbol" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Symbol Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtSymbol"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">ISO Code:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtISO" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="ISO Code Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtISO"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Unit Name:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtUnit" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Unit Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtUnit"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Sub Unit Name:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtSubUnit" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Sub Unit Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtSubUnit"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-5">
                                    <label class="control-label">Unit Name(Single):</label>
                                </div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtUnitSingle" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Single Unit Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtUnitSingle"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-5">
                                    <label class="control-label">Sub Unit Name(Single):</label>
                                </div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtSubUnitSngle" CssClass="form-control" ValidationGroup="currencyValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Single Sub Unit Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="currencyValidation" ControlToValidate="txtSubUnitSngle"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="Div1">
                                <div class="col-md-5">
                                    <label class="control-label">Base Currency:</label>
                                </div>
                                <div class="col-md-5">
                                    <asp:CheckBox runat="server" ID="chkBaseCurrency"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="organisationCB">
                                <div class="col-md-5">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-5">
                                    <asp:CheckBox runat="server" ID="chkActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveUser" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="currencyValidation" OnClick="btnSaveUser_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>


</asp:Content>

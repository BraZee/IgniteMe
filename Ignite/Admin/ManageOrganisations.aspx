<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageOrganisations.aspx.cs" Inherits="Ignite.Admin.ManageOrganisations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info" style="background-color: transparent">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Organisation Management<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Organisation</asp:LinkButton></h5>

        </div>
        <div class="panel-body " style="padding: 10px">
            <div class="table-responsive">
                <asp:GridView ID="dgvOrganisations" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid" OnRowEditing="dgvOrganisations_OnRowEditing" OnRowDeleting="dgvOrganisations_OnRowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Telephone" HeaderText="Telephone" SortExpression="Telephone" />
                        <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                        <asp:CommandField ShowEditButton="True" EditText="">
                            <ControlStyle CssClass=" fa fa-edit" />
                        </asp:CommandField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="">
                            <ControlStyle CssClass="fa fa-trash-o" ForeColor="red" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="modal fade container-fluid" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 800px">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Organisation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-7 ">
                                    <asp:TextBox runat="server" ID="txtOrgName" CssClass="form-control" ValidationGroup="orgValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Name Required!" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">SMS Name:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" MaxLength="11" ID="txtOrgCode" CssClass="form-control" ValidationGroup="orgValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Code Required!" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgCode"></asp:RequiredFieldValidator>

                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Address:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgAddress1" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Address 1 Required!" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgAddress1"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label"></label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgAddress2" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label"></label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgAddress3" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">P.O.Box:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgPOBox1" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label"></label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgPOBox2" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label"></label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgPOBox3" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Telephone:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgTelephone" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Telephone number required!" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgTelephone"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ValidationExpression="\+233\d{9}" ErrorMessage="Wrong format! (eg. +233302123456)" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgTelephone"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Fax:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgFax" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="\+233\d{9}" ErrorMessage="Wrong format! (eg. +233302123456)" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgFax"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Email:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgEmail" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ErrorMessage="Wrong email format!" ControlToValidate="txtOrgEmail" CssClass="label label-danger" Display="Dynamic" ValidationGroup="orgValidation" ValidationExpression="\w+([-+.’]\w+)*@\w+([-.]
\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Email required!" Display="Dynamic" ValidationGroup="orgValidation" CssClass="label label-danger" ControlToValidate="txtOrgEmail"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Facebook:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgFacebook" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Twitter:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgTwitter" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Google+:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtOrgGoogle" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:CheckBox runat="server" ID="chkOrgActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnsavefacility" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" ValidationGroup="orgValidation" OnClick="btnsavefacility_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>


</asp:Content>

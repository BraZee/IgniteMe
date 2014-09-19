<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageLocations.aspx.cs" Inherits="Ignite.Organisations.ManageLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    <div class="panel panel-info" style="background-color: transparent">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Location Management<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Location</asp:LinkButton></h5>

        </div>
        <div class="panel-body " style="padding: 10px">
            <!--Table here-->
            <asp:GridView ID="dgvLocations" runat="server" OnRowDeleting="dgvLocations_OnRowDeleting" OnRowEditing="dgvLocations_OnRowEditing" EmptyDataText="No Locations set." DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Latitude" HeaderText="Latitude" SortExpression="Latitude" />
                    <asp:BoundField DataField="Longitude" HeaderText="Longitude" SortExpression="Longitude" />
                    <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
                    <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                    <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town" />
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
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Location</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtName" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Longitude:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtlongitude" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Longitude Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtlongitude" />
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Double" ControlToValidate="txtlongitude" Operator="DataTypeCheck" ErrorMessage="Wrong Longitude format!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Latitude:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtLatitude" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Latitude Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtLatitude" />
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" Type="Double" ControlToValidate="txtLatitude" Operator="DataTypeCheck" ErrorMessage="Wrong Latitude format!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Country:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtCountry" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Country Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtCountry"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">City:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="City Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtCity"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-4">
                                    <label class="control-label">Town:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" ID="txtTown" CssClass="form-control" ValidationGroup="locationValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Town Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="locationValidation" ControlToValidate="txtTown"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="organisationCB">
                                <div class="col-md-4">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-5">
                                    <asp:CheckBox runat="server" ID="chkActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                        <asp:LinkButton ID="btnSaveLocation" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="locationValidation" OnClick="btnSaveLocation_OnClick">Save</asp:LinkButton>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </div>
</asp:Content>

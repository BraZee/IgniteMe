<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="EventPricing.aspx.cs" Inherits="Ignite.Organisations.Events.EventPricing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    
    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <asp:LinkButton ID="btnEventCategories" ForeColor="white" runat="server" Text="Categories" CssClass="backTo" OnClick="btnEventCategories_OnClick"/>
            <asp:LinkButton ID="btnEventCoordinator" ForeColor="white" runat="server" Text="Coordinator" CssClass="backTo" OnClick="btnEventCoordinator_OnClick"/>
            <asp:LinkButton ID="btnEventInterests" ForeColor="white" runat="server" Text="Interests" CssClass="backTo" OnClick="btnEventInterests_OnClick"/>
            <asp:LinkButton ID="btnEventSessions" ForeColor="white" Visible="False" runat="server" Text="Sessions" CssClass="backTo" OnClick="btnEventSessions_OnClick"/>
            <asp:LinkButton ID="btnEventSocialMediaPage" runat="server" ForeColor="white" Text="Social Media Page" CssClass="backTo" OnClick="btnEventSocialMediaPage_OnClick"/>
            <asp:LinkButton ID="btnScheduleAlerts" ForeColor="white" runat="server" Text="Schedule Alerts" CssClass="backTo" OnClick="btnScheduleAlerts_OnClick"/>
            <asp:LinkButton ID="btnEventPricing" ForeColor="white" runat="server" Text="Pricing" CssClass="backTo" OnClick="btnEventPricing_OnClick"/>
            <asp:LinkButton ID="btnViewAttendance" ForeColor="white" runat="server" Text="View Attendance" CssClass="backTo" OnClick="btnViewAttendance_OnClick"/>
            <asp:LinkButton ID="btnFeedback" ForeColor="white" runat="server" Text="Respond to Feedback" CssClass="backTo" OnClick="btnFeedback_OnClick"/>
            <asp:LinkButton ID="btnMonitirinterest" ForeColor="white" runat="server" Text="Monitor Interest" CssClass="backTo" OnClick="btnMonitirinterest_OnClick" />
            
        </div>
    </div>

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Event Tickets for <asp:Label runat="server" ID="lblEventName"/><asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Ticket</asp:LinkButton></h5>
        </div>
        <div class="panel-body">
            <!--Table here-->
            <asp:GridView ID="dgvTicket" EmptyDataText="No tickets" runat="server" DataKeyNames="Id" OnRowEditing="dgvTicket_OnRowEditing" OnRowDeleting="dgvTicket_OnRowDeleting" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"/>
                    <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                    <asp:BoundField DataField="CurrencyId" HeaderText="CurrencyId" SortExpression="CurrencyId" HeaderStyle-CssClass ="hideGridColumn" ItemStyle-CssClass ="hideGridColumn"/>
                    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                    <asp:BoundField DataField="MaxSession" HeaderText="Max. No. of Sessions" SortExpression="MaxSession" />
                    <asp:BoundField DataField="MaxEntries" HeaderText="Max. No. of Entries" SortExpression="MaxEntries" />
                    <asp:BoundField DataField="NumDays" HeaderText="Number of Days" SortExpression="NumDays" />
                    <asp:BoundField DataField="Duplicates" HeaderText="Duplicates" SortExpression="Duplicates"/>
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
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
            <div class="modal-content" style="width: 800px">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Ticket</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Description:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" TextMode="MultiLine" Rows="8" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Description Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Currency:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:DropDownList runat="server" ID="cbCurrency" CssClass="form-control"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Price:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" TextMode="Number" ID="txtPrice" CssClass="form-control" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Price Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtPrice"/>
                                    <asp:CompareValidator runat="server" Type="Currency" ControlToValidate="txtPrice" Operator="DataTypeCheck" ErrorMessage="Wrong Price format!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation"/>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            
                        </div>
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-5">
                                    <label class="control-label">Maximum No. of Sessions:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtMaxSession" CssClass="form-control" TextMode="Number" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Maximum number of sessions required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtMaxSession"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" Type="Integer" ErrorMessage="Must be an integer!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtMaxSession" Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-5">
                                    <label class="control-label">Max. No. of Entries:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtMaxEntries" CssClass="form-control" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Max number of entries required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtMaxEntries"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Integer" ErrorMessage="Must be an integer!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtMaxEntries" Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-5">
                                    <label class="control-label">Number of Days:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtNumDays" CssClass="form-control" ValidationGroup="ticketValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Max numbre of days required!!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtNumDays"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" Type="Integer" ErrorMessage="Must be an integer!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="ticketValidation" ControlToValidate="txtNumDays" Operator="DataTypeCheck"></asp:CompareValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="Div1">
                                <div class="col-md-5">
                                    <label class="control-label">Duplicates:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox runat="server" ID="chkDuplicates"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12" runat="server" id="organisationCB">
                                <div class="col-md-5">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox runat="server" ID="chkActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveTicket" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="ticketValidation" OnClick="btnSaveTicket_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>


</asp:Content>

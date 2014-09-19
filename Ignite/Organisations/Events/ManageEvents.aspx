<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageEvents.aspx.cs" Inherits="Ignite.Organisations.Events.ManageEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Event Management<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Event</asp:LinkButton></h5>

        </div>
        <div class="panel-body">
            <asp:GridView ID="dgvOrgEvents" DataKeyNames="Id" runat="server" OnRowCommand="dgvOrgEvents_OnRowCommand" OnRowDeleting="dgvOrgEvents_OnRowDeleting" OnRowEditing="dgvOrgEvents_OnRowEditing" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                    <asp:BoundField DataField="BeginDate" HeaderText="Starts" SortExpression="BeginDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="Ends" SortExpression="EndDate" />
                    <asp:BoundField DataField="isRoutine" HeaderText="Routine" SortExpression="isRoutine" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                    <asp:BoundField DataField="BeginTime" HeaderText="BeginTime" SortExpression="BeginTime" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="LocationId" HeaderText="LocationId" SortExpression="LocationId" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="hasSession" HeaderText="hasSession" SortExpression="hasSession" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:CommandField ShowSelectButton="True" SelectText="Event Setup">
                        <ControlStyle CssClass=" btn btn-outline btn-info"/>
                    </asp:CommandField>
                    <asp:CommandField ShowEditButton="True" EditText="">
                            <ControlStyle CssClass=" fa fa-pencil" ForeColor="blue"/>
                        </asp:CommandField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="">
                            <ControlStyle CssClass="fa fa-trash-o" ForeColor="red"/>
                        </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog" style="width: 800px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Events</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-7 ">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="eventValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" Display="Dynamic" ValidationGroup="eventValidation" CssClass="label label-danger" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Begin Date:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server"  ID="dateBegin" TextMode="date"  CssClass="form-control" ValidationGroup="eventValidation"/>
                                    <asp:CompareValidator CssClass="label label-danger" Display="Dynamic" ControlToValidate="datebegin" ID="CompareValidator2" runat="server" ControlToCompare="currDate" ValidationGroup="eventValidation" Operator="GreaterThanEqual" ErrorMessage="Begin date must occur on or after today."/>
                                    <asp:TextBox runat="server" TextMode="Date" CssClass="hideGridColumn" ID="currDate"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">End Date:</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" ID="dateEnd" TextMode="Date" ValidationGroup="eventValidation" CssClass="form-control"></asp:TextBox>
                                    <asp:CompareValidator CssClass="label label-danger" Display="Dynamic" ControlToValidate="dateEnd" ID="CompareValidator1" runat="server" ControlToCompare="dateBegin" ValidationGroup="eventValidation" Operator="GreaterThanEqual" ErrorMessage="End date must occur on or after begin date."></asp:CompareValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Begin Time:</label>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:TextBox runat="server" ID="timeBegin" CssClass="form-control" TextMode="Time" ValidationGroup="eventValidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Begin Time required!" Display="Dynamic" ValidationGroup="eventValidation" CssClass="label label-danger" ControlToValidate="timeBegin"></asp:RequiredFieldValidator>
                                    </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">End Time:</label>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:TextBox runat="server" ID="timeEnd" CssClass="form-control" TextMode="Time" ValidationGroup="eventValidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="End time required!" Display="Dynamic" ValidationGroup="eventValidation" CssClass="label label-danger" ControlToValidate="timeEnd"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-1">
                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                                </div>
                            <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Location:</label>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:DropDownList runat="server" ID="cbLocations" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Description:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" TextMode="MultiLine" CssClass="form-control" ID="txtDesc" Rows="7"/>
                                    </div>
                                </div>
                                
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Routine:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:CheckBox runat="server" ID="chkRoutine"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Has Sessions:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:CheckBox runat="server" ID="chkHasSessions"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Active:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:CheckBox runat="server" ID="chkActive" Checked="True" Enabled="False"></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveEvent" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="eventValidation" OnClick="btnSaveEvent_OnClick">Save</asp:LinkButton>
               </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    
    

</asp:Content>
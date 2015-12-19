﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SA.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form" runat="server">
        <asp:ScriptManager ID="menadżer" runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:ObjectDataSource
                        ID="źródłoDanych"
                        runat="server"
                        SelectMethod="PobierzLoty"
                        DeleteMethod="UsuńLot"
                        UpdateMethod="AktualizujLot"
                        InsertMethod="DodajLot"
                        TypeName="SA.App_Code.LotyAdapter"
                        DataObjectTypeName="SA.Loty"
                        OldValuesParameterFormatString="oryginalny"
                        ConflictDetection="CompareAllValues" />
                    <asp:GridView ID="grid" DataSourceID="źródłoDanych" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
                            <asp:BoundField DataField="PortDocelowy" HeaderText="PortDocelowy" SortExpression="PortDocelowy" />
                            <asp:BoundField DataField="CenaBezRabatu" HeaderText="CenaBezRabatu" SortExpression="CenaBezRabatu" />
                            <asp:BoundField DataField="CenaZRabatem" HeaderText="CenaZRabatem" SortExpression="CenaZRabatem" />
                            <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" />
                        </Columns>
                    </asp:GridView>
                    <asp:DetailsView runat="server" DataSourceID="źródłoDanych" DefaultMode="Insert">
                        
                        <Fields>
                            <asp:CommandField ShowInsertButton="True" />
                        </Fields>
                    </asp:DetailsView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>

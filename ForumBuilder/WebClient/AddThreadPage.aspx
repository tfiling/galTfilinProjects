<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddThreadPage.aspx.cs" Inherits="WebClient.AddThreadPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <asp:Label ID="Label3" runat="server" Text="Add Thread" Font-Bold="true"></asp:Label>
    </div>
    
    <div>
        <asp:Label ID="Label1" runat="server" Text="Title"></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </div>
        <div>
        <asp:Label ID="Label2" runat="server" Text="Content"></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" />
    </div>

</asp:Content>

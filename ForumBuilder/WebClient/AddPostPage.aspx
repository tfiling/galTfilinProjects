<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddPostPage.aspx.cs" Inherits="WebClient.AddPostPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodyHolder" runat="server">
        <div>
        <asp:Label ID="Label3" runat="server" Text="Add Post" Font-Bold="true"></asp:Label>
    </div>
    
    <div>
        <asp:Label ID="Label1" runat="server" Text="Title"></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="titleBox" runat="server"></asp:TextBox>
    </div>
        <div>
        <asp:Label ID="Label2" runat="server" Text="Content"></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="contentBox" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" />
    </div>
    <asp:Button ID="backButton" runat="server" Text="back" OnClick="backButton_Click"/>
</asp:Content>

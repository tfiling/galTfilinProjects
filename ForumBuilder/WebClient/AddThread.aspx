<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddThread.aspx.cs" Inherits="WebClient.AddThread" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <br />
        <br />

        <asp:Label ID="Label1" runat="server" Text="Add Thread" Font-Size="Larger"></asp:Label>
        <br />
        <br />
        <p>
            <asp:Label AssociatedControlID="Title" runat="server" CssClass="col-sm-2 control-label">Title</asp:Label>
            <asp:TextBox ID="Title" runat="server" class="myText-control" Width="231px"></asp:TextBox>
        </p>
        <p>
            <asp:Label AssociatedControlID="Content" runat="server" CssClass="col-sm-2 control-label">Content</asp:Label>
            <asp:TextBox ID="Content" runat="server" class="myText-control" Height="98px" Width="231px"></asp:TextBox>
        </p>

        <p>
            <asp:Button ID="Button1" runat="server" Text="Add Thread" Width="116px" Font-Bold="true" OnClick="Button1_Click" />
        </p>

        <asp:Button ID="backButton" runat="server" Text="back" OnClick="backButton_Click"/>
    </div>

</asp:Content>

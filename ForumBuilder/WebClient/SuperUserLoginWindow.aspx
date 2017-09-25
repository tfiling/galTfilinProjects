<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="SuperUserLoginWindow.aspx.cs" Inherits="WebClient.SuperUserLoginWindow" %>

<asp:Content ID="superUserReplace" ContentPlaceHolderID="bodyHolder" runat="server">

            <h1>Super User</h1>
    <div>


        <p>
            <asp:label AssociatedControlID="textBox_userName" runat="server" Cssclass="col-sm-2 control-label">User Name</asp:label>
            <asp:TextBox ID="textBox_userName" runat="server" class="myText-control"></asp:TextBox>
        </p>

         <p>
            <asp:label AssociatedControlID="textBox_password" runat="server" Cssclass="col-sm-2 control-label">Password</asp:label>
            <asp:TextBox ID="textBox_password" runat="server" TextMode="Password" class="myText-control"></asp:TextBox>
        </p>

        <p>
            <asp:label AssociatedControlID="textBox_email" runat="server" Cssclass="col-sm-2 control-label">Email</asp:label>
            <asp:TextBox ID="textBox_email" type="email" runat="server" class="myText-control"></asp:TextBox>
        </p>
        <p>

            <asp:Button ID="btn_back" runat="server" class="btn btn-default" Text="Back" OnClick="btnClick_back"/>
            <asp:Button ID="btn_login" runat="server" class="btn btn-default" Text="Log In" OnClick="btnClick_login"/>

        </p>
    
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="MainWindow.aspx.cs" Inherits="WebClient.MainWindow" %>
<asp:Content runat="server" ID="content1" ContentPlaceHolderID="bodyHolder">



    <div>
    <p>
        <img alt="LOGO" src="http://www.tudiabetes.org/forum/uploads/default/original/3X/3/5/35d47232d1d9cb26dcd2a226952f98137a9080c8.jpg" style="height: 186px; width: 874px" />
    </p>

    </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
        <p>
            <asp:label AssociatedControlID="forum_dropList" runat="server" Cssclass="control-label">choose forum:</asp:label>
            <asp:DropDownList ID="forum_dropList" runat="server" OnLoad="forum_dropList_Load" OnSelectedIndexChanged="forum_dropList_SelectedIndexChanged">
            </asp:DropDownList>
        </p>
                </div>
                         <div class="col-sm-offset-2 col-sm-10">

        <p>
                        <asp:label AssociatedControlID="ID" runat="server" Cssclass="control-label">please log in:</asp:label>
            <asp:label AssociatedControlID="ID" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;As Guest:</asp:label>

            <asp:CheckBox ID="CheckBox_Guest" runat="server"/>
        </p>
                             </div>
        <p>
            <asp:label ID="userNameLabel" AssociatedControlID="ID" runat="server" Cssclass="col-sm-2 control-label">User Name</asp:label>
            <asp:TextBox ID="ID" runat="server" class="myText-control"></asp:TextBox>
        </p>
        <p>
            <asp:label ID="passwordLabel" AssociatedControlID="Password" runat="server" Cssclass="col-sm-2 control-label">Password</asp:label>
            <asp:TextBox ID="Password" TextMode="Password" runat="server" class="myText-control"></asp:TextBox>
        </p>
        <p>
            <asp:label ID="sessionKeyLabel" AssociatedControlID="SessionKeyTextField" Visible="false" runat="server" Cssclass="col-sm-2 control-label">Session Key:</asp:label>
            <asp:TextBox ID="SessionKeyTextField" TextMode="Number" Visible="false" runat="server" class="myText-control"></asp:TextBox>
        </p>
             <div class="col-sm-offset-2 col-sm-10">
        
            <asp:Button ID="Btn_Login" class="btn btn-default" runat="server" Text="Login" OnClick="Btn_Login_Click" />
            <asp:Button ID="Btn_signUp" class="btn btn-default" runat="server" Text="Sign up" OnClick="Btn_signUp_Click" />
            <asp:Button ID="LogInWithDiffUserButton" Visible="false" class="btn btn-default" runat="server" Text="log in with a different user" OnClick="LogInWithDiffUserButton_Click" />

            </div>
        </div>


</asp:Content>


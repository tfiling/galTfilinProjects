<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ForumWindow.aspx.cs" Inherits="WebClient.ForumWindow" %>

<asp:Content ID="forumWindowContent" ContentPlaceHolderID="bodyHolder" runat="server">

    <style type="text/css">
        #nav
        {
            width:100%;
            margin:auto;
        }
        #nav ul
        {
            padding: 0;
            list-style: none;
        }
        #nav ul li
        {
            float:left;
            text-align:center;
            width:250px;
            margin:1px;
        }
        #nav ul li a
        {
            color: #fff;
            text-decoration:none;
            font-family:Calibri;
            background:#0a0;
            display: block;
            padding:10px;
            float:left;
            text-align:center;
            width:250px;
            margin:1px;
        }
    </style>




















    
    <h1>Sub-Forums</h1>
    <div id="nav">
        <asp:menu ID="menu" Orientation="Horizontal" runat="server" Enabled="true" onmenuitemclick="NavigationMenu_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Create Sub-Forum-stub" Value="AddSub" Enabled="false"/>
                <asp:MenuItem Text="View Private Messages-stub" Value="privateMessages" Enabled="false"/>
                <asp:MenuItem Text="Set\Change Preferences-sutb" Value="Set" Enabled="false"/>
                <asp:MenuItem Text="Logout" Value="menuLogout" Enabled="true"/>
            </Items>
        </asp:menu>

        
        <p>
        <asp:Label ID="lbl_forumName" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Table ID="tbl_subForumList" runat="server" class="table-hover" Width="100%"></asp:Table>
    </p>
    </div>
    </asp:Content>
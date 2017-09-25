using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL_Back_End;

namespace DataBase
{
    interface ICache
    {
        void setPostExpiration(int hours, int minutes, int secs);

        void AddForum(Forum f);
        void AddSubforum(SubForum sf);
        void AddSuperUser(User su);
        void AddUser(User user);
        void AddPost(Post post);

        void RemoveForum(int f);
        void RemoveSubforum(int sf);
        void RemoveSuperUser(int su);
        void RemoveUser(int user, int forum);
        void RemovePost(int p);

        Forum GetForum(int f);
        SubForum GetSubforum(int sf);
        User GetSuperUser(int su);
        User GetUser(int user, int forum);
        Post GetPost(int p);

    }
}

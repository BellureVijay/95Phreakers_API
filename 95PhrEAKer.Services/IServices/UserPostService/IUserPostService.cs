using _95PhrEAKer.Domain.UserPost;
using _95PhrEAKer.Persistence.DbModals;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _95PhrEAKer.Services.IServices.UserPostService
{
    public interface IUserPostService
    {
        List<Post> GetMyPosts(string email);
        Task<IActionResult> CreatePost(string message, string email);
        void DeletMyPost(int postid);
    }
}

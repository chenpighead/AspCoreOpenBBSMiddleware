﻿using ApplicationCore;
using AspCoreOpenBBSMiddleware.Controllers.Base;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AspCoreOpenBBSMiddleware.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// GetUser(取得所有使用者)
        /// </summary>
        /// <param name="name">部分使用者名稱</param>
        /// <param name="desc">由新至舊排序</param>
        /// <param name="max">一次取回幾筆資料，最多1000</param>
        [HttpGet()]
        public ActionResult<IEnumerable<User>> GetAll([FromQuery] string name = "",
                                                      [FromQuery] bool desc = true,
                                                      [FromQuery] int max = 1000)
        {
            var result = from u in _userRepository.Get()
                         where (string.IsNullOrWhiteSpace(name) || u.Name.Contains(name))
                         select u;
            if (desc) result = result.OrderByDescending(a => a.LastLogin);

            return Ok(result.Take(max));
        }

        /// <summary>
        /// 取得特定使用者
        /// </summary>
        /// <param name="uid">唯一性編號</param>
        [HttpGet("{uid}")]
        public ActionResult<IEnumerable<User>> GetUser(int uid)
        {
            var result = from u in _userRepository.Get()
                         where u.Id == uid
                         select u;
            return Ok(result);
        }

        /// <summary>
        /// 刪除特定使用者
        /// </summary>
        /// <param name="uid">唯一性編號</param>
        [HttpDelete("{uid}")]
        public IActionResult DeleteBoardById(int uid)
        {
            User user = (from a in _userRepository.Get()
                         where a.Id == uid
                         select a)
                        .SingleOrDefault();
            if (null != user)
            {
                _userRepository.Delete(user);
                _userRepository.Save();
            }
            return Ok();
        }

        /// <summary>
        /// 取得特定使用者公告
        /// </summary>
        /// <param name="uid">唯一性編號</param>
        [HttpGet("{uid}/Comments")]
        public ActionResult<IEnumerable<User>> GetUserComments(int uid)
        {
            var result = from u in _userRepository.Get()
                         where u.Id == uid
                         select null == u ? null : u.Comments;
            return Ok(result);
        }

        /// <summary>
        /// 取得特定使用者最愛
        /// </summary>
        /// <param name="uid">唯一性編號</param>
        [HttpGet("{uid}/Favorites")]
        public ActionResult<IEnumerable<Board>> GetUserFavorites(int uid)
        {
            var result = from u in _userRepository.Get()
                         where u.Id == uid
                         select null == u ? null : u.Favorites;
            return Ok(result);
        }

        /// <summary>
        /// 取得特定使用者文章
        /// </summary>
        /// <param name="uid">唯一性編號</param>
        [HttpGet("{uid}/Articals")]
        public ActionResult<IEnumerable<Artical>> GetUserArticals(int uid)
        {
            var result = from u in _userRepository.Get()
                         where u.Id == uid 
                         select null == u ? null : u.Articals;
            return Ok(result);
        }
    }
}

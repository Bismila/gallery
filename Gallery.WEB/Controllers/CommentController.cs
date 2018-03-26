using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery.WEB.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;

        public CommentController(ICommentService _commentService, IUserService _userService)
        {
            commentService = _commentService;
            userService = _userService;
        }
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("AddComment")]
        public JsonResult AddComment(CommentDTO comment)
        {
            if (!string.IsNullOrWhiteSpace(comment.Text))
            {
                var comm = commentService.AddComment(comment);
                return Json(comm, JsonRequestBehavior.AllowGet);
            }
            var str = "";
            return Json(str, JsonRequestBehavior.AllowGet);

        }

        [HttpGet, ActionName("DelComment")]
        public JsonResult DelComment(long id)
        {
            if (id != 0)
            {
                commentService.DeleteComment(id);
                var str = "Your comment was deleted.";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            return Json("Oops... something went wrong.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("EditComment")]
        public JsonResult EditComment(int? id)
        {
            if (id != null)
            {
                var currentComment = commentService.Get((long)id);
                return Json(currentComment, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("UpdateComment")]
        public JsonResult UpdateComment(CommentDTO updateComment)
        {
            var currComm = commentService.Get(updateComment.Id);
            if (string.IsNullOrWhiteSpace(updateComment.Text))
            {
                return Json(currComm, JsonRequestBehavior.AllowGet);
            }
            else if (currComm.Text != updateComment.Text)
            {
                currComm.Text = updateComment.Text;
                commentService.UpdateComment(currComm);
                return Json(updateComment, JsonRequestBehavior.AllowGet);
            }
                return Json(currComm, JsonRequestBehavior.AllowGet);

        }


    }
}
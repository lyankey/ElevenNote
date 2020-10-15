using ElevenNote.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    public class CategoryController : ApiController
    {
            public IHttpActionResult Get(int id)
            {
                CategoryService categoryService = CreateCategoryService();
                var note = categoryService.GetCategoryById(id);
                return Ok(note);
            }
            public IHttpActionResult Post(CategoryCreate note)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var service = CreateCategoryService();

                if (!service.CreateCategory(note))
                    return InternalServerError();

                return Ok();
            }
            public IHttpActionResult Get()
            {
                CategoryService categoryService = CreateCategoryService();
                var categories = categoryService.GetCategory();
                return Ok(categories);
            }
            private CategoryService CreateCategoryService()
            {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var categoryService = new CategoryService(userId);
                return categoryService;
            }
            public IHttpActionResult Put(CategoryEdit note)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var service = CreateCategoryService();

                if (!service.UpdateCategory(note))
                    return InternalServerError();

                return Ok();
            }

            public IHttpActionResult Delete(int id)
            {
                var service = CreateCategoryService();

                if (!service.DeleteCategory(id))
                    return InternalServerError();

                return Ok();
            }
        }
    }
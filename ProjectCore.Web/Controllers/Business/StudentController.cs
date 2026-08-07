using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class StudentController
    {
        public SqlSugarClient db;

        public StudentController(SqlSugarClient db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取学生信息集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/student/studentPage")]
        [AllowAnonymous]
        public async Task<dynamic> GetStuList(StudentInput input)
        {
            var pageList = await db.Queryable<Student>().Where(r => r.Status > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }

        /// <summary>
        /// 保存学深信息(新增/编辑)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/student/saveStu")]
        public async Task SaveStudent(EditStudnetInput input)
        {
            var model = input.Adapt<Student>();
            if (string.IsNullOrEmpty(model.GUID))
            {
                model.GUID = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;

                await db.Insertable(model).ExecuteCommandAsync();
            }
            else
            {

            }
        }
    }
}

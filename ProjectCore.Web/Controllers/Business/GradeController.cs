using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers
{
    [ApiController]
    public class GradeController : ControllerBase
    {
        public SqlSugarClient db;

        public GradeController(SqlSugarClient db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取年级信息集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/grade/getGradeList")]
        public async Task<dynamic> GetGradeList(StudentInput input)
        {
            var pageList = await db.Queryable<Student>().Where(r => r.Status > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
    }
}

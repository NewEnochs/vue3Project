using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        public SqlSugarClient db;

        public StudentController()
        {
            this.db = new DBContext().db;
        }

        #region 获取学生分页列表
        /// <summary>
        /// 获取学生分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>学生分页数据</returns>
        [HttpPost("/student/studentPage")]
        [AllowAnonymous]
        public async Task<dynamic> GetStuList(StudentInput input)
        {
            var pageList = await db.Queryable<Student>().Where(r => r.Status > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取学生详情
        /// <summary>
        /// 获取学生详情
        /// </summary>
        /// <param name="input">学生主键参数</param>
        /// <returns>学生信息</returns>
        [HttpPost("/student/getStudent")]
        public async Task<Student> GetStudent(StudentIdInput input)
        {
            return await db.Queryable<Student>().FirstAsync(r => r.GUID == input.GUID);
        }
        #endregion

        #region 保存学生信息
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="input">学生编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/student/saveStu")]
        public async Task<bool> SaveStudent(EditStudnetInput input)
        {
            var model = input.Adapt<Student>();
            if (string.IsNullOrEmpty(model.GUID))
            {
                model.GUID = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除学生信息
        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="input">学生主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/student/deleteStudent")]
        public async Task<bool> DeleteStudent(StudentIdInput input)
        {
            return await db.Updateable<Student>().SetColumns(r => r.Status == 0).Where(r => r.GUID == input.GUID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

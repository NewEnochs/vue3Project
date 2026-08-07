using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class GradeController : ControllerBase
    {
        public SqlSugarClient db;

        public GradeController()
        {
            this.db = new DBContext().db;
        }

        #region 获取班级分页列表
        /// <summary>
        /// 获取班级分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>班级分页数据</returns>
        [HttpPost("/grade/gradePage")]
        public async Task<dynamic> GetGradeList(GradeInput input)
        {
            var pageList = await db.Queryable<Grade>().Where(r => r.ZT > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取班级详情
        /// <summary>
        /// 获取班级详情
        /// </summary>
        /// <param name="input">班级主键参数</param>
        /// <returns>班级信息</returns>
        [HttpPost("/grade/getGrade")]
        public async Task<Grade> GetGrade(GradeIdInput input)
        {
            return await db.Queryable<Grade>().FirstAsync(r => r.GradeGUID == input.GradeGUID);
        }
        #endregion

        #region 保存班级信息
        /// <summary>
        /// 保存班级信息
        /// </summary>
        /// <param name="input">班级编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/grade/saveGrade")]
        public async Task<bool> SaveGrade(EditGradeInput input)
        {
            var model = input.Adapt<Grade>();
            if (string.IsNullOrEmpty(model.GradeGUID))
            {
                model.GradeGUID = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除班级信息
        /// <summary>
        /// 删除班级信息
        /// </summary>
        /// <param name="input">班级主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/grade/deleteGrade")]
        public async Task<bool> DeleteGrade(GradeIdInput input)
        {
            return await db.Updateable<Grade>().SetColumns(r => r.ZT == 0).Where(r => r.GradeGUID == input.GradeGUID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

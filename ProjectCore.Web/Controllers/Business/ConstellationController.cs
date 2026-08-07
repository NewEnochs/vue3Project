using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class ConstellationController : ControllerBase
    {
        public SqlSugarClient db;

        public ConstellationController()
        {
            this.db = new DBContext().db;
        }

        #region 获取星座分页列表
        /// <summary>
        /// 获取星座分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>星座分页数据</returns>
        [HttpPost("/constellation/constellationPage")]
        public async Task<dynamic> GetConstellationList(ConstellationInput input)
        {
            var pageList = await db.Queryable<Constellation>().Where(r => r.IsShow > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取星座详情
        /// <summary>
        /// 获取星座详情
        /// </summary>
        /// <param name="input">星座主键参数</param>
        /// <returns>星座信息</returns>
        [HttpPost("/constellation/getConstellation")]
        public async Task<Constellation> GetConstellation(ConstellationIdInput input)
        {
            return await db.Queryable<Constellation>().FirstAsync(r => r.ConstellationID == input.ConstellationID);
        }
        #endregion

        #region 保存星座信息
        /// <summary>
        /// 保存星座信息
        /// </summary>
        /// <param name="input">星座编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/constellation/saveConstellation")]
        public async Task<bool> SaveConstellation(EditConstellationInput input)
        {
            var model = input.Adapt<Constellation>();
            if (model.ConstellationID == 0)
            {
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除星座信息
        /// <summary>
        /// 删除星座信息
        /// </summary>
        /// <param name="input">星座主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/constellation/deleteConstellation")]
        public async Task<bool> DeleteConstellation(ConstellationIdInput input)
        {
            return await db.Updateable<Constellation>().SetColumns(r => r.IsShow == 0).Where(r => r.ConstellationID == input.ConstellationID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

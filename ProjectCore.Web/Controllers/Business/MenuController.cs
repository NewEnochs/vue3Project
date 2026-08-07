using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class MenuController : ControllerBase
    {
        public SqlSugarClient db;

        public MenuController()
        {
            this.db = new DBContext().db;
        }

        #region 获取菜单分页列表
        /// <summary>
        /// 获取菜单分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>菜单分页数据</returns>
        [HttpPost("/menu/menuPage")]
        public async Task<dynamic> GetMenuList(MenuInput input)
        {
            var pageList = await db.Queryable<Menu>().Where(r => r.IsVisible > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取菜单详情
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="input">菜单主键参数</param>
        /// <returns>菜单信息</returns>
        [HttpPost("/menu/getMenu")]
        public async Task<Menu> GetMenu(MenuIdInput input)
        {
            return await db.Queryable<Menu>().FirstAsync(r => r.MenuGUID == input.MenuGUID);
        }
        #endregion

        #region 保存菜单信息
        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="input">菜单编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/menu/saveMenu")]
        public async Task<bool> SaveMenu(EditMenuInput input)
        {
            var model = input.Adapt<Menu>();
            if (string.IsNullOrEmpty(model.MenuGUID))
            {
                model.MenuGUID = Guid.NewGuid().ToString();
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除菜单信息
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="input">菜单主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/menu/deleteMenu")]
        public async Task<bool> DeleteMenu(MenuIdInput input)
        {
            return await db.Updateable<Menu>().SetColumns(r => r.IsVisible == 0).Where(r => r.MenuGUID == input.MenuGUID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

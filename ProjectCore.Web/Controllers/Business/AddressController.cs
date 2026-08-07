using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class AddressController : ControllerBase
    {
        public SqlSugarClient db;

        public AddressController()
        {
            this.db = new DBContext().db;
        }

        #region 获取地址分页列表
        /// <summary>
        /// 获取地址分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>地址分页数据</returns>
        [HttpPost("/address/addressPage")]
        public async Task<dynamic> GetAddressList(AddressInput input)
        {
            var pageList = await db.Queryable<Address>().Where(r => r.ZT > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取地址详情
        /// <summary>
        /// 获取地址详情
        /// </summary>
        /// <param name="input">地址主键参数</param>
        /// <returns>地址信息</returns>
        [HttpPost("/address/getAddress")]
        public async Task<Address> GetAddress(AddressIdInput input)
        {
            return await db.Queryable<Address>().FirstAsync(r => r.AddressID == input.AddressID);
        }
        #endregion

        #region 保存地址信息
        /// <summary>
        /// 保存地址信息
        /// </summary>
        /// <param name="input">地址编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/address/saveAddress")]
        public async Task<bool> SaveAddress(EditAddressInput input)
        {
            var model = input.Adapt<Address>();
            if (model.AddressID == 0)
            {
                model.CreateDate = DateTime.Now;
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除地址信息
        /// <summary>
        /// 删除地址信息
        /// </summary>
        /// <param name="input">地址主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/address/deleteAddress")]
        public async Task<bool> DeleteAddress(AddressIdInput input)
        {
            return await db.Updateable<Address>().SetColumns(r => r.ZT == 0).Where(r => r.AddressID == input.AddressID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

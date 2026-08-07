using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using SqlSugar;

namespace ProjectCore.Web.Controllers.Business
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public SqlSugarClient db;

        public CompanyController()
        {
            this.db = new DBContext().db;
        }

        #region 获取公司分页列表
        /// <summary>
        /// 获取公司分页列表
        /// </summary>
        /// <param name="input">分页查询参数</param>
        /// <returns>公司分页数据</returns>
        [HttpPost("/company/companyPage")]
        public async Task<dynamic> GetCompanyList(CompanyInput input)
        {
            var pageList = await db.Queryable<Company>().Where(r => r.ZT > 0).ToPagedListAsync(input.PageNo, input.PageSize);
            return pageList;
        }
        #endregion

        #region 获取公司详情
        /// <summary>
        /// 获取公司详情
        /// </summary>
        /// <param name="input">公司主键参数</param>
        /// <returns>公司信息</returns>
        [HttpPost("/company/getCompany")]
        public async Task<Company> GetCompany(CompanyIdInput input)
        {
            return await db.Queryable<Company>().FirstAsync(r => r.CompanyGUID == input.CompanyGUID);
        }
        #endregion

        #region 保存公司信息
        /// <summary>
        /// 保存公司信息
        /// </summary>
        /// <param name="input">公司编辑信息</param>
        /// <returns>是否保存成功</returns>
        [HttpPost("/company/saveCompany")]
        public async Task<bool> SaveCompany(EditCompanyInput input)
        {
            var model = input.Adapt<Company>();
            if (string.IsNullOrEmpty(model.CompanyGUID))
            {
                model.CompanyGUID = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                return await db.Insertable(model).ExecuteCommandAsync() > 0;
            }

            return await db.Updateable(model).ExecuteCommandAsync() > 0;
        }
        #endregion

        #region 删除公司信息
        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="input">公司主键参数</param>
        /// <returns>是否删除成功</returns>
        [HttpPost("/company/deleteCompany")]
        public async Task<bool> DeleteCompany(CompanyIdInput input)
        {
            return await db.Updateable<Company>().SetColumns(r => r.ZT == 0).Where(r => r.CompanyGUID == input.CompanyGUID).ExecuteCommandAsync() > 0;
        }
        #endregion
    }
}

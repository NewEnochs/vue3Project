using SqlSugar;

namespace ProjectCore.DAL
{
    public static class SqlSugarPageExtension
    {
        public static async Task<SqlSugarPagedList<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> entity, int pageIndex, int pageSize) where TEntity : new()
        {
            RefAsync<int> totalCount = 0;
            List<TEntity> items = await entity.ToPageListAsync(pageIndex, pageSize, totalCount);
            int num = (int)Math.Ceiling((double)(int)totalCount / (double)pageSize);
            return new SqlSugarPagedList<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items,
                TotalCount = totalCount,
                TotalPages = num,
                HasNextPages = (pageIndex < num),
                HasPrevPages = (pageIndex - 1 > 0)
            };
        }
    } }

public class SqlSugarPagedList<TEntity> where TEntity : new()
{

    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; }


    /// <summary>
    /// 每页显示条数
    /// </summary>
    public int PageSize { get; set; }


    /// <summary>
    /// 总条数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 当前页集合
    /// </summary>
    public IEnumerable<TEntity>? Items { get; set; }

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPrevPages { get; set; }

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPages { get; set; }
}

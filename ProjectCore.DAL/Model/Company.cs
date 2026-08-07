using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 公司组织架构树形表
    ///</summary>
    [SugarTable("Company")]
    public class Company
    {
     
        /// <summary>
        /// 公司GUID主键
        ///</summary>
        [SugarColumn(ColumnName="CompanyGUID" ,IsPrimaryKey = true) ]
        [DisplayName("公司GUID主键")]
        public string CompanyGUID  { get; set;  } = null!;
     
        /// <summary>
        /// 公司/部门名称（支持中文）
        ///</summary>
        [SugarColumn(ColumnName="CompanyName" ) ]
        [DisplayName("公司/部门名称（支持中文）")]
        public string CompanyName  { get; set;  } = null!;
     
        /// <summary>
        /// 父级GUID（NULL表示根节点）
        ///</summary>
        [SugarColumn(ColumnName="ParentGuID" ) ]
        [DisplayName("父级GUID（NULL表示根节点）")]
        public string? ParentGuID  { get; set;  } 
     
        /// <summary>
        /// 创建日期
        ///</summary>
        [SugarColumn(ColumnName="CreateDate" ) ]
        [DisplayName("创建日期")]
        public DateTime CreateDate  { get; set;  } 
     
        /// <summary>
        /// 状态（0.禁用 1.启用）
        ///</summary>
        [SugarColumn(ColumnName="ZT" ) ]
        [DisplayName("状态（0.禁用 1.启用）")]
        public int ZT  { get; set;  } 
     
        /// <summary>
        /// 组织层级（1.根节点 2.一级 3.二级）
        ///</summary>
        [SugarColumn(ColumnName="Level" ) ]
        [DisplayName("组织层级（1.根节点 2.一级 3.二级）")]
        public byte Level  { get; set;  } 
     
        /// <summary>
        /// 排序序号（同级排序）
        ///</summary>
        [SugarColumn(ColumnName="SortOrder" ) ]
        [DisplayName("排序序号（同级排序）")]
        public int SortOrder  { get; set;  } 
    

    }
    
}
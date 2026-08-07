using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 星座信息表
    ///</summary>
    [SugarTable("Constellation")]
    public class Constellation
    {
     
        /// <summary>
        /// 星座ID自增主键
        ///</summary>
        [SugarColumn(ColumnName="ConstellationID" ,IsPrimaryKey = true,IsIdentity = true) ]
        [DisplayName("星座ID自增主键")]
        public int ConstellationID  { get; set;  } 
     
        /// <summary>
        /// 星座名称（支持中文）
        ///</summary>
        [SugarColumn(ColumnName="ConstellationName" ) ]
        [DisplayName("星座名称（支持中文）")]
        public string ConstellationName  { get; set;  } = null!;
     
        /// <summary>
        /// 星座图标图片路径
        ///</summary>
        [SugarColumn(ColumnName="ImagePath" ) ]
        [DisplayName("星座图标图片路径")]
        public string? ImagePath  { get; set;  } 
     
        /// <summary>
        /// 是否显示（0.不显示 1.显示）
        ///</summary>
        [SugarColumn(ColumnName="IsShow" ) ]
        [DisplayName("是否显示（0.不显示 1.显示）")]
        public int IsShow  { get; set;  } 
     
        /// <summary>
        /// 排序序号（数值越小越靠前）
        ///</summary>
        [SugarColumn(ColumnName="SortOrder" ) ]
        [DisplayName("排序序号（数值越小越靠前）")]
        public int SortOrder  { get; set;  } 
     
        /// <summary>
        /// 备注信息
        ///</summary>
        [SugarColumn(ColumnName="Remark" ) ]
        [DisplayName("备注信息")]
        public string? Remark  { get; set;  } 
    

    }
    
}
using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 班级信息表
    ///</summary>
    [SugarTable("Grade")]
    public class Grade
    {
     
        /// <summary>
        /// 班级GUID主键
        ///</summary>
        [SugarColumn(ColumnName="GradeGUID" ,IsPrimaryKey = true) ]
        [DisplayName("班级GUID主键")]
        public string GradeGUID  { get; set;  } = null!;
     
        /// <summary>
        /// 班级名称（支持中文）
        ///</summary>
        [SugarColumn(ColumnName="GradeName" ) ]
        [DisplayName("班级名称（支持中文）")]
        public string GradeName  { get; set;  } = null!;
     
        /// <summary>
        /// 班级编码（如 G2024001）
        ///</summary>
        [SugarColumn(ColumnName="GradeCode" ) ]
        [DisplayName("班级编码（如 G2024001）")]
        public string GradeCode  { get; set;  } = null!;
     
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
        /// 备注信息
        ///</summary>
        [SugarColumn(ColumnName="Remark" ) ]
        [DisplayName("备注信息")]
        public string? Remark  { get; set;  } 
     
        /// <summary>
        /// 租户标识（用于多租户隔离）
        ///</summary>
        [SugarColumn(ColumnName="TenantID" ) ]
        [DisplayName("租户标识（用于多租户隔离）")]
        public string TenantID  { get; set;  } = null!;
    

    }
    
}
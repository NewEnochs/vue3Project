using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 学生信息表
    ///</summary>
    [SugarTable("Student")]
    public class Student
    {
     
        /// <summary>
        /// 学生ID自增主键
        ///</summary>
        [SugarColumn(ColumnName= "GUID", IsPrimaryKey = true) ]
        [DisplayName("学生GUID主键")]
        public string? GUID { get; set;  } 
     
        /// <summary>
        /// 学号（如 S20240001）
        ///</summary>
        [SugarColumn(ColumnName="StudentCode" ) ]
        [DisplayName("学号（如 S20240001）")]
        public string StudentCode  { get; set;  } = null!;
     
        /// <summary>
        /// 学生姓名（支持中文）
        ///</summary>
        [SugarColumn(ColumnName="StudentName" ) ]
        [DisplayName("学生姓名（支持中文）")]
        public string StudentName  { get; set;  } = null!;
     
        /// <summary>
        /// 登录账号（唯一）
        ///</summary>
        [SugarColumn(ColumnName="LoginName" ) ]
        [DisplayName("登录账号（唯一）")]
        public string LoginName  { get; set;  } = null!;
     
        /// <summary>
        /// 登录密码（实际应用应存储哈希值）
        ///</summary>
        [SugarColumn(ColumnName="Password" ) ]
        [DisplayName("登录密码（实际应用应存储哈希值）")]
        public string Password  { get; set;  } = null!;
     
        /// <summary>
        /// 性别: 0-未知, 1-男, 2-女
        ///</summary>
        [SugarColumn(ColumnName="Gender" ) ]
        [DisplayName("性别: 0-未知, 1-男, 2-女")]
        public int? Gender  { get; set;  } 
     
        /// <summary>
        /// 年龄
        ///</summary>
        [SugarColumn(ColumnName="Age" ) ]
        [DisplayName("年龄")]
        public int? Age  { get; set;  } 
     
        /// <summary>
        /// 手机号码
        ///</summary>
        [SugarColumn(ColumnName="Phone" ) ]
        [DisplayName("手机号码")]
        public string? Phone  { get; set;  } 
     
        /// <summary>
        /// 家庭地址
        ///</summary>
        [SugarColumn(ColumnName="Address" ) ]
        [DisplayName("家庭地址")]
        public string? Address  { get; set;  } 
     
        /// <summary>
        /// 状态: 1-在读, 2-休学, 3-毕业, 4-退学
        ///</summary>
        [SugarColumn(ColumnName="Status" ) ]
        [DisplayName("状态: 1-在读, 2-休学, 3-毕业, 4-退学")]
        public int Status  { get; set;  } 
     
        /// <summary>
        /// 备注信息
        ///</summary>
        [SugarColumn(ColumnName="Remark" ) ]
        [DisplayName("备注信息")]
        public string? Remark  { get; set;  } 
     
        /// <summary>
        /// 创建日期
        ///</summary>
        [SugarColumn(ColumnName="CreateDate" ) ]
        [DisplayName("创建日期")]
        public DateTime? CreateDate  { get; set;  } 
     
        /// <summary>
        /// 外键：关联班级GUID
        ///</summary>
        [SugarColumn(ColumnName="GradeGUID" ) ]
        [DisplayName("外键：关联班级GUID")]
        public string GradeGUID  { get; set;  } = null!;
    

    }
    
}
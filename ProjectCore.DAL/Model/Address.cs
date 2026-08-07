using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 城市地址字典表
    ///</summary>
    [SugarTable("Address")]
    public class Address
    {
     
        /// <summary>
        /// 地址ID自增主键
        ///</summary>
        [SugarColumn(ColumnName="AddressID" ,IsPrimaryKey = true,IsIdentity = true) ]
        [DisplayName("地址ID自增主键")]
        public int AddressID  { get; set;  } 
     
        /// <summary>
        /// 城市名称（唯一）
        ///</summary>
        [SugarColumn(ColumnName="AddressName" ) ]
        [DisplayName("城市名称（唯一）")]
        public string AddressName  { get; set;  } = null!;
     
        /// <summary>
        /// 城市编码
        ///</summary>
        [SugarColumn(ColumnName="CityCode" ) ]
        [DisplayName("城市编码")]
        public string? CityCode  { get; set;  } 
     
        /// <summary>
        /// 所属省份
        ///</summary>
        [SugarColumn(ColumnName="Province" ) ]
        [DisplayName("所属省份")]
        public string? Province  { get; set;  } 
     
        /// <summary>
        /// 创建人
        ///</summary>
        [SugarColumn(ColumnName="CreateUser" ) ]
        [DisplayName("创建人")]
        public string CreateUser  { get; set;  } = null!;
     
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
    

    }
    
}
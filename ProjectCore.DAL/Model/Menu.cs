using System.ComponentModel;
using SqlSugar;
using System;

namespace ProjectCore.DAL
{
    /// <summary>
    /// 系统菜单权限表
    ///</summary>
    [SugarTable("Menu")]
    public class Menu
    {
     
        /// <summary>
        /// 菜单GUID主键
        ///</summary>
        [SugarColumn(ColumnName="MenuGUID" ,IsPrimaryKey = true) ]
        [DisplayName("菜单GUID主键")]
        public string MenuGUID  { get; set;  } = null!;
     
        /// <summary>
        /// 菜单编码（唯一）
        ///</summary>
        [SugarColumn(ColumnName="MenuCode" ) ]
        [DisplayName("菜单编码（唯一）")]
        public string MenuCode  { get; set;  } = null!;
     
        /// <summary>
        /// 菜单名称（支持中文）
        ///</summary>
        [SugarColumn(ColumnName="MenuName" ) ]
        [DisplayName("菜单名称（支持中文）")]
        public string MenuName  { get; set;  } = null!;
     
        /// <summary>
        /// 父菜单GUID（NULL表示根菜单）
        ///</summary>
        [SugarColumn(ColumnName="ParentGUID" ) ]
        [DisplayName("父菜单GUID（NULL表示根菜单）")]
        public string? ParentGUID  { get; set;  } 
     
        /// <summary>
        /// 菜单层级（1.一级 2.二级 3.三级）
        ///</summary>
        [SugarColumn(ColumnName="Level" ) ]
        [DisplayName("菜单层级（1.一级 2.二级 3.三级）")]
        public byte Level  { get; set;  } 
     
        /// <summary>
        /// 路由路径（前端路由）
        ///</summary>
        [SugarColumn(ColumnName="Path" ) ]
        [DisplayName("路由路径（前端路由）")]
        public string? Path  { get; set;  } 
     
        /// <summary>
        /// 组件路径（前端组件）
        ///</summary>
        [SugarColumn(ColumnName="Component" ) ]
        [DisplayName("组件路径（前端组件）")]
        public string? Component  { get; set;  } 
     
        /// <summary>
        /// 图标名称
        ///</summary>
        [SugarColumn(ColumnName="Icon" ) ]
        [DisplayName("图标名称")]
        public string? Icon  { get; set;  } 
     
        /// <summary>
        /// 图标背景色
        ///</summary>
        [SugarColumn(ColumnName="IconBgColor" ) ]
        [DisplayName("图标背景色")]
        public string? IconBgColor  { get; set;  } 
     
        /// <summary>
        /// 标签（显示名称）
        ///</summary>
        [SugarColumn(ColumnName="Label" ) ]
        [DisplayName("标签（显示名称）")]
        public string? Label  { get; set;  } 
     
        /// <summary>
        /// 排序序号（同级排序）
        ///</summary>
        [SugarColumn(ColumnName="SortOrder" ) ]
        [DisplayName("排序序号（同级排序）")]
        public int SortOrder  { get; set;  } 
     
        /// <summary>
        /// 是否可见（0.隐藏 1.显示）
        ///</summary>
        [SugarColumn(ColumnName="IsVisible" ) ]
        [DisplayName("是否可见（0.隐藏 1.显示）")]
        public int IsVisible  { get; set;  } 
    

    }
    
}
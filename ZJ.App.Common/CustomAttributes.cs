using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJ.App.Common
{
    /// <summary>
    /// 对应数据库中表的名称
    /// </summary>
    public class TableName : Attribute
    {
        string name = string.Empty;

        public TableName(string tableName)
        {
            name = tableName;
        }

        public string Name
        {
            get { return name; }
        }
    }


    /// <summary>
    /// 导航属性
    /// </summary>
    public class Navigate : Attribute 
    {
        //private EntityBase _EntityBase;
        private string _NavigetFrom;
        private string _NavigetTo;
        private Type _EntityType;
        private Type _Handler;
        /// <summary>
        /// 导航属性构造
        /// </summary>
        /// <param name="EntityType">属性类型</param>
        /// <param name="Handler">处理导航属性代理类型</param>
        /// <param name="NavigetFrom">主实体键值信息</param>
        /// <param name="NavigetTo">使用主实体键值索引导航属性</param>
        public Navigate(Type EntityType, Type Handler, string NavigetFrom, string NavigetTo)
        {
            _Handler = Handler;
            _EntityType = EntityType;
            _NavigetFrom = NavigetFrom;
            _NavigetTo = NavigetTo;
        }

        public Type EntityType
        {
            get
            {
                return _EntityType;
            }
        }

        public Type Handler
        {
            get
            {
                return _Handler;
            }
        }

        public string NavigetFrom
        {
            get
            {
                return _NavigetFrom;
            }
        }

        public string NavigetTo
        {
            get
            {
                return _NavigetTo;
            }
        }
    }

    public class Column : Attribute
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    public class NoColumn : Attribute
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    /// <summary>
    /// 对应数据库中的主键
    /// </summary>
    public class PrimaryKey : Column
    {
    }

    public class MaxLength : Column
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public MaxLength(int Value)
        {
            this._value = Value;
        }
    }

    /// <summary>
    /// 标识此字段是自增字段
    /// </summary>
    public class Identity : Column
    { }

}

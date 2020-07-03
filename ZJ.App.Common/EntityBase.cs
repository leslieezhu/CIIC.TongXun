using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Runtime.Serialization;

namespace ZJ.App.Common
{
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class EntityBase
    {

        public EntityBase()
        {
            this._IsQueryTemplate = false;
        }

        public EntityBase(bool IsQueryTempate)
        {
            this._IsQueryTemplate = IsQueryTempate;
        }

        protected bool _IsQueryTemplate = false;

        public bool IsQueryTemplate
        {
            get
            {
                return this._IsQueryTemplate;
            }
            set {
                this._IsQueryTemplate = value;
            }
        }

        public List<SqlDbParameter> QueryCondition = new List<SqlDbParameter>();

        public void ClearQueryCondition()
        {
            QueryCondition.Clear();
        }

        /// <summary>
        /// 内部使用查询模版注册方法
        /// </summary>
        /// <param name="Key">列名</param>
        /// <param name="Value">值</param>
        protected void RegisterQueryCondition(string Key, object Value)
        {
            SqlDbParameter existedParameter = QueryCondition.FirstOrDefault(p => p.ParameterName == Key);
            if (existedParameter == null)
            {
                SqlDbParameter Parameter = new SqlDbParameter();
                Parameter.ColumnName = Key;
                Parameter.ParameterName = Key;
                Parameter.ParameterValue = Value;
                Parameter.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                QueryCondition.Add(Parameter);
            }
            else
            {
                existedParameter.ParameterValue = Value;
            }
        }

        /// <summary>
        /// 如果当前实体为查询模版,则使用该方法注册查询模版属性
        /// </summary>
        /// <param name="ParameterName">参数名称</param>
        /// <param name="ColumnName">列名</param>
        /// <param name="Value">参数值</param>
        /// <param name="QualificationSymbol">列和值的谓词关系</param>
        public void RegisterQueryCondition(string ParameterName, string ColumnName, object Value, ZJ.App.Common.SqlDbParameter.QualificationSymbol QualificationSymbol)
        {
                SqlDbParameter Parameter = new SqlDbParameter();
                Parameter.ColumnName = ColumnName;
                Parameter.ParameterName = ParameterName;
                Parameter.ParameterValue = Value;
                Parameter.QualificationType = QualificationSymbol;
                QueryCondition.Add(Parameter);
        }

      
        /// <summary>
        /// 拷贝实体
        /// </summary>
        /// <returns></returns>
        public object Copy()
        {
            object objEntity = Activator.CreateInstance(this.GetType());
            Type objType = this.GetType();

            PropertyInfo[] properties = objType.GetProperties();


            foreach (PropertyInfo property in properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(Column), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (pkAtts[0] is PrimaryKey) continue;

                    object value = property.GetValue(this, null);
                    property.SetValue(objEntity, value, null);
                }
            }

            return objEntity;
        }

        /// <summary>
        /// 拷贝实体
        /// </summary>
        /// <returns></returns>
        public object CopyFull()
        {
            object objEntity = Activator.CreateInstance(this.GetType());
            Type objType = this.GetType();

            PropertyInfo[] properties = objType.GetProperties();


            foreach (PropertyInfo property in properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(Column), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    //if (pkAtts[0] is PrimaryKey) continue;

                    object value = property.GetValue(this, null);
                    property.SetValue(objEntity, value, null);
                }
            }

            return objEntity;
        }

    }
}

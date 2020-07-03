using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ZJ.App.Common
{
    public interface IDalHandler
    {
        object LoadNavigateProperty(object entity, bool Enumerable);
        void SaveNavigateProperty(object entity, bool IsUpdate);
        void SetWithNolock(bool Enabled);
    }

    public class DalUtil<T> : IDalHandler where T : EntityBase
    {
        private DalBase<T> Handler = new DalBase<T>();
        private bool WithNoClockEnabled = false;

        public object LoadNavigateProperty(object entity, bool Enumerable)
        {
            Handler.WithNoClockEnabled = WithNoClockEnabled;

            T tempEntity = entity as T;
            if (Enumerable)
            {
                return Handler.GetFromQueryTemplate(tempEntity);
            }
            else
            {
                List<T> retVal = Handler.GetFromQueryTemplate(tempEntity);
                if (retVal.Count > 0)
                {
                    return retVal[0];
                }
                else
                {
                    return null;
                }
            }
        }


        public void SaveNavigateProperty(object entity, bool IsUpdate)
        {
            T tempEntity = entity as T;
            if (IsUpdate)
            {
                Handler.Update(tempEntity);
            }
            else
            {
                Handler.Insert(tempEntity);
            }
        }


        public void SetWithNolock(bool Enabled)
        {
            WithNoClockEnabled = Enabled;
        }
    }

    public class DalBase<T> : DatabaseBase where T : EntityBase
    {
        #region Field
        private DbTransaction _tran;

        public DbTransaction Tran
        {
            get { return _tran; }
        }

        public bool WithNoClockEnabled = false;

        #endregion

        #region Construct

        public DalBase()
        {



        }

        public DalBase(string dbName)
            : base(dbName)
        {
        }

        public DalBase(DbTransaction tran)
        {

            _tran = tran;
        }

        #endregion

        #region public method(Insert Update Delete)
        /// <summary>
        /// 根据实体创建数据库记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Insert(T entity)
        {
            Type type = typeof(T);
            string tbName = type.Name;
            object[] tbNameAtts = type.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }

            string sqlStr = "INSERT INTO {0} ({1}) VALUES ({2})";
            StringBuilder sbFieldNames = new StringBuilder();
            StringBuilder sbFieldValues = new StringBuilder();

            PropertyInfo[] Properties = type.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                //Set SQL when the Primary Key is identity
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        sqlStr += "; set @" + property.Name + " = scope_identity();";
                        continue;
                    }
                }

                sbFieldNames.Append("[").Append(property.Name).Append("]").Append(",");
                sbFieldValues.Append("@").Append(property.Name).Append(",");
            }

            string fieldNames = sbFieldNames.ToString();
            string fieldValues = sbFieldValues.ToString();
            if (fieldNames != "") fieldNames = fieldNames.Trim(',');
            if (fieldValues != "") fieldValues = fieldValues.Trim(',');

            sqlStr = string.Format(sqlStr, tbName, fieldNames, fieldValues);

            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);

            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                //Set OutPut parameter when the Primary Key is identity
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        CurrentDatabase.AddOutParameter(dbCommand, "@" + property.Name, DbType.Int32, 4);
                        continue;
                    }
                }

                Type columnType = property.PropertyType;
                // We need to check whether the property is NULLABLE
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    columnType = property.PropertyType.GetGenericArguments()[0];
                }

                CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(columnType), property.GetValue(entity, null));
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);

            //当主键为整型自增时，返回值
            foreach (PropertyInfo property in Properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        object keyid = CurrentDatabase.GetParameterValue(dbCommand, "@" + property.Name);
                        property.SetValue(entity, keyid, null);
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// 根据实体主键更新实体
        /// </summary>
        /// <param name="entity">实体主键</param>
        public void Update(T entity)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;

            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);
            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }

            string sqlStr = "UPDATE {0} SET {1} WHERE {2}";
            StringBuilder sbFieldWhere = new StringBuilder();
            StringBuilder sbFieldSet = new StringBuilder();
            PropertyInfo[] Properties = objType.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    sbFieldWhere.Append(property.Name).Append("=@").Append(property.Name).Append(" And ");
                    continue;
                }
                sbFieldSet.Append("[").Append(property.Name).Append("]").Append("=@").Append(property.Name).Append(",");
            }
            string fieldSets = sbFieldSet.ToString();
            string fieldWheres = sbFieldWhere.ToString();

            if (fieldWheres != "") fieldWheres = fieldWheres.Substring(0, fieldWheres.Length - 4);
            if (fieldSets != "") fieldSets = fieldSets.Trim(',');

            sqlStr = string.Format(sqlStr, tbName, fieldSets, fieldWheres);
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                Type columnType = property.PropertyType;
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = property.PropertyType.GetGenericArguments()[0];
                }
                CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(columnType), property.GetValue(entity, null));
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
        }

        /// <summary>
        /// 根据实体对象删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Delete(T entity)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;

            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);
            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }

            string sqlStr = "Delete from {0} Where {1}";
            string fieldWheres = "";
            PropertyInfo[] Properties = objType.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    fieldWheres += property.Name + "=@" + property.Name + " And ";
                }
            }

            if (fieldWheres != "") fieldWheres = fieldWheres.Substring(0, fieldWheres.Length - 4);

            sqlStr = string.Format(sqlStr, tbName, fieldWheres);
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(property.PropertyType), property.GetValue(entity, null));
                }
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
        }

        public void Delete(List<SqlDbParameter> parameters)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }

            StringBuilder sqlStr = new StringBuilder("DELETE FROM ");
            string sqlFormat = sqlStr.Append(tbName).Append(" WHERE 1=1  {1}").ToString();

            string sql = SqlDbParameter.BuildSqlString(sqlFormat, parameters);
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(sql);
            foreach (SqlDbParameter parameter in parameters)
            {
                parameter.SetDbCommond(dbCommand);
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
        }

        /// <summary>
        /// 根据主键逻辑删除实体
        /// </summary>
        /// <param name="primaryID">实体主键</param>
        public void Disabled(object primaryID)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;

            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);
            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }
            string sqlStr = "UPDATE  {0} SET Enabled=0 WHERE {1}";
            string fieldWheres = "";
            PropertyInfo[] Properties = objType.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    fieldWheres += property.Name + "=@" + property.Name + " And ";
                }
            }

            if (fieldWheres != "") fieldWheres = fieldWheres.Substring(0, fieldWheres.Length - 4);

            sqlStr = string.Format(sqlStr, tbName, fieldWheres);
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(property.PropertyType), primaryID);
                }
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
        }

        /// <summary>
        /// 根据实体对象修改或插入记录
        /// </summary>
        /// <param name="entity"></param>
        public void InsertOrUpdate(T entity)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);
            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
            }

            string sqlStr = @"IF NOT EXISTS(SELECT * FROM {0} WHERE {2}) 
BEGIN
INSERT INTO {0} ({3}) VALUES ({4});
{5}
END
ELSE
BEGIN
UPDATE {0} SET {1} WHERE {2} 
END
";
            string fieldSets = "";
            string fieldWheres = "";
            string fieldNames = "";
            string fieldValues = "";
            string fieldInsertInfo = "";

            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        fieldInsertInfo += "; set @" + property.Name + " = scope_identity();";
                    }
                    fieldWheres += property.Name + "=@" + property.Name + " And ";
                }
                else
                {
                    fieldSets += "[" + property.Name + "]=@" + property.Name + ",";
                    fieldNames += "[" + property.Name + "],";
                    fieldValues += "@" + property.Name + ",";
                }
            }

            if (fieldNames != "") fieldNames = fieldNames.Trim(',');
            if (fieldValues != "") fieldValues = fieldValues.Trim(',');
            if (fieldWheres != "") fieldWheres = fieldWheres.Substring(0, fieldWheres.Length - 4);
            if (fieldSets != "") fieldSets = fieldSets.Trim(',');

            sqlStr = string.Format(sqlStr, tbName, fieldSets, fieldWheres, fieldNames, fieldValues, fieldInsertInfo);

            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);

            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                //Set OutPut parameter when the Primary Key is identity
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        object objValue = property.GetValue(entity, null);
                        int intValue = 0;
                        int.TryParse(objValue.ToString(), out intValue);
                        if (intValue == 0)
                        {
                            CurrentDatabase.AddOutParameter(dbCommand, "@" + property.Name, DbType.Int32, 4);
                            continue;
                        }
                    }
                }

                Type columnType = property.PropertyType;
                // We need to check whether the property is NULLABLE
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    columnType = property.PropertyType.GetGenericArguments()[0];
                }

                CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(columnType), property.GetValue(entity, null));
            }

            if (_tran == null)
                CurrentDatabase.ExecuteNonQuery(dbCommand);
            else
                CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);

            //当主键为整型自增时，返回值
            foreach (PropertyInfo property in properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        object keyid = CurrentDatabase.GetParameterValue(dbCommand, "@" + property.Name);
                        property.SetValue(entity, keyid, null);
                        break;
                    }
                }
            }
        }

        #endregion

        #region public method(Select)

        /// <summary> 根据主键获得实体对象
        /// </summary>
        /// <param name="primaryID">实体主键</param>
        /// <returns></returns>
        public T Get(object primaryID)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
                if (this.WithNoClockEnabled)
                {
                    tbName += " with(nolock) ";
                }
            }
            string sqlStr = "SELECT TOP (1) {1} FROM {0} WHERE {2}";
            string fieldWheres = "";
            string selectFields = "";

            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }
                selectFields += "[" + property.Name + "],";
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    fieldWheres += property.Name + "=@" + property.Name + " And ";
                }
            }

            if (selectFields != "") selectFields = selectFields.Trim(',');
            if (fieldWheres != "") fieldWheres = fieldWheres.Substring(0, fieldWheres.Length - 4);

            sqlStr = string.Format(sqlStr, tbName, selectFields, fieldWheres);
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sqlStr);
            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }
                object[] pkAtts = property.GetCustomAttributes(typeof(PrimaryKey), false);

                if (pkAtts != null && pkAtts.Length > 0)
                {
                    CurrentDatabase.AddInParameter(dbCommand, "@" + property.Name, CovertToDBType(property.PropertyType), primaryID);
                }
            }

            if (_tran == null)
            {
                using (IDataReader theReader = CurrentDatabase.ExecuteReader(dbCommand))
                {
                    return GetFromReader<T>(theReader);
                }
            }
            else
            {
                using (IDataReader theReader = CurrentDatabase.ExecuteReader(dbCommand, _tran))
                {
                    return GetFromReader<T>(theReader);
                }
            }
        }

        /// <summary>
        /// 根据查询参数，的匹配参数的第一条对象
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public T Get(List<SqlDbParameter> parameters)
        {
            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
                if (this.WithNoClockEnabled)
                {
                    tbName += " with(nolock) ";
                }
            }

            StringBuilder sbAllField = new StringBuilder();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                sbAllField.Append("[").Append(property.Name).Append("],");
            }

            StringBuilder sqlStr = new StringBuilder("SELECT TOP (1) ");
            sqlStr.Append(sbAllField.ToString().TrimEnd(',')).Append(" FROM ");

            string sqlFormat = sqlStr.Append(tbName).Append(" WHERE 1=1 {1}").ToString();
            string sql = SqlDbParameter.BuildSqlString(sqlFormat, parameters);

            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(sql);
            foreach (SqlDbParameter parameter in parameters)
            {
                parameter.SetDbCommond(dbCommand);
            }
            if (_tran == null)
            {
                using (IDataReader theReader = this.CurrentDatabase.ExecuteReader(dbCommand))
                {
                    return GetFromReader<T>(theReader);
                }
            }
            else
            {
                using (IDataReader theReader = this.CurrentDatabase.ExecuteReader(dbCommand, _tran))
                {
                    return GetFromReader<T>(theReader);
                }
            }
        }

        /// <summary>
        /// 根据查询实体，获得符合结果的集合
        /// </summary>
        /// <param name="QueryEntity">查询实体</param>
        /// <returns></returns>
        public List<T> GetFromQueryTemplate(T QueryEntity)
        {
            EntityBase entityBase = QueryEntity as EntityBase;
            if (entityBase.QueryCondition.Count == 0)
            {
                return new List<T>();
            }
            return GetAll(entityBase.QueryCondition);
        }

        /// <summary>
        /// 根据参数列表，获得符合查询结果的实体集合
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public List<T> GetAll(List<SqlDbParameter> parameters)
        {
            return GetAll(parameters, string.Empty);
        }

        /// <summary>
        /// 根据参数获得查询类别，带排序
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="OrderBy">排序字段名称</param>
        /// <returns></returns>
        public List<T> GetAll(List<SqlDbParameter> parameters, string OrderBy)
        {
            List<T> objList = new List<T>();

            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
                if (this.WithNoClockEnabled)
                {
                    tbName += " with(nolock) ";
                }
            }

            StringBuilder sbAllField = new StringBuilder();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                sbAllField.Append("[").Append(property.Name).Append("]").Append(",");
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT  {0} ").Append(sbAllField.ToString().TrimEnd(',')).Append(" FROM ").Append(tbName).Append(" WHERE 1=1 {1} ");
            if (!string.IsNullOrEmpty(OrderBy))
            {
                sbSql.Append(" ORDER BY ").Append(OrderBy);
            }
            string sql = SqlDbParameter.BuildSqlString(sbSql.ToString(), parameters);
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(sql);
            foreach (SqlDbParameter parameter in parameters)
            {
                parameter.SetDbCommond(dbCommand);
            }
            if (_tran == null)
            {
                using (IDataReader theReader = this.CurrentDatabase.ExecuteReader(dbCommand))
                {
                    return GetListFromReader<T>(theReader);
                }
            }
            else
            {
                using (IDataReader theReader = this.CurrentDatabase.ExecuteReader(dbCommand, _tran))
                {
                    return GetListFromReader<T>(theReader);
                }
            }
        }

        /// <summary>
        /// 根据参数获得分页实体集合
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="PageSize">每页包含记录数量</param>
        /// <param name="PageIndex">获取指定的页</param>
        /// <param name="RecordCount">总数</param>
        /// <returns></returns>
        public List<T> GetAll(List<SqlDbParameter> parameters, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {
            List<T> objList = new List<T>();

            Type objType = typeof(T);
            string tbName = objType.Name;
            object[] tbNameAtts = objType.GetCustomAttributes(typeof(TableName), false);

            if (tbNameAtts != null && tbNameAtts.Length > 0)
            {
                TableName tableName = (TableName)tbNameAtts[0];
                tbName = tableName.Name;
                if (this.WithNoClockEnabled)
                {
                    tbName += " with(nolock) ";
                }
            }

            string KeyName = "";
            StringBuilder sbAllColumn = new StringBuilder();
            PropertyInfo[] Properties = objType.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                if (colAttr == null || colAttr.Length == 0)
                {
                    continue;
                }

                object[] pkAttr = property.GetCustomAttributes(typeof(PrimaryKey), false);
                if (pkAttr != null && pkAttr.Length > 0)
                {
                    KeyName = property.Name;
                }

                if (sbAllColumn.Length != 0)
                {
                    sbAllColumn.Append(",");
                }
                sbAllColumn.Append("[").Append(property.Name).Append("]");
            }

            if (string.IsNullOrEmpty(OrderBy))
            {
                OrderBy = KeyName;
            }

            StringBuilder sqlStr = new StringBuilder("SELECT {0} ").Append(sbAllColumn.ToString()).Append(" FROM ");
            sqlStr.Append(tbName);
            sqlStr.Append(" WHERE 1=1 {1}");
            string sql = SqlDbParameter.BuildSqlString(sqlStr.ToString(), parameters);

            string CmdText = @"WITH T AS
(
SELECT ROW_NUMBER() OVER(ORDER BY {2} ) AS row_number, * 
  FROM ({0}) as A
)
SELECT * FROM T WHERE row_number > @StartRowNum AND  row_number <= @EndRowNum
SELECT COUNT(1) FROM ({1}) AS B 
";

            CmdText = string.Format(CmdText, sql.ToString(), sql.ToString(), OrderBy);

            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(CmdText);
            CurrentDatabase.AddInParameter(dbCommand, "@StartRowNum", System.Data.DbType.Int32, (PageIndex) * PageSize);
            CurrentDatabase.AddInParameter(dbCommand, "@EndRowNum", System.Data.DbType.Int32, (PageIndex + 1) * PageSize);

            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            IDataReader reader = null;
            if (_tran == null)
            {
                reader = CurrentDatabase.ExecuteReader(dbCommand);
            }
            else
            {
                reader = CurrentDatabase.ExecuteReader(dbCommand, _tran);
            }

            using (reader)
            {
                objList = GetListFromReader<T>(reader);
                if (reader.NextResult() && reader.Read())
                    RecordCount = reader.GetInt32(0);
                else
                    RecordCount = 0;
            }
            return objList;
        }

        /// <summary>
        /// 根据参数获得分页集合
        /// </summary>
        /// <param name="SqlText">SQL查询文本,不需要包含分页逻辑</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="PageSize">每页包含记录数量</param>
        /// <param name="PageIndex">获取指定的页</param>
        /// <param name="RecordCount">总数</param>
        /// <returns>DataTable</returns>
        public DataTable GetAll(string SqlText, List<SqlDbParameter> parameters, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {

            string sql = @"SELECT M.* from ({0}) M ";

            sql = string.Format(sql, SqlText);

            string CmdText = @"WITH T AS
                                (
                                SELECT ROW_NUMBER() OVER(ORDER BY {2} ) AS row_number, * 
                                  FROM ({0}) as A
                                )
                                SELECT * FROM T WHERE row_number > @StartRowNum AND  row_number <= @EndRowNum
                                SELECT COUNT(1) FROM ({1}) AS B 
                                ";

            CmdText = string.Format(CmdText, sql.ToString(), sql.ToString(), OrderBy);

            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(CmdText);
            CurrentDatabase.AddInParameter(dbCommand, "@StartRowNum", System.Data.DbType.Int32, (PageIndex) * PageSize);
            CurrentDatabase.AddInParameter(dbCommand, "@EndRowNum", System.Data.DbType.Int32, (PageIndex + 1) * PageSize);

            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            using (DataSet dsCount = CurrentDatabase.ExecuteDataSet(dbCommand))
            {
                RecordCount = Convert.ToInt32(dsCount.Tables[1].Rows[0][0].ToString());
                DataTable retTemp = dsCount.Tables[0];
                dsCount.Tables.Remove(retTemp);
                return retTemp;
            }
        }

        /// <summary>
        /// 根据参数获得分页集合
        /// </summary>
        /// <param name="SqlText">SQL查询文本,不需要包含分页逻辑</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="PageSize">每页包含记录数量</param>
        /// <param name="PageIndex">获取指定的页</param>
        /// <param name="RecordCount">总数</param>
        /// <returns>范型对象列表</returns>
        public List<T> GetAllExten(string SqlText, List<SqlDbParameter> parameters, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {

            string sql = @"SELECT M.* from ({0}) M ";

            sql = string.Format(sql, SqlText);

            string CmdText = @"WITH T AS
                                (
                                SELECT ROW_NUMBER() OVER(ORDER BY {2} ) AS row_number, * 
                                  FROM ({0}) as A
                                )
                                SELECT * FROM T WHERE row_number > @StartRowNum AND  row_number <= @EndRowNum
                                SELECT COUNT(1) FROM ({1}) AS B 
                                ";

            CmdText = string.Format(CmdText, sql.ToString(), sql.ToString(), OrderBy);

            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(CmdText);
            CurrentDatabase.AddInParameter(dbCommand, "@StartRowNum", System.Data.DbType.Int32, (PageIndex) * PageSize);
            CurrentDatabase.AddInParameter(dbCommand, "@EndRowNum", System.Data.DbType.Int32, (PageIndex + 1) * PageSize);

            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            List<T> objList = new List<T>();
            using (DataSet dsCount = CurrentDatabase.ExecuteDataSet(dbCommand))
            {
                RecordCount = Convert.ToInt32(dsCount.Tables[1].Rows[0][0].ToString());
                objList = GetListFromDataTable<T>(dsCount.Tables[0]);
            }
            return objList;
        }

        /// <summary>
        /// 根据存储过程名称，获得实体集合
        /// </summary>
        /// <param name="ProcName">存储过程名程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public List<T> GetAll(string ProcName, List<SqlParameter> parameters)
        {
            List<T> objList = new List<T>();
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            dbCommand.Parameters.AddRange(parameters.ToArray());
            IDataReader reader = null;
            using (reader = CurrentDatabase.ExecuteReader(dbCommand))
            {
                objList = GetListFromReader<T>(reader);
            }
            return objList;
        }
        /// <summary>
        /// 根据存储过程名称，获得实体集合
        /// </summary>
        /// <param name="ProcName">存储过程名程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public List<T> GetAll(string ProcName, List<SqlDbParameter> parameters)
        {
            List<T> objList = new List<T>();
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }
            IDataReader reader = null;
            if (_tran == null)
            {
                reader = CurrentDatabase.ExecuteReader(dbCommand);
            }
            else
            {
                reader = CurrentDatabase.ExecuteReader(dbCommand, _tran);
            }
            using (reader)
            {
                objList = GetListFromReader<T>(reader);
            }
            return objList;
        }
        /// <summary>
        /// 根据存储过程名称，获得DataTable
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataTable GetProcData(string ProcName, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            dbCommand.Parameters.AddRange(parameters.ToArray());
            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }
            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    DataTable retTemp = dataSet.Tables[0];
                    dataSet.Tables.Remove(retTemp);
                    return retTemp;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据存储过程名称，获得DataTable
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataTable GetProcData(string ProcName, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    DataTable retTemp = dataSet.Tables[0];
                    dataSet.Tables.Remove(retTemp);
                    return retTemp;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据SQL文本，获得DataTable
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable</returns>
        public DataTable GetSqlData(string SqlText, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    DataTable retTemp = dataSet.Tables[0];
                    dataSet.Tables.Remove(retTemp);
                    return retTemp;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据存储过程名称，获得DataTable
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable</returns>
        public DataTable GetSqlData(string SqlText, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    DataTable retTemp = dataSet.Tables[0];
                    dataSet.Tables.Remove(retTemp);
                    return retTemp;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据SQL文本，获得范型对象集合
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>范型对象列表</returns>
        public List<T> GetSqlDataExten(string SqlText, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            dbCommand.Parameters.AddRange(parameters.ToArray());
            List<T> objList = new List<T>();

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    return objList = GetListFromDataTable<T>(dataSet.Tables[0]);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据存储过程名称，获得范型对象集合
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>范型对象列表</returns>
        public List<T> GetSqlDataExten(string SqlText, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }
            List<T> objList = new List<T>();

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            using (dataSet)
            {
                if (dataSet.Tables.Count > 0)
                {
                    return objList = GetListFromDataTable<T>(dataSet.Tables[0]);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据存储过程名称，获得DataSet
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataSet GetProcDataSet(string ProcName, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 根据存储过程名称，获得DataSet
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataSet GetProcDataSet(string ProcName, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            return dataSet;
        }
        /// <summary>
        /// 根据存储过程名称，获得DataSet
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataSet GetSqlDataSet(string SqlText, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            return dataSet;
        }

        /// <summary>
        /// 根据存储过程名称，获得DataSet
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataSet GetSqlDataSet(string SqlText, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            DataSet dataSet = null;
            if (_tran == null)
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand);
            }
            else
            {
                dataSet = CurrentDatabase.ExecuteDataSet(dbCommand, _tran);
            }

            return dataSet;
        }

        /// <summary>
        /// 根据存储过程名称，获得标量值
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public object GetProcScalarData(string ProcName, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteScalar(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteScalar(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 根据存储过程名称，获得标量值
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public object GetProcScalarData(string ProcName, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteScalar(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteScalar(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 根据存储过程名称，获得标量值
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public object GetSqlScalarData(string SqlText, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteScalar(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteScalar(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 根据存储过程名称，获得标量值
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public object GetSqlScalarData(string SqlText, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteScalar(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteScalar(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public int ExeProcNonQuery(string ProcName, List<SqlParameter> parameters)
        {
            List<T> objList = new List<T>();
            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public int ExeProcNonQuery(string ProcName, List<SqlDbParameter> parameters)
        {

            DbCommand dbCommand = this.CurrentDatabase.GetStoredProcCommand(ProcName);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public int ExeSqlNonQuery(string SqlText, List<SqlParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            dbCommand.Parameters.AddRange(parameters.ToArray());

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public int ExeSqlNonQuery(string SqlText, List<SqlDbParameter> parameters)
        {
            DbCommand dbCommand = this.CurrentDatabase.GetSqlStringCommand(SqlText);
            if (parameters != null)
            {
                foreach (SqlDbParameter parameter in parameters)
                {
                    parameter.SetDbCommond(dbCommand);
                }
            }

            if (_tran == null)
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand);
            }
            else
            {
                return CurrentDatabase.ExecuteNonQuery(dbCommand, _tran);
            }
        }

        #endregion

        #region protected method
        /// <summary>
        /// convert to BType from System.Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected DbType CovertToDBType(System.Type type)
        {
            String dbTypeName = String.Empty;
            if (type.FullName.Contains("ZJ.App.Common.Enumerator"))
            {
                dbTypeName = "Int32";
            }
            else
            {
                int keyCount = DBTypeConversionKey.GetLength(0);
                for (int i = 0; i < keyCount; i++)
                {
                    if (DBTypeConversionKey[i, 1].Equals(type.FullName))
                    {
                        dbTypeName = DBTypeConversionKey[i, 0];
                        break;
                    }

                }
            }
            if (dbTypeName == String.Empty)
            {
                dbTypeName = "String";
            }
            //dbTypeName = "Variant";
            return (DbType)Enum.Parse(typeof(DbType), dbTypeName);

        }

        protected T GetFromReader<T>(IDataReader theReader)
        {
            Type objType = typeof(T);
            object objEntity = Activator.CreateInstance(objType);
            PropertyInfo[] properties = objType.GetProperties();
            if (theReader.Read())
            {
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object[] colAttr = propertyInfo.GetCustomAttributes(typeof(Column), false);
                    if (colAttr == null || colAttr.Length == 0)
                    {
                        continue;
                    }
                    if (theReader[propertyInfo.Name] == null || theReader[propertyInfo.Name] == DBNull.Value) continue;

                    propertyInfo.SetValue(objEntity, theReader[propertyInfo.Name], null);
                }
            }
            else return default(T);
            return (T)objEntity;
        }

        protected T GetFromDataTable<T>(DataTable dataTable)
        {
            Type objType = typeof(T);
            object objEntity = Activator.CreateInstance(objType);
            PropertyInfo[] properties = objType.GetProperties();
            foreach (DataRow theReader in dataTable.Rows)
            {
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object[] colAttr = propertyInfo.GetCustomAttributes(typeof(Column), false);
                    if (colAttr == null || colAttr.Length == 0)
                    {
                        continue;
                    }
                    if (theReader[propertyInfo.Name] == null || theReader[propertyInfo.Name] == DBNull.Value) continue;

                    propertyInfo.SetValue(objEntity, theReader[propertyInfo.Name], null);
                }
                break;
            }
            return (T)objEntity;
        }


        protected List<T> GetListFromReader<T>(IDataReader theReader)
        {
            List<T> objList = new List<T>();
            object objEntity = null;

            Type objType = typeof(T);
            PropertyInfo[] properties = objType.GetProperties();

            while (theReader.Read())
            {
                objEntity = Activator.CreateInstance(objType);

                foreach (PropertyInfo property in properties)
                {
                    object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                    if (colAttr == null || colAttr.Length == 0)
                    {
                        continue;
                    }
                    if (theReader[property.Name] == null || theReader[property.Name] == DBNull.Value) continue;

                    property.SetValue(objEntity, theReader[property.Name], null);
                }

                objList.Add((T)objEntity);
            }


            return objList;
        }

        protected List<T> GetListFromDataTable<T>(DataTable dataTable)
        {
            List<T> objList = new List<T>();
            object objEntity = null;

            Type objType = typeof(T);
            PropertyInfo[] properties = objType.GetProperties();

            foreach (DataRow theReader in dataTable.Rows)
            {
                objEntity = Activator.CreateInstance(objType);

                foreach (PropertyInfo property in properties)
                {
                    object[] colAttr = property.GetCustomAttributes(typeof(Column), false);
                    if (colAttr == null || colAttr.Length == 0)
                    {
                        continue;
                    }
                    if (theReader[property.Name] == null || theReader[property.Name] == DBNull.Value) continue;

                    property.SetValue(objEntity, theReader[property.Name], null);
                }

                objList.Add((T)objEntity);
            }


            return objList;
        }
        #endregion

        #region private method
        /// <summary>
        ///  the mapping of DbType and System.Type
        /// </summary>
        private static String[,] DBTypeConversionKey = new String[,] 
            {
             {"Binary", "System.Byte[]"},
             {"Byte", "System.Byte"},
             {"Boolean", "System.Boolean"},
             {"DateTime", "System.DateTime"},
             {"Decimal", "System.Decimal"},
             {"Double", "System.Double"},
             {"Guid", "System.Guid"},
             {"Int16", "System.Int16"},
             {"Int32", "System.Int32"},
             {"Int64", "System.Int64"},
             {"Object", "System.Object"},
             {"SByte", "System.SByte"},
             {"Single", "System.Single"},
             {"String", "System.String"},
             {"UInt16", "System.UInt16"},
             {"UInt32", "System.UInt32"},
             {"UInt64", "System.UInt64"},
             {"Xml", "System.Xml"}
            };

        #endregion

        #region [导航属性]
        /// <summary>读取当前对象的导航属性
        /// </summary>
        public void LoadNavigateProperty(T Entity)
        {
            PropertyInfo[] properties = Entity.GetType().GetProperties();
            //get navigate property
            foreach (PropertyInfo property in properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(Navigate), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    object PropertyObj = Activator.CreateInstance(property.PropertyType) as IEnumerable;

                    Navigate Navigate = pkAtts[0] as Navigate;
                    object NavigetFromVal = GetValProperty(Entity, Navigate.NavigetFrom);

                    object objEntity = Activator.CreateInstance(Navigate.EntityType);
                    IDalHandler Handler = Activator.CreateInstance(Navigate.Handler) as IDalHandler;
                    Handler.SetWithNolock(this.WithNoClockEnabled);
                    EntityBase baseEntity = objEntity as EntityBase;
                    baseEntity.IsQueryTemplate = true;
                    baseEntity.RegisterQueryCondition(Navigate.NavigetTo, Navigate.NavigetTo, NavigetFromVal, ZJ.App.Common.SqlDbParameter.QualificationSymbol.Equal);
                    if (PropertyObj != null)
                    {
                        property.SetValue(Entity, Handler.LoadNavigateProperty(baseEntity, true), null);
                    }
                    else
                    {
                        property.SetValue(Entity, Handler.LoadNavigateProperty(baseEntity, false), null);
                    }
                }
            }
        }

        /// <summary>获取实体主键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private object GetValProperty(object entity, string PropertyName)
        {
            Type objType = entity.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] pkAtts = property.GetCustomAttributes(typeof(Column), false);
                if (pkAtts != null && pkAtts.Length > 0)
                {
                    if (property.Name.Equals(PropertyName))
                    {
                        return property.GetValue(entity, null);
                    }
                }
            }
            return null;
        }

        #endregion
    }

    public class TransactionHelper : DatabaseBase
    {
        #region public method(Transaction)

        public void SafeDbExecute(Action<DbTransaction> action)
        {
            //using (IDbConnection Idbconn = CurrentDatabase.CreateConnection())
            //{
            //    Idbconn.Open();
            //    DbTransaction Idbtran = null;
            //    try
            //    {
            //        Idbtran = Idbconn.BeginTransaction() as DbTransaction;
            //        action(Idbtran);
            //        Idbtran.Commit();
            //    }
            //    catch
            //    {
            //        if (Idbtran != null) Idbtran.Rollback();
            //        throw;
            //    }
            //    finally
            //    {
            //        Idbconn.Close();

            //    }
            //}
            TransactionOptions TranOption = new TransactionOptions();
            TranOption.Timeout = TimeSpan.FromSeconds(200);
            TranOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (System.Transactions.TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew, TranOption))
            {
                action(null);
                tran.Complete();
            }
        }
        #endregion
    }
}

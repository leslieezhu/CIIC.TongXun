using System;
using System.Collections.Generic; 
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;

namespace ZJ.App.Common
{
    /// <summary>
    /// 参数查询类
    /// </summary>
    [Serializable]
    [KnownType(typeof(System.DBNull))]
    public class SqlDbParameter //: WhereClauseBuilder
    { 
        protected   char ParameterChar
        {
            get { return '@'; }
            set {  }
        }

        private bool _isWhereClause = true; 
        public bool IsWhereClause
        {
            get { return _isWhereClause; }
            set { _isWhereClause = value; }
        }

        private string _columnName = string.Empty;
        /// <summary>
        /// 列名
        /// </summary>
        /// <remarks>
        /// 对应SQL中的字段名
        /// </remarks> 
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private DbType _columnType = DbType.String;
        /// <summary>
        /// 列类型-默认DbType.String
        /// </summary> 
        public DbType ColumnType 
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        public string _parameterName = string.Empty;
        /// <summary>
        /// 参数名 - 默认为列名(ColumnName)
        /// </summary> 
        public string ParameterName
        {
            get
            {
                string paraName = "{0}{1}";
                if (string.IsNullOrEmpty(_parameterName))
                {
                    _parameterName = ColumnName;
                }

                return string.Format(paraName,ParameterChar,_parameterName);
            }
            set
            {
                _parameterName = value;
            }
        } 
        public object ParameterValue { get; set; }
         
        private QualificationSymbol _qualificationType = QualificationSymbol.Equal;
        /// <summary>
        /// 默认为Equal
        /// </summary> 
        public QualificationSymbol QualificationType 
        {
            get { return _qualificationType; }
            set { _qualificationType = value; }
        }
         
        public List<SqlDbParameter> SqlDbParameters { get; set; }

        public SqlDbParameter()
        {
            this.SqlDbParameters = new List<SqlDbParameter>();
        }

        public SqlDbParameter(string ColumnName, DbType type, object value)
        {
            this.ColumnName = ColumnName;
            this.ColumnType = type;
            this.ParameterValue = value;
        }

        public static string BuildSqlString(string sqlFormat, List<SqlDbParameter> parameters)
        {
            if (parameters == null) return string.Format(sqlFormat, "", "");
            StringBuilder str = new StringBuilder();

            string top = string.Empty;

            foreach (SqlDbParameter parameter in parameters)
            {
                if (!parameter.IsWhereClause) continue;
                switch (parameter.QualificationType)
                {
                    case QualificationSymbol.Equal:
                        str.Append(" and " + parameter.ColumnName + " = " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Greater:
                        str.Append(" and " + parameter.ColumnName + " > " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.GreaterAndEqual:
                        str.Append(" and " + parameter.ColumnName + " >= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Less:
                        str.Append(" and " + parameter.ColumnName + " < " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.LessAndEqual:
                        str.Append(" and " + parameter.ColumnName + " <= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.NotEqual:
                        str.Append(" and " + parameter.ColumnName + " <> " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Like:
                        str.Append(" and " + parameter.ColumnName + " like  " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.IsNull:
                        str.Append(" and " + parameter.ColumnName + " is Null");
                        break;
                    case QualificationSymbol.In:
                        str.Append(" and " + parameter.ColumnName + " in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.NotIn:
                        str.Append(" and " + parameter.ColumnName + " not in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.Top:
                        top = "top " + parameter.ParameterValue.ToString() + " ";
                        break;
                    case QualificationSymbol.Or:
                        str.Append(" and (" + BuildSqlOrString(parameter.SqlDbParameters) + ")");
                        break;
                    case QualificationSymbol.IsNotNull:
                        str.Append(" and " + parameter.ColumnName + " is not Null");
                        break;

                }
            }

            string sql = string.Format(sqlFormat, top, str);

            return sql;

        }

        public static string BuildSqlString(string sqlFormat,string SqlText, List<SqlDbParameter> parameters)
        {
            if (parameters == null) return string.Format(sqlFormat, "", "", SqlText);
            StringBuilder str = new StringBuilder();

            string top = string.Empty;

            foreach (SqlDbParameter parameter in parameters)
            {
                if (!parameter.IsWhereClause) continue;
                switch (parameter.QualificationType)
                {
                    case QualificationSymbol.Equal:
                        str.Append(" and " + parameter.ColumnName + " = " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Greater:
                        str.Append(" and " + parameter.ColumnName + " > " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.GreaterAndEqual:
                        str.Append(" and " + parameter.ColumnName + " >= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Less:
                        str.Append(" and " + parameter.ColumnName + " < " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.LessAndEqual:
                        str.Append(" and " + parameter.ColumnName + " <= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.NotEqual:
                        str.Append(" and " + parameter.ColumnName + " <> " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Like:
                        str.Append(" and " + parameter.ColumnName + " like  " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.IsNull:
                        str.Append(" and " + parameter.ColumnName + " is Null");
                        break;
                    case QualificationSymbol.In:
                        str.Append(" and " + parameter.ColumnName + " in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.NotIn:
                        str.Append(" and " + parameter.ColumnName + " not in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.Top:
                        top = "top " + parameter.ParameterValue.ToString() + " ";
                        break;
                    case QualificationSymbol.Or:
                        str.Append(" and (" + BuildSqlOrString(parameter.SqlDbParameters) + ")");
                        break;
                    case QualificationSymbol.IsNotNull:
                        str.Append(" and " + parameter.ColumnName + " is not Null");
                        break;

                }
            }

            string sql = string.Format(sqlFormat, top, str, SqlText);

            return sql;

        }

        public static string BuildSqlOrString(List<SqlDbParameter> parameters)
        {
            StringBuilder str = new StringBuilder();

            foreach (SqlDbParameter parameter in parameters)
            {
                if (str.Length > 0 && parameter.QualificationType != QualificationSymbol.Top)
                {
                    str.Append(" or ");
                }

                switch (parameter.QualificationType)
                {
                    case QualificationSymbol.Equal:
                        str.Append(parameter.ColumnName + " = " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Greater:
                        str.Append(parameter.ColumnName + " > " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.GreaterAndEqual:
                        str.Append(parameter.ColumnName + " >= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Less:
                        str.Append(parameter.ColumnName + " < " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.LessAndEqual:
                        str.Append(parameter.ColumnName + " <= " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.NotEqual:
                        str.Append(parameter.ColumnName + " <> " + parameter.ParameterName);
                        break;
                    case QualificationSymbol.Like:
                        str.Append(parameter.ColumnName + " like " + parameter.ParameterName );
                        break;
                    case QualificationSymbol.IsNull:
                        str.Append(parameter.ColumnName + " is Null");
                        break;
                    case QualificationSymbol.In:
                        str.Append(parameter.ColumnName + " in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.NotIn:
                        str.Append(parameter.ColumnName + " not in ( " + parameter.ParameterValue + ")");
                        break;
                    case QualificationSymbol.Or:
                        str.Append("(" + BuildSqlOrString(parameter.SqlDbParameters) + ")");
                        break;

                }
            }

            return str.ToString();
        }

        public void SetDbCommond(DbCommand dbCommand)
        {

            switch (this.QualificationType)
            {
                case QualificationSymbol.In:
                case QualificationSymbol.Top:
                case QualificationSymbol.NotIn:
                case QualificationSymbol.IsNull:
                case QualificationSymbol.IsNotNull:
                    break;
                case QualificationSymbol.Or:
                    foreach (SqlDbParameter item in this.SqlDbParameters)
                    {
                        DbParameter parm = dbCommand.CreateParameter();

                        parm.DbType = item.ColumnType;
                        parm.ParameterName = item.ParameterName;
                        if (item.QualificationType == QualificationSymbol.Like)
                            parm.Value = "%" + item.ParameterValue + "%";
                        else
                            parm.Value = item.ParameterValue;

                        dbCommand.Parameters.Add(parm);
                    }
                    break;
                default:
                    DbParameter parameter = dbCommand.CreateParameter();

                    parameter.DbType = this.ColumnType;
                    parameter.ParameterName = this.ParameterName;
                    if (QualificationType == QualificationSymbol.Like)
                        parameter.Value = "%" + ParameterValue + "%";
                    else
                        parameter.Value = this.ParameterValue;

                    dbCommand.Parameters.Add(parameter);
                    break;
            }
        }
         
        public enum QualificationSymbol
        { 
            Equal = 0, 
            Greater = 1, 
            GreaterAndEqual = 2, 
            Less = 3, 
            LessAndEqual = 4, 
            NotEqual = 5, 
            Like = 6, 
            In = 7, 
            Top = 8, 
            NotIn = 9, 
            IsNull = 10, 
            Or = 11, 
            IsNotNull=12
        }
    }
}

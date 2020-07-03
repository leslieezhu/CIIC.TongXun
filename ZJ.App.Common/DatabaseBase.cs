using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace ZJ.App.Common
{
    /// <summary>
    /// 数据库基类
    /// </summary>
    public abstract class DatabaseBase
    {
        private static Database _db = null;

        public DatabaseBase()
        {
            if(_db == null)
                _db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
        }

        public DatabaseBase(string dbName)
        {
            if (_db == null)
                _db = EnterpriseLibraryContainer.Current.GetInstance<Database>(dbName);
        }

        public virtual Database CurrentDatabase
        {
            get { return _db; }
        }
    }
}

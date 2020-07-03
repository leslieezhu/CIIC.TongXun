using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Reflection;
using System.Configuration;
using ZJ.App.Entity;
using ZJ.App.Common.Cache;
using ZJ.App.DAL;
using ZJ.App.Common;

namespace ZJ.App
{
    public abstract class BllBase 
    {
        #region Private Variables
        //DalBase dal = null; 
        #endregion

        #region Construced Function
        public BllBase()
        {
            //dal = new DalBase();
        }

        public BllBase(string dbName)
        {
            //dal = new DalBase(dbName);
        }
        #endregion

        #region Protected Method

        /// <summary>
        /// 根据DataRow数据构建Info信息实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityInfo">需要构建的实体</param>
        /// <param name="entityInfoData">实体信息</param>
        protected T BuildEntityFromDataRow<T>(DataRow entityInfoData)
        {
            object entityInfo = null;
            Type entityType = typeof(T);

            entityInfo = Activator.CreateInstance(entityType);

            PropertyInfo[] fields = entityType.GetProperties();

            foreach (PropertyInfo field in fields)
            {
                if (entityInfoData.Table.Columns.IndexOf(field.Name) < 0 ||
                    entityInfoData[field.Name] == null ||
                    entityInfoData[field.Name] == DBNull.Value)
                {
                    continue;
                }

                field.SetValue(entityInfo, entityInfoData[field.Name], null);
            }

            return (T)entityInfo;
        }

        protected List<T> BuildEntityListFromDataTable<T>(DataTable dt)
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(BuildEntityFromDataRow<T>(row));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        protected DataTable QuerySubDataTable(DataTable allDatas, string filterString)
        {
            DataRow[] dictionaryDatas = this.QuerySubDataRowCollection(allDatas, filterString);
            DataTable queryResult = null;

            if (dictionaryDatas != null)
            {
                queryResult = allDatas.Clone();
                foreach (DataRow dictionaryData in dictionaryDatas)
                {
                    queryResult.ImportRow(dictionaryData);
                }

                queryResult.AcceptChanges();
            }

            return queryResult;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allDatas"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        protected DataRow[] QuerySubDataRowCollection(DataTable allDatas, string filterString)
        {
            DataRow[] dictionaryDatas = null;

            if (allDatas != null)
            {
                dictionaryDatas = allDatas.Select(filterString);
            }

            return dictionaryDatas;

        }

        #endregion

        #region public method

        public void SafeDbExecute(Action<System.Data.Common.DbTransaction> action)
        {
            TransactionHelper transactionHelper = new TransactionHelper();
            transactionHelper.SafeDbExecute(action);
        }
        #endregion

        #region Cache Data
        /// <summary>
        /// 所有Menu数据
        /// </summary>
        /// <returns></returns>
        //protected List<MenuEntity> CachedMenu
        //{
        //    get
        //    {
        //        List<MenuEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_MENU_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_MENU_ALL) as List<MenuEntity>;
        //        }
        //        else
        //        {
        //            MenuDAL dal = new MenuDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            parms.Add(new SqlDbParameter("Enabled", System.Data.DbType.Boolean, true));
        //            if (ConfigHelper.ReadConfig("IsInternetPortal").Equals("true",StringComparison.OrdinalIgnoreCase))
        //            {
        //                parms.Add(new SqlDbParameter(MenuEntity.FieldName_IsInternet, System.Data.DbType.Boolean, true));
        //            }
        //            list = dal.GetAll(parms," [Order] ASC");
                     

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_MENU_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 所有TCFG_BizCenter数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<BizCenterEntity> CachedBizCenter
        //{
        //    get
        //    {
        //        List<BizCenterEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_BizCenter_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_BizCenter_ALL) as List<BizCenterEntity>;
        //        }
        //        else
        //        {
        //            BizCenterDAL dal = new BizCenterDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_BizCenter_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 所有Dictionary数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<DictionaryEntity> CachedDictionaryAll
        //{
        //    get
        //    {
        //        List<DictionaryEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_Dictionary_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_Dictionary_ALL) as List<DictionaryEntity>;
        //        }
        //        else
        //        {
        //            DictionaryDAL dal = new DictionaryDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            SqlDbParameter parm = new SqlDbParameter();
        //            parm.ColumnName = "Enabled";
        //            parm.ColumnType = System.Data.DbType.Boolean;
        //            parm.ParameterValue = true;
        //            parms.Add(parm);
        //            list = dal.GetAll(parms,DictionaryEntity.FieldName_DictOrder);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_Dictionary_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 所有Employee数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<EmployeeEntity> CachedEmployee
        //{
        //    get
        //    {
        //        List<EmployeeEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_Employee_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_Employee_ALL) as List<EmployeeEntity>;
        //        }
        //        else
        //        {
        //            EmployeeDAL dal = new EmployeeDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_Employee_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 所有FolderStruct数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<FolderStructEntity> CachedFolderStruct
        //{
        //    get
        //    {
        //        List<FolderStructEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_FolderStruct_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_FolderStruct_ALL) as List<FolderStructEntity>;
        //        }
        //        else
        //        {
        //            FolderStructDAL dal = new FolderStructDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_FolderStruct_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义所有LegalEntity数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<LegalEntityEntity> CachedLegalEntity
        //{
        //    get
        //    {
        //        List<LegalEntityEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_LegalEntity_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_LegalEntity_ALL) as List<LegalEntityEntity>;
        //        }
        //        else
        //        {
        //            LegalEntityDAL dal = new LegalEntityDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_LegalEntity_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义Organization数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<OrganizationEntity> CachedOrganization
        //{
        //    get
        //    {
        //        List<OrganizationEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_Organization_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_Organization_ALL) as List<OrganizationEntity>;
        //        }
        //        else
        //        {
        //            OrganizationDAL dal = new OrganizationDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_Organization_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义OrgSubTypeMapping数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<OrgSubTypeMappingEntity> CachedOrgSubTypeMapping
        //{
        //    get
        //    {
        //        List<OrgSubTypeMappingEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_OrgSubTypeMapping_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_OrgSubTypeMapping_ALL) as List<OrgSubTypeMappingEntity>;
        //        }
        //        else
        //        {
        //            OrgSubTypeMappingDAL dal = new OrgSubTypeMappingDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_OrgSubTypeMapping_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义PreTask数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<PreTaskEntity> CachedPreTask
        //{
        //    get
        //    {
        //        List<PreTaskEntity> list = null;
        //        //if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_PreTask_ALL))
        //        //{
        //        //    list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_PreTask_ALL) as List<PreTaskEntity>;
        //        //}
        //        //else
        //        //{
        //        //    PreTaskDAL dal = new PreTaskDAL();
        //        //    List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        //    list = dal.GetAll(parms);

        //        //    CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_PreTask_ALL, list);
        //        //}
        //        PreTaskDAL dal = new PreTaskDAL();
        //        List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        list = dal.GetAll(parms);
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义PreTaskTemplate数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<PreTaskTemplateEntity> CachedPreTaskTemplate
        //{
        //    get
        //    {
        //        List<PreTaskTemplateEntity> list = null;
        //        //if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_PreTaskTemplate_ALL))
        //        //{
        //        //    list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_PreTaskTemplate_ALL) as List<PreTaskTemplateEntity>;
        //        //}
        //        //else
        //        //{
        //        //    PreTaskTemplateDAL dal = new PreTaskTemplateDAL();
        //        //    List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        //    list = dal.GetAll(parms);

        //        //    CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_PreTaskTemplate_ALL, list);
        //        //}
        //        PreTaskTemplateDAL dal = new PreTaskTemplateDAL();
        //        List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        list = dal.GetAll(parms);
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义ProjectTemplate数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<ProjectTemplateEntity> CachedProjectTemplate
        //{
        //    get
        //    {
        //        List<ProjectTemplateEntity> list = null;
        //        //if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ProjectTemplate_ALL))
        //        //{
        //        //    list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ProjectTemplate_ALL) as List<ProjectTemplateEntity>;
        //        //}
        //        //else
        //        //{
        //        //    ProjectTemplateDAL dal = new ProjectTemplateDAL();
        //        //    List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        //    list = dal.GetAll(parms);

        //        //    CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ProjectTemplate_ALL, list);
        //        //}
        //        ProjectTemplateDAL dal = new ProjectTemplateDAL();
        //        List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        list = dal.GetAll(parms);
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义ProjectToolSet数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<ProjectToolSetEntity> CachedProjectToolSet
        //{
        //    get
        //    {
        //        List<ProjectToolSetEntity> list = null;
        //        //if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ProjectToolSet_ALL))
        //        //{
        //        //    list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ProjectToolSet_ALL) as List<ProjectToolSetEntity>;
        //        //}
        //        //else
        //        //{
        //        //    ProjectToolSetDAL dal = new ProjectToolSetDAL();
        //        //    List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        //    list = dal.GetAll(parms);

        //        //    CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ProjectToolSet_ALL, list);
        //        //}
        //        ProjectToolSetDAL dal = new ProjectToolSetDAL();
        //        List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        list = dal.GetAll(parms);
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义ServiceSubType数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<ServiceSubTypeEntity> CachedServiceSubType
        //{
        //    get
        //    {
        //        List<ServiceSubTypeEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ServiceSubType_Enabled))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ServiceSubType_Enabled) as List<ServiceSubTypeEntity>;
        //        }
        //        else
        //        {
        //            ServiceSubTypeDAL dal = new ServiceSubTypeDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            SqlDbParameter parm = new SqlDbParameter();
        //            parm.ColumnName = ServiceSubTypeEntity.FieldName_Enabled;
        //            parm.ColumnType = System.Data.DbType.Boolean;
        //            parm.ParameterValue = true;
        //            parms.Add(parm);
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ServiceSubType_Enabled, list);
        //        }

        //        return list;
        //    }
        //}

        //protected List<ServiceSubTypeEntity> CachedServiceSubTypeAll
        //{
        //    get
        //    {
        //        List<ServiceSubTypeEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ServiceSubType_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ServiceSubType_ALL) as List<ServiceSubTypeEntity>;
        //        }
        //        else
        //        {
        //            ServiceSubTypeDAL dal = new ServiceSubTypeDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>(); 
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ServiceSubType_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义ServiceTaskMapping数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<ServiceTaskMappingEntity> CachedServiceTaskMapping
        //{
        //    get
        //    {
        //        //List<ServiceTaskMappingEntity> list = null;
        //        //if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ServiceTaskMapping_ALL))
        //        //{
        //        //    list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ServiceTaskMapping_ALL) as List<ServiceTaskMappingEntity>;
        //        //}
        //        //else
        //        //{
        //        //    ServiceTaskMappingDAL dal = new ServiceTaskMappingDAL();
        //        //    List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        //    list = dal.GetAll(parms);

        //        //    CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ServiceTaskMapping_ALL, list);
        //        //}
        //        List<ServiceTaskMappingEntity> list = null;
        //        ServiceTaskMappingDAL dal = new ServiceTaskMappingDAL();
        //        List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //        list = dal.GetAll(parms);

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// 定义ServiceType数据
        ///// </summary>
        ///// <returns></returns>
        //protected List<ServiceTypeEntity> CachedServiceType
        //{
        //    get
        //    {
        //        List<ServiceTypeEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ServiceType_Enabled))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ServiceType_Enabled) as List<ServiceTypeEntity>;
        //        }
        //        else
        //        {
        //            ServiceTypeDAL dal = new ServiceTypeDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            SqlDbParameter parm = new SqlDbParameter();
        //            parm.ColumnName = ServiceTypeEntity.FieldName_Enabled;
        //            parm.ColumnType = System.Data.DbType.Boolean;
        //            parm.ParameterValue = true;
        //            parms.Add(parm);

        //            list = dal.GetAll(parms);

        //            foreach (ServiceTypeEntity entity in list)
        //            {
        //                dal.LoadNavigateProperty(entity);
        //            }

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ServiceType_Enabled, list);
        //        }

        //        return list;
        //    }
        //}

        //protected List<ServiceTypeEntity> CachedServiceTypeAll
        //{
        //    get
        //    {
        //        List<ServiceTypeEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_ServiceType_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_ServiceType_ALL) as List<ServiceTypeEntity>;
        //        }
        //        else
        //        {
        //            ServiceTypeDAL dal = new ServiceTypeDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            foreach (ServiceTypeEntity entity in list)
        //            {
        //                dal.LoadNavigateProperty(entity);
        //            }

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_ServiceType_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        //protected List<EmployeeOrgEntity> CachedEmployeeOrg
        //{
        //    get
        //    {
        //        List<EmployeeOrgEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_EmployeeOrg_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_EmployeeOrg_ALL) as List<EmployeeOrgEntity>;
        //        }
        //        else
        //        {
        //            EmployeeOrgDAL dal = new EmployeeOrgDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_EmployeeOrg_ALL, list);
        //        }

        //        return list;
        //    }
        //}

        //protected List<RoleEntity> CachedRole
        //{
        //    get
        //    {
        //        List<RoleEntity> list = null;
        //        if (CacheHelper.GetManagerInstance().ExistsCacheData(CacheKeyConst.CACHE_KEY_Role_ALL))
        //        {
        //            list = CacheHelper.GetManagerInstance().GetCacheData(CacheKeyConst.CACHE_KEY_Role_ALL) as List<RoleEntity>;
        //        }
        //        else
        //        {
        //            RoleDAL dal = new RoleDAL();
        //            List<SqlDbParameter> parms = new List<SqlDbParameter>();
        //            list = dal.GetAll(parms);

        //            CacheHelper.GetManagerInstance().AddCache(CacheKeyConst.CACHE_KEY_Role_ALL, list);
        //        }

        //        return list;
        //    }
        //}
        #endregion

    }
}


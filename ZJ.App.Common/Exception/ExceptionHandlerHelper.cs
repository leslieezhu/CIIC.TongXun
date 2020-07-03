//-------------------------------------------------------------------
//版权所有：版权所有(C) 2012，Microsoft(China) Co.,LTD
//系统名称：
//文件名称：ExceptionHandlerHelper.cs
//模块名称：
//模块编号：
//作　　者：Liu Feiting
//完成日期：
//功能说明：
//-----------------------------------------------------------------
//修改记录：
//修改人：  
//修改时间：
//修改内容：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.IO;

namespace ZJ.App.Common
{
  
    public class ExceptionHandlerHelper
    {
        #region [Exception Handler Method]
     
        public static void HandleUIException(Exception ex)
        {
            ExceptionHandlerFactory factory = new ExceptionHandlerFactory();
            ExceptionhandlerBase handler = factory.GetExceptionHandler(ExceptionHandlerConst.UI_EXCEPTION_POLICY);
            handler.HandleException(ex);
        }
       

        public static void HandleServiceException(Exception ex)
        {
            ExceptionHandlerFactory factory = new ExceptionHandlerFactory();
            ExceptionhandlerBase handler = factory.GetExceptionHandler(ExceptionHandlerConst.SERVICE_EXCEPTION_POLICY);
            handler.HandleException(ex);
        }

        public static void HandleInterfaceException(Exception ex)
        {
            ExceptionHandlerFactory factory = new ExceptionHandlerFactory();
            ExceptionhandlerBase handler = factory.GetExceptionHandler(ExceptionHandlerConst.INTERFACE_EXCEPTION_POLICY);
            handler.HandleException(ex);
        }

        #endregion

        #region [Exception Base]

        public abstract class ExceptionhandlerBase
        {
            private static void LogException(System.Exception innerEx)
            {
                using (StreamWriter writer = File.CreateText("ExceptionhandlerError.log"))
                {
                    writer.Write("-------------------------------------------" + Environment.NewLine);
                    writer.Write("time stamp:" + DateTime.Now.ToString() + Environment.NewLine);
                    writer.Write(innerEx.ToString() + Environment.NewLine);
                    writer.Close();
                }
            }

            protected virtual void HandleException(System.Exception ex, string policy)
            {
                Exception outException = null;
                bool rethrow = false;
                try
                {
                    rethrow = ExceptionPolicy.HandleException(ex, policy, out outException);
                }
                catch (System.Exception innerEx)
                {
                    LogException(innerEx);
                }
                if (rethrow)
                {
                    throw (outException != null) ? outException : ex;
                }
            }

            public abstract void HandleException(System.Exception ex);
        }

        public class ExceptionHandlerFactory
        {
            private Dictionary<string, ExceptionhandlerBase> dictExceptionHandler;

            public ExceptionHandlerFactory()
            {
                dictExceptionHandler = new Dictionary<string, ExceptionhandlerBase>();
                dictExceptionHandler.Add(ExceptionHandlerConst.SERVICE_EXCEPTION_POLICY, new ServiceExceptionHandler());
                dictExceptionHandler.Add(ExceptionHandlerConst.UI_EXCEPTION_POLICY, new UIExceptionHandler());
                dictExceptionHandler.Add(ExceptionHandlerConst.INTERFACE_EXCEPTION_POLICY,new InterfaceExceptionHandler());
            }

            public ExceptionhandlerBase GetExceptionHandler(string exceptionType)
            {
                return dictExceptionHandler[exceptionType];
            }
        }

        public class UIExceptionHandler : ExceptionhandlerBase
        {
            #region IExceptionHandler Members

            public override void HandleException(System.Exception ex)
            {
                base.HandleException(ex, ExceptionHandlerConst.UI_EXCEPTION_POLICY);
            }

            private System.Exception _lastError;
            public System.Exception LastError
            {
                get
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        _lastError = System.Web.HttpContext.Current.Items["LastError"] as System.Exception;
                    }
                    return _lastError;
                }
            }

            #endregion
        }

        public class ServiceExceptionHandler : ExceptionhandlerBase
        {
            #region ExceptionhandlerBase Members

            public override void HandleException(System.Exception ex)
            {
                base.HandleException(ex, ExceptionHandlerConst.SERVICE_EXCEPTION_POLICY);

            }

            #endregion
        }

        public class InterfaceExceptionHandler : ExceptionhandlerBase
        {
            #region ExceptionhandlerBase Members

            public override void HandleException(System.Exception ex)
            {
                base.HandleException(ex, ExceptionHandlerConst.INTERFACE_EXCEPTION_POLICY);

            }

            #endregion
        }

        #endregion
    }
}

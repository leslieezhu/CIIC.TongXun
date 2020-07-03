using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace ZJ.App.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        private string enumCNDisplayText;
        private string enumENDisplayText;
        private int enumRank;
        private FieldInfo fieldIno;

        public EnumDescription(string enText, string cnText, int enumRank)
        {
            this.enumENDisplayText = enText;
            this.enumCNDisplayText = cnText;
            this.enumRank = enumRank;
        }

        public EnumDescription(string enText, string cnText)
            : this(enText, cnText, 5)
        { }

        public EnumDescription(string text, int enumRank)
            : this(text, text, enumRank)
        { }


        public EnumDescription(string text)
            : this(text, text, 5) { }

        public string ENText
        {
            get { return this.enumENDisplayText; }
        }

        public string CNText
        {
            get { return this.enumCNDisplayText; }
        }

        public int Rank
        {
            get { return this.enumRank; }
        }

        public int EnumValue
        {
            get { return (int)fieldIno.GetValue(null); }
        }

        public string FieldName
        {
            get { return fieldIno.Name; }
        }

        #region  =========================================对枚举描述属性的解释相关函数

        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType
        {
            /// <summary>
            ///按枚举顺序默认排序
            /// </summary>
            Default,
            /// <summary>
            /// 按描述值排序
            /// </summary>
            DisplayText,
            /// <summary>
            /// 按排序
            /// </summary>
            Rank
        }

        private static System.Collections.Hashtable cachedEnum = new Hashtable();

        public static string GetEnumText(Type enumType)
        {
            EnumDescription[] eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (eds.Length != 1) return string.Empty;

            string text = string.Empty;

            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Equals("zh-CN"))
                return eds[0].CNText;
            else
                return eds[0].ENText;
        }

        public static string GetFieldText(object enumValue)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString())
                {
                    if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Equals("zh-CN"))
                        return ed.CNText;
                    else
                        return ed.ENText;
                }
            }
            return string.Empty;
        }

        public static string GetFieldText(object enumValue, bool English)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString())
                {
                    if (!English)
                        return ed.CNText;
                    else
                        return ed.ENText;
                }
            }
            return string.Empty;
        }

        /// <summary>返回enum的所有成员,每个成员是EnumDescription类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumDescription[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Default);
        }

        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            EnumDescription[] descriptions = null;

            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList edAL = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                    if (eds.Length != 1) continue;
                    ((EnumDescription)eds[0]).fieldIno = fi;
                    edAL.Add(eds[0]);
                }

                cachedEnum.Add(enumType.FullName, (EnumDescription[])edAL.ToArray(typeof(EnumDescription)));
            }
            descriptions = (EnumDescription[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0) throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");

            for (int m = 0; m < descriptions.Length; m++)
            {
                if (sortType == SortType.Default) break;

                for (int n = m; n < descriptions.Length; n++)
                {
                    EnumDescription temp;
                    bool swap = false;

                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (string.Compare(descriptions[m].ENText, descriptions[n].ENText) > 0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].Rank > descriptions[n].Rank) swap = true;
                            break;
                    }

                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        }

        public static string GetFieldTextByValue(Type enumType, string value)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.EnumValue.ToString() == value)
                {
                    if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Equals("zh-CN"))
                        return ed.CNText;
                    else
                        return ed.ENText;
                }
            }
            return string.Empty;
        }

        public static string GetFieldEnTextByValue(Type enumType, string value)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.EnumValue.ToString() == value)
                {
                    return ed.ENText;
                }
            }
            return string.Empty;
        }

        public static string GetFieldTextByValue(Type enumType, string value, bool IsChinese)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.EnumValue.ToString() == value)
                {
                    if (IsChinese==false)
                        return ed.ENText;
                    else
                        return ed.CNText;
                }
            }
            return string.Empty;
        }

        public static int? GetFieldValueByText(Type enumType, string Text)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.CNText.ToString() == Text)
                {
                    return ed.EnumValue;
                }
            }
            return null;
        }

        #endregion
    }
}

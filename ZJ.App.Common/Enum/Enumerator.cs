using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJ.App.Common
{
    /// <summary>
    /// 获取枚举描述，例如：
    /// EnumDescription.GetFieldText(TaskStatus.NotStart);
    /// 方法根据当前线程文化，自动获取中英文值
    /// </summary>
    public class Enumerator
    {
        /// <summary>
        ///  文章分类
        /// </summary>
        public enum ArticleType : int
        {
            [EnumDescription("其它", "其它")]
            Other = 0,
            [EnumDescription("集团新闻", "集团新闻")]
            JT = 1,
            [EnumDescription("公司新闻", "公司新闻")]
            GS = 2,
            [EnumDescription("业务新闻", "业务新闻")]
            YW = 3,
            [EnumDescription("聚焦国企", "聚焦国企")]
            JuJiaoGuoQi = 4
        }


        public enum RecomState : int
        {
            [EnumDescription("未推荐", "未推荐")]
            RecomState_10 = 10,
            [EnumDescription("已推荐", "已推荐")]
            RecomState_20 = 20,
            [EnumDescription("通知面试", "通知面试")]
            RecomState_30 = 30,
            [EnumDescription("已面试", "已面试")]
            RecomState_40 = 40,
            [EnumDescription("预录取", "预录取")]
            RecomState_50 = 50,
            [EnumDescription("已入职", "已入职")]
            RecomState_60 = 60,
            [EnumDescription("已收款", "已收款")]
            RecomState_70 = 70,
        }


        public enum MailSucceed : int
        {
            [EnumDescription("通知邮件", "通知邮件")]
            Succeed = 2,
            [EnumDescription("提醒邮件", "提醒邮件")]
            Failed = 3,
            [EnumDescription("通知邮件", "通知邮件")]
            Sending = 1,
        }

        #region SendState
        public enum SendState : int
        {
            /// <summary>
            /// 个人
            /// </summary>
            [EnumDescription("未发送", "未发送")]
            IsNotSend = 0,
            /// <summary>
            /// 个人
            /// </summary>
            [EnumDescription("发送中", "发送中")]
            Sending = 1,
            /// <summary>
            /// 公司
            /// </summary>
            [EnumDescription("已发送", "已发送")]
            IsSend = 2
        }
        #endregion


        /// <summary>
        /// 字典表记录软删标识
        /// </summary>
        public enum DictionaryEnabled : int
        {
            [EnumDescription("无效", "无效")]
            Disabled = 0,
            [EnumDescription("启用", "启用")]
            Enabled = 1,
        }

        public enum EnablePuzzle : int
        {
            [EnumDescription("关闭", "关闭")]
            Disabled = 0,
            [EnumDescription("开启", "开启")]
            Enabled = 1,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum AccountState : int
        {
            [EnumDescription("停用", "停用")]
            Termination = 2,
            [EnumDescription("使用", "使用")]
            Use = 1,
        }

        public enum Gender : int
        {
            [EnumDescription("女", "女")]
            Female = 2,
            [EnumDescription("男", "男")]
            Male = 1,
            [EnumDescription("不限", "不限")]
            Nolimit = -1
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        public enum AuditState : int
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [EnumDescription("未审核", "未审核")]
            NotAudit = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            [EnumDescription("已审核", "已审核")]
            Audit = 1,
            /// <summary>
            /// 拒绝
            /// </summary>
            [EnumDescription("拒绝", "拒绝")]
            Reject = 2
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        public enum ProjectState : int
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [EnumDescription("未审核", "未审核")]
            NotAudit = 0,
            /// <summary>
            /// 发布中
            /// </summary>
            [EnumDescription("发布中", "发布中")]
            Audit = 1,
            /// <summary>
            /// 进行中
            /// </summary>
            [EnumDescription("进行中", "进行中")]
            Process = 2,
            /// <summary>
            /// 已完成
            /// </summary>
            [EnumDescription("已完成", "已完成")]
            Finish = 3,
            /// <summary>
            /// 拒绝
            /// </summary>
            [EnumDescription("拒绝", "拒绝")]
            Reject = 4
        }

        //项目费用类型
        public enum FeeType : int
        {
            /// <summary>
            /// 合同相关信息
            /// </summary>
            [EnumDescription("合同相关信息", "合同相关信息")]
            Client = 0,
            /// <summary>
            /// BD付款信息
            /// </summary>
            [EnumDescription("客户拓展公司付款信息", "客户拓展公司付款信息")]
            BD = 1,
            /// <summary>
            /// 猎头公司付款信息
            /// </summary>
            [EnumDescription("猎头公司付款信息", "猎头公司付款信息")]
            Vendor = 2
        }

        //收费方式
        public enum PayType : int
        {
            /// <summary>
            /// 收费比例
            /// </summary>
            [EnumDescription("收费比例", "收费比例")]
            Ratio = 1,
            /// <summary>
            /// 固定收费
            /// </summary>
            [EnumDescription("固定收费", "固定收费")]
            Fix = 2
        }

        //收付款状态
        public enum PayStatus : int
        {
            /// <summary>
            /// 未收款
            /// </summary>
            [EnumDescription("未收款", "未收款")]
            NotGather = 1,
            /// <summary>
            /// 已通知
            /// </summary>
            [EnumDescription("已通知", "已通知")]
            Informed = 2,
            /// <summary>
            /// 已收款
            /// </summary>
            [EnumDescription("已收款", "已收款")]
            Gathered = 3,
            /// <summary>
            /// 未付款
            /// </summary>
            [EnumDescription("未付款", "未付款")]
            Unpaid = 4,
            /// <summary>
            /// 已付款
            /// </summary>
            [EnumDescription("已付款", "已付款")]
            Paid = 5
        }

        public enum CompanyType : int
        {
            /// <summary>
            ///客户拓展企业
            /// </summary>
            [EnumDescription("客户拓展企业", "客户拓展企业")]
            BD = 3,
            /// <summary>
            ///招聘企业
            /// </summary>
            [EnumDescription("招聘企业", "招聘企业")]
            Client = 5,
            /// <summary>
            /// 猎头公司
            /// </summary>
            [EnumDescription("猎头公司", "猎头公司")]
            Vendor = 2,
            /// <summary>
            ///平台
            /// </summary>
            [EnumDescription("平台", "平台")]
            Admin = 1,
            /// <summary>
            ///猎头顾问
            /// </summary>
            [EnumDescription("猎头顾问", "猎头顾问")]
            Counselor = 4,

            [EnumDescription("企业", "企业")]
            Enterprise = 6
        }

        /*职位类型*/
        public enum PositionType : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("高级职位", "高级职位")]
            Senior = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("中级职位", "中级职位")]
            Middle = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("初级职位", "初级职位")]
            Junior = 3
        }

        /*职位大类*/
        public enum ProfessionLvOne : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("计算机软件", "计算机软件")]
            Software = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("计算机硬件", "计算机硬件")]
            Hardware = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("财务", "财务")]
            Finance = 3,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("销售", "销售")]
            Sale = 4
        }


        /*职位小类*/
        public enum ProfessionLvTwo : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("软件工程师", "软件工程师")]
            Software = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("数据库管理员", "数据库管理员")]
            Database = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("硬件工程师", "硬件工程师")]
            Hardware = 3,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("其他", "其他")]
            Other = 4
        }

        /*年薪范围*/
        public enum SalaryYear : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("1-10W", "1-10W")]
            one = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("11-20W", "11-20W")]
            two = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("20-50W", "20-50W")]
            three = 3,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("50W以上", "50W以上")]
            four = 4
        }

        /*工作年限*/
        public enum WorkYear : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("应届生", "应届生")]
            one = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("1-3年", "1-3年")]
            two = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("3-5年", "3-5年")]
            three = 3,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("5-10年", "5-10年")]
            four = 4,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("10年以上", "10年以上")]
            Five = 5
        }

        ///*学历*/
        //public enum Degree : int
        //{
        //    /// <summary>
        //    /// 1-10W
        //    /// </summary>
        //    [EnumDescription("MBA", "MBA")]
        //    one = 1,
        //    /// <summary>
        //    /// 11-20W
        //    /// </summary>
        //    [EnumDescription("博士", "博士")]
        //    two = 2,
        //    /// <summary>
        //    /// 20-50W
        //    /// </summary>
        //    [EnumDescription("硕士", "硕士")]
        //    three = 3,
        //    /// <summary>
        //    /// 50W以上
        //    /// </summary>
        //    [EnumDescription("本科", "本科")]
        //    four = 4,
        //    /// <summary>
        //    /// 50W以上
        //    /// </summary>
        //    [EnumDescription("大专", "大专")]
        //    Five = 5,
        //    /// <summary>
        //    /// 50W以上
        //    /// </summary>
        //    [EnumDescription("高中", "高中")]
        //    six = 6,
        //    /// <summary>
        //    /// 50W以上
        //    /// </summary>
        //    [EnumDescription("初中", "初中")]
        //    seven = 7,
        //    /// <summary>
        //    /// 50W以上
        //    /// </summary>
        //    [EnumDescription("初中及以下", "初中及以下")]
        //    eight = 8
        //}

        /*学历*/
        public enum Degree : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("初中", "初中")]
            MiddleSchool = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("中技", "中技")]
            TechnicalSchool = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("中专", "中专")]
            SpecializedSchool = 3,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("高中", "高中")]
            HighSchool = 4,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("大专", "大专")]
            JuniorCollege = 5,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("本科", "本科")]
            Bachelor = 6,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("MBA&EMBA", "MBA&EMBA")]
            MBA = 7,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("硕士", "硕士")]
            Master = 8,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("博士", "博士")]
            Doctor = 9,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("其他", "其他")]
            Other = 10
        }
        
        /*年龄要求*/
        public enum AgeRank : int
        {
            /// <summary>
            /// 18-30岁
            /// </summary>
            [EnumDescription("18-30岁", "18-30岁")]
            one = 1,
            /// <summary>
            ///30-40岁
            /// </summary>
            [EnumDescription("30-40岁", "30-40岁")]
            two = 2,
            /// <summary>
            /// 40-50岁
            /// </summary>
            [EnumDescription("40-50岁", "40-50岁")]
            three = 3,
            /// <summary>
            /// 50-60岁
            /// </summary>
            [EnumDescription("50-60岁", "50-60岁")]
            Five = 4,
            /// <summary>
            /// 60-65岁
            /// </summary>
            [EnumDescription("60-65岁", "60-65岁")]
            six = 5
        }


        /*语言能力*/
        public enum Language : int
        {
            /// <summary>
            /// 1-10W
            /// </summary>
            [EnumDescription("英语四级", "英语四级")]
            one = 1,
            /// <summary>
            /// 11-20W
            /// </summary>
            [EnumDescription("英语六级", "英语六级")]
            two = 2,
            /// <summary>
            /// 20-50W
            /// </summary>
            [EnumDescription("英语八级", "英语八级")]
            three = 3,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("日语一级", "日语一级")]
            four = 4,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("德语四级", "德语四级")]
            Five = 5,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("英语不流利", "英语不流利")]
            six = 6,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("英语较流利", "英语较流利")]
            seven = 7,
            /// <summary>
            /// 50W以上
            /// </summary>
            [EnumDescription("英语很流利", "英语很流利")]
            eight = 8
        }

        /*项目选择添加内容类型*/
        public enum ProjectDescription : int
        {
            /// <summary>
            /// 3-5年发展规划
            /// </summary>
            [EnumDescription("3-5年发展规划", "比如:该职位的3-5年发展规划")]
            One = 1,
            /// <summary>
            /// 招聘背景
            /// </summary>
            [EnumDescription("招聘背景", "该职位上下级关系与同级别关系,招聘的具体背景")]
            Two = 2,
            /// <summary>
            /// 职位情况
            /// </summary>
            [EnumDescription("职位情况", "之前该职位上的人情况,现在的工作重心是什么?公司目前最期望达到什么目标?")]
            Three = 3,
            /// <summary>
            /// 硬件要求
            /// </summary>
            [EnumDescription("硬件要求", "学历-专业-年龄-英文-工作年限-管理年限-是否需本专业等")]
            Four = 4,
            /// <summary>
            /// 软件要求
            /// </summary>
            [EnumDescription("软件要求", "性格、潜在要求、直线老板的风格等")]
            Five = 5,
            /// <summary>
            /// 薪酬架构及细节
            /// </summary>
            [EnumDescription("薪酬架构及细节", "")]
            Six = 6,
            /// <summary>
            /// 面试流程及安排
            /// </summary>
            [EnumDescription("面试流程及安排", "")]
            Seven = 7,
            /// <summary>
            /// 公司及该职位亮点
            /// </summary>
            [EnumDescription("公司及职位亮点", "")]
            Eight = 8,
            /// <summary>
            ///其他供应商&进度与问题
            /// </summary>
            [EnumDescription("其他供应商&进度与问题", "是否已经有其它供应商开始搜寻该职位?现在的进度与问题")]
            Nine = 9,
            /// <summary>
            /// 备注
            /// </summary>
            [EnumDescription("备注", "是否有竞业条款等")]
            Ten = 10
        }

        public enum RelationType : int
        {
            /// <summary>
            /// 项目发起
            /// </summary>
            [EnumDescription("项目发起", "项目发起")]
            ProjectToCounselor = 1,
            /// <summary>
            /// 顾问发起
            /// </summary>
            [EnumDescription("顾问发起", "顾问发起")]
            CounselorToProject = 2
        }

        public enum RelationStatus : int
        {
            /// <summary>
            /// 申请中
            /// </summary>
            [EnumDescription("申请中", "申请中")]
            Applying = 1,
            /// <summary>
            /// 待承接
            /// </summary>
            [EnumDescription("待承接", "待承接")]
            WaitTake = 2,
            /// <summary>
            /// 已承接
            /// </summary>
            [EnumDescription("已承接", "已承接")]
            UnderTake = 3,
            /// <summary>
            /// 已停止
            /// </summary>
            [EnumDescription("已停止", "已停止")]
            Stop = 4,
            /// <summary>
            /// 平台拒绝
            /// </summary>
            [EnumDescription("平台拒绝", "平台拒绝")]
            PlatformRefuse = 5,
            /// <summary>
            /// 猎头拒绝
            /// </summary>
            [EnumDescription("猎头拒绝", "猎头拒绝")]
            CounselorRefuse = 6
        }

         /// <summary>
         /// UserInfo表State字段枚举值
         /// </summary>
        public enum UserInfoState : int
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [EnumDescription("未审核", "未审核")]
            NotAudit = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            [EnumDescription("已审核", "已审核")]
            Audit = 1,
            /// <summary>
            /// 拒绝
            /// </summary>
            [EnumDescription("拒绝", "拒绝")]
            Reject = 2
        }

        public enum UserInfoStateForSelect : int
        {
            /// <summary>
            /// 停用
            /// </summary>
            [EnumDescription("停用", "停用")]
            NotAudit = 0,
            /// <summary>
            /// 使用
            /// </summary>
            [EnumDescription("使用", "使用")]
            Audit = 1
        }

        //顾问审批状态
        public enum CounselorState : int
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [EnumDescription("未审核", "未审核")]
            NotAudit = 0,
            /// <summary>
            /// 已审核
            /// </summary>
            [EnumDescription("已审核", "已审核")]
            Audit = 1,
            /// <summary>
            /// 拒绝
            /// </summary>
            [EnumDescription("拒绝", "拒绝")]
            Reject = 2
        }


        public enum AttachmentType : int
        {
            [EnumDescription("客户拓展企业", "客户拓展企业")]
            BD = 1,
            /// <summary>
            ///招聘企业
            /// </summary>
            [EnumDescription("招聘企业", "招聘企业")]
            Client = 2,
            /// <summary>
            /// 猎头公司
            /// </summary>
            [EnumDescription("猎头公司", "猎头公司")]
            Vendor = 3,
            /// <summary>
            ///平台
            /// </summary>
            [EnumDescription("平台", "平台")]
            Admin = 4,
            /// <summary>
            ///猎头顾问
            /// </summary>
            [EnumDescription("猎头顾问", "猎头顾问")]
            Counselor = 5,
            /// <summary>
            ///项目
            /// </summary>
            [EnumDescription("项目", "项目")]
            Project = 19,
            /// <summary>
            ///项目
            /// </summary>
            [EnumDescription("简历", "简历")]
            Resume = 21,
        }

        /// <summary>
        /// 统计类别
        /// </summary>
        public enum StatisticsType : int
        {
            [EnumDescription("招聘信息", "招聘信息")]
            StaffingNeeds = 1,

            /// <summary>
            /// 人才库简历总数
            /// </summary>
            [EnumDescription("人才库简历总数", "人才库简历总数")]
            PuzzleBasicTotal = 2,

            /// <summary>
            ///  简历详情总数
            /// </summary>
            [EnumDescription("简历详情总数", "简历详情总数")]
            PuzzleBasicDetailTotal = 3,

            /// <summary>
            ///  下载总数
            /// </summary>
            [EnumDescription("下载总数", "下载总数")]
            PuzzleDownLoadTotal = 4,

            /// <summary>
            /// 更新总数
            /// </summary>
            [EnumDescription("更新总数", "更新总数")]
            PuzzleUpdateTotal = 5,

            /// <summary>
            /// 职位立项总数
            /// </summary>
            [EnumDescription("职位立项总数", "职位立项总数")]
            PuzzlePositionTotal = 6,

            /// <summary>
            /// 当天调用简历的数量
            /// </summary>
            [EnumDescription("当天调用简历的数量", "当天调用简历的数量")]
            PuzzleViewResumeOfDayTotal = 7,

        }

        public enum ReadState : int
        {
            [EnumDescription("未处理", "未处理")]
            NoRead = 0,

            [EnumDescription("已处理", "已处理")]
            Read = 1
        }

        /// <summary>
        ///  对应tbiz_PuzzleCapability 能力类别
        /// </summary>
        public enum PuzzleCapabilityType : int
        {
            /// <summary>
            ///  培训
            /// </summary>
            [EnumDescription("培训", "培训")]
            Training =1,
            /// <summary>
            ///  技能
            /// </summary>
            [EnumDescription("技能", "技能")]
            Skill = 2,
            [EnumDescription("语言", "语言")]
            Language = 3,
            [EnumDescription("考试", "考试")]
            Exam = 4,
            [EnumDescription("获奖", "获奖")]
            Award = 5,
            [EnumDescription("证书", "证书")]
            Certification = 6
        }

        /// <summary>
        /// 对应tbiz_PuzzleProjectActivity 项目类别
        /// </summary>
        public enum ProjectType : int
        {
            /// <summary>
            /// 项目(经历)
            /// </summary>
            [EnumDescription("项目", "项目")]
            Project = 1,
            /// <summary>
            ///  社会实践
            /// </summary>
            [EnumDescription("社会实践", "社会实践")]
            Activity = 2
        }

        /// <summary>
        /// 巧达简历更新通知状态
        /// </summary>
        public enum PuzzleQiaoDaPushUpdateStatue : int
        {
            /// <summary>
            ///  待更新
            /// </summary>
            [EnumDescription("待更新", "待更新")]
            IniUpdateStatue = 0,

            /// <summary>
            ///  可更新
            /// </summary>
            [EnumDescription("可更新", "可更新")]
            EnableUpdateStatue = 1,

            /// <summary>
            ///  已更新
            /// </summary>
            [EnumDescription("已更新", "已更新")]
            BeUpdateStatued = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum PuzzleFeeTypeForPortal : int
        {
            /// <summary>
            /// 供应商(外部)为平台充值
            /// </summary>
            [EnumDescription("下载购买数", "下载购买")]
            AddBuyResume = 1,
            /// <summary>
            ///  供应商(外部)为平台充值
            /// </summary>
            [EnumDescription("更新购买数", "更新购买")]
            AddUpdateResume = 2
        }

        /// <summary>
        /// 简历下载和简历更新操作流水类别(平台额度分配企业,企业额度分配账号,账号使用额度)
        /// </summary>
        public enum PuzzleFeeType : int
        {
            /// <summary>
            /// 下载购买,即平台为企业(客户)充值
            /// </summary>
            [EnumDescription("购买下载数", "下载购买")] //文字用于下拉列表
            AddBuyResume = 1,
            /// <summary>
            ///  更新购买,即平台为企业(客户)充值
            /// </summary>
            [EnumDescription("购买更新数", "更新购买")]
            AddUpdateResume = 2,
            /// <summary>
            /// 简历下载,指用户的消费行为
            /// </summary>
            [EnumDescription("简历下载", "简历下载")] //
            BuyResume = 3,
            /// <summary>
            /// 简历更新,指用户的消费行为
            /// </summary>
            [EnumDescription("简历更新", "简历更新")]
            UpdateResume = 4,

            /// <summary>
            ///  企业(客户)为顾问(账号)充值
            /// </summary>
            [EnumDescription("增配下载", "增配下载")]
            BuyAssign = 5,

            /// <summary>
            /// 企业(客户)为顾问(账号)充值
            /// </summary>
            [EnumDescription("增配更新", "增配更新")]
            UpdateAssign = 6,

            /// <summary>
            /// 下载优惠,即Admin为企业账户充值
            /// </summary>
            [EnumDescription("下载优惠数", "下载优惠")]
            FavourableBuyResume = 7,
            /// <summary>
            ///  更新优惠,即Admin为企业账户充值
            /// </summary>
            [EnumDescription("更新优惠数", "更新优惠")]
            FavourableUpdateResume = 8,

            /// <summary>
            /// 下载回收:管理员将简历下载次数回收到公司账号
            /// </summary>
            [EnumDescription("下载回收", "下载回收")]
            CancelBuyResume = 9,

            /// <summary>
            /// 更新回收:管理员将简历更新次数回收到公司账号
            /// </summary>
            [EnumDescription("更新回收", "更新回收")]
            CancelUpdateResume = 10,

            [EnumDescription("简历检索", "简历检索")]
            QDSearch = 11,
            /// <summary>
            ///  简历详细
            /// </summary> 
            [EnumDescription("简历详细", "简历详细")]
            QDRequestDetail = 12,
        }

        /// <summary>
        /// 巧达简历更新通知状态
        /// </summary>
        public enum ResumeSourceType : int
        {
            /// <summary>
            /// 巧达简历库
            /// </summary>
            [EnumDescription("巧达简历库", "巧达简历库")]
            QiaoDaData = 1,
            /// <summary>
            ///  中智简历库
            /// </summary>
            [EnumDescription("中智简历库", "中智简历库")]
            CIICData = 2,
            /// <summary>
            /// 租户简历上传
            /// </summary>
            [EnumDescription("租户简历上传", "租户简历上传")]
            CompanyResume = 3,
            /// <summary>
            ///  个人上传
            /// </summary>
            [EnumDescription("个人上传", "个人上传")]
            PersonResume = 4,
            /// <summary>
            ///  个人批量上传
            /// </summary>
            [EnumDescription("个人批量上传", "个人批量上传")]
            ResumeUpdate = 5,


        }

        public enum RoleCategory : int
        {
            /// <summary>
            /// 巧达检索详情
            /// </summary>
            [EnumDescription("平台管理员", "平台管理员")]
            Admin = 1,
            /// <summary>
            ///  简历详细
            /// </summary>
            [EnumDescription("猎头公司", "猎头公司")]
            CounselorClient = 2,
            /// <summary>
            /// 简历购买
            /// </summary>
            [EnumDescription("客户拓展企业", "客户拓展企业")]
            BD = 3,
            /// <summary>
            /// 
            /// </summary>
            [EnumDescription("猎头顾问", "猎头顾问")]
            Counselor = 4,
            /// <summary>
            /// 
            /// </summary>
            [EnumDescription("招聘企业", "招聘企业")]
            Client = 5,
            /// <summary>
            /// 
            /// </summary>
            [EnumDescription("企业用户管理员", "企业用户管理员")] //即人才拼图企业Admin
            PuzzleAdmin = 6,

            [EnumDescription("企业顾问", "企业顾问")]
            PuzzleClient = 7,

            /// <summary>
            /// 人才拼图 个人用户
            /// </summary>
            [EnumDescription("个人用户", "个人用户")]
            PuzzleCustomer = 8

        }

        /// <summary>
        /// 是否海外留学
        /// </summary>
        public enum StudyAbroad : int
        {
            [EnumDescription("无", "无")]
            No = 0,

            [EnumDescription("有", "有")]
            Yes = 1
        }

        /// <summary>
        /// 企业性质
        /// </summary>
        public enum CompanyStockType : int
        {
            /// <summary>
            /// 外商独资/外企办事处
            /// </summary>
            [EnumDescription("外商独资/外企办事处", "外商独资/外企办事处")]
            WhollyForeignOwned = 1,

            /// <summary>
            /// 中外合资(合资/合资)
            /// </summary>
            [EnumDescription("中外合资(合资/合资)", "中外合资(合资/合资)")]
            SinoForeignJointVenture = 2,

            /// <summary>
            /// 私营/民营企业
            /// </summary>
            [EnumDescription("私营/民营企业", "私营/民营企业")]
            PrivateEnterprises= 3,

            /// <summary>
            /// 国有企业
            /// </summary>
            [EnumDescription("国有企业", "国有企业")]
            StateOwnedEnterprise = 4,

            /// <summary>
            /// 国内上市公司
            /// </summary>
            [EnumDescription("国内上市公司", "国内上市公司")]
            ListedCompany = 5,

            /// <summary>
            /// 政府机构/非盈利机构
            /// </summary>
            [EnumDescription("政府机构/非盈利机构", "政府机构/非盈利机构")]
            GovernmentOrgans =6,

            /// <summary>
            /// 事业单位
            /// </summary>
            [EnumDescription("事业单位", "事业单位")]
            PublicServices = 7,

            //其他
            [EnumDescription("其他", "其他")]
            Other = 8
        }

        /// <summary>
        /// 
        /// </summary>
        public enum PurchFromType : int
        {
            /// <summary>
            /// 从巧达购买简历
            /// </summary>
            [EnumDescription("从巧达购买简历", "从巧达购买简历")]
            PurchFromQiaoDa = 1,


        }

        /// <summary>
        /// 职位文档的编辑状态
        /// </summary>
        public enum PublicState : int
        {
            [EnumDescription("草稿", "草稿")]
            Draft = 0,

            [EnumDescription("正式", "正式")]
            Formal = 1,

            //[EnumDescription("修改", "修改")]
            //Modify = 2,
        }

        /// <summary>
        /// 简历表[tbiz_PuzzleBasic] 字段 [IsPublic] 枚举值
        /// </summary>
        public enum IsPublic : int
        {
            /// <summary>
            /// 不公开
            /// </summary>
            [EnumDescription("不公开", "不公开")]
            No = 0,

            /// <summary>
            ///  个人注册可公开
            /// </summary>
            [EnumDescription("可公开", "可公开")]
            Yes = 1,


        }

    }
}

//  Created by ZhuCong on 1/1/14.
//  Copyright 2014 Umeng.com . All rights reserved.
//  Version 1.1



using UnityEngine;
using System.Runtime.InteropServices;
namespace Umeng
{
    
    /// <summary>
    /// 友盟游戏统计
    /// </summary>
    public class GA : Analytics
    {

        public enum Gender
        {
            Unknown = 0,
            Male = 1,
            Female = 2
        }


        /// <summary>
        /// 设置玩家等级
        /// </summary>
        /// <param name="level">玩家等级</param>

        public static void SetUserLevel(string level)
        {
			Logger.Log("SetUserLevelA");
#if UNITY_EDITOR
			Debug.Log("SetUserLevel");
#elif UNITY_IPHONE
            _SetUserLevel(level);
#elif UNITY_ANDROID
            Agent.CallStatic("setPlayerLevel", level);
#endif
			Logger.Log("SetUserLevelB");
        }


        /// <summary>
        /// 设置玩家属性
        /// </summary>
        /// <param name="userId">玩家Id</param>
        /// <param name="gender">性别</param>
        /// <param name="age">年龄</param>
        /// <param name="platform">来源</param>
        public static void SetUserInfo(string userId, Gender gender, int age, string platform)
        {
			Logger.Log("SetUserInfoA");
#if UNITY_EDITOR
			Debug.Log("SetUserInfo");
#elif UNITY_IPHONE
            _SetUserInfo(userId, (int)gender, age, platform);

#elif UNITY_ANDROID
            Agent.CallStatic("setPlayerInfo",userId, age,  (int)gender, platform);
#endif
			Logger.Log("SetUserInfoB");

        }





        
        /// <summary>
        /// 玩家进入关卡
        /// </summary>
        /// <param name="level">关卡</param>
        public static void StartLevel(string level)
        {
			Logger.Log("StartLevelA");
#if UNITY_EDITOR
            Debug.Log("StartLevel");
#elif UNITY_IPHONE
            _StartLevel(level);
#elif UNITY_ANDROID
           // Agent.CallStatic("startLevel",level);
#endif
			Logger.Log("StartLevelB");
        }

        /// <summary>
        /// 玩家通过关卡
        /// </summary>
        /// <param name="level">如果level设置为null 则为当前关卡</param>

        public static void FinishLevel(string level)
        {
			Logger.Log("FinishLevelA");
#if UNITY_EDITOR
            Debug.Log("FinishLevel");
#elif UNITY_IPHONE
            _FinishLevel(level);
#elif UNITY_ANDROID
           // Agent.CallStatic("finishLevel",level);
#endif
			Logger.Log("FinishLevelB");
        }

        /// <summary>
        /// 玩家未通过关卡
        /// </summary>
        /// <param name="level">如果level设置为null 则为当前关卡</param>
        public static void FailLevel(string level)
        {
			Logger.Log("FailLevelA");
#if UNITY_EDITOR
            Debug.Log("FailLevel");
#elif UNITY_IPHONE
            _FailLevel(level);
#elif UNITY_ANDROID
           // Agent.CallStatic("failLevel",level);
#endif
			Logger.Log("FailLevelB");
        }




        /// <summary>
        /// Source9 到Source 20 请在友盟后台网站设置 对应的定义
        /// </summary>
        public enum PaySource
        {
            AppStore = 1,
            支付宝 = 2,
            网银 = 3,
            财付通 = 4,
            移动 = 5,
            联通 = 6,
            电信 = 7,
            Paypal = 8,
            Source9,
            Source10,
            Source11,
            Source12,
            Source13,
            Source14,
            Source15,
            Source16,
            Source17,
            Source18,
            Source19,
            Source20,

        }
        /// <summary>
        /// 游戏中真实消费（充值）的时候调用此方法
        /// </summary>
        /// <param name="cash">本次消费金额</param>
        /// <param name="source">本次消费的途径，网银，支付宝 等</param>
        /// <param name="coin">本次消费等值的虚拟币</param>
        public static void Pay(double cash, PaySource source, double coin)
        {
			Logger.Log("PayA");
#if UNITY_EDITOR
            Debug.Log("Pay");
#elif UNITY_IPHONE
            _PayCashForCoin(cash,(int)source,coin);
#elif UNITY_ANDROID
            Agent.CallStatic("pay",cash , coin, (int)source);
#endif
			Logger.Log("PayB");
        }


        /// <summary>
        /// 玩家支付货币购买道具
        /// </summary>
        /// <param name="cash">真实货币数量</param>
        /// <param name="source">支付渠道</param>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        public static void Pay(double cash, PaySource source, string item, int amount, double price)
        {
			Logger.Log("PayA");
#if UNITY_EDITOR
            Debug.Log("Pay");
#elif UNITY_IPHONE
            _PayCashForItem(cash,(int)source,item,amount,price);
#elif UNITY_ANDROID
            Agent.CallStatic("pay",cash, item, amount, price, (int)source);
#endif
			Logger.Log("PayB");
        }


        /// <summary>
        /// 玩家使用虚拟币购买道具
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>

        public static void Buy(string item, int amount, double price)
        {
			Logger.Log("BuyA");
#if UNITY_EDITOR
            Debug.Log("Buy");
#elif UNITY_IPHONE
            _Buy(item,amount,price);
#elif UNITY_ANDROID
            Agent.CallStatic("buy", item, amount, price);
#endif
			Logger.Log("BuyB");
        }


        /// <summary>
        /// 玩家使用虚拟币购买道具
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        public static void Use(string item, int amount, double price)
        {
			Logger.Log("UseA");
#if UNITY_EDITOR
            Debug.Log("Use");
#elif UNITY_IPHONE
            _Use(item, amount, price);
#elif UNITY_ANDROID
            Agent.CallStatic("use", item, amount, price);
#endif
			Logger.Log("UseB");
        }


        /// <summary>
        /// Source4 到Source 10 请在友盟后台网站设置 对应的定义
        /// </summary>
        public enum BonusSource
        {
            
            玩家赠送 = 1,
			Source2 =2,
			Source3 =3,
			Source4,
            Source5,
            Source6,
            Source7,
            Source8,
            Source9,
            Source10,

        }
        /// <summary>
        /// 玩家获虚拟币奖励
        /// </summary>
        /// <param name="coin">虚拟币数量</param>
        /// <param name="source">奖励方式</param>
        public static void Bonus(double coin, BonusSource source)
        {
			Logger.Log("BonusA");
#if UNITY_EDITOR
            Debug.Log("Bonus");
#elif UNITY_IPHONE
            _BonusCoin(coin, (int)source);
#elif UNITY_ANDROID
            Agent.CallStatic("bonus", coin, (int)source);
#endif
			Logger.Log("BonusB");
        }


        /// <summary>
        /// 玩家获道具奖励
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        /// <param name="source">奖励方式</param>
        ///         

        public static void Bonus(string item, int amount, double price, BonusSource source)
        {
			Logger.Log("BonusA");
#if UNITY_EDITOR
            Debug.Log("Bonus");
#elif UNITY_IPHONE
			_BonusItem(item, amount, price, (int)source);
#elif UNITY_ANDROID
            Agent.CallStatic("bonus", item, amount, price, (int)source);
#endif
			Logger.Log("BonusB");
        }





#if	UNITY_IPHONE

        [DllImport("__Internal")]
        private static extern void _SetUserLevel(string level);

        [DllImport("__Internal")]
        private static extern void _SetUserInfo(string userId, int gender, int age, string platform);

        [DllImport("__Internal")]
        private static extern void _StartLevel(string level);

        [DllImport("__Internal")]
        private static extern void _FinishLevel(string level);

        [DllImport("__Internal")]
        private static extern void _FailLevel(string level);

        [DllImport("__Internal")]
        private static extern void _PayCashForCoin(double cash, int source, double coin);

        [DllImport("__Internal")]
        private static extern void _PayCashForItem(double cash, int source, string item, int amount, double price);

        [DllImport("__Internal")]
        private static extern void _Buy(string item, int amount, double price);

        [DllImport("__Internal")]
        private static extern void _Use(string item, int amount, double price);

        [DllImport("__Internal")]
        private static extern void _BonusCoin(double coin, int source);

        [DllImport("__Internal")]
        private static extern void _BonusItem(string item, int amount, double price, int source);

#endif




    }
}

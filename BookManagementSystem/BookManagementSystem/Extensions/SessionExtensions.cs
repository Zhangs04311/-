using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BookManagementSystem.Extensions
{
    /// <summary>
    /// Sessionにおける拡張メソッド群
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Sessionにobject(インスタンス)をセットする
        /// HttpContext.Session.SetObject<型名>(key, value);
        /// 上記のように呼び出せる
        /// </summary>
        /// <typeparam name="T">セットするvalueの型</typeparam>
        /// <param name="session">拡張メソッドの機能 呼び出す際は省略</param>
        /// <param name="key">セットするキー</param>
        /// <param name="value">セットする値</param>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Sessionからobject(インスタンス)をゲットする
        /// HttpContext.Session.GetObject<型名>(key);
        /// 上記のように呼び出せる
        /// </summary>
        /// <typeparam name="T">ゲットするvalueの型</typeparam>
        /// <param name="session">拡張メソッドの機能 呼び出す際は省略</param>
        /// <param name="key">ゲットするキー</param>
        /// <returns>Sessionから取得した値</returns>
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
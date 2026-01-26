using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Runtime.Core.ExcelTableReader
{
    public static class ExcelCellParser
    {
        /// <summary>
        /// 按分隔符转换为int列表
        /// </summary>
        /// <param name="value">cell内容</param>
        /// <param name="separator">分隔符</param>
        /// <returns>int列表，填错了会返回空列表</returns>
        public static List<int> ParseIntList(string value, char separator = '|')
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<int>();

            return value
                .Split(separator)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .SelectMany(s =>
                    int.TryParse(s, out var v)
                        ? new[] {v}
                        : Array.Empty<int>()
                )
                .ToList();
            // 没有配数字（例如汉字）直接跳过，返回空列表
            // .Select(s =>
            // {
            //     if (!int.TryParse(s, out var v))
            //         throw new Exception($"列表中包含非法数字：\"{s}\"");
            //     return v;
            // })
        }

        /// <summary>
        /// 严格转换成int
        /// </summary>
        /// <param name="value">cell内容</param>
        /// <param name="contextInfo">上下文信息，用于提示</param>
        /// <returns>int值</returns>
        /// <exception cref="Exception">空值，不是数字</exception>
        public static int ParseIntStrict(string value, string contextInfo = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"{contextInfo} 为空，无法解析为 int");

            if (!int.TryParse(value.Trim(), out var result))
                throw new Exception($"{contextInfo} 值 [{value}] 不是合法的 int");

            return result;
        }

        public static int ParseInt(string value, int defaultValue = 0, string contextInfo = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (!int.TryParse(value.Trim(), out var result))
                throw new Exception($"{contextInfo} 值 [{value}] 不是合法的 int");

            return result;
        }

        /// <summary>
        /// 转换成枚举类型
        /// </summary>
        /// <param name="value">cell内容</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="contextInfo">context信息</param>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T ParseEnumOrDefault<T>(string value, T defaultValue, string contextInfo = "")
            where T : struct, Enum
        {
            // 交互类型没填：None
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            value = value.Trim();

            if (Enum.TryParse<T>(value, out var result))
            {
                // 数字也能解析，但必须要在 InteractionType 范围内。
                if (Enum.IsDefined(typeof(T), result))
                    return result;
            }

            throw new Exception(
                $"枚举类型：[{value}] 不合法。 {contextInfo}"
            );
        }
    }
}
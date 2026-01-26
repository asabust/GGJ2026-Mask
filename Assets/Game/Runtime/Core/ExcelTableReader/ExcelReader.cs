using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Game.Runtime.Core.Attributes;
using OfficeOpenXml;
using UnityEngine;

namespace Game.Runtime.Core.ExcelTableReader
{
    /// <summary>
    /// 读取Excel文件
    /// </summary>
    public static class ExcelReader
    {
        public static ExcelTableContext ReadAll(string excelPath)
        {
            // 所有表一起打包，用于一次性序列化和反序列化
            var context = new ExcelTableContext();

            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                var workbook = package.Workbook;

                var readers = CreateAllReaders();

                foreach (var (sheetName, reader) in readers)
                {
                    var sheet = workbook.Worksheets[sheetName];
                    if (sheet == null)
                    {
                        Debug.LogWarning($"Sheet not found: {sheetName}");
                        continue;
                    }

                    reader.Read(sheet, context);
                }
            }

            return context;
        }

        private static Dictionary<string, IExcelTableReader> CreateAllReaders()
        {
            var dict = new Dictionary<string, IExcelTableReader>();

            // 从 Game.Runtime.Core 程序集里找表
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IExcelTableReader).IsAssignableFrom(t) &&
                    !t.IsInterface &&
                    !t.IsAbstract);

            foreach (var type in types)
            {
                // 从属性中获取读表的名字
                var attr = type.GetCustomAttribute<ExcelSheetAttribute>();
                if (attr == null)
                    continue;

                var reader = (IExcelTableReader) Activator.CreateInstance(type);
                dict[attr.sheetName] = reader;
            }

            return dict;
        }
    }
}
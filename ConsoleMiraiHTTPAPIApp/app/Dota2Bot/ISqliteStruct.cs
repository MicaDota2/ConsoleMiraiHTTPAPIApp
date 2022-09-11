using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    // 所有要记录在数据库里的数据，都需要继承ISqliteStruct的接口
    interface ISqliteStruct
    {
        /// <summary>
        /// 获取数据库的表格的格式，用于创建表格
        /// </summary>
        /// <returns></returns>
        public string GetTableFormat();

        /// <summary>
        /// 获取数据库的列的格式，用于插入数据
        /// </summary>
        /// <returns></returns>
        public string GetTableColumns();

        /// <summary>
        /// 获取数据的格式，用于插入数据
        /// </summary>
        /// <returns></returns>
        public string GetTableValues();

        /// <summary>
        /// 使用从数据库中返回的数据填充当前数据类型
        /// </summary>
        /// <param name="reader"></param>
        public void FromTableValues(SQLiteDataReader reader);

        /// <summary>
        /// 获取第propertyID个数据的名字
        /// </summary>
        /// <param name="propertyID"></param>
        /// <returns></returns>
        public string GetPropertyName(int propertyID);

        /// <summary>
        /// 获取第propertyID个数据的string值
        /// </summary>
        /// <param name="propertyID"></param>
        /// <returns></returns>
        public string GetPropertyValue(int propertyID);
    }

    // 用于达成近似于调用接口的静态方法的功能
    // https://stackoverflow.com/a/66927384
    static class SqliteStruct<T> where T : ISqliteStruct, new()
    {
        static public readonly T value = new();
        static public string GetTableFormat()
        {
            return value.GetTableFormat();
        }

        static public string GetTableColumns()
        {
            return value.GetTableColumns();
        }

        static public string GetPropertyName(int propertyID)
        {
            return value.GetPropertyName(propertyID);
        }
    }
}

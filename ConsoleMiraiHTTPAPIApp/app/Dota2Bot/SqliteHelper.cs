using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    /// <summary>
    /// 所有数据库的操作都应该继承SqliteHelper类，T是要储存在数据库中的数据类型，都应该实现ISqliteStruct这个接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SqliteHelper<T> where T : ISqliteStruct, new()
    {
        SQLiteConnection connection;
        string tableName;

        public SqliteHelper(string databaseName, string tableName)
        {
            connection = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;New=True;Compress=True", databaseName));
            this.tableName = tableName;
        }

        /// <summary>
        /// 打开数据库，如果没有的话就创建一个新的
        /// </summary>
        public void OpenOrCreateSql()
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 如果没有表的话就创建一个新的表
        /// </summary>
        public void CreateTableIfNotExists()
        {
            string createStr = string.Format("CREATE TABLE IF NOT EXISTS {0} ({1})",
                tableName, SqliteStruct<T>.GetTableFormat());

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = createStr;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 删除这个表
        /// </summary>
        public void DropTable()
        {
            string dropStr = string.Format("DROP TABLE {0}", tableName);

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = dropStr;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 判断一个数据是否为空
        /// </summary>
        /// <param name="propertyID"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public bool CheckIfDataExists(int propertyID, string searchValue)
        {
            try
            {
                string checkStr = string.Format("SELECT * FROM {0} WHERE {1}={2} LIMIT 1",
                    tableName, SqliteStruct<T>.GetPropertyName(propertyID), searchValue);

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = checkStr;

                using (SQLiteDataReader sqliteDataReader = command.ExecuteReader())
                {
                    bool result = sqliteDataReader.Read();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// 插入一个数据
        /// </summary>
        /// <param name="data"></param>
        public void InsertData(T data)
        {
            try
            {
                string insertStr = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, data.GetTableColumns(), data.GetTableValues());

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = insertStr;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 如果不存在的话插入数据
        /// propertyID和searchValue用于判断数据库中是否存在数据，使得当前数据类型的第propertyID个数据的值为searchValue
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyID"></param>
        /// <param name="searchValue"></param>
        public void InsertDataIfNotExists(T data, int propertyID, string searchValue)
        {
            if (CheckIfDataExists(propertyID, searchValue)) return;
            
            try
            {
                string insertStr = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, data.GetTableColumns(), data.GetTableValues());

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = insertStr;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 插入一组数据
        /// </summary>
        /// <param name="dataList"></param>
        public void InsertData(IEnumerable<T> dataList)
        {
            try
            {
                SQLiteCommand command = connection.CreateCommand();
                int dataCount = dataList.Count();
                string valueStr = "";
                for (int i = 0; i < dataCount; i++)
                {
                    valueStr += "(" + dataList.ElementAt(i).GetTableValues() + ")";
                    if (i != dataCount - 1) valueStr += ", ";
                }
                string insertStr = string.Format("INSERT INTO {0} ({1}) VALUES {2}", tableName, SqliteStruct<T>.GetTableColumns(), valueStr);
                Console.WriteLine(insertStr);
                command.CommandText = insertStr;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 删除数据库中满足“当前数据类型的第propertyID个数据的值为searchValue”的数据
        /// </summary>
        /// <param name="propertyID"></param>
        /// <param name="searchValue"></param>
        public void DeleteData(int propertyID, string searchValue)
        {
            try
            {
                string deleteStr = string.Format("DELETE * FROM {0} WHERE {1}={2}", tableName, SqliteStruct<T>.GetPropertyName(propertyID), searchValue);

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = deleteStr;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 删除所有的数据
        /// </summary>
        public void DeleteAllData()
        {
            try
            {
                string deleteStr = string.Format("DELETE FROM {0}", tableName);

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = deleteStr;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 更新满足“当前数据类型的第propertyID[0]个数据的值为searchValue”的数据，
        /// 使得这些数据的第propertyID[1, 2...]个数据的值和data相同
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyIDs"></param>
        /// <param name="searchValue"></param>
        public void UpdateData(T data, int[] propertyIDs, string searchValue)
        {
            try
            {
                string setString = "";
                int idCount = propertyIDs.Length;
                for (int i = 1; i < idCount; i++)
                {
                    int propertyID = propertyIDs[i];
                    setString += string.Format(" {0}={1}", SqliteStruct<T>.GetPropertyName(propertyID), data.GetPropertyValue(propertyID));
                    if (i != idCount - 1) setString += ",";
                }

                string updateString = string.Format("UPDATE {0} SET{1} WHERE {2}={3}", tableName, setString, SqliteStruct<T>.GetPropertyName(propertyIDs[0]), searchValue);
                Console.WriteLine(updateString);
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = updateString;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
           
        }

        /// <summary>
        /// 查找某一个数据使得其第propertyID个数据的值为searchValue
        /// </summary>
        /// <param name="propertyID"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<T> GetData(int propertyID, string searchValue)
        {
            List<T> dataList = new List<T>();
            try
            {
                string selectString = string.Format("SELECT * FROM {0} WHERE {1}={2}", tableName, SqliteStruct<T>.GetPropertyName(propertyID), searchValue);
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = selectString;
                using (SQLiteDataReader sqliteDataReader = command.ExecuteReader())
                { 
                    while (sqliteDataReader.Read())
                    {
                        T data = new T();
                        data.FromTableValues(sqliteDataReader);
                        dataList.Add(data);
                    }
                }
                return dataList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return new List<T>();
            }
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllData()
        {
            List<T> toReturn = new List<T>();
            try
            {
                string selectString = string.Format("SELECT * FROM {0}", tableName);
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = selectString;
                using (SQLiteDataReader sqliteDataReader = command.ExecuteReader())
                {
                    while (sqliteDataReader.Read())
                    {
                        T data = new T();
                        data.FromTableValues(sqliteDataReader);
                        toReturn.Add(data);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return toReturn;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void CloseSql()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

using BF1.ServerAdminTools.Common.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace BF1.ServerAdminTools.Common.Helper;

public enum DataShell
{
    KICKOK, KICKFAIL
}

internal static class SQLiteHelper
{
    private static string kickDbFile = $"{ConfigLocal.Base}/AdminLog.db";

    private static SqliteConnection connection;

    /// <summary>
    /// 数据库初始化
    /// </summary>
    public static void Initialize()
    {
        var connStr = new SqliteConnectionStringBuilder("Data Source=" + kickDbFile)
        {
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();

        connection = new SqliteConnection(connStr);
        connection.Open();

        if (ExecuteScalar("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='kick_ok'") == 0)
        {
            ExecuteNonQuery("CREATE TABLE kick_ok ( " +
                "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "name TEXT, " +
                "personaId TEXT, " +
                "reason TEXT, " +
                "status TEXT, " +
                "date TimeStamp NOT NULL DEFAULT CURRENT_TIMESTAMP)");
        }

        if (ExecuteScalar("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='kick_fail'") == 0)
        {
            ExecuteNonQuery("CREATE TABLE kick_fail ( " +
                "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "name TEXT, " +
                "personaId TEXT, " +
                "reason TEXT, " +
                "status TEXT, " +
                "date TimeStamp NOT NULL DEFAULT CURRENT_TIMESTAMP)");
        }

        if (ExecuteScalar("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='change_team'") == 0)
        {
            ExecuteNonQuery("CREATE TABLE change_team ( " +
                "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "rank TEXT, name TEXT, personaId TEXT, " +
                "status TEXT, " +
                "date TimeStamp NOT NULL DEFAULT CURRENT_TIMESTAMP)");
        }
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public static void CloseConnection()
    {
        if (connection?.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    /// <summary>
    /// 执行SQL命令，执行对数据表的增加、删除、修改操作（有SQL注入风险）
    /// </summary>
    /// <param name="sqlStr"></param>
    public static void ExecuteNonQuery(string sqlStr)
    {
        using SqliteCommand cmd = new(sqlStr, connection);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// 执行SQL命令，返回查询结果中第 1 行第 1 列的值（有SQL注入风险）
    /// </summary>
    /// <param name="sqlStr"></param>
    /// <returns></returns>
    public static int ExecuteScalar(string sqlStr)
    {
        using var cmd = new SqliteCommand(sqlStr, connection);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>
    /// 增加数据库记录
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="info"></param>
    public static void AddLog2SQLite(DataShell sheetName, BreakRuleInfo info)
    {
        switch (sheetName)
        {
            case DataShell.KICKOK:
                connection.Execute("INSERT INTO kick_ok(name, personaId, reason, status)VALUES(@Name, @PersonaId, @Reason, @Status)", info);
                break;
            case DataShell.KICKFAIL:
                connection.Execute("INSERT INTO kick_fail(name, personaId, reason, status)VALUES(@Name, @PersonaId, @Reason, @Status)", info);
                break;
        }
    }

    /// <summary>
    /// 更换队伍记录日志
    /// </summary>
    /// <param name="info"></param>
    public static void AddLog2SQLite(ChangeTeamInfo info)
    {
        connection.Execute("INSERT INTO change_team(rank, name, personaId, status)VALUES(@Rank,@Name,@PersonaId,@Status)", info);
    }
}
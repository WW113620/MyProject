using Application.Common;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAL.DAL.MSSQL
{
    public class SearchUserInfo
    {
        public static UserInfo isCheckLogin(string userName, string password, int type = 3)
        {
            UserInfo info = new UserInfo();
            try
            {
                string sql = @"SELECT TOP 1 [ID],[UserGuid],[Name],[Password],[UserType]
                                  FROM [dbo].[UserInfoes] WHERE [Name]=@Name AND [Password]=@Password AND [UserType]=@UserType";
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "@Name", Value = userName });
                parameters.Add(new SqlParameter() { ParameterName = "@Password", Value = password });
                parameters.Add(new SqlParameter() { ParameterName = "@UserType", Value = type });

                var dt = new DataTable();
                dt = MSSqlHelper.ExecuteDateTable(sql, parameters.ToArray());
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    info.ID = row[0].ToInt(0);
                    info.UserGuid = row[1].ToString("");
                    info.Name = row[2].ToString("");
                    info.UserType = row[4].ToInt(0);
                }
            }
            catch (Exception e)
            {

            }
            return info;
        }

        public static List<AppointMents> SearchAllStudentInfo(string contion = "")
        {
            string sql = @"SELECT a.[Id],a.[Date],a.[Time] ,a.[Message],a.[SId],a.Acceptance ,c.Name as CName,s.Name as SName
                            FROM [dbo].[AppointMents] as a
                            LEFT JOIN [dbo].[UserInfoes] as s on s.ID=a.[SId] and s.UserType=3
                            LEFT JOIN [dbo].[UserInfoes] as c on c.ID=a.[Acceptance] and c.UserType=2 {0} ";
            var parameters = new List<SqlParameter>();
            string wherePart = string.Empty;
            if (!string.IsNullOrEmpty(contion))
            {
                wherePart += " WHERE s.Name like @Name ";
                parameters.Add(new SqlParameter("@Name", string.Format("%{0}%", contion)));
            }
            var strSql = string.Format(sql, wherePart);
            var dt = new DataTable();
            dt = MSSqlHelper.ExecuteDateTable(strSql, parameters.ToArray());

            if (dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new AppointMents()
                {
                    ID = row["Id"].ToInt(0),
                    Date = row["Date"].ToString(""),
                    Time = row["Time"].ToString(""),
                    AcceptanceName = row["CName"].ToString(""),
                    SName = row["SName"].ToString(""),
                }).ToList();
            }
            else
            {
                return null;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VakaxaIDServer.Commons.Helpers
{
	public static class SqlHelper
	{

		public static string Query_Search(string tableName, Dictionary<string, string> whereValue, int limit = -1, int offset = -1, string[] orderByValue = null)
		{
			var whereStr = new StringBuilder("");
			var orderStr = new StringBuilder("");
			var count = 0;
			foreach (var prop in whereValue)
			{
				if (prop.Value == null) continue;
				if (count > 0)
					whereStr.Append(" AND ");
				whereStr.AppendFormat(" {0}='{1}'", prop.Key, prop.Value);
				count++;
			}

			var output = string.Format("SELECT * FROM {0} WHERE {1}", tableName, whereStr);
			if (orderByValue != null)
			{
				count = 0;
				foreach (var prop in orderByValue)
				{
					//if (prop.Value != null)
					{
						if (count > 0)
							orderStr.Append(",");
						orderStr.AppendFormat(" {0}", prop);
						count++;
					}
				}
				output += " ORDER BY " + orderStr;
			}
			if (limit > 0)
			{
				output += " LIMIT " + limit;
			}
			if (offset > 0)
			{
				output += " OFFSET " + offset;
			}
			Console.WriteLine(output);
			return output;
		}

		public static string Query_Update(string tableName, object updateValue, Dictionary<string, string> whereValue)
		{
			var updateStr = new StringBuilder("");
			var whereStr = new StringBuilder("");

			var count = 0;
			foreach (var prop in updateValue.GetType().GetProperties())
			{
				if (prop.GetValue(updateValue, null) == null) continue;
				if (count > 0)
					updateStr.Append(",");
				updateStr.AppendFormat(" {0}='{1}'", prop.Name, prop.GetValue(updateValue, null));
				count++;
			}

			// if (whereStr != null)
			count = 0;
			foreach (var prop in whereValue)
			{
				if (prop.Value == null) continue;
				if (count > 0)
					whereStr.Append(" AND ");
				whereStr.AppendFormat(" {0}='{1}'", prop.Key, prop.Value);
				count++;
			}
			var output = string.Format(@"UPDATE {0} SET {1} WHERE {2}", tableName, updateStr, whereStr);
			//Console.WriteLine(output);
			return output;
		}

		public static string Query_Update(string tableName, Dictionary<string, string> updateValue, Dictionary<string, string> whereValue)
		{
			var updateStr = new StringBuilder("");
			var whereStr = new StringBuilder("");
			//StringBuilder orderStr = new StringBuilder("");
			var count = 0;
			foreach (var prop in updateValue)
			{
				if (prop.Value != null)
				{
					if (count > 0)
						updateStr.Append(",");
					updateStr.AppendFormat(" {0}='{1}'", prop.Key, prop.Value);
					count++;
				}
			}

			// if (whereStr != null)
			count = 0;
			foreach (var prop in whereValue)
			{
				if (prop.Value == null) continue;
				if (count > 0)
					whereStr.Append(" AND ");
				whereStr.AppendFormat(" {0}='{1}'", prop.Key, prop.Value);
				count++;
			}

			var output = string.Format(@"UPDATE {0} SET {1} WHERE {2}", tableName, updateStr, whereStr);

			//	Console.WriteLine(output);
			return output;
		}


	}
}

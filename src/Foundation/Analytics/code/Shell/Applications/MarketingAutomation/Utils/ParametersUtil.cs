using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace MCH.Foundation.Analytics.Shell.Applications.MarketingAutomation.Utils
{
	public class ParametersUtil
	{
		public static string GetParameters(IEnumerable<FieldDescriptor> fieldDescriptors)
		{
			if (fieldDescriptors == null)
			{
				return string.Empty;
			}

			Item item = ParametersUtil.GetItem(fieldDescriptors);
			string result = string.Empty;
			foreach (FieldDescriptor current in fieldDescriptors)
			{
				if (item.Fields[current.FieldID].Name == "Parameters")
				{
					result = current.Value;
				}
			}
			return result;
		}

		public static string NameValueCollectionToUrlParameters(NameValueCollection values)
		{
			Assert.ArgumentNotNull(values, "values");
			StringBuilder stringBuilder = new StringBuilder();
			int count = values.Count;
			int num = 0;
			string[] allKeys = values.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string text = allKeys[i];
				num++;
				stringBuilder.Append(string.Format("{0}={1}", text, values[text]));
				if (num < count)
				{
					stringBuilder.Append('&');
				}
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<FieldDescriptor> SetParameters(IEnumerable<FieldDescriptor> fieldDescriptors, string parameters)
		{
			if (fieldDescriptors == null)
			{
				return null;
			}
			Item item = ParametersUtil.GetItem(fieldDescriptors);
			foreach (FieldDescriptor current in fieldDescriptors)
			{
				if (item.Fields[current.FieldID].Name == "Parameters")
				{
					current.Value = parameters;
				}
			}
			return fieldDescriptors;
		}

		private static Item GetItem(IEnumerable<FieldDescriptor> fieldDescriptors)
		{
			Assert.ArgumentNotNull(fieldDescriptors, "fieldDescriptors");
			List<FieldDescriptor> list = new List<FieldDescriptor>(fieldDescriptors);
			Assert.IsTrue(list.Count > 0, "Field descriptors are empty.");
			FieldDescriptor fieldDescriptor = new List<FieldDescriptor>(fieldDescriptors)[0];
			ID itemID = fieldDescriptor.ItemUri.ItemID;
			Assert.IsNotNull(itemID, "Item ID is null.");
			Database database = Database.GetDatabase(fieldDescriptor.ItemUri.DatabaseName);
			Assert.IsNotNull(database, "Database is null.");
			Item item = database.GetItem(itemID);
			return Assert.ResultNotNull<Item>(item);
		}
	}
}
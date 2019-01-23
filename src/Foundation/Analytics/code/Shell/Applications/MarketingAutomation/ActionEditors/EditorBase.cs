using MCH.Foundation.Analytics.Shell.Applications.MarketingAutomation.Utils;
using Sitecore.Configuration;
using Sitecore.Controls;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.XamlSharp.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace MCH.Foundation.Analytics.Shell.Applications.MarketingAutomation.ActionEditors
{
	public class EditorBase : DialogPage
	{
		protected NameValueCollection NvParams;
		public Database CurrentDatabase
		{
			get
			{
				return Factory.GetDatabase(WebUtil.GetQueryString("db"));
			}
		}

		public Item CurrentStateItem
		{
			get
			{
				return CurrentDatabase.GetItem(WebUtil.GetQueryString("stateId"));
			}
		}

		public Item CurrentEngagementPlanItem
		{
			get
			{
				return CurrentDatabase.GetItem(WebUtil.GetQueryString("engagementPlanId"));
			}
		}

		public virtual string Params
		{
			get
			{
				return ParametersUtil.GetParameters(FieldEditorOptions.Parse(WebUtil.GetRawUrl()).Fields);
			}
		}

		public void SaveParameters()
		{
			string parameters = string.Empty;
			if (this.NvParams != null)
			{
				parameters = ParametersUtil.NameValueCollectionToUrlParameters(this.NvParams);
			}
			UrlString url = new UrlString(WebUtil.GetQueryString());
			FieldEditorOptions fieldEditorOptions = FieldEditorOptions.Parse(url);
			ParametersUtil.SetParameters(fieldEditorOptions.Fields, parameters);
			UrlHandle urlHandle = fieldEditorOptions.ToUrlHandle();
			urlHandle.Add(new UrlString(WebUtil.GetRawUrl()), "hdl");
			XamlControl.AjaxScriptManager.SetDialogValue(urlHandle.Handle);
		}

		public string GetParameterValueByKey(string key)
		{
			return this.GetParameterValueByKey(key, null);
		}

		public string GetParameterValueByKey(string key, string defaultValue)
		{
			if (this.NvParams == null)
			{
				this.NvParams = WebUtil.ParseUrlParameters(this.Params);
			}
			return this.NvParams[key] ?? defaultValue;
		}

		public void SetParameterValue(string key, string value)
		{
			if (this.NvParams == null)
			{
				this.NvParams = WebUtil.ParseUrlParameters(this.Params);
			}
			this.NvParams[key] = value;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!this.Page.IsPostBack)
			{
				this.Localize();
			}
		}

		protected virtual void Localize()
		{
		}

		protected virtual void ConfigureSaveParameters()
		{
		}

		protected override void OK_Click()
		{
			ConfigureSaveParameters();
			SaveParameters();
			base.OK_Click();
		}
	}
}
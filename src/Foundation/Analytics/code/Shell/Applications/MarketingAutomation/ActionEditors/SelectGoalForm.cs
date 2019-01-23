using MCH.Foundation.Analytics.Automation.AutomationActions;
using Sitecore.Analytics;
using Sitecore.Analytics.Data.Items;
using Sitecore.Data;
using Sitecore.Extensions.StringExtensions;
using Sitecore.Globalization;
using Sitecore.Marketing.Definitions;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MCH.Foundation.Analytics.Shell.Applications.MarketingAutomation.ActionEditors
{
	public class SelectGoalForm : EditorBase
	{

		protected Scrollbox PageEvents
		{
			get;
			set;
		}

		protected string SelectedPageEvents
		{
			get;
			set;
		}

		protected Edit PageEventFilter
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Sitecore.Context.ClientPage.IsEvent)
			{
				return;
			}
			PageEventFilter.Value = Translate.Text("Filter");
			SelectedPageEvents = base.GetParameterValueByKey(TriggerGoalAction.Parameters.SelectedGoals, string.Empty);
			RenderPageEvents();
		}

		protected override void ConfigureSaveParameters()
		{
			ListString listString = new ListString();
			foreach (string text in HttpContext.Current.Request.Form.Keys)
			{
				if (!string.IsNullOrEmpty(text) && text.StartsWith("pe_"))
				{
					string value = ShortID.Decode(text.Mid(3));
					listString.Add(value);
				}
			}
			base.SetParameterValue(TriggerGoalAction.Parameters.SelectedGoals, listString.ToString());
		}

		private void RenderPageEvents()
		{
			StringWriter stringWriter = new StringWriter();
			using (HtmlTextWriter output = new HtmlTextWriter(stringWriter))
			{
				var definitionManager = DefinitionManagerFactory.Default.GetDefinitionManager<Sitecore.Marketing.Definitions.Goals.IGoalDefinition>();
				var allGoals = definitionManager.GetAll(Sitecore.Context.Language.CultureInfo);
				RenderPageEvents(output, "Goals", allGoals);
				PageEvents.InnerHtml = stringWriter.ToString();
			}
		}

		private void RenderPageEvents(HtmlTextWriter output, string title, Sitecore.Marketing.Core.ResultSet<DefinitionResult<Sitecore.Marketing.Definitions.Goals.IGoalDefinition>> results)
		{
			output.Write("<div class=\"scSection\" style=\"padding:0px 0px 2px 0px; border-bottom:1px solid #999999; margin:8px 0px 4px 0px; font-weight:bold\">");
			output.Write(Translate.Text(title));
			output.Write("</div>");
			output.Write("<div>");
			if (results != null)
			{
				foreach (var current in results.DataPage)
				{
					bool flag = SelectedPageEvents.IndexOf(current.Data?.Id.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0;
					output.Write("<div style=\"padding:1px 0px 1px 8px\">");
					output.Write("<input id=\"pe_{0}\" name=\"pe_{0}\" type=\"checkbox\" style=\"margin:0px 4px 0px 0px\"", current.Data?.Id.ToShortID());
					if (flag)
					{
						output.Write("checked=\"checked\"");
					}
					output.Write(" />");
					output.Write("<span class=\"scPageEventOption\">");
					output.Write(current.Data?.Name);
					output.Write("</span>");
					output.Write("</div>");
				}
			}
			output.Write("</div>");
		}
	}
}
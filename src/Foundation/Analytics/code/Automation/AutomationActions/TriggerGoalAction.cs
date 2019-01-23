using Sitecore.Analytics.Automation;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Services;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MCH.Foundation.Analytics.Automation.AutomationActions
{
	public class TriggerGoalAction : IAutomationAction
	{
		public static class Parameters
		{
			public static readonly string SelectedGoals = "selectedgoals";
		}

		public AutomationActionResult Execute(AutomationActionContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			NameValueCollection parameters = context.Parameters;
			string selectedGoals = parameters[TriggerGoalAction.Parameters.SelectedGoals];
			if (!string.IsNullOrEmpty(selectedGoals))
			{
				var trackerService = new TrackerService();
				ListString listString = new ListString(selectedGoals);
				foreach (var goal in listString)
				{
					ID goalID = ID.Null;
					if (Sitecore.Data.ID.TryParse(goal, out goalID))
					{
						trackerService.TrackPageEvent(goalID);
					}
				}
			}
			return AutomationActionResult.Continue;
		}
	}
}
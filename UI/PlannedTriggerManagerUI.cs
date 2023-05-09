using HateMeLikeAPro.EffectExtensions.Behaviors;
using HateMeLikeAPro.EffectExtensions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.UI
{
	public class PlannedTriggerManagerUI : BaseUI
	{
		public PlannedTriggerManagerUI()
			: base(1337, new Rect(EffectControllerBehavior.Instance.plannerX, EffectControllerBehavior.Instance.plannerY, 400, 200), "Effect planner")
		{ }

		protected override void DrawContent()
		{
			var triggerManager = EffectControllerBehavior.Instance.PlannedTriggerManager;
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Create new planned trigger"))
				CreateNewPlannedTrigger();
			GUILayout.EndHorizontal();

			foreach(var triggerEntry in triggerManager.plannedTriggerEntries)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label(triggerEntry.Value.displayName, GUILayout.ExpandWidth(true));
				if (GUILayout.Button("Edit", GUILayout.Width(75)))
					EditTriggerEntry(triggerEntry.Key);

				if (GUILayout.Button("Remove", GUILayout.Width(75)))
					RemoveTriggerEntry(triggerEntry.Key);

				GUILayout.EndHorizontal();
			}



			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button("close"))
			{
				EffectControllerBehavior.Instance.CloseMainWindow();
			}

			GUILayout.EndHorizontal();
		}

		private void EditTriggerEntry(Guid guid)
		{
			EffectControllerBehavior.Instance.OpenEditWindow(guid);
		}

		private void RemoveTriggerEntry(Guid guid)
		{
			CommandController.Instance.addCommand(new RemoveTriggerEntryCommand(guid));
		}

		private void CreateNewPlannedTrigger()
		{			
			EffectControllerBehavior.Instance.PlannedTriggerManager.NewEntry();
		}

		public override void OnClose()
		{
			EffectControllerBehavior.Instance.plannerX = windowRect.x;
			EffectControllerBehavior.Instance.plannerY = windowRect.y;
		}
	}
}

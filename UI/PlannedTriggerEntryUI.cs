using HateMeLikeAPro.EffectExtensions.Behaviors;
using HateMeLikeAPro.EffectExtensions.Utils;
using Parkitect.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.UI
{
	public class PlannedTriggerEntryUI : BaseUI
	{
		public readonly Guid guid;


		private string displayName;
		private string totalTimeModulus;
		private string triggerTime;
		private string secondBetweenNotificationAndStart;
		private bool triggerNotification;
		private readonly List<EffectBox> effectBoxesToTrigger = new List<EffectBox>();

		public PlannedTriggerEntryUI(Guid guid)
			: base(1338, new Rect(EffectControllerBehavior.Instance.editorX, EffectControllerBehavior.Instance.editorY, 400, 400), "Edit planned effect")
		{
			this.guid = guid;

			var entry = EffectControllerBehavior.Instance.PlannedTriggerManager.GetEntry(guid);

			if (entry != null)
			{
				displayName = entry.displayName;
				totalTimeModulus = entry.totalModulus.ToString();
				triggerTime = entry.triggerTime.ToString();

				var gc = GameController.Instance;
				foreach (var uuid in entry.effectBoxesToTrigger)
				{
					var box = gc.getSerializedObject<EffectBox>(uuid);
					if (box != null)
						effectBoxesToTrigger.Add(box);
				}
				triggerNotification = entry.triggerNotification;
				secondBetweenNotificationAndStart = entry.secondBetweenNotificationAndStart.ToString();
			}
		}

		protected override void DrawContent()
		{
			GUILayout.BeginHorizontal();

			GUILayout.Label("UUID: ");
			var currentColor = GUI.contentColor;
			GUI.contentColor = SomeColors.DARK_GRAY;
			GUILayout.Label(guid.ToString(), GUILayout.ExpandWidth(true));
			GUI.contentColor = currentColor;

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			GUILayout.Label("Name: ", GUILayout.Width(50));
			displayName = GUILayout.TextField(displayName, GUILayout.ExpandWidth(true));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			GUILayout.Label("Total time modulus: ", GUILayout.Width(125));
			GUI.contentColor = hasValidInteger(totalTimeModulus) && !"0".Equals(totalTimeModulus) ? currentColor : SomeColors.VALUE_ERROR;

			totalTimeModulus = GUILayout.TextField(totalTimeModulus, GUILayout.ExpandWidth(true));

			GUI.contentColor = currentColor;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			GUILayout.Label("Trigger time: ", GUILayout.Width(75));
			GUI.contentColor = hasValidInteger(triggerTime) && !"0".Equals(triggerTime) ? currentColor : SomeColors.VALUE_ERROR;
			triggerTime = GUILayout.TextField(triggerTime, GUILayout.ExpandWidth(true));
			GUI.contentColor = currentColor;

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Receive notification: ", GUILayout.Width(125));
			triggerNotification = GUILayout.Toggle(triggerNotification, "");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Head's up (seconds): ", GUILayout.Width(125));
			GUI.contentColor = hasValidInteger(secondBetweenNotificationAndStart) ? currentColor : SomeColors.VALUE_ERROR;
			secondBetweenNotificationAndStart = GUILayout.TextField(secondBetweenNotificationAndStart, GUILayout.ExpandWidth(true));
			GUI.contentColor = currentColor;
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			GUILayout.Label("Use the 'Trigger EffectBox' hotkey while pointing at an effect box to add/remove it. Only when this window is open!");
			GUILayout.EndHorizontal();


			foreach (var box in effectBoxesToTrigger)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label(box.getCustomizedName());
				GUI.contentColor = SomeColors.DARK_GRAY;
				GUILayout.Label(box.objectID.ToString());
				GUI.contentColor = currentColor;

				GUILayout.EndHorizontal();
			}


			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("cancel"))
			{
				EffectControllerBehavior.Instance.CloseEditWindow(guid);
				//cancel
			}
			if (GUILayout.Button("save"))
			{
				Save();
				//save
			}
			GUILayout.EndHorizontal();
		}



		private static bool hasValidInteger(string str)
		{
			foreach (var ch in str)
			{
				if (!char.IsNumber(ch))
					return false;
			}

			return true;
		}

		public void HandleEffectBox(EffectBox effectBox)
		{
			if (effectBoxesToTrigger.Contains(effectBox))
				effectBoxesToTrigger.Remove(effectBox);
			else
				effectBoxesToTrigger.Add(effectBox);
		}

		private void Save()
		{

			if (!hasValidInteger(totalTimeModulus) || "0".Equals(totalTimeModulus))
			{
				NotificationBar.Instance.addNotification(new Notification("Invalid fields", "Please provide a valid 'total time modulus'"));
				return;
			}

			if (!hasValidInteger(triggerTime) || "0".Equals(triggerTime))
			{
				NotificationBar.Instance.addNotification(new Notification("Invalid fields", "Please provide a valid 'trigger time'"));
				return;
			}

			if (!hasValidInteger(secondBetweenNotificationAndStart))
			{
				NotificationBar.Instance.addNotification(new Notification("Invalid fields", "Please provide a valid 'heads up time'"));
				return;
			}

			EffectControllerBehavior.Instance.PlannedTriggerManager.UpdateEntry(guid, displayName, int.Parse(totalTimeModulus), int.Parse(triggerTime), effectBoxesToTrigger.Select(o => o.objectID).ToList(), triggerNotification, int.Parse(secondBetweenNotificationAndStart));
			EffectControllerBehavior.Instance.CloseEditWindow(guid);
		}

		public override void OnClose()
		{
			EffectControllerBehavior.Instance.editorX = windowRect.x;
			EffectControllerBehavior.Instance.editorY = windowRect.y;
		}
	}
}

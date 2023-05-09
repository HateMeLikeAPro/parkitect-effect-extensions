using HateMeLikeAPro.EffectExtensions.Utils;
using Parkitect.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HateMeLikeAPro.EffectExtensions.Behaviors
{
	public class PlannedTriggerEntry
	{

		public readonly List<uint> effectBoxesToTrigger = new List<uint>();
		
		public string displayName;
		
		public int totalModulus;
		public int triggerTime;
		
		public bool hasTriggered;


		public bool hasTriggeredNotification;
		public bool triggerNotification;
		public int secondBetweenNotificationAndStart;






		public void UpdatePlannedTrigger(GameController gameController, PlannedTriggerManager manager, Guid entryIndex)
		{
			var currentTimeModulus = ParkInfo.ParkTime % totalModulus;


			if (triggerNotification)
			{
				DoNotificationLogic(currentTimeModulus);
			}
			


			if (hasTriggered)
			{
				/*
				 * Untrigger when the trigger time becomes bigger then our currentTimeModulus (totalModulus has wrapped our number)
				 */
				if (currentTimeModulus < triggerTime)
					hasTriggered = false;

				return;
			}

			//here we are not triggered, check if we need to trigger
			if (currentTimeModulus >= triggerTime)
			{
				hasTriggered = true;
				//traverse in reverse order so when we remove items, this doesnt' become a problem
				for (var i = effectBoxesToTrigger.Count - 1; i >= 0; i--)
				{
					var boxUUID = effectBoxesToTrigger[i];
					var effectBox = gameController.getSerializedObject<EffectBox>(boxUUID);

					if (effectBox == null || effectBox.hasBeenKilled)
					{
						// the box has been invalidated
						manager.RemoveBoxUUID(entryIndex, boxUUID);
						continue;
					}


					EffectBoxUtils.TriggerBox(effectBox);
				}

			}
		}

		internal void RemoveBoxUUIDLocal(uint boxUUID)
		{
			var index = effectBoxesToTrigger.FindIndex(uuid => uuid == boxUUID);

			if (index == -1)
				return;

			effectBoxesToTrigger.RemoveAt(index);
		}

		private void DoNotificationLogic(int currentTimeModulus)
		{
			var notificationTime = triggerTime - secondBetweenNotificationAndStart;

			if (notificationTime < 0)
				notificationTime += totalModulus;

			if (!hasTriggeredNotification && currentTimeModulus >= notificationTime)
			{
				UnityEngine.Vector3? location = null;

				if (effectBoxesToTrigger.Count > 0)
				{
					var box = GameController.Instance.getSerializedObject<EffectBox>(effectBoxesToTrigger[0]);
					if (box != null)
						location = box.getPositionForSerialization();
				}

				hasTriggeredNotification = true;
				NotificationBar.Instance.addNotification(new Notification("Show time", displayName + " is about to start!", Notification.Type.DEFAULT, location));
				return;
			}

			if (currentTimeModulus < notificationTime)
				hasTriggeredNotification = false;
		}


		public void Serialize(Dictionary<string, object> values)
		{
			values.Add(nameof(displayName), displayName);
			values.Add(nameof(effectBoxesToTrigger), effectBoxesToTrigger);
			values.Add(nameof(totalModulus), totalModulus);
			values.Add(nameof(triggerTime), triggerTime);
			values.Add(nameof(hasTriggered), hasTriggered);
			values.Add(nameof(hasTriggeredNotification), hasTriggeredNotification);
			values.Add(nameof(triggerNotification), triggerNotification);
			values.Add(nameof(secondBetweenNotificationAndStart), secondBetweenNotificationAndStart);
		}

		public void Deserialize(Dictionary<string, object> values)
		{
			effectBoxesToTrigger.Clear();
			if (values.ContainsKey(nameof(displayName)))
				displayName = (string)values[nameof(displayName)];
			if (values.ContainsKey(nameof(effectBoxesToTrigger)))
			{
				var boxes = (List<object>)values[nameof(effectBoxesToTrigger)];
				
				foreach (var box in boxes)
				{
					effectBoxesToTrigger.Add((uint)(long)box);
				}
			}
			if (values.ContainsKey(nameof(totalModulus)))
			{
				var v = values[nameof(totalModulus)];
				totalModulus = (int)(long)values[nameof(totalModulus)];
			}
			if (values.ContainsKey(nameof(triggerTime)))
				triggerTime = (int)(long)values[nameof(triggerTime)];
			if (values.ContainsKey(nameof(hasTriggered)))
				hasTriggered = (bool)values[nameof(hasTriggered)];
			if (values.ContainsKey(nameof(hasTriggeredNotification)))
				hasTriggeredNotification = (bool)values[nameof(hasTriggeredNotification)];
			if (values.ContainsKey(nameof(triggerNotification)))
				triggerNotification = (bool)values[nameof(triggerNotification)];
			if (values.ContainsKey(nameof(secondBetweenNotificationAndStart)))
				secondBetweenNotificationAndStart = (int)(long)values[nameof(secondBetweenNotificationAndStart)];
		}

	}
}

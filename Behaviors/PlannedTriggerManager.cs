using HateMeLikeAPro.EffectExtensions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static StarConstellation;

namespace HateMeLikeAPro.EffectExtensions.Behaviors
{
	public class PlannedTriggerManager
	{
		public readonly Dictionary<Guid, PlannedTriggerEntry> plannedTriggerEntries = new Dictionary<Guid, PlannedTriggerEntry>();

		public void UpdateManager(GameController gameController)
		{
			foreach(var kvp in plannedTriggerEntries)
			{
				kvp.Value.UpdatePlannedTrigger(gameController, this, kvp.Key);
			}
		}


		public void RemoveBoxUUID(Guid entryKey, uint boxUUID)
		{
			CommandController.Instance.addCommand(new RemoveBoxUUIDCommand(entryKey, boxUUID));
		}

		internal void RemoveBoxUUIDLocal(Guid entryKey, uint boxUUID)
		{
			if (plannedTriggerEntries.TryGetValue(entryKey, out var plannedTrigger))
			{
				plannedTrigger.RemoveBoxUUIDLocal(boxUUID);
			}
		}

		public PlannedTriggerEntry GetEntry(Guid guid)
		{
			plannedTriggerEntries.TryGetValue(guid, out var entry);

			return entry;
		}

		public void NewEntry()
		{
			NewEntry(Guid.NewGuid());
		}
		public void NewEntry(Guid guid)
		{
			var entryName = $"Planned trigger {plannedTriggerEntries.Count + 1}";

			UpdateEntry(guid, entryName, 3600, 3540, new List<uint>(), false, 10);
		}

		public void UpdateEntry(Guid guid, string entryName, int timeModulus, int triggerTime, List<uint> effectBoxesToTrigger, bool triggerNotification, int secondBetweenNotificationAndStart)
		{
			CommandController.Instance.addCommand(new UpdatePlannedTriggerCommand(guid, entryName, timeModulus, triggerTime, effectBoxesToTrigger, triggerNotification, secondBetweenNotificationAndStart));
		}

		internal void HandleUpdatePacket(UpdatePlannedTriggerCommand packet)
		{
			var guid = Guid.Parse(packet.Guid);
			if (!plannedTriggerEntries.ContainsKey(guid))
				plannedTriggerEntries.Add(guid, new PlannedTriggerEntry());

			var entry = plannedTriggerEntries[guid];
			entry.displayName = packet.Name;
			entry.totalModulus = packet.TimeModulus;
			entry.triggerTime = packet.TriggerTime;
			entry.effectBoxesToTrigger.Clear();
			foreach (var box in packet.EffectBoxesToTrigger)
				entry.effectBoxesToTrigger.Add(box);

			entry.triggerNotification = packet.TriggerNotification;
			entry.hasTriggeredNotification = entry.hasTriggered;
			entry.secondBetweenNotificationAndStart = packet.SecondBetweenNotificationAndStart;
		}

		public Dictionary<string, object> Serialize()
		{
			var dict = new Dictionary<string, object>();


			var entriesList = new List<Dictionary<string, object>>();

			/*foreach (var entry in entries)
			{
				var entryObject = new Dictionary<string, object>();

				entry.Serialize(entryObject);
				entriesList.Add(entryObject);
			}


			dict.Add(nameof(entries), entriesList);*/

			foreach (var kvp in plannedTriggerEntries)
			{
				var entryObject = new Dictionary<string, object>();
				entryObject.Add("guid", kvp.Key.ToString());
				kvp.Value.Serialize(entryObject);
				entriesList.Add(entryObject);
			}

			dict.Add(nameof(plannedTriggerEntries), entriesList);

			return dict;
		}


		public void Deserialize(Dictionary<string, object> values)
		{
			/*entries.Clear();
			//var valueRaw = MiniJSON.Json.Deserialize(str);

			if (values.TryGetValue(nameof(entries), out object entriesRaw))
			{
				if (entriesRaw is Dictionary<string, object> entryDictionary)
				{
					var plannedTrigger = new PlannedTriggerEntry();
					plannedTrigger.Deserialize(entryDictionary);
					entries.Add(plannedTrigger);
				}
			}*/

			plannedTriggerEntries.Clear();
			if (values.TryGetValue(nameof(plannedTriggerEntries), out object entriesRaw))
			{
				if (entriesRaw is List<object> entriesList)
				{
					foreach (var entry in entriesList)
					{
						if (entry is Dictionary<string, object> entryDictionary)
						{
							var plannedTrigger = new PlannedTriggerEntry();
							plannedTrigger.Deserialize(entryDictionary);
							var triggerGuidRaw = entryDictionary["guid"];

							if (!(triggerGuidRaw is string triggerGuidString))
							{
								Debug.Log($"The Trigger '{plannedTrigger.displayName}' contained an invalid guid, skipping!");
								continue;
							}

							plannedTriggerEntries.Add(Guid.Parse(triggerGuidString), plannedTrigger);
						}
					}
				}
			}
		}
	}
}

using HateMeLikeAPro.EffectExtensions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.Commands
{
	public class UpdatePlannedTriggerCommand : OrderedBufferedCommand
	{
		[SerializeField]
		public string Guid;
		[SerializeField]
		public string Name;
		[SerializeField]
		public int TimeModulus;
		[SerializeField]
		public int TriggerTime;
		[SerializeField]
		public List<uint> EffectBoxesToTrigger;
		[SerializeField]
		public bool TriggerNotification;
		[SerializeField]
		public int SecondBetweenNotificationAndStart;

		public UpdatePlannedTriggerCommand(Guid guid, string name, int timeModulus, int triggerTime, List<uint> effectBoxesToTrigger, bool triggerNotification, int secondBetweenNotificationAndStart)
		{
			Guid = guid.ToString();
			Name = name;
			TimeModulus = timeModulus;
			TriggerTime = triggerTime;
			EffectBoxesToTrigger = effectBoxesToTrigger;
			TriggerNotification = triggerNotification;
			SecondBetweenNotificationAndStart = secondBetweenNotificationAndStart;
		}

		public UpdatePlannedTriggerCommand()
		{
		}

		public override void run()
		{
			EffectControllerBehavior.Instance.PlannedTriggerManager.HandleUpdatePacket(this);
		}

		public override bool triggerOnSend()
		{
			return true;
		}

		public override bool triggerOnReceive()
		{
			return true;
		}
	}
}

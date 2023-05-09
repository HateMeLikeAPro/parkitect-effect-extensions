using HateMeLikeAPro.EffectExtensions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.Commands
{
	public class RemoveTriggerEntryCommand : OrderedBufferedCommand
	{
		[SerializeField]
		public string guidString;

		public override void run()
		{
			Guid guid = Guid.Parse(guidString);

			EffectControllerBehavior.Instance.PlannedTriggerManager.plannedTriggerEntries.Remove(guid);
		}

		public override bool triggerOnSend()
		{
			return true;
		}

		public override bool triggerOnReceive()
		{
			return true;
		}

		public RemoveTriggerEntryCommand()
		{

		}

		public RemoveTriggerEntryCommand(Guid guid)
		{
			guidString = guid.ToString();
		}
	}
}
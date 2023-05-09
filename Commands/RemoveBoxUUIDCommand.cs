using HateMeLikeAPro.EffectExtensions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.Commands
{
	public class RemoveBoxUUIDCommand : OrderedBufferedCommand
	{

		[SerializeField]
		public string entryKeyString;
		[SerializeField]
		public uint boxUUID;

		public override void run()
		{
			EffectControllerBehavior.Instance.PlannedTriggerManager.RemoveBoxUUIDLocal(Guid.Parse(entryKeyString), boxUUID);
		}

		public override bool triggerOnSend()
		{
			return true;
		}

		public override bool triggerOnReceive()
		{
			return true;
		}

		public RemoveBoxUUIDCommand()
		{

		}

		public RemoveBoxUUIDCommand(Guid entryKey, uint boxUUID)
		{
			this.entryKeyString = entryKey.ToString();
			this.boxUUID = boxUUID;
		}


	}
}

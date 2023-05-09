using HateMeLikeAPro.EffectExtensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.Commands
{
	public class TriggerEffectBoxCommand : OrderedBufferedCommand
	{
		[SerializeField]
		private uint effectBoxId;

		public override void run()
		{
			var effectBox = GameController.Instance.getSerializedObject<EffectBox>(effectBoxId);
			EffectBoxUtils.TriggerBox(effectBox);
		}

		public override bool triggerOnSend()
		{
			return true;
		}

		public override bool triggerOnReceive()
		{
			return true;
		}


		public override uint getContextId()
		{
			return effectBoxId;
		}

		public TriggerEffectBoxCommand()
		{
		}

		public TriggerEffectBoxCommand(uint effectBoxId)
		{
			this.effectBoxId = effectBoxId;
		}
	}
}

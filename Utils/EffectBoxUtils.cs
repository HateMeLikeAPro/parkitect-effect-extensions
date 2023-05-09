using HateMeLikeAPro.EffectExtensions.Behaviors;
using HateMeLikeAPro.EffectExtensions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace HateMeLikeAPro.EffectExtensions.Utils
{
	public static class EffectBoxUtils
	{
		public static void TriggerBox(EffectBox effectBox)
		{

			if (effectBox.effectRunner.state == EffectRunner.State.STOPPED)
			{
				if (effectBox.effectRunner != null && effectBox.effectRunner.state == EffectRunner.State.STOPPED)
					effectBox.effectRunner.triggerPlay();
			}
		}
	}
}

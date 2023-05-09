using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions.UI
{
	public abstract class BaseUI
	{
		public readonly int uiIdentifier;
		protected Rect windowRect;
		protected readonly Rect windowTitleBar;
		protected string windowName;


		protected BaseUI(int uiIdentifier, Rect windowRect, string windowName)
		{
			this.uiIdentifier = uiIdentifier;
			this.windowRect = windowRect;
			this.windowTitleBar = new Rect(0, 0, windowRect.width, 20);
			this.windowName = windowName;
		}

		public void DrawUI()
		{
			windowRect = GUILayout.Window(uiIdentifier, windowRect, DrawWindow, windowName);			
		}

		protected void DrawWindow(int windowIdentifier)
		{
			GUI.BeginGroup(new Rect(0, 0, windowRect.width, windowRect.height));

			DrawContent();

			GUI.EndGroup();
			GUI.DragWindow(windowTitleBar);
		}



		protected abstract void DrawContent();

		public virtual void OnClose()
		{ }
	}
}

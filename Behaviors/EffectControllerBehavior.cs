using HateMeLikeAPro.EffectExtensions.Commands;
using HateMeLikeAPro.EffectExtensions.UI;
using Parkitect.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace HateMeLikeAPro.EffectExtensions.Behaviors
{
	public class EffectControllerBehavior : SerializedMonoBehaviour
	{
		public static EffectControllerBehavior Instance;


		private bool _isPrefab = false;
		private bool _loaded = false;

		public readonly PlannedTriggerManager PlannedTriggerManager = new PlannedTriggerManager();

		private PlannedTriggerManagerUI triggerManagerUI;
		private PlannedTriggerEntryUI triggerEntryUI;


		internal float plannerX = 0;
		internal float plannerY = 0;

		internal float editorX = 0;
		internal float editorY = 0;


		public EffectControllerBehavior()
		{

		}

		public void Update()
		{
			if (_isPrefab || !_loaded)
				return;

			if (InputManager.getKeyDown(MainMod.KEY_TRIGGER_EFFECTBOX))
				handleEffectBoxTriggerKey();


			if (InputManager.getKeyDown(MainMod.KEY_TRIGGER_EFFECT_PLANNER))
				handleEffectPlannerKey();


			PlannedTriggerManager.UpdateManager(gameController);
		}

		private void handleEffectPlannerKey()
		{
			if (triggerManagerUI == null)
			{
				triggerManagerUI = new PlannedTriggerManagerUI();
			}
			else
			{
				CloseMainWindow();
			}
		}

		private void handleEffectBoxTriggerKey()
		{
			var hitInfo = Utility.getObjectBelowMouse<EffectBox>();

			if (hitInfo.hitSomething && hitInfo.hitObject is EffectBox effectBox)
			{
				if (triggerEntryUI == null)
				{
					CommandController.Instance.addCommand(new TriggerEffectBoxCommand(effectBox.objectID));
					return;
				}

				triggerEntryUI.HandleEffectBox(effectBox);
			}
		}




		public void OnGUI()
		{
			if (!_loaded)
				return;

			if (triggerManagerUI != null)
				triggerManagerUI.DrawUI();

			if (triggerEntryUI != null)
				triggerEntryUI.DrawUI();
		}

		public void OnStartPlayingPark()
		{
			if (_isPrefab)
				return;

			/*if (GameController.Instance.isInScenarioEditor)
				return;*/

			if (_loaded)
				Unload();


			GameController.Instance.addSerializedObject(this);

			_loaded = true;
		}

		private void Unload()
		{
			if (_isPrefab)
				return;

			_loaded = false;
		}

		public override void serialize(SerializationContext context, Dictionary<string, object> values)
		{
			if (_isPrefab)
				return;

			base.serialize(context, values);

			try
			{
				values.Add(nameof(PlannedTriggerManager), PlannedTriggerManager.Serialize());
				values.Add(nameof(editorX), editorX);
				values.Add(nameof(editorY), editorY);
				values.Add(nameof(plannerX), plannerX);
				values.Add(nameof(plannerY), plannerY);
			}
			catch (Exception ex)
			{
				Debug.LogError("Something went wrong saving our data...");
				Debug.LogException(ex);
			}
		}

		public override void deserialize(SerializationContext context, Dictionary<string, object> values)
		{
			if (_isPrefab)
				return;

			base.deserialize(context, values);


			try
			{
				if (values.TryGetValue(nameof(PlannedTriggerManager), out var tmp))
				{

					if (tmp is Dictionary<string, object> dict)
						PlannedTriggerManager.Deserialize(dict);

				}

				if (values.TryGetValue(nameof(editorX), out var raw))
					editorX = (float)(double)raw;
				if (values.TryGetValue(nameof(editorY), out var raw2))
					editorY = (float)(double)raw2;

				if (values.TryGetValue(nameof(plannerX), out var raw3))
					plannerX = (float)(double)raw3;
				if (values.TryGetValue(nameof(plannerY), out var raw4))
					plannerY = (float)(double)raw4;

			}
			catch (Exception ex)
			{
				Debug.LogError("Something went wrong loading our data...");
				Debug.LogException(ex);
			}

			EventManager.Instance.OnStartPlayingPark -= Instance.OnStartPlayingPark;
			Instance.Kill();
			Instance = this;
			EventManager.Instance.OnStartPlayingPark += OnStartPlayingPark;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		public void SetAsPrefab()
		{
			_isPrefab = true;
		}


		public void OpenEditWindow(Guid guid)
		{
			if (triggerEntryUI == null)
			{

				triggerEntryUI = new PlannedTriggerEntryUI(guid);
			}
		}

		public void CloseEditWindow(Guid guid)
		{
			if (triggerEntryUI != null && guid.Equals(triggerEntryUI.guid))
			{
				triggerEntryUI.OnClose();
				triggerEntryUI = null;
			}
		}

		public void CloseMainWindow()
		{
			if (triggerManagerUI != null)
			{
				triggerManagerUI.OnClose();
				triggerManagerUI = null;
			}
		}
	}
}

using HateMeLikeAPro.EffectExtensions.Behaviors;
using HateMeLikeAPro.EffectExtensions.Commands;
using UnityEngine;

namespace HateMeLikeAPro.EffectExtensions
{
	public class MainMod : AbstractMod
	{

		private EffectControllerBehavior effectControllerBehaviorPrefab;

		private GameObject _controllerGO;

		public const string MOD_ID = "hatemelikeapro.effect-extensions";

		public const string KEY_GROUP_ID = MOD_ID + ".keygroup";

		public const string KEY_TRIGGER_EFFECTBOX = MOD_ID + ".trigger-effectbox";
		public const string KEY_TRIGGER_EFFECT_PLANNER = MOD_ID + ".trigger-effect-planner";


		public MainMod()
		{
			RegisterKeyBindings();
		}



		public override void onDisabled()
		{
			base.onDisabled();

			if (EffectControllerBehavior.Instance != null)
			{
				EventManager.Instance.OnStartPlayingPark -= EffectControllerBehavior.Instance.OnStartPlayingPark;
				EffectControllerBehavior.Instance.TryKill();
			}

			AssetManager.Instance.unregisterObject(effectControllerBehaviorPrefab);
			effectControllerBehaviorPrefab.TryKill();
		}

		public override void onEnabled()
		{
			base.onEnabled();

			var controllerPrefabGO = new GameObject();
			effectControllerBehaviorPrefab = controllerPrefabGO.AddComponent<EffectControllerBehavior>();
			effectControllerBehaviorPrefab.SetAsPrefab();
			AssetManager.Instance.registerObject(effectControllerBehaviorPrefab);

			_controllerGO = new GameObject();
			EffectControllerBehavior.Instance = _controllerGO.AddComponent<EffectControllerBehavior>();

			//Debug.Log("Hello world from B's mod!");
			EventManager.Instance.OnStartPlayingPark += EffectControllerBehavior.Instance.OnStartPlayingPark;

			CommandController.Instance.registerCommandType<TriggerEffectBoxCommand>();
			CommandController.Instance.registerCommandType<RemoveBoxUUIDCommand>();
			CommandController.Instance.registerCommandType<RemoveTriggerEntryCommand>();
			CommandController.Instance.registerCommandType<UpdatePlannedTriggerCommand>();
		}





		private void CreateKeyGroup(string name)
		{
			var keyGroup = new KeyGroup(getIdentifier() + ".keygroup");
			keyGroup.keyGroupName = "Effect Extensions keys";
			InputManager.Instance.registerKeyGroup(keyGroup);
		}

		private void CreateKeyBind(string identifier, string groupIdentifier, string name, KeyCode defaultKey, string description = "", KeyCode fixedKey = KeyCode.None)
		{
			var keyBind = new KeyMapping(identifier, defaultKey, fixedKey);
			keyBind.keyName = name;
			keyBind.keyDescription = description;
			keyBind.keyGroupIdentifier = groupIdentifier;
			InputManager.Instance.registerKeyMapping(keyBind);
		}

		private void RegisterKeyBindings()
		{
			CreateKeyGroup(KEY_GROUP_ID);

			CreateKeyBind(KEY_TRIGGER_EFFECTBOX, KEY_GROUP_ID, "Trigger EffectBox", KeyCode.None, "starts the effectbox you're currently pointing at (if any)");
			CreateKeyBind(KEY_TRIGGER_EFFECT_PLANNER, KEY_GROUP_ID, "Open effect planner", KeyCode.None, "opens the effect planner window");
		}






		public override string getDescription()
		{
			return "HateMeLikeAPro's effect extensions mod. Source and documentation are on my GitHub";
		}

		public override string getIdentifier()
		{
			return MOD_ID;
		}

		public override string getName()
		{
			return "Effect Extensions";
		}

		public override string getVersionNumber()
		{
			return "1.2";
		}

		public override bool isMultiplayerModeCompatible()
		{
			return true;
		}

		public override bool isRequiredByAllPlayersInMultiplayerMode()
		{
			return true;
		}
	}
}

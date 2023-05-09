# How to use this mod

This mod is not straight forward to use, but quite simple. Its UI will most likely change if I get the opportunity 👀

## Trigger EffectBox
When you press this hotkey, the mod will check if you are pointing at an effect box. If so this effect box will start running. This will be a global trigger; in a multiplayer game everyone will see the effect box start. **This behavior changes when you open the `Editor window`**

## Open Effect Planner
When you press this hotkey, the effect planner window will get toggled. Allowing you to add, remove and edit planned effects.

### Editor window
When you edit a planned effect, this window will show up. To add or remove effect boxes from this planned effect, you will need to point at (use your cursor to hover over an effect box, do not click or select) the effect box and use the `Trigger EffectBox` hotkey. This will not start playing the effect box but rather add or remove it from our current planned trigger.

Make sure you press save otherwise the changes are lost.
## Field description
### Name
The display name of this planned effect, will be used for notifications

### Total time modulus
The total time modulus to use in seconds. `3600` means once every year, `7200` means once every 2 years. **Make sure** this number is not to low.

### Trigger time
The time at which this planned effect will trigger in seconds. Using a `total time modulus` of `3600` and a `trigger time` of `1800` will make it trigger the 1st of june.

### Receive notification
If enabled this planned effect will create a notification when the show is about to start. Click on the notification to pan directly to the **first** effect box that is listed with this entry.

### Heads up
The amount of heads up we get in seconds. This is used only when `receive notification` is enabled and will create a notification this amount of seconds **before** the actual effect boxes will trigger.

### Effect box list
After the bit of text there will follow a list (empty by default.) Here it will show you all the effect boxes (their 'customized name' and 'objectID') that are part of this planned effect.
		

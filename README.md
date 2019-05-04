![Fluid Hierarchical Task Network](https://i.imgur.com/xKfIV0f.png)
# Fluid Troll Bridge
A simple Unity example that takes advantage of the planning capabilities of the [Fluid Hierarchical Task Network](https://github.com/ptrefall/fluid-hierarchical-task-network) planner.

## Disclaimer
Big thank you to [SythianCat](https://assetstore.unity.com/publishers/21747) and [Dungeon Mason](https://assetstore.unity.com/publishers/23554) for allowing me to use their assets in the development of this example.

## About
The example sets up a scenario where a troll patrols two bridges and humans are nearby. The focus of the example is to show how useful it can be to customize the domain builder, effects, conditions and operators to the particular needs of a game. We also strive to set up a decent framework around the agents, with modular sensors and monster-agnostic APIs that all the agents in the game can share most, if not all, of the AI features we customize for it.

This is just a simple example that show 'a' way to use the planner.

This example will improve over time.

![Screenshot](https://i.imgur.com/ihO9WC0.png)

## Technical
The example contains multiple convenient generic classes set up around Fluid HTN, to easily add new types of AI Agents into the game, and specialize their behavior, while sharing as much code as possible.

### Connecting the project with Fluid HTN
To get the project to speak with Fluid HTN, the proposed method is to link to it via the Package Manager. To do so, open fluid-troll-bridge/Unity/Packages/manifest.json. At the top of the json file you should see an entry for fluid.htn. Make sure this points to the folder where you cloned Fluid HTN.

Unity will automatically detect a change to this json file and hot-load its changes, and you should now have Fluid HTN import into your project.

### AI Agent
The AIAgent class is the main brain of the agent implementation that brings it all together. It will instantiate all the HTN specific classes.

### AI Domain Definition
The AIDomainDefinition is an abstract ScriptableObject. The idea was that each type of AI Domain would have to overlead this common abstract class, so that we have a generic API to create domains from. There is the Human Domain Definition and Troll Domain Definition.

#### Human Domain Definition
Here we define the HTN Domain of the Human. In order of priority, highest first, the human can receive damage. When he gets tired he will pause for 2 seconds. If the human has enemies in sight, he will find the best enemy among the known ones, then proceed to attack it, or walk up to the enemy if he's not close enough to hit it. Finally he will be moving to the best bridge available that he has in sight as his idle behavior, and pause for 2 seconds once he get there.

#### Troll Domain Definition
Here we define the HTN Domain of the Troll, or Golem if you prefer. In order of priority, highest first, the troll can receive damage, or he will move to the best bridge available that he has in sight as his idle behavior, and pause for 2 seoncds once he get there. It is an excersize for you to extend his behavior to fight back against the human.

### AI Context
We have implemented an AIContext on top of the BaseContext class. Its implemented as a partial to separate the overrides it does over BaseContext and special method extensions for state handling, and the data specific to our example. Note how we only need to store state inside of WorldState that is relevant to predicting the future. Thus lists of known objects, general getters, etc can be simply stored as a blackboard or whatever else is preferable. Here we just opt for a simple list of properties caching what we need to access at runtime.

### AI World State
Here we define the enum of our world state, used by the planner to store both the current state and the predicted state of the future. We also define a simple destination target enum here, that helps us add hints to where we want to go when a MoveTo operator is invoked in the HTN domain.

### AI Domain Builder
We have extended the base domain builder with a range of custom conditions, effects and operators. These makes the domains we write much easier to read, and more convenient to design with. The flexibility of the domain builder is such that we can encapsulate as much as we want into our extensions, making re-use of certain methods much easier, and reduce duplication. E.g. we can encapsulate an entire Action, with its internal conditions, effects and operator, and an End() call, before we return back out. How you decide to make extensions like these all depends on how you see its intended use. It can be more flexible to not call End() in these encapsulated methods, as that will allow the user to add more conditions and effects externally, but then they must remember to call End() on their own.

Here are some of the more interesting behavior extensions to the builder:

#### Attack Enemy
Starts an Action, checks whether we're in the state HasEnemyInMeleeRange, sets the AttackOperator, then applies the IncrementState effect over the Stamina state during planning and execution. This ensures that the attack enemy behavior will only be used when we are in melee range of the enemy, and it cost us 1 stamina point to do it. Finally we End before returning the builder.

#### Be Tired
Takes in a rest time, which is the requried duration to wait while being tired and starts an Action. We check whether the Stamina state is greater than 2, then use the WaitOperator with the rest time as input for the action's operator. Finally we reset the Stamina state to 0. This means that every third attack the attacker gets tired and needs to rest. Finally we End before returning the builder.

#### Move To Enemy
We start an Action, then sets the MoveToOperator with Enemy as destination type. We proceed with applying the SetState effect over the HasEnemyInMeleeRange state during planning and execution. We know that if the MoveTo operator ends successfully, that we are automatically in Melee Range, so we don't have to wait for the Melee Range sensor to catch up with this information (that sensor is actually more important for when we're no longer in range). Finally we End before returning the builder. 

#### Received Damage
We start a new Action, then proceed to check the condition whether we have received damage. If this is true we apply the TakeDamage operator. We apply the SetState effect when the operator returns successfully, with the intention of changing the HasReceivedDamage state back to false. Finally we End before returning the builder.

### Custom Conditions
Since we know our world state types now, we can write some convenience conditions for use with our domain definitions. We write a Has World State and Has World State Greater Than condition. These just allow us to conveniently check against the state of World State entries.

### Custom Effects
Since we know our world state types now, we can write some convenience conditions for use with our domain definitions. We write Set World State and Increment World State effects. These just allow us to conveniently modify the value of World State entries.

### Custom Operators
With custom operators we have the opportunity to encapsulate small modules of game-logic inside of operator classes. We write Attack, FindBridge, FindEnemy, MoveTo, TakeDamage, and Wait operators.

#### Attack Operator
This operator simply triggers an animation, extracts time information about the animation, and will return task-status Continue until the time duration of the animation has played itself out, we then return task-status Success. We also set the CanSense flag on the context, which effectively prevents the AIAgent from updating the sensors of the agent. We do this to ensure that an Attack operator can't cancel out, because we really want to always trigger the effects of the attack action, and besides, we never want a triggered attack animation to stop mid-attack, we always want it to complete.

If we didn't set CanSense to false here, as our enemy moves away and out of melee range, this would cause a replan, and the planner would find that we'd now have to move to our enemy in order to get into melee range again, and thus effectively stop/cancel the attack operator. This would in turn prevent the effects associated with the action from getting applied (we only apply effects after a task-status of Success).

#### Find Bridge Operator
This operator simply iterates over the list of known bridges, and picks the best one. We choose the best one by checking which bridge was patrolled the longest ago. We could certainly store the information about the bridge visit timer better, because now the Huamn and Troll will contend for which bridge to control. This should probably be stored in the AIContext instead, or perhaps be set per Mobile type. If we have any known bridges, we set the HasBridgesInSight world state.

#### Find Enemy Operator
This operator simply iterates over the list of known enemies, and picks the best one. We choose the best one by checking the agent's distance to the enemy. If we have any known enemies, we set the HasEnemyInSight world state.

#### Move To Operator
This operator use the Nav Agent API to move the agent toward a destination. It translates the DestinationType set on the operator instance, and deals with bridges and enemies accordingly. Since a bridge is a static target, we only set its destination once, while an enemy is a dynamic target, so we have to continusouly set destination to its position while we try to complete the path. We use the stop distance of the Nav Agent to determin when the operator should return successfully, thus make sure your stopping distance isn't set to 0 on the Nav Agent (or alternatively add a bit of a margin in this operator).

#### Take Damage Operator
This operator is quite similar to Attack operator. We trigger an animation, extract the duration time for the animation and return success when the duration time has passed.

#### Wait Operator
A very simple operator that sets the GenericTime in context and return continue until the duration of time is complete, then we return success.

### Sensory
We implement a simple, modular sensory system where each sensor type is added to the respective agent gameobject. The sensory system will automatically detect these and use them for sensing.

#### Bridge Sensor
Uses overlap sphere to find bridges in sight range.

#### Mobile Sensor
Uses overlap sphere to find all Mobiles in sight range, then identifies which of those mobiles are friends and enemies.

#### Melee Range Sensor
Checks whether our current enemy is in melee range.

## Support
Join the [discord channel](https://discord.gg/MuccnAz) to share your experience and get support on the usage of Fluid HTN.

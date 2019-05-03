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

## Support
Join the [discord channel](https://discord.gg/MuccnAz) to share your experience and get support on the usage of Fluid HTN.

# TuringMachine

A Turing machine library that allows creating machines with user defined state and symbol types. Users can define their own machine types with customized postcondition checks and able to track computation progress.

Setting up the machine happens both declaratively and progmaticcaly. Transitions are defined declaratively while tracking computation progress initialization happens by subscribing to events or examining machine's properties.

The project uses .NET Core 3.1 and C# 8.0 for nullable reference types.

### Goals in order:
- [ ] Single tape machine with tracking computation progress and postconditions
- [ ] Multi tape machine
- [ ] Customizable postcondition definitions
- [ ] Simulating activities by machine, *this may never be implemented if the usage is too complicated to be user friendly*
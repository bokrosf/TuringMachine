# TuringMachine

A Turing machine library that allows creating machines with user defined state and symbol types. Users can define their own machine types with customized postcondition checks and able to track computation progress.

Setting up the machine happens both declaratively and progmaticcaly. Transitions are defined declaratively while tracking computation progress initialization happens by subscribing to events or examining machine's properties.

The project uses .NET 5 and C# 9.0 for nullable reference types.

### Goals in order:
- [x] Single tape machine with computation progress tracking and postcondition enforcement
- [x] Customizable postcondition definitions
- [ ] Multi tape machine
- [ ] Simulating activities by machine, *this may never be implemented if the usage is too complicated to be user friendly*

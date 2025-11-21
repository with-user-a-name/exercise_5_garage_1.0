
# C# Exercise 5 - Garage 1.0

**Note:** The result of the exercise must be shown to a teacher and approved before it can be considered completed.

## An Initial Overview Project

To integrate much of what you have learned, we will now build a garage application. This application should provide the functionality that a system might need to simulate a simple garage. This includes the ability to park vehicles, retrieve vehicles, check which vehicles are available, and what characteristics they have—all within a console application featuring a main menu and submenus.

The reason for programming a garage is that it is easy to anchor the division within the entire application. We can primarily divide a garage into the following parts:

- **The Garage:** A representation of the building itself. The garage is a place where a variety of vehicles can be stored. It could thus be represented as a collection of vehicles.
- **Vehicles:** Cars, motorcycles, unicycles, or any type of vehicle one wishes to place in the garage.

These are the two "object types" observed in a physical garage. However, upon closer inspection, there should also be subclasses for each type of vehicle, meaning that every vehicle type represents its own subclass in the system. In addition, we require functionality that handles parking vehicles in the garage, retrieving vehicles from the garage, and presenting what is available in the garage along with search capabilities.

In more programming-friendly terms, we should have at a **minimum**:

* A collection of vehicles; the class `Garage`.
* A vehicle class, the class `Vehicle`.
* Several subclasses for vehicles.
* A user interface that allows us to utilize the functionality of a garage. All interaction with the user occurs here.
* A `GarageHandler`. To abstract a layer such that there is no direct contact between the user interface and the garage class. This is ideally achieved through a class that manages the functionality the interface needs to access.
* We do not program directly against concrete types, so we utilize Interfaces for that, e.g., `IUI`, `IHandler`, `IVehicle`. (A tip is to extract to interfaces once the implementation is complete if you find this part challenging.)

---

## Requirements Specification

The vehicles should be implemented as the class `Vehicle` and its subclasses.

* `Vehicle` contains all the properties that should be present in all vehicle types, e.g., registration number, color, number of wheels, and other characteristics you can think of.

* The registration number must be unique.

* There must be at least the following subclasses:
  - `Airplane`
  - `Motorcycle`
  - `Car`
  - `Bus`
  - `Boat`

* Each of these should implement at least one unique property, e.g.:
  - *Number of Engines*
  - *Cylinder Volume*
  - *Fuel Type (Gasoline/Diesel)*
  - *Number of Seats*
  - *Length*

The garage itself should be implemented as a generic collection of vehicles:

`class Garage<T>`

Additionally, the generic type should be constrained using a constraint:

`class Garage<T> where ...`

Furthermore, it should be possible to iterate over an instance of Garage using foreach. This means that Garage must implement the generic version of the interface IEnumerable:

`class Garage<T> : ....`

The class does not need to inherit from any other class or implement any other interface.

The collection of vehicles should be internally managed as an array within the class. The internal array must be `private`. When instantiating a new garage, the capacity must be specified as an argument to the constructor.

We must **NOT** use a `List<Vehicle>` internally in the Garage class!

---

## Functionality

The following features should be available:

* List all parked vehicles
* List vehicle types and how many of each are in the garage
* Add and remove vehicles from the garage
* Set a capacity (number of parking spaces) when instantiating a new garage
* Ability to populate the garage with a number of vehicles from the start
* Locate a specific vehicle by registration number. It should work with both ABC123 as well as Abc123 or AbC123.
* Search for vehicles based on one or more properties (all possible combinations from the base class `Vehicle`). For example:
  - *All black vehicles with four wheels.*
  - *All pink motorcycles with three wheels.*
  - *All trucks*
  - *All red vehicles*
* Users should receive feedback on whether actions were successful or not. For instance, when a vehicle is parked, we want confirmation that it has been parked. If it cannot be parked, the user should be informed why.

The program will be a console application with a text-based user interface, allowing users to:

* Navigate to **all** functionalities from the garage through the interface
* Create a garage with a user-specified size
* Shut down the application via the interface

The application should robustly handle input errors, ensuring it does **not crash** due to incorrect input or usage.

---

## Unit Testing

Tests should be created in a separate test project. We will focus on testing the public methods in the `Garage` class. (Writing tests for the entire application is considered an additional task if time permits.)

Feel free to write the tests before implementing the functionality! Then use `<ctrl>+'.'` to generate your objects and methods. Move these generated classes to the appropriate project. After that, implement the functionality until the tests pass.

Structure the tests according to the following principles:
1. **Arrange**: Set up what will be tested, instantiate objects, and define inputs.
2. **Act**: Call the method that will be tested.
3. **Assert**: Verify that you receive the expected results.

Also, consider naming the tests descriptively. When a test fails, we want to know what didn't work just by looking at the test method name. 

For example:

**[MethodName_StateUnderTest_ExpectedBehavior]**

**Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()**

---

## Suggested Additional Functionality (Not Requirements)

The ability to also search based on vehicle-specific properties.

Manage multiple garages that can contain different types of vehicles, for example, a hangar, a standard garage, and a motorcycle garage.
This will entail being able to navigate between the different garages in the UI to perform the previously mentioned functions, which should occur only in the selected garage.
It should be clearly displayed which garage you are currently working with.

A garage will no longer consist merely of vehicles but of parking spaces that can hold vehicles.

You can write to and read from the file system using C#. Find out how to save your garage (via a menu option or automatically upon shutdown) and load your garage (via a menu option or automatically at application startup).

Different vehicles take up varying amounts of space; for example, a car takes 1 space, a boat takes 2 spaces, an airplane requires 3 spaces, and a motorcycle only takes 1/3 of a space.

When parking, only the vehicles that the garage has room for should be displayed as options.

Read the size of the garage from a configuration file.

Any additional functionality you believe should be included.

---

# Good luck!

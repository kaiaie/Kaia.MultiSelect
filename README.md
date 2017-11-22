
# About

This is a proof-of-concept derived from an old PHP project. One of the shortcomings of a lot of CRUD apps is the 
inability to edit multiple records at once, a task which is easily accomplished in SQL or even a spreadsheet. This project 
comprises a rough outline of an approach that allows multi-record editing, as well as being able to duplicate

The project uses a SQLite3 database implementing the simple product/ suppliers model used in Chris Date's *An Introduction to 
Database Systems* textbook to demonstrate.


## Theory of Operation

Instead of a single entity class, there are (at least) 3:

- The entity class itself, used to return entities from the database. The entity is *read-only* (its properties are 
  declared `readonly` and initialized via constructor arguments) to emphasise that it's not intended to be edited.
- The class used to create new instances of the entity, a simple POCO. In theory, you could have multiple such classes for 
  creating new entities in different initial states
- The class used to modify (update, duplicate or delete) one or more existing entities. Extension methods are provided on 
  the entity class or lists of same to create an instance of the modifier class. The properties on this class can be in 
  an indeterminate state depending on the values in the underlying entities.


## Prerequisites

- Visual Studio 2015


## TODO

- Extend to other entity types
- Add code generation for entities
- Improve error handling and functionality of the MVC model binder and helper classes to support JSON 
  serialization, localization and better rendering of HTML.
- Investigate possiblity of extending to use Entity Framework

© 2017 Kaia Limited. All rights reserved.

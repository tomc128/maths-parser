Maths Parser
------------

### What is it?

A simple mathematical expression parser written in C#. It supports all basic operators, follows the order of operations, and can handle nested expressions.

### How do I use it?

First, set up an `Environment` object with two dictionaries, one for variables and one for functions.

Then, create a `Parser` object. Call the `Read()` method with the expression you want to parse.

This will return a Node, which is a tree-like structure representing the expression.

To evaluate the expression, call the `Evaluate()` method, passing in the `Environment` object created earlier.

This returns a `double` value. However, you may want to convert this into a `Number` object for easier fraction and precision handling.

### Use cases

Maths Parser is the engine used in my <a href="https://github.com/tomc128/calcu">*Calcu*</a> bot for Discord. It allows users to perform calculations quickly and easily in a Discord server.
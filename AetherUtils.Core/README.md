# Aether Utilities
This is a collection of classes and custom controls that are used across my various projects. This package contains
many useful methods and helpers that make starting a new project with it easy.

I found that I was always copying most of the same boilerplate code between projects of mine, so I decided that a single,
Nuget package would be better. That way, it can just be installed into the project to access all the helper code.

As you can tell from below, this utility package contains quite a bit of functionality; there is more than what's listed here.
I will also be updating this package with new functionality as needed by my projects.

Full documentation can be found here: https://ethan-hann.github.io/AetherUtils/

## Features (in no particular order):
- Reading/writing JSON files and XML files from disk or to/from .NET objects.
- File/.NET encryption/decryption.
- Extended file, folder, and path manipulators.
- Simple licensing framework; built by extending the Standard.NET licensing package.
- Logging framework
- Various reflection classes to help get data from a class at runtime.
- File/.NET object hashing (and verification).
- Password generation based on rules. Rules can be saved to files and .NET objects and then encrypted.
- Various custom WinForms controls targeting .NET Core 8.0 and above.
  - Extended property grid with extra buttons
  - Image combo box (allows for an image and text to be displayed in a combo box)
  - Input box pop up form
  - Split button
  - Icon lists and readers
- Serializable dictionary
- Proportional random selector
- 2FA framework
- **LOTS** of extension methods.
- Full configuration helper framework
  - Read/Write custom configuration files using YAML.
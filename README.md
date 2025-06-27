# BASIC language implementation

This is a hobby project to implement BASIC language from source code to VM runtime in C#. Language version is based on [True BASIC](https://physics.clarku.edu/sip/tutorials/True_BASIC.html) with minor adjustments.

## Goals

Basic (pun definitely intended) goal is to have a CLI application that would be able to parse and execute a program written with BASIC, similar to what [dotnet](https://learn.microsoft.com/en-us/dotnet/core/tools/), but much more limited. Application should have compiler frontend, backend, and a VM that can run stack based bytecode.

Possible, but unlikely, goals include LSP for VS Code, CLI and WASM as compilation targets.

## Design

TODO - link to Crafting Interpreters, describe parser implementation, error handling, VM overview

## Specification

* [BASIC language specification](./documentation/LanguageSpecification.md)
* TODO - bytecode specification

## How to run

Currently there is no implementation of a CLI, but tests allow to have executions from source code to console outputs.
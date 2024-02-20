# Industrious.Starter

Quickly bootstrap or update "Industrious-style" multi-platform .NET solutions.

I like to "design in code" and often spin up projects to explore a new idea or technique. This little command line
executable quickly generates the necessary solution and project files for all the platforms I like to play on, with the
settings I prefer.

See [Example](./Example/) for a sample generated project.

## Usage

Create a new solution named "Starter.Example".

```shell
$ Industrious.Starter new Starter.Example
```

By default, the generated projects will use the solution name as the application title. Use the `--title` parameter to
set a different value.

```shell
$ Industrious.Starter new Starter.Example --title Example
```

To rename an existing solution, changing project names, references, and namespaces.

```shell
$ Industrious.Starter rename NewName
```

This operation will rename files and fix up projects but intentionally leaves some sources, such as storyboards, alone.
You will need to search and replace to find those occurrences.

## // TODO:

- [x] Generate support files (`.editorconfig`, etc.)
- [x] Generate solution
- [x] Generate common code library
- [x] Generate console project
- [x] Generate macOS project
- [x] Generate iOS project
- [ ] Generate Windows project
- [ ] Generate Android project
- [ ] Generate web project
- [x] Rename an existing solution

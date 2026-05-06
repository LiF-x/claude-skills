# claude-skills

Claude Code skills for working with the [LiFx](https://lifxmod.com) Life is Feudal: Your Own mod framework.

## Skills

- [`skills/lifx-recipe-mod/`](skills/lifx-recipe-mod/) — Generate a drop-in `mod.cs` that adds new recipes (via `LiFx::registerRecipe`) and/or edits existing vanilla recipes and recipe requirements (via `dbi.Update` SQL), validated against the vanilla `dump.sql`. See [`examples/swap-apple-to-berry-2264`](skills/lifx-recipe-mod/examples/swap-apple-to-berry-2264/).

## Use

Symlink or copy a skill directory into `~/.claude/skills/`:

```sh
ln -s "$(pwd)/skills/lifx-recipe-mod" ~/.claude/skills/lifx-recipe-mod
```

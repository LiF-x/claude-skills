---
name: lifx-recipe-mod
description: Generate a drop-in mod.cs for the LiFx Life is Feudal framework that adds new recipes (via LiFx::registerRecipe) and/or edits existing vanilla recipes and recipe_requirements (via dbi.Update SQL). Use when the user asks to create, edit, swap, add, or remove a Life is Feudal recipe or recipe ingredient and wants the result as a LiFx mod.cs file.
---

# lifx-recipe-mod

Generate a single drop-in `mod.cs` file that plugs into the [LiFx framework](https://lifxmod.com) for Life is Feudal: Your Own.

## When to use

The user wants any of:

- A brand-new recipe (with its `recipe_requirement` rows) added to the game.
- A swap, add, or remove of an ingredient (`recipe_requirement`) on an existing vanilla recipe.
- A change to a field on an existing vanilla `recipe` row (result, quantity, skill level, etc.).

…and they want the output as a LiFx-compatible `mod.cs`, not raw SQL.

## Inputs to gather

Ask only for what's missing:

1. **Mod name** — PascalCase. Used for the `ScriptObject` name and package name (e.g. `LiFxBerryPie`).
2. **Operations** — one or more of: `new-recipe`, `edit-recipe`, `add-requirement`, `remove-requirement`, `swap-requirement`.
3. **Per operation, the specifics** — recipe ID or name, and ingredient names (preferred — names are resolved against `dump.sql`) or IDs.

Item and recipe names may be ambiguous in `dump.sql`. If a name resolves to >1 ID, list the candidates and ask the user to pick.

## Source of truth

Resolve all IDs from the local vanilla dump:

```
/mnt/c/LiFx/server-vanilla/sql/dump.sql
```

Relevant tables:

- `objects_types(ID, ParentID, Name, …, MaxContSize, MaxStackSize, …, ImagePath, Description, Weight, …)`
- `recipe(ID, Name, Description, StartingToolsID, SkillTypeID, SkillLvl, ResultObjectTypeID, SkillDepends, Quantity, Autorepeat, IsBlueprint, ImagePath)`
- `recipe_requirement(ID, RecipeID, MaterialObjectTypeID, Quality, Influence, Quantity, IsRegionItemRequired)`

### Lookup recipes

`grep`-style examples (run via Bash):

```sh
# Resolve item name → ID
grep -nE "'Apple'|'Edible Berries'" /mnt/c/LiFx/server-vanilla/sql/dump.sql

# All requirements for a recipe ID
awk '/INSERT INTO `recipe_requirement` \(/{flag=1} flag' \
  /mnt/c/LiFx/server-vanilla/sql/dump.sql \
  | grep -E '^\([0-9]+,2264,'

# The recipe row itself
awk '/INSERT INTO `recipe` \(/{flag=1} flag' \
  /mnt/c/LiFx/server-vanilla/sql/dump.sql \
  | grep -E '^\(2264,'
```

## Validation rules

Before emitting any SQL or registration call:

1. **Names → IDs.** Every named ingredient/recipe must resolve to exactly one row in `dump.sql`. On ambiguity, stop and ask.
2. **Edit / swap / remove on existing recipe.** Confirm the target row exists. For swaps, confirm the "from" material is currently a `recipe_requirement` of that recipe ID.
3. **New IDs.** When picking IDs for new `recipe`, `recipe_requirement`, or `objects_types` rows, choose values ≥ 100000 (mod-space floor) and grep `dump.sql` to confirm no collision.
4. **Cite sources.** The emitted `mod.cs` carries a header comment block listing the source rows from `dump.sql` it derived from.

## Emit style — pick per operation

| Operation | Style |
|---|---|
| `new-recipe` (and any new `objects_types` it depends on) | `LiFx::registerObjectsTypes(...)` + `LiFx::registerRecipe(...)` with `ScriptObject(... : Recipes)` and `%recipe.Requirements.Push(...)` per ingredient |
| `edit-recipe`, `add-requirement`, `remove-requirement`, `swap-requirement` | `dbi.Update("…")` inside a `DbChanges()` callback, registered to `$LiFx::hooks::onInitServerDBChangesCallbacks` |

A single `mod.cs` may contain both styles when an operation set mixes new content with edits to vanilla rows.

## File envelope

Use the same shape as [`LiF-x/Containers/mod.cs`](https://github.com/LiF-x/Containers/blob/main/mods/LiFx/Containers/mod.cs):

1. Header comment block (`<author>`, `<email>`, `<url>`, `<description>`, `<license>`, source-row citations).
2. `if (!isObject(<Mod>)) { new ScriptObject(<Mod>) {}; }`
3. `package <Mod> { … };`
4. `activatePackage(<Mod>);`
5. `LiFx::registerCallback($LiFx::hooks::mods, setup, <Mod>);`

Inside the package:

- `function <Mod>::setup()` — register only the hooks actually needed.
- `function <Mod>::version()` — returns a semver string.
- `function <Mod>::DbChanges()` — present only when raw SQL is emitted; runs `dbi.Update("…")` per statement.
- New-recipe / new-objects-types builder functions invoked from `setup()` via `LiFx::registerRecipe` / `LiFx::registerObjectsTypes`.

See `templates/` for ready-to-fill copies of each shape and `examples/swap-apple-to-berry-2264/mod.cs` for a worked example.

## Output

Write the generated file to a path the user picks (default: `mods/LiFx/<ModName>/mod.cs` under their working dir). Don't commit it — let the user wire it into their mod repo / Rampart manifest themselves.

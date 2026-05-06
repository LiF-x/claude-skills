# Example: swap apples → edible berries on recipe 2264

Input: "for recipe 2264, replace apples with edible berries".

Resolved from `server-vanilla/sql/dump.sql`:

- Apple = `objects_types.ID` 1026
- Edible Berries = `objects_types.ID` 668
- Recipe 2264 has a current `recipe_requirement` row with `MaterialObjectTypeID = 1026`

Output: [`mod.cs`](mod.cs) — uses the `dbchanges` template since this only edits a vanilla row.

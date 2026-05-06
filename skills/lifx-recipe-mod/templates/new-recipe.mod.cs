/**
* <author>{{AUTHOR}}</author>
* <email>{{EMAIL}}</email>
* <url>{{URL}}</url>
* <description>{{DESCRIPTION}}</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*
* New recipe: {{RECIPE_NAME}} (ID {{RECIPE_ID}})
* Result:     {{RESULT_NAME}} (ID {{RESULT_ID}})
* Skill:      {{SKILL_TYPE_ID}} @ lvl {{SKILL_LVL}}
* Requires:
{{REQUIREMENT_CITATIONS}}
*/

if (!isObject({{MOD}}))
{
    new ScriptObject({{MOD}})
    {
    };
}

package {{MOD}}
{
    function {{MOD}}::setup() {
        LiFx::registerRecipe({{MOD}}::Recipe{{RECIPE_KEY}}(), {{MOD}});
    }

    function {{MOD}}::version() {
        return "{{VERSION}}";
    }

    function {{MOD}}::Recipe{{RECIPE_KEY}}() {
        %recipe = new ScriptObject(Recipe{{RECIPE_KEY}} : Recipes)
        {
            RecipeName          = "{{RECIPE_NAME}}";
            Description         = "{{RECIPE_DESCRIPTION}}";
            StartingToolsID     = {{STARTING_TOOLS_ID}};
            SkillTypeID         = {{SKILL_TYPE_ID}};
            SkillLvl            = {{SKILL_LVL}};
            ResultObjectTypeID  = {{RESULT_ID}};
            SkillDepends        = {{SKILL_DEPENDS}};
            Quantity            = {{QUANTITY}};
            Autorepeat          = {{AUTOREPEAT}};
            IsBlueprint         = {{IS_BLUEPRINT}};
            ImagePath           = "{{IMAGE_PATH}}";
            Requirements        = JettisonArray("Recipe{{RECIPE_KEY}}Requirements");
        };

{{REQUIREMENT_PUSHES}}

        return %recipe;
    }
};
activatePackage({{MOD}});
LiFx::registerCallback($LiFx::hooks::mods, setup, {{MOD}});

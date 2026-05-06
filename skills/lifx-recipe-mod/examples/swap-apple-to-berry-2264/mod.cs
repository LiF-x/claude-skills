/**
* <author>Christophe Roblin</author>
* <email>christophe@roblin.no</email>
* <url>lifxmod.com</url>
* <description>Swap Apples to Edible Berries as a requirement of recipe 2264.</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*
* Source rows (from server-vanilla/sql/dump.sql):
*   objects_types: (1026, 232, 'Apple',          ...)
*   objects_types: (668,  232, 'Edible Berries', ...)
*   recipe_requirement: (..., 2264, 1026, 0, 10, 1, 0)   // current Apple requirement
*/

if (!isObject(LiFxRecipeBerrySwap2264))
{
    new ScriptObject(LiFxRecipeBerrySwap2264)
    {
    };
}

package LiFxRecipeBerrySwap2264
{
    function LiFxRecipeBerrySwap2264::setup() {
        LiFx::registerCallback($LiFx::hooks::onServerCreatedCallbacks, DbChanges, LiFxRecipeBerrySwap2264);
        LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, DbChanges, LiFxRecipeBerrySwap2264);
    }

    function LiFxRecipeBerrySwap2264::version() {
        return "1.0.0";
    }

    function LiFxRecipeBerrySwap2264::DbChanges() {
        // Apple (1026) -> Edible Berries (668) on recipe 2264
        dbi.Update("UPDATE `recipe_requirement` SET MaterialObjectTypeID = 668 WHERE RecipeID = 2264 AND MaterialObjectTypeID = 1026");
    }
};
activatePackage(LiFxRecipeBerrySwap2264);
LiFx::registerCallback($LiFx::hooks::mods, setup, LiFxRecipeBerrySwap2264);

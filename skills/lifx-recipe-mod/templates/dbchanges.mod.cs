/**
* <author>{{AUTHOR}}</author>
* <email>{{EMAIL}}</email>
* <url>{{URL}}</url>
* <description>{{DESCRIPTION}}</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*
* Source rows (from server-vanilla/sql/dump.sql):
{{SOURCE_CITATIONS}}
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
        LiFx::registerCallback($LiFx::hooks::onInitServerDBChangesCallbacks, DbChanges, {{MOD}});
    }

    function {{MOD}}::version() {
        return "{{VERSION}}";
    }

    function {{MOD}}::DbChanges() {
{{DBI_UPDATES}}
    }
};
activatePackage({{MOD}});
LiFx::registerCallback($LiFx::hooks::mods, setup, {{MOD}});

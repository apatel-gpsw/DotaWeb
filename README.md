# DotaWeb

Sample Table (Match ID: 4169885095)
<p>

![table](https://user-images.githubusercontent.com/9336827/47464231-73f3e080-d79d-11e8-86c3-f7856d20d74d.PNG)

C# MVC web application to show match details using the Steam APIs.

`First things first, register yourself on https://steamcommunity.com/dev/apikey and replace the "APIKey" you receive in the web.config`

**Steam API documentation used** (Dota Game ID: 570)
https://steamwebapi.azurewebsites.net/

**Subset of useful APIs**
https://dev.dota2.com/showthread.php?t=58317

**Get Images**
https://dev.dota2.com/showthread.php?t=138016


## Flow:
1. Build Items List object using [GetGameItems()](https://wiki.teamfortress.com/wiki/WebAPI/GetGameItems).
   > API: http://api.steampowered.com/IEconDOTA2_570/GetGameItems/v0001/?key=api_key&language=en <br>
   > Item Image URL example: http://cdn.dota2.com/apps/dota2/images/items/blink_lg.png <br>
   <details>
   <summary>Item JSON Object</summary>
   <p>
   
   ```json
   {
   "result":{
      "items":[
            {
               "id":1,
               "name":"item_blink",
               "cost":2250,
               "secret_shop":0,
               "side_shop":1,
               "recipe":0,
               "localized_name":"Blink Dagger"
            }
         ]
      }
   }
2. Build Heroes List object using [GetHeroes()](https://wiki.teamfortress.com/wiki/WebAPI/GetHeroes). 
   > API: https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=api_key&language=en <br>
   > Hero Image URL example: http://cdn.dota2.com/apps/dota2/images/heroes/antimage_lg.png <br>
   <details>
   <summary>Hero JSON Object</summary>
   <p>
   
   ```json
   {
   "result":{
      "heroes":[
            {
               "name":"npc_dota_hero_antimage",
               "id":1,
               "localized_name":"Anti-Mage"
            }
         ]
      }
   }
3. Build Abilities List object using `npc_abilities.txt` file. Steam doesn't provide any API to fetch the abilities, not sure why.
   > Ability Image URL example: http://cdn.dota2.com/apps/dota2/images/abilities/antimage_blink_md.png
   <details>
   <summary>Ability text Object</summary>
   <p>
   
   ```
   {
      "DOTAAbilities":{
        "antimage_mana_break"
        {
          "ID"                  "5003"
          "AbilityBehavior"         "DOTA_ABILITY_BEHAVIOR_PASSIVE"
          "AbilityUnitDamageType"      "DAMAGE_TYPE_PHYSICAL"      
          "SpellImmunityType"         "SPELL_IMMUNITY_ENEMIES_NO"
          "AbilitySpecial"
          {
            "01"
            {
               "var_type"         "FIELD_FLOAT"
               "damage_per_burn"   "0.6"
            }
            "02"
            {
               "var_type"         "FIELD_INTEGER"
               "mana_per_hit"      "28 40 52 64"
            }
          }
        }
      }
   }
4. Get match details using [GetMatchDetails()](https://wiki.teamfortress.com/wiki/WebAPI/GetMatchDetails). For now, only the search by match id is available.
5. Show the match data in a tabular format on the UI.
> API: https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?&key=api_key&match_id=4142945482&language=en

TODO:
Player Profile
http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=api_key&steamids=76561197989505287&language=en



# DotaWeb

C# MVC web application to show match details using the Steam APIs.

`First things first, register yourself on https://steamcommunity.com/dev/apikey and replace the API Key you receive in the App.config`

**Better Steam API documentation** (Dota Game ID: 570)
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
2. Build Heros List object using [GetHeroes()](https://wiki.teamfortress.com/wiki/WebAPI/GetHeroes). 
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
4. Get match details using [GetMatchDetails()](https://wiki.teamfortress.com/wiki/WebAPI/GetMatchDetails). For now, the match id is hardcoded.
5. Show data on the console.
<br>
<details>
<summary>Sample Copnsole Output</summary>
<p>

```
Match ID: 4142945482
Match SeqNum: 3590197866
Human Players: 10
Duration: 3496
Game Mode: 22

Account ID: 347142169
Player Name: Solid.Miracle
Hero: Witch Doctor
	 Hero Level: 25
K/D/A: 7/14/21
CS: 123/9
	 GPM: 402
	 XPM: 482
Items:
Slot 1: Glimmer Cape | Slot 2: Dust Of Appearance | Slot 3: Urn Of Shadows | Slot 4: Aghanim's Scepter | Slot 5: Arcane Boots | Slot 6: Observer And Sentry Wards

Ability Upgrade Path:
 Paralyzing Cask upgraded at 1 @ 9/29/2018 2:51:39 AM
 Maledict upgraded at 2 @ 9/29/2018 2:54:28 AM
 Paralyzing Cask upgraded at 3 @ 9/29/2018 2:55:53 AM
 Maledict upgraded at 4 @ 9/29/2018 2:58:02 AM
 Paralyzing Cask upgraded at 5 @ 9/29/2018 3:00:55 AM
 Death Ward upgraded at 6 @ 9/29/2018 3:03:03 AM
 Maledict upgraded at 7 @ 9/29/2018 3:05:33 AM
 Voodoo Restoration upgraded at 8 @ 9/29/2018 3:07:14 AM
 Paralyzing Cask upgraded at 9 @ 9/29/2018 3:09:27 AM
 upgraded at 10 @ 9/29/2018 3:11:25 AM
 Voodoo Restoration upgraded at 11 @ 9/29/2018 3:12:34 AM
 Death Ward upgraded at 12 @ 9/29/2018 3:15:21 AM
 Maledict upgraded at 13 @ 9/29/2018 3:17:36 AM
 Voodoo Restoration upgraded at 14 @ 9/29/2018 3:19:58 AM
 upgraded at 15 @ 9/29/2018 3:21:38 AM
 Voodoo Restoration upgraded at 16 @ 9/29/2018 3:25:15 AM
 Death Ward upgraded at 17 @ 9/29/2018 3:27:12 AM
 upgraded at 18 @ 9/29/2018 3:35:10 AM
 upgraded at 19 @ 9/29/2018 3:49:11 AM

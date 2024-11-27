using System.Collections.Generic;

namespace Tomat.TRPV.Content;

internal static class Sounds
{
    private const int sound_coin_length       = 5;
    private const int sound_drip_length       = 3;
    private const int sound_zombie_length     = 131;
    private const int sound_liquid_length     = 2;
    private const int sound_roar_length       = 3;
    private const int sound_splash_length     = 6;
    private const int sound_item_length       = 179;
    private const int sound_npc_hit_length    = 58;
    private const int sound_npc_killed_length = 67;

    public static readonly List<string> SOUND_NAMES = [];

    static Sounds()
    {
        SOUND_NAMES.Add("Sounds/Mech_0");
        SOUND_NAMES.Add("Sounds/Grab");
        SOUND_NAMES.Add("Sounds/Pixie");
        SOUND_NAMES.Add("Sounds/Dig_0");
        SOUND_NAMES.Add("Sounds/Dig_1");
        SOUND_NAMES.Add("Sounds/Dig_2");
        SOUND_NAMES.Add("Sounds/Thunder_0");
        SOUND_NAMES.Add("Sounds/Thunder_1");
        SOUND_NAMES.Add("Sounds/Thunder_2");
        SOUND_NAMES.Add("Sounds/Thunder_3");
        SOUND_NAMES.Add("Sounds/Thunder_4");
        SOUND_NAMES.Add("Sounds/Thunder_5");
        SOUND_NAMES.Add("Sounds/Thunder_6");
        SOUND_NAMES.Add("Sounds/Research_0");
        SOUND_NAMES.Add("Sounds/Research_1");
        SOUND_NAMES.Add("Sounds/Research_2");
        SOUND_NAMES.Add("Sounds/Research_3");
        SOUND_NAMES.Add("Sounds/Tink_0");
        SOUND_NAMES.Add("Sounds/Tink_1");
        SOUND_NAMES.Add("Sounds/Tink_2");
        SOUND_NAMES.Add("Sounds/Player_Hit_0");
        SOUND_NAMES.Add("Sounds/Player_Hit_1");
        SOUND_NAMES.Add("Sounds/Player_Hit_2");
        SOUND_NAMES.Add("Sounds/Female_Hit_0");
        SOUND_NAMES.Add("Sounds/Female_Hit_1");
        SOUND_NAMES.Add("Sounds/Female_Hit_2");
        SOUND_NAMES.Add("Sounds/Player_Killed");
        SOUND_NAMES.Add("Sounds/Chat");
        SOUND_NAMES.Add("Sounds/Grass");
        SOUND_NAMES.Add("Sounds/Door_Opened");
        SOUND_NAMES.Add("Sounds/Door_Closed");
        SOUND_NAMES.Add("Sounds/Menu_Tick");
        SOUND_NAMES.Add("Sounds/Menu_Open");
        SOUND_NAMES.Add("Sounds/Menu_Close");
        SOUND_NAMES.Add("Sounds/Shatter");
        SOUND_NAMES.Add("Sounds/Camera");

        for (var i = 0; i < sound_coin_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Coin_" + i);
        }

        for (var i = 0; i < sound_drip_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Drip_" + i);
        }

        for (var i = 0; i < sound_zombie_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Zombie_" + i);
        }

        for (var i = 0; i < sound_liquid_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Liquid_" + i);
        }

        for (var i = 0; i < sound_roar_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Roar_" + i);
        }

        for (var i = 0; i < sound_splash_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Splash_" + i);
        }

        SOUND_NAMES.Add("Sounds/Double_Jump");
        SOUND_NAMES.Add("Sounds/Run");
        SOUND_NAMES.Add("Sounds/Coins");
        SOUND_NAMES.Add("Sounds/Unlock");
        SOUND_NAMES.Add("Sounds/MaxMana");
        SOUND_NAMES.Add("Sounds/Drown");

        for (var i = 1; i < sound_item_length; i++)
        {
            SOUND_NAMES.Add("Sounds/Item_" + i);
        }

        for (var i = 1; i < sound_npc_hit_length; i++)
        {
            SOUND_NAMES.Add("Sounds/NPC_Hit_" + i);
        }

        for (var i = 1; i < sound_npc_killed_length; i++)
        {
            SOUND_NAMES.Add("Sounds/NPC_Killed_" + i);
        }

        CreateTrackable("achievement_complete");
        CreateTrackable("blizzard_inside_building_loop");
        CreateTrackable("blizzard_strong_loop");
        CreateTrackable("liquids_honey_water",        3);
        CreateTrackable("liquids_honey_lava",         3);
        CreateTrackable("liquids_water_lava",         3);
        CreateTrackable("dd2_ballista_tower_shot",    3);
        CreateTrackable("dd2_explosive_trap_explode", 3);
        CreateTrackable("dd2_flameburst_tower_shot",  3);
        CreateTrackable("dd2_lightning_aura_zap",     4);
        CreateTrackable("dd2_defense_tower_spawn");
        CreateTrackable("dd2_betsy_death",           3);
        CreateTrackable("dd2_betsy_fireball_shot",   3);
        CreateTrackable("dd2_betsy_fireball_impact", 3);
        CreateTrackable("dd2_betsy_flame_breath");
        CreateTrackable("dd2_betsy_flying_circle_attack");
        CreateTrackable("dd2_betsy_hurt", 3);
        CreateTrackable("dd2_betsy_scream");
        CreateTrackable("dd2_betsy_summon",              3);
        CreateTrackable("dd2_betsy_wind_attack",         3);
        CreateTrackable("dd2_dark_mage_attack",          3);
        CreateTrackable("dd2_dark_mage_cast_heal",       3);
        CreateTrackable("dd2_dark_mage_death",           3);
        CreateTrackable("dd2_dark_mage_heal_impact",     3);
        CreateTrackable("dd2_dark_mage_hurt",            3);
        CreateTrackable("dd2_dark_mage_summon_skeleton", 3);
        CreateTrackable("dd2_drakin_breath_in",          3);
        CreateTrackable("dd2_drakin_death",              3);
        CreateTrackable("dd2_drakin_hurt",               3);
        CreateTrackable("dd2_drakin_shot",               3);
        CreateTrackable("dd2_goblin_death",              3);
        CreateTrackable("dd2_goblin_hurt",               6);
        CreateTrackable("dd2_goblin_scream",             3);
        CreateTrackable("dd2_goblin_bomber_death",       3);
        CreateTrackable("dd2_goblin_bomber_hurt",        3);
        CreateTrackable("dd2_goblin_bomber_scream",      3);
        CreateTrackable("dd2_goblin_bomber_throw",       3);
        CreateTrackable("dd2_javelin_throwers_attack",   3);
        CreateTrackable("dd2_javelin_throwers_death",    3);
        CreateTrackable("dd2_javelin_throwers_hurt",     3);
        CreateTrackable("dd2_javelin_throwers_taunt",    3);
        CreateTrackable("dd2_kobold_death",              3);
        CreateTrackable("dd2_kobold_explosion",          3);
        CreateTrackable("dd2_kobold_hurt",               3);
        CreateTrackable("dd2_kobold_ignite");
        CreateTrackable("dd2_kobold_ignite_loop");
        CreateTrackable("dd2_kobold_scream_charge_loop");
        CreateTrackable("dd2_kobold_flyer_charge_scream", 3);
        CreateTrackable("dd2_kobold_flyer_death",         3);
        CreateTrackable("dd2_kobold_flyer_hurt",          3);
        CreateTrackable("dd2_lightning_bug_death",        3);
        CreateTrackable("dd2_lightning_bug_hurt",         3);
        CreateTrackable("dd2_lightning_bug_zap",          3);
        CreateTrackable("dd2_ogre_attack",                3);
        CreateTrackable("dd2_ogre_death",                 3);
        CreateTrackable("dd2_ogre_ground_pound");
        CreateTrackable("dd2_ogre_hurt", 3);
        CreateTrackable("dd2_ogre_roar", 3);
        CreateTrackable("dd2_ogre_spit");
        CreateTrackable("dd2_skeleton_death", 3);
        CreateTrackable("dd2_skeleton_hurt",  3);
        CreateTrackable("dd2_skeleton_summoned");
        CreateTrackable("dd2_wither_beast_aura_pulse",     2);
        CreateTrackable("dd2_wither_beast_crystal_impact", 3);
        CreateTrackable("dd2_wither_beast_death",          3);
        CreateTrackable("dd2_wither_beast_hurt",           3);
        CreateTrackable("dd2_wyvern_death",                3);
        CreateTrackable("dd2_wyvern_hurt",                 3);
        CreateTrackable("dd2_wyvern_scream",               3);
        CreateTrackable("dd2_wyvern_dive_down",            3);
        CreateTrackable("dd2_etherian_portal_dryad_touch");
        CreateTrackable("dd2_etherian_portal_idle_loop");
        CreateTrackable("dd2_etherian_portal_open");
        CreateTrackable("dd2_etherian_portal_spawn_enemy", 3);
        CreateTrackable("dd2_crystal_cart_impact",         3);
        CreateTrackable("dd2_defeat_scene");
        CreateTrackable("dd2_win_scene");
        CreateTrackable("dd2_book_staff_cast", 3);
        CreateTrackable("dd2_book_staff_twister_loop");
        CreateTrackable("dd2_ghastly_glaive_impact_ghost", 3);
        CreateTrackable("dd2_ghastly_glaive_pierce",       3);
        CreateTrackable("dd2_monk_staff_ground_impact",    3);
        CreateTrackable("dd2_monk_staff_ground_miss",      3);
        CreateTrackable("dd2_monk_staff_swing",            4);
        CreateTrackable("dd2_phantom_phoenix_shot",        3);
        CreateTrackable("dd2_sonic_boom_blade_slash",      3);
        CreateTrackable("dd2_sky_dragons_fury_circle",     3);
        CreateTrackable("dd2_sky_dragons_fury_shot",       3);
        CreateTrackable("dd2_sky_dragons_fury_swing",      4);
        CreateTrackable("lucyaxe_talk",                    5);
        CreateTrackable("deerclops_hit",                   3);
        CreateTrackable("deerclops_death");
        CreateTrackable("deerclops_scream",     3);
        CreateTrackable("deerclops_ice_attack", 3);
        CreateTrackable("deerclops_rubble_attack");
        CreateTrackable("deerclops_step");
        CreateTrackable("chester_open",  2);
        CreateTrackable("chester_close", 2);
        CreateTrackable("abigail_summon");
        CreateTrackable("abigail_cry", 3);
        CreateTrackable("abigail_attack");
        CreateTrackable("abigail_upgrade", 3);
        CreateTrackable("glommer_bounce",  2);
        CreateTrackable("dst_male_hit",    3);
        CreateTrackable("dst_female_hit",  3);
        CreateTrackable("Drone");

        return;

        static void CreateTrackable(string name, int? variations = null)
        {
            if (variations.HasValue)
            {
                for (var i = 0; i < variations; i++)
                {
                    SOUND_NAMES.Add("Sounds/Custom/" + name + "_" + i);
                }
            }
            else
            {
                SOUND_NAMES.Add("Sounds/Custom/" + name);
            }
        }
    }
}
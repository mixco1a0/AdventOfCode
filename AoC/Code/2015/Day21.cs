using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day21 : Core.Day
    {
        public Day21() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v1",
                Core.Part.Two => "v1",
                _ => base.GetSolutionVersion(part),
            };
        }

        protected override List<Core.TestDatum> GetTestData()
        {
            List<Core.TestDatum> testData =
            [
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "",
                    RawInput =
@""
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "",
                    RawInput =
@""
                },
            ];
            return testData;
        }

        static readonly string ItemStore =
@"Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage+1    25     1       0
Damage+2    50     2       0
Damage+3   100     3       0
Defense+1   20     0       1
Defense+2   40     0       2
Defense+3   80     0       3";

        enum ItemType
        {
            Invalid,
            Weapon,
            Armor,
            Ring
        }

        record Item(string Name, int Cost, int Damage, int Armor) { }

        private static void ParseStore(out List<Item> weapons, out List<Item> armor, out List<Item> rings)
        {
            weapons = [];
            armor = [];
            rings = [];

            ItemType cur = ItemType.Invalid;
            foreach (string line in Util.String.Split(ItemStore, "\n\r"))
            {
                if (line.Contains(':'))
                {
                    ++cur;
                    continue;
                }

                string[] parts = Util.String.Split(line, "\n\r");
                switch (cur)
                {
                    case ItemType.Weapon:
                        weapons.Add(new Item(parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
                        break;
                    case ItemType.Armor:
                        armor.Add(new Item(parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
                        break;
                    case ItemType.Ring:
                        rings.Add(new Item(parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
                        break;
                }
            }
        }

        record Attacker(int HP, int Damage, int Armor) { }

        static bool RunBattleSimulation(Attacker player, Attacker boss)
        {
            int playerHP = player.HP;
            int bossHP = boss.HP;
            bool playerTurn = true;
            while (playerHP > 0 && bossHP > 0)
            {
                if (playerTurn)
                {
                    bossHP -= (player.Damage - boss.Armor);
                }
                else
                {
                    playerHP -= (boss.Damage - player.Armor);
                }
                playerTurn = !playerTurn;
            }
            return playerHP > bossHP;
        }

        static int GetBestPriceForRing(Item weapon, Item armor, Item ring, Attacker boss, List<Item> rings)
        {
            int bestPrice = int.MaxValue;
            if (RunBattleSimulation(new Attacker(100, weapon.Damage + ring.Damage, armor.Armor + ring.Armor), boss))
            {
                bestPrice = weapon.Cost + armor.Cost + ring.Cost;
                // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Successful battle! {weapon.Name} + {armor.Name} + {ring.Name} @ ${bestPrice}");
            }

            if (rings != null)
            {
                foreach (Item nextRing in rings)
                {
                    Item combinedRings = new($"{ring.Name}_{nextRing.Name}", ring.Cost + nextRing.Cost, ring.Damage + nextRing.Damage, ring.Armor + nextRing.Armor);
                    bestPrice = Math.Min(bestPrice, GetBestPriceForRing(weapon, armor, combinedRings, boss, null));
                }
            }
            return bestPrice;
        }

        static int GetBestPriceForArmor(Item weapon, Item armor, Attacker boss, List<Item> rings)
        {
            bool armorEnough = false;
            int bestPrice = int.MaxValue;
            // get price for armor, if successful, ignore ring check with armor
            if (RunBattleSimulation(new Attacker(100, weapon.Damage, armor.Armor), boss))
            {
                armorEnough = true;
                bestPrice = weapon.Cost + armor.Cost;
                // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Successful battle! {weapon.Name} + {armor.Name} @ ${bestPrice}");
            }

            // get price without armor
            Item nullArmor = new("NA", 0, 0, 0);
            foreach (Item ring in rings)
            {
                if (!armorEnough)
                {
                    bestPrice = Math.Min(bestPrice, GetBestPriceForRing(weapon, armor, ring, boss, [.. rings.Where(r => r.Name != ring.Name)]));
                }
                bestPrice = Math.Min(bestPrice, GetBestPriceForRing(weapon, nullArmor, ring, boss, [.. rings.Where(r => r.Name != ring.Name)]));
            }
            return bestPrice;
        }

        static int GetBestPriceForWeapon(Item weapon, Attacker boss, List<Item> armors, List<Item> rings)
        {
            int bestPrice = int.MaxValue;
            int d = weapon.Damage, a = 0;
            // if weapon is enough, dont spend any more
            if (RunBattleSimulation(new Attacker(100, d, a), boss))
            {
                // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Successful battle! {weapon.Name} + @ ${weapon.Cost}");
                return weapon.Cost;
            }
            else
            {
                foreach (Item armor in armors)
                {
                    bestPrice = Math.Min(bestPrice, GetBestPriceForArmor(weapon, armor, boss, rings));
                }
            }
            return bestPrice;
        }

        static int GetWorstPriceForRing(Item weapon, Item armor, Item ring, Attacker boss, List<Item> rings)
        {
            if (RunBattleSimulation(new Attacker(100, weapon.Damage + ring.Damage, armor.Armor + ring.Armor), boss))
            {
                return int.MinValue;
            }

            int worstPrice = weapon.Cost + armor.Cost + ring.Cost;
            // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Unsuccessful battle! {weapon.Name} + {armor.Name} + {ring.Name} @ ${worstPrice}");

            if (rings != null)
            {
                foreach (Item nextRing in rings)
                {
                    Item combinedRings = new($"{ring.Name}_{nextRing.Name}", ring.Cost + nextRing.Cost, ring.Damage + nextRing.Damage, ring.Armor + nextRing.Armor);
                    worstPrice = Math.Max(worstPrice, GetWorstPriceForRing(weapon, armor, combinedRings, boss, null));
                }
            }
            return worstPrice;
        }

        static int GetWorstPriceForArmor(Item weapon, Item armor, Attacker boss, List<Item> rings)
        {
            bool armorEnough = true;
            int worstPrice = int.MinValue;
            if (!RunBattleSimulation(new Attacker(100, weapon.Damage, armor.Armor), boss))
            {
                armorEnough = false;
                worstPrice = weapon.Cost + armor.Cost;
                // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Unsuccessful battle! {weapon.Name} + {armor.Name} @ ${worstPrice}");
            }

            // get price without armor
            Item nullArmor = new("NA", 0, 0, 0);
            foreach (Item ring in rings)
            {
                if (!armorEnough)
                {
                    worstPrice = Math.Max(worstPrice, GetWorstPriceForRing(weapon, armor, ring, boss, [.. rings.Where(r => r.Name != ring.Name)]));
                }
                worstPrice = Math.Max(worstPrice, GetWorstPriceForRing(weapon, nullArmor, ring, boss, [.. rings.Where(r => r.Name != ring.Name)]));
            }
            return worstPrice;
        }

        static int GetWorstPriceForWeapon(Item weapon, Attacker boss, List<Item> armors, List<Item> rings)
        {
            int d = weapon.Damage, a = 0;
            // if the weapon is enough to kill, ignore the weapon
            if (RunBattleSimulation(new Attacker(100, d, a), boss))
            {
                return int.MinValue;
            }
            // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"Unsuccessful battle! {weapon.Name} + @ ${weapon.Cost}");

            int worstPrice = int.MinValue;
            foreach (Item armor in armors)
            {
                worstPrice = Math.Max(worstPrice, GetWorstPriceForArmor(weapon, armor, boss, rings));
            }
            return worstPrice;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findWorstPrice)
        {
            List<int> bossVals = [.. inputs.Select(i => int.Parse(Util.String.Split(i, " :").Last()))];
            Attacker boss = new(bossVals[0], bossVals[1], bossVals[2]);
            ParseStore(out List<Item> weapons, out List<Item> armors, out List<Item> rings);

            int price = int.MaxValue;
            if (findWorstPrice)
            {
                price = int.MinValue;
            }

            for (int w = 0; w < weapons.Count; ++w)
            {
                if (findWorstPrice)
                {
                    price = Math.Max(price, GetWorstPriceForWeapon(weapons[w], boss, armors, rings));
                }
                else
                {
                    price = Math.Min(price, GetBestPriceForWeapon(weapons[w], boss, armors, rings));
                }
            }

            return price.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}
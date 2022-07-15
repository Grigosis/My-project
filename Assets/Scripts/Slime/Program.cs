using System;
using ROR.Core;
using ROR.Core.Serialization;
using RPGFight.Core;

namespace RPGFight
{
    class Program
    {

        private static void InitDefault()
        {
            var l1 = new LivingStateInBattle();
            l1.Attributes = Attrs.NewWithSubs();
            l1.x = 1;
            l1.y = 2;

            l1.Attributes.CUT.ATK_MAX_MLT = 999;

            var l2 = new LivingStateInBattle();
            l2.Attributes = Attrs.NewWithSubs();
            l2.x = 1;
            l2.y = 2;

            var testBattle = new BattleSceneState();
            //test.Id = "BattleScene/TestBattle";
            testBattle.Party1 = new LivingStateInBattle[] {l1};
            testBattle.Party2 = new LivingStateInBattle[] {l2};

            var defs = new Definitions();
            defs.DefinitionList = new SerializableList<Definition>();

            var simpleSkill = new SkillDefinition();
            simpleSkill.Attacks = new[]
            {
                new ElementAttack("CRUSH", 0.5f),
                new ElementAttack("PIERCE", 0.5f)
            };

            simpleSkill.HitChanceMlt = 0.7f;
            simpleSkill.IsRangedAttack = true;
            simpleSkill.Id = "Skill/SimpleAttack";
            simpleSkill.Name = "Simple attack";

            defs.DefinitionList.Add(simpleSkill);

            D.Instance.WriteToFile("XmlData/testbattle.xml", testBattle, false);
            D.Instance.WriteToFile("XmlData/definitions.xml", defs, false);
        }

        static void Main(string[] args)
        {
            //InitDefault();
            var defs = D.Instance.ReadFromFile<Definitions>("XmlData/definitions.xml");
            var battleSceneState = D.Instance.ReadFromFile<BattleSceneState>("XmlData/testbattle.xml");

            LivingEntity p1 = null, p2 = null;
            foreach (var entity in battleSceneState.Party1)
            {
                var livingEntity = Builder.Build(entity);
                p1 = livingEntity;
                Entities.Add(livingEntity);
            }

            foreach (var entity in battleSceneState.Party2)
            {
                var livingEntity = Builder.Build(entity);
                p2 = livingEntity;
                Entities.Add(livingEntity);
            }

            var skillDefinition = D.Instance.Get<SkillDefinition>("Skill/SimpleAttack");

            while (true)
            {
                Balance.UseDamageSkill(p1, p2, skillDefinition);
                Console.WriteLine(p2);
                Console.WriteLine("=======");
                Balance.UseDamageSkill(p2, p1, skillDefinition);
                Console.WriteLine(p1);
                Console.WriteLine("==================PRESS ENTER TO DAMAGE======================");
                Console.ReadLine();
            }


            Console.WriteLine("Hello World!");
        }
    }
}
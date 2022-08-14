using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using ClassLibrary1.Inventory;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Assert(bool cond, string description = "")
        {
            if (!cond)
            {
                throw new Exception(description);
            }
        }


        public static ItemDefinition DefaultItemDefinition = new ItemDefinition()
        {
            Id = "Item/Apple",
            Sellable = true,
            Storable = true,
            Weight = 0.5f,
            ItemTypes = 0,
            StackSize = 100
        };
        
        private static bool useInfiniteInventory = false;
        private static IInventory GetInventoryForTests()
        {
            IInventory inventory;
            if (useInfiniteInventory)
            {
                inventory = new InfiniteInventory(float.MaxValue);
            }
            else
            {
                inventory = new FixedInventory(float.MaxValue, 60);
            }

            return inventory;
        }
        
        public static void Main(string[] args)
        {

            TestItemCount1();
            TestClear();

            TestStacking();
            //TestStacking2();

            TestEvents();

            useInfiniteInventory = false;
            
            TestItemCount1();
            TestClear();

            TestStacking();
            //TestStacking2();

            TestEvents();

            Console.WriteLine("Hello");
            Console.ReadLine();
        }

        public static void TestItemCount1()
        {
            var count = 350;
            var item = new ItemStack(DefaultItemDefinition, null, count);
            var inv = GetInventoryForTests();
            inv.Add(item, 0);

            Assert(inv.UsedCells == count.DivideWithUpperRound(DefaultItemDefinition.StackSize), "Wrong item amount");
        }

        public static void TestClear()
        {
            var item = new ItemStack(DefaultItemDefinition, null, 350);
            var inv = GetInventoryForTests();
            inv.Add(item, 0);

            for (int i = inv.Count-1; i >= 0; i--)
            {
                inv.RemoveAt(i, -1);
            }

            Assert(inv.UsedCells == 0, "Wrong item amount");
        }

        public static void TestStacking()
        {
            var item1 = new ItemStack(DefaultItemDefinition, null, 25);
            var item2 = new ItemStack(DefaultItemDefinition, null, 25);
            var inv = GetInventoryForTests();
            inv.Add(item1, 0);
            inv.Add(item2, 0);
            
            Assert(inv.GetAt(0).ItemStack.Count == 50, "Not stacked");
        }

        

        public static void TestStacking2()
        {
            var item1 = new ItemStack(DefaultItemDefinition, null, 25);
            var item2 = new ItemStack(DefaultItemDefinition, null, 25);
            var item3 = new ItemStack(DefaultItemDefinition, null, 500);
            var inv = GetInventoryForTests();
            inv.Add(item1, 0);
            inv.Add(item2, 1);
            inv.Add(item3, 0);
            
            Assert(inv.GetAt(0).ItemStack.Count == DefaultItemDefinition.StackSize, "Not stacked");
            Assert(inv.GetAt(1).ItemStack.Count == DefaultItemDefinition.StackSize, "Not stacked");
        }
        
        public static void TestEvents()
        {
            Random random = new Random(100);
            var defs = new ItemDefinition[10];
            for (int i = 0; i < defs.Length; i++)
            {
                defs[i] = new ItemDefinition()
                {
                    Id = "Item/"+random.NextString(10),
                    Sellable = true,
                    Storable = true,
                    Weight = (float)random.NextDouble(0.1, 2),
                    ItemTypes = 0,
                    StackSize = random.Next(5, 100)
                };
            }
            
            Dictionary<ItemDefinition, int> amounts = new Dictionary<ItemDefinition, int>();
            Dictionary<ItemStack, int> positions = new Dictionary<ItemStack, int>();
            Dictionary<ItemStack, int> testPositions = new Dictionary<ItemStack, int>();
            Dictionary<ItemDefinition, int> testAmounts = new Dictionary<ItemDefinition, int>();
            
            
            var inv = GetInventoryForTests();
            inv.OnItemCountChanged += (stack, from, neww) =>
            {
                var newV = amounts.GetOrNew(stack.Definition) + (neww - from);
                if (newV == 0)
                {
                    amounts.Remove(stack.Definition);
                }
                else
                {
                    amounts[stack.Definition] = newV;
                }
            };
            
            inv.OnItemAdded += (stack, where) =>
            {
                positions[stack] = where;
            };
            
            inv.OnItemRemoved += (stack, where) =>
            {
                positions.Remove(stack);
            };

            inv.OnItemMoved += (stack, from, to) =>
            {
                positions[stack] = to;
            };


            //for (int i = 0; i < 100; i++)
            //{
            //    ItemStack stack = new ItemStack();
            //    stack.Count = random.Next(1, 100);
            //    stack.Definition = random.Next(defs);
            //    inv.Add(stack, random.Next(20));
            //}
            
            for (int i = 0; i < 100; i++)
            {
                inv.Remove(random.Next(defs), random.Next(1, 20));
            }
            
            inv.Foreach((stack, index) =>
            {
                testAmounts[stack.Definition] = testAmounts.GetOrNew(stack.Definition) + stack.Count;
                testPositions[stack] = index;
            });

            Assert(testAmounts.AreEqual(amounts), "TestAmounts = wrong");
            Assert(testPositions.AreEqual(positions), "TestPositions = wrong");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;

namespace ROR.Core
{
    public class Entities
    {
        private static Dictionary<long, IEntity> AllEntities = new Dictionary<long, IEntity>();
        private static Random Random = new Random();
        
        public static long GenerateNextEntityId()
        {
            long id = Random.Next();
            lock (AllEntities)
            {
                while (AllEntities.ContainsKey(id))
                {
                    id = Random.Next();
                }
                
                AllEntities[id] = null;
            }

            return id;
        }

        public static void Add(IEntity entity)
        {
            if (AllEntities == null)
            {
                return;    
            }
            
            if (entity.EntityId == 0)
            {
                entity.EntityId = GenerateNextEntityId();
            }
            
            if (AllEntities.TryGetValue(entity.EntityId, out var oldEntity) && oldEntity != entity && oldEntity != null)
            {
                throw new DuplicateNameException($"Already exists entity at key {entity.EntityId} : {oldEntity}/{entity}");
            }
            
            AllEntities[entity.EntityId] = entity;
        }

        public static void Remove(IEntity entity)
        {
            AllEntities[entity.EntityId] = null;
        }

        public static T Get<T>(long entity) where T : class, IEntity
        {
            return AllEntities[entity] as T;
        }
    }


    
}
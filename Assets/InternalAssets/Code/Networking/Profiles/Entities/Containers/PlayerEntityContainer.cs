using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Networking.Profiles.Entities.Containers
{
    public class PlayerEntityContainer : EntityContainerBase
    {
        public EntityProvider GetPlayerEntity(int id)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                ref var networkIdentity = ref Entities[i].Entity.GetComponent<NetworkPlayer>();

                if (networkIdentity.UserID == id)
                {
                    return Entities[i];
                }
            }

            return null;
        }

        public bool RemovePlayerEntity(int id)
        {
            var entity = GetPlayerEntity(id);
            return entity != null && Entities.Remove(entity);
        }

        public bool TryGetPlayerEntity(int id, out EntityProvider entityProvider)
        {
            entityProvider = GetPlayerEntity(id);
            return entityProvider != null;
        }

        public override bool IsAvaliableToAdd(EntityProvider entityProvider)
        {
            return entityProvider.Has<NetworkIdentity>() && entityProvider.Has<NetworkPlayer>();
        }
    }
}